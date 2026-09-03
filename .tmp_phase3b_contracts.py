import re, pathlib

root = pathlib.Path('c:/Users/aipal/OneDrive/WIP/LENA')
entity_dir = root / 'LENA' / 'Entity'
contracts_dir = root / 'LENA.API' / 'Contracts'

area_map = {
    'Bottle': 'Wine',
    'Country': 'Wine',
    'Region': 'Wine',
    'Vintage': 'Wine',
    'Type': 'Wine',
    'Item': 'Inventory',
    'FoodFlavor': 'Inventory',
    'FoodNutrient': 'Inventory',
    'NutrientType': 'Inventory',
    'FlavorProfile': 'Inventory',
    'Recipe': 'Recipe',
    'RecipeItem': 'Recipe',
    'RecipeStep': 'Recipe',
    'MealPlan': 'MealPlan',
    'MealSlot': 'MealPlan',
    'MealSlotItem': 'MealPlan',
    'GroceryList': 'Grocery',
    'GroceryListItem': 'Grocery',
}

child_collections = {'GroceryListItems', 'MealSlots', 'MealSlotItems', 'RecipeItems', 'RecipeSteps'}
route_excluded_create = {
    'MealSlot': {'MealPlanID'},
    'MealSlotItem': {'MealSlotID'},
    'GroceryListItem': {'GroceryListID'},
    'RecipeStep': {'RecipeID'},
    'RecipeItem': {'RecipeID'},
}
audit_props = {'CreatedBy', 'CreateDate', 'LastUpdatedBy', 'LastUpdatedDate'}

def parse_entities():
    entities = {}
    for f in entity_dir.rglob('*.cs'):
        text = f.read_text(encoding='utf-8', newline='')
        class_match = re.search(r'public\s+(?:class|record)\s+(\w+)\s*(?::\s*([\w.]+))?', text)
        if not class_match:
            continue
        name = class_match.group(1)
        ns_match = re.search(r'namespace\s+([\w.]+)', text)
        namespace = ns_match.group(1) if ns_match else ''
        properties = []
        for line in text.splitlines():
            m = re.match(r'^\s*public\s+(?:required\s+)?([\w<>,\.\?\s]+?)\s+(\w+)\s*\{\s*get;\s*set;\s*\}(?:\s*=\s*([^;]+);)?', line)
            if not m:
                continue
            prop_type = m.group(1).strip()
            if prop_type.endswith('class') or prop_type.endswith('record'):
                continue
            prop_name = m.group(2)
            default = m.group(3).strip() if m.group(3) else None
            properties.append({'type': prop_type, 'name': prop_name, 'default': default})
        entities[name] = {'namespace': namespace, 'base': class_match.group(2) or '', 'properties': properties}
    return entities

def core_class(prop_type):
    t = prop_type.replace(' ', '').rstrip('?')
    if '<' in t:
        m = re.match(r'^(?:ICollection|IEnumerable|IReadOnlyList|IList|List)<(.+)>$', t)
        if m:
            return m.group(1).rstrip('?').split('.')[-1]
    return t.split('.')[-1]

def is_collection(prop_type):
    return bool(re.match(r'^(?:ICollection|IEnumerable|IReadOnlyList|IList|List)<.+>\??$', prop_type.replace(' ', '')))

def classify_property(prop, entity_name, all_entity_names):
    name = prop['name']
    t = prop['type']
    core = core_class(t)
    if is_collection(t):
        if name in child_collections and core in area_map:
            return ('collection', core)
        return ('skip', None)
    if core in all_entity_names and core != entity_name and not name.endswith('ID'):
        return ('skip', None)
    return ('scalar', None)

def primary_key_name(properties, entity_name):
    candidates = [p['name'] for p in properties if p['name'].lower() == (entity_name.lower() + 'id')]
    return candidates[0] if candidates else None

def get_scalar_properties(props, entity_name, mode, all_entity_names):
    pk = primary_key_name(props, entity_name)
    excluded = set(audit_props)
    excluded.add('UserID')
    excluded.update(route_excluded_create.get(entity_name, set()))
    if mode == 'create':
        if pk:
            excluded.add(pk)
    results = []
    for p in props:
        kind, core = classify_property(p, entity_name, all_entity_names)
        if kind == 'skip':
            continue
        if kind == 'collection':
            if mode == 'response':
                results.append(('collection', p['name'], core, None))
            continue
        if p['name'] in excluded and mode != 'response':
            continue
        results.append(('scalar', p['name'], p['type'], p.get('default')))
    return results

