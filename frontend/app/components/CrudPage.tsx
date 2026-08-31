"use client";

import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import Box from "@mui/material/Box";
import FormControl from "@mui/material/FormControl";
import FormControlLabel from "@mui/material/FormControlLabel";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import Select from "@mui/material/Select";
import Switch from "@mui/material/Switch";
import DataTable from "./DataTable";
import CrudDialog, { FieldDef } from "./CrudDialog";

interface FilterDef {
  label: string;
  optionsFn: () => Promise<{ id: number; name: string }[]>;
  filterFn: (id: number) => Promise<any[]>;
}

interface CrudPageProps {
  title: string;
  queryKey: string[];
  listFn: () => Promise<any[]>;
  activeOnlyFn?: () => Promise<any[]>;
  filterBy?: FilterDef;
  fields: FieldDef[];
  createFn: (row: Record<string, unknown>) => Promise<any>;
  updateFn: (row: Record<string, unknown>) => Promise<any>;
  deleteFn: (row: any) => Promise<any>;
  prepareForEdit?: (row: any) => Record<string, unknown>;
}

export default function CrudPage({
  title,
  queryKey,
  listFn,
  activeOnlyFn,
  filterBy,
  fields,
  createFn,
  updateFn,
  deleteFn,
  prepareForEdit,
}: CrudPageProps) {
  const queryClient = useQueryClient();
  const [dialogOpen, setDialogOpen] = useState(false);
  const [dialogData, setDialogData] = useState<Record<string, unknown>>({});
  const [isCreate, setIsCreate] = useState(false);
  const [activeOnly, setActiveOnly] = useState(false);
  const [filterId, setFilterId] = useState<string>("");
  const [dialogError, setDialogError] = useState<Error | null>(null);
  const [tableError, setTableError] = useState<Error | null>(null);

  const listQuery = useQuery<any[]>({
    queryKey: [...queryKey, activeOnly, filterId],
    queryFn: () => {
      if (activeOnly && activeOnlyFn) return activeOnlyFn();
      if (filterBy && filterId) return filterBy.filterFn(Number(filterId));
      return listFn();
    },
  });

  const optionsQuery = useQuery<{ id: number; name: string }[]>({
    queryKey: [filterBy?.label ?? "filter-options"],
    queryFn: () => filterBy?.optionsFn() ?? Promise.resolve([]),
    enabled: !!filterBy,
  });

  const createMutation = useMutation({
    mutationFn: createFn,
    onSuccess: () => {
      setDialogError(null);
      setDialogOpen(false);
      queryClient.invalidateQueries({ queryKey });
    },
    onError: (err: unknown) => setDialogError(err as Error),
  });

  const updateMutation = useMutation({
    mutationFn: updateFn,
    onSuccess: () => {
      setDialogError(null);
      setDialogOpen(false);
      queryClient.invalidateQueries({ queryKey });
    },
    onError: (err: unknown) => setDialogError(err as Error),
  });

  const deleteMutation = useMutation({
    mutationFn: deleteFn,
    onSuccess: () => {
      setTableError(null);
      queryClient.invalidateQueries({ queryKey });
    },
    onError: (err: unknown) => setTableError(err as Error),
  });

  const rows = listQuery.data ?? [];

  const handleCreate = () => {
    setIsCreate(true);
    setDialogData({});
    setDialogError(null);
    setDialogOpen(true);
  };

  const handleEdit = (row: any) => {
    setIsCreate(false);
    setDialogError(null);
    setDialogData(prepareForEdit ? prepareForEdit(row) : { ...row });
    setDialogOpen(true);
  };

  const handleDelete = (row: any) => {
    setTableError(null);
    if (window.confirm(`Delete this ${title.toLowerCase()}?`)) {
      deleteMutation.mutate(row);
    }
  };

  const handleSave = (values: Record<string, unknown>) => {
    if (isCreate) {
      createMutation.mutate(values);
    } else {
      updateMutation.mutate(values);
    }
  };

  const handleClose = () => {
    setDialogError(null);
    setDialogOpen(false);
  };

  return (
    <Box>
      {filterBy && (
        <FormControl fullWidth sx={{ mb: 2 }}>
          <InputLabel>{filterBy.label}</InputLabel>
          <Select
            value={filterId}
            onChange={(e) => setFilterId(e.target.value)}
            label={filterBy.label}
          >
            <MenuItem value="">All</MenuItem>
            {optionsQuery.data?.map((o) => (
              <MenuItem key={o.id} value={String(o.id)}>
                {o.name}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      )}

      {activeOnlyFn && (
        <FormControlLabel
          control={
            <Switch
              checked={activeOnly}
              onChange={(e) => setActiveOnly(e.target.checked)}
            />
          }
          label="Active only"
          sx={{ mb: 2, display: "block" }}
        />
      )}

      <DataTable
        title={title}
        rows={rows}
        isLoading={listQuery.isLoading}
        error={tableError || (listQuery.error as Error | null)}
        onCreate={handleCreate}
        onEdit={handleEdit}
        onDelete={handleDelete}
      />

      <CrudDialog
        open={dialogOpen}
        title={isCreate ? `Create ${title}` : `Edit ${title}`}
        fields={fields}
        values={dialogData}
        error={dialogError}
        onClose={handleClose}
        onSave={handleSave}
      />
    </Box>
  );
}
