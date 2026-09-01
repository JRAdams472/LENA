#!/usr/bin/env bash
# Provisions the LENA database (schemas, tables, indexes, seed data, stored procedures)
# from the .sql fragments in this directory. Safe to re-run: existing objects are skipped
# and stored procedures are applied as CREATE OR ALTER.
set -euo pipefail

SQLCMD="${SQLCMD:-/opt/mssql-tools18/bin/sqlcmd}"
DB_HOST="${DB_HOST:-db}"
DB_USER="${DB_USER:-sa}"
DB_PASSWORD="${MSSQL_SA_PASSWORD:-${SA_PASSWORD:-}}"
DB_NAME="${DB_NAME:-LENA}"
ROOT="${DB_ROOT:-$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)}"
DOMAINS=(Wine Inventory MealPlan Recipe)

if [ -z "$DB_PASSWORD" ]; then
    echo "MSSQL_SA_PASSWORD (or SA_PASSWORD) must be set" >&2
    exit 1
fi

master_query() {
    "$SQLCMD" -C -b -S "$DB_HOST" -U "$DB_USER" -P "$DB_PASSWORD" -h -1 -W -Q "SET NOCOUNT ON; $1"
}

db_query() {
    "$SQLCMD" -C -b -S "$DB_HOST" -U "$DB_USER" -P "$DB_PASSWORD" -d "$DB_NAME" -h -1 -W -Q "SET NOCOUNT ON; $1"
}

run_file() {
    echo "  applying $(basename "$1")"
    "$SQLCMD" -C -b -S "$DB_HOST" -U "$DB_USER" -P "$DB_PASSWORD" -d "$DB_NAME" -i "$1"
}

# Reads a file with its BOM stripped so leading keywords match.
file_body() {
    sed '1s/^\xEF\xBB\xBF//' "$1"
}

# Extracts the "[Schema].[Object]" pair following the given keyword pattern.
qualified_name() {
    file_body "$1" | tr -d '\r' | grep -ioP "$2\s*\[\K[^]]+\]\.\[[^]]+" | head -1 | tr -d '[]'
}

echo "Waiting for SQL Server at $DB_HOST..."
for attempt in $(seq 1 60); do
    if master_query "SELECT 1" >/dev/null 2>&1; then
        echo "SQL Server is accepting connections."
        break
    fi
    if [ "$attempt" -eq 60 ]; then
        echo "SQL Server did not become available in time" >&2
        exit 1
    fi
    sleep 5
done

echo "Ensuring database $DB_NAME exists..."
master_query "IF DB_ID(N'$DB_NAME') IS NULL EXEC(N'CREATE DATABASE [$DB_NAME]');"

echo "Schemas..."
for domain in "${DOMAINS[@]}"; do
    schema_file="$ROOT/$domain/Schema.sql"
    [ -f "$schema_file" ] || continue
    exists=$(db_query "SELECT COUNT(*) FROM sys.schemas WHERE name = N'$domain';" | tr -d '[:space:]')
    if [ "$exists" = "0" ]; then
        run_file "$schema_file"
    else
        echo "  skipping $domain/Schema.sql (schema exists)"
    fi
done

echo "Tables..."
pending=()
for domain in "${DOMAINS[@]}"; do
    for file in "$ROOT/$domain"/Tables/*.sql; do
        [ -e "$file" ] || continue
        name=$(qualified_name "$file" 'CREATE\s+TABLE')
        if [ -n "$name" ]; then
            exists=$(db_query "SELECT COUNT(*) FROM sys.tables t JOIN sys.schemas s ON s.schema_id = t.schema_id WHERE s.name + '.' + t.name = N'$name';" | tr -d '[:space:]')
            if [ "$exists" != "0" ]; then
                echo "  skipping $(basename "$file") ($name exists)"
                continue
            fi
        fi
        pending+=("$file")
    done
done

# Tables carry inline foreign keys, so apply them in passes: anything whose referenced
# table does not exist yet is retried after the rest of the pass has run.
while [ ${#pending[@]} -gt 0 ]; do
    deferred=()
    errors=""
    for file in "${pending[@]}"; do
        if output=$(run_file "$file" 2>&1); then
            echo "$output"
            continue
        fi
        echo "  deferring $(basename "$file") (unresolved dependency)"
        errors+="$output"$'\n'
        deferred+=("$file")
    done
    if [ ${#deferred[@]} -eq ${#pending[@]} ]; then
        echo "$errors" >&2
        echo "Could not create tables: ${deferred[*]}" >&2
        exit 1
    fi
    pending=("${deferred[@]}")
done

echo "Indexes..."
for domain in "${DOMAINS[@]}"; do
    for file in "$ROOT/$domain"/Indexes/*.sql; do
        [ -e "$file" ] || continue
        index_name=$(file_body "$file" | tr -d '\r' | grep -ioP 'INDEX\s*\[\K[^]]+' | head -1)
        table_name=$(qualified_name "$file" 'ON\s')
        if [ -n "$index_name" ] && [ -n "$table_name" ]; then
            exists=$(db_query "SELECT COUNT(*) FROM sys.indexes WHERE name = N'$index_name' AND object_id = OBJECT_ID(N'$table_name');" | tr -d '[:space:]')
            if [ "$exists" != "0" ]; then
                echo "  skipping $(basename "$file") ($index_name exists)"
                continue
            fi
        fi
        run_file "$file"
    done
done

echo "Seed data..."
for file in "$ROOT"/*/Seed/*.sql "$ROOT"/SeedData/*.sql; do
    [ -e "$file" ] || continue
    target=$(qualified_name "$file" 'INSERT\s+INTO')
    if [ -n "$target" ]; then
        rows=$(db_query "SELECT ISNULL((SELECT SUM(p.rows) FROM sys.partitions p WHERE p.object_id = OBJECT_ID(N'$target') AND p.index_id IN (0, 1)), 0);" | tr -d '[:space:]')
        if [ "$rows" != "0" ]; then
            echo "  skipping $(basename "$file") ($target already populated)"
            continue
        fi
    fi
    run_file "$file"
done

echo "Stored procedures..."
for domain in "${DOMAINS[@]}"; do
    for file in "$ROOT/$domain"/StoredProcedures/*.sql; do
        [ -e "$file" ] || continue
        echo "  applying $domain/StoredProcedures/$(basename "$file")"
        # The .sql fragments use bare CREATE PROCEDURE so the sqlproj can build them;
        # applying them as CREATE OR ALTER keeps re-runs against a populated volume working.
        file_body "$file" \
            | sed -E 's/^([[:space:]]*)CREATE[[:space:]]+PROCEDURE/\1CREATE OR ALTER PROCEDURE/I' \
            | "$SQLCMD" -C -b -S "$DB_HOST" -U "$DB_USER" -P "$DB_PASSWORD" -d "$DB_NAME"
    done
done

echo "Database $DB_NAME initialization complete."