def decl_for(prop_type, name, default):
    default_expr = ''
    if default:
        default_expr = f' = {default}'
    elif prop_type.endswith('?'):
        default_expr = ' = null'
    elif prop_type == 'string':
        default_expr = ' = string.Empty'
    elif prop_type == 'bool':
        default_expr = ' = false'
    elif prop_type == 'decimal':
        default_expr = ' = 0m'
    elif prop_type == 'byte':
        default_expr = ' = 0'
    elif prop_type == 'int':
        default_expr = ' = 0'
    if default_expr:
        return f'public {prop_type} {name} {{ get; init; }}' + default_expr + ';'
    return f'public {prop_type} {name} {{ get; init; }}'

def generate_request_record(entity_name, entity, mode, all_entity_names):
    props = get_scalar_properties(entity['properties'], entity_name, mode, all_entity_names)
    ns = entity['namespace']
    record_name = ('Create' if mode == 'create' else 'Update') + entity_name + 'Request'
    lines = [f'public record {record_name}', '{']
    for kind, name, t, default in props:
        if kind == 'collection':
            continue
        lines.append('    ' + decl_for(t, name, default))
    lines.append('')
    lines.append(f'    public {ns}.{entity_name} ToEntity() => new()')
    lines.append('    {')
    for kind, name, t, default in props:
        if kind == 'collection':
            continue
        lines.append(f'        {name} = {name},')
    lines.append('    };')
    lines.append('}')
    return '\n'.join(lines)

def is_non_nullable_reference(prop_type):
    if prop_type.endswith('?'):
        return False
    value_types = {'int', 'decimal', 'byte', 'bool', 'DateTime'}
    if prop_type in value_types:
        return False
    return True

def generate_response_record(entity_name, entity, all_entity_names):
    props = get_scalar_properties(entity['properties'], entity_name, 'response', all_entity_names)
    has_audit = 'AuditableEntity' in entity['base']
    ns = entity['namespace']
    lines = [f'public record {entity_name}Response', '{']
    for kind, name, t, default in props:
        if kind == 'scalar':
            prefix = 'required ' if is_non_nullable_reference(t) else ''
            lines.append(f'    public {prefix}{t} {name} {{ get; init; }}')
        elif kind == 'collection':
            child = name[:-1] if name.endswith('s') else name
            lines.append(f'    public IReadOnlyList<{child}Response>? {name} {{ get; init; }}')
    if has_audit:
        for ap in audit_props:
            if not any(p[1] == ap for p in props):
                t = 'string?' if ap in ('LastUpdatedBy',) else ('DateTime?' if ap in ('LastUpdatedDate',) else ('string' if ap == 'CreatedBy' else 'DateTime'))
                prefix = 'required ' if is_non_nullable_reference(t) else ''
                lines.append(f'    public {prefix}{t} {ap} {{ get; init; }}')
    lines.append('')
    lines.append(f'    public static {entity_name}Response FromEntity({ns}.{entity_name} entity) => new()')
    lines.append('    {')
    for kind, name, t, default in props:
        if kind == 'scalar':
            lines.append(f'        {name} = entity.{name},')
        elif kind == 'collection':
            child = name[:-1] if name.endswith('s') else name
            lines.append(f'        {name} = entity.{name}?.Select({child}Response.FromEntity).ToList(),')
    if has_audit:
        for ap in audit_props:
            if not any(p[1] == ap for p in props):
                lines.append(f'        {ap} = entity.{ap},')
    lines.append('    };')
    lines.append('}')
    return '\n'.join(lines)

def generate_contracts(entities, whitelist):
    for name in whitelist:
        if name not in entities:
            print('missing entity', name)
            continue
        area = area_map[name]
        path = contracts_dir / area / f'{name}Contracts.cs'
        path.parent.mkdir(parents=True, exist_ok=True)
        ns = entities[name]['namespace']
        create = generate_request_record(name, entities[name], 'create', set(entities))
        update = generate_request_record(name, entities[name], 'update', set(entities))
        response = generate_response_record(name, entities[name], set(entities))
        content = f'using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing {ns};\n\nnamespace LENA.API.Contracts.{area}\n{{\n{create}\n\n{update}\n\n{response}\n}}\n'
        path.write_text(content, encoding='utf-8', newline='')
        print('wrote', path)

if __name__ == '__main__':
    entities = parse_entities()
    whitelist = list(area_map.keys())
    generate_contracts(entities, whitelist)
