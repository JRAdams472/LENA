import pathlib, re, sys

ctrl_dir = pathlib.Path('c:/Users/aipal/OneDrive/WIP/LENA/LENA.API/Controllers')

for path in sorted(ctrl_dir.glob('*.cs')):
    text = path.read_text(encoding='utf-8', newline='')
    # add null-forgiving operator to simple FromEntity(variable) calls
    new_text, count = re.subn(r'FromEntity\((\w+)\)', r'FromEntity(\1!)', text)
    if count:
        path.write_text(new_text, encoding='utf-8', newline='')
        print(f'patched {path.name}: {count} replacements')
