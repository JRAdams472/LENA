"use client";

import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Link from "next/link";
import { api } from "@/lib/api";
import DataTable from "@/app/components/DataTable";
import CrudDialog, { FieldDef } from "@/app/components/CrudDialog";
import { Recipe } from "@/lib/types";

const recipeFields: FieldDef[] = [
  { key: "recipeName", label: "Name" },
  { key: "description", label: "Description" },
  { key: "servings", label: "Servings", type: "number" },
  { key: "prepTimeMinutes", label: "Prep Time (min)", type: "number" },
  { key: "cookTimeMinutes", label: "Cook Time (min)", type: "number" },
  { key: "isActive", label: "Active", type: "boolean" },
];

function toRow(recipe: Recipe) {
  return {
    recipeID: recipe.recipeID,
    recipeName: recipe.recipeName,
    description: recipe.description,
    servings: recipe.servings,
    prepTimeMinutes: recipe.prepTimeMinutes,
    cookTimeMinutes: recipe.cookTimeMinutes,
    isActive: recipe.isActive,
  };
}

export default function RecipesPage() {
  const queryClient = useQueryClient();
  const [dialogOpen, setDialogOpen] = useState(false);
  const [dialogData, setDialogData] = useState<Record<string, unknown>>({});
  const [isCreate, setIsCreate] = useState(false);

  const listQuery = useQuery({
    queryKey: ["recipes"],
    queryFn: api.getRecipes,
  });

  const createMutation = useMutation({
    mutationFn: (row: Record<string, unknown>) =>
      api.createRecipe(row as unknown as Parameters<typeof api.createRecipe>[0]),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["recipes"] }),
  });

  const updateMutation = useMutation({
    mutationFn: (row: Record<string, unknown>) =>
      api.updateRecipe(row.recipeID as number, row as Partial<Recipe>),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["recipes"] }),
  });

  const deleteMutation = useMutation({
    mutationFn: (row: Record<string, unknown>) =>
      api.deleteRecipe(row.recipeID as number),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["recipes"] }),
  });

  const handleCreate = () => {
    setIsCreate(true);
    setDialogData({});
    setDialogOpen(true);
  };

  const handleEdit = (row: Record<string, unknown>) => {
    setIsCreate(false);
    setDialogData({ ...row });
    setDialogOpen(true);
  };

  const handleDelete = (row: Record<string, unknown>) => {
    if (window.confirm("Delete this recipe?")) {
      deleteMutation.mutate(row);
    }
  };

  const handleSave = (values: Record<string, unknown>) => {
    if (isCreate) {
      createMutation.mutate(values);
    } else {
      updateMutation.mutate(values);
    }
    setDialogOpen(false);
  };

  const extraActions = (row: Record<string, unknown>) => (
    <Button
      size="small"
      component={Link}
      href={`/recipes/${row.recipeID as number}`}
    >
      Manage
    </Button>
  );

  return (
    <Box>
      <DataTable
        title="Recipes"
        rows={(listQuery.data ?? []).map(toRow)}
        isLoading={listQuery.isLoading}
        error={listQuery.error as Error | null}
        onCreate={handleCreate}
        onEdit={handleEdit}
        onDelete={handleDelete}
        extraActions={extraActions}
      />
      <CrudDialog
        open={dialogOpen}
        title={isCreate ? "Create Recipe" : "Edit Recipe"}
        fields={recipeFields}
        values={dialogData}
        onClose={() => setDialogOpen(false)}
        onSave={handleSave}
      />
    </Box>
  );
}
