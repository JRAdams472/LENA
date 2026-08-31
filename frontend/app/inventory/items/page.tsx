"use client";

import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import { api } from "@/lib/api";
import DataTable from "@/app/components/DataTable";
import CrudDialog from "@/app/components/CrudDialog";

const itemFields = [
  { key: "name", label: "Name" },
  { key: "brand", label: "Brand" },
  { key: "upc12", label: "UPC12" },
  { key: "upc14", label: "UPC14" },
  { key: "categoryID", label: "Category ID", type: "number" as const },
  { key: "unit", label: "Unit" },
  { key: "currentQuantity", label: "Current Quantity", type: "number" as const },
  { key: "minQuantity", label: "Min Quantity", type: "number" as const },
  { key: "purchaseDate", label: "Purchase Date" },
  { key: "expiryDate", label: "Expiry Date" },
  { key: "notes", label: "Notes" },
  { key: "isFavorite", label: "Favorite", type: "boolean" as const },
];

export default function ItemsPage() {
  const queryClient = useQueryClient();
  const [dialogOpen, setDialogOpen] = useState(false);
  const [dialogData, setDialogData] = useState<Record<string, unknown>>({});
  const [isCreate, setIsCreate] = useState(false);

  const listQuery = useQuery({
    queryKey: ["items"],
    queryFn: api.getItems,
  });

  const createMutation = useMutation({
    mutationFn: api.createItem,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["items"] }),
  });

  const updateMutation = useMutation({
    mutationFn: (row: Record<string, unknown>) =>
      api.updateItem(row.itemID as number, row as any),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["items"] }),
  });

  const deleteMutation = useMutation({
    mutationFn: (row: any) => api.deleteItem(row.itemID),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["items"] }),
  });

  const changeCategoryMutation = useMutation({
    mutationFn: ({ id, categoryId }: { id: number; categoryId: number }) =>
      api.changeItemCategory(id, categoryId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["items"] }),
  });

  const setUPC12Mutation = useMutation({
    mutationFn: ({ id, upc12 }: { id: number; upc12: string }) =>
      api.setItemUPC12(id, upc12),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["items"] }),
  });

  const setUPC14Mutation = useMutation({
    mutationFn: ({ id, upc14 }: { id: number; upc14: string }) =>
      api.setItemUPC14(id, upc14),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["items"] }),
  });

  const adjustQuantityMutation = useMutation({
    mutationFn: ({
      id,
      quantity,
      purchaseDate,
    }: {
      id: number;
      quantity: number;
      purchaseDate?: string;
    }) => api.adjustItemQuantity(id, quantity, purchaseDate),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["items"] }),
  });

  const setFavoriteMutation = useMutation({
    mutationFn: ({ id, isFavorite }: { id: number; isFavorite: boolean }) =>
      api.setItemFavorite(id, isFavorite),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["items"] }),
  });

  const handleCreate = () => {
    setIsCreate(true);
    setDialogData({});
    setDialogOpen(true);
  };

  const handleEdit = (row: any) => {
    setIsCreate(false);
    setDialogData({ ...row });
    setDialogOpen(true);
  };

  const handleDelete = (row: any) => {
    if (window.confirm("Delete this item?")) {
      deleteMutation.mutate(row);
    }
  };

  const handleSave = (values: Record<string, unknown>) => {
    if (isCreate) {
      createMutation.mutate(values as any);
    } else {
      updateMutation.mutate(values);
    }
    setDialogOpen(false);
  };

  const handleChangeCategory = (id: number) => {
    const value = window.prompt("Enter new Category ID");
    if (value === null) return;
    const categoryId = Number(value);
    if (isNaN(categoryId)) {
      alert("Category ID must be a number");
      return;
    }
    changeCategoryMutation.mutate({ id, categoryId });
  };

  const handleUPC12 = (id: number) => {
    const upc12 = window.prompt("Enter UPC12");
    if (upc12 === null) return;
    setUPC12Mutation.mutate({ id, upc12 });
  };

  const handleUPC14 = (id: number) => {
    const upc14 = window.prompt("Enter UPC14");
    if (upc14 === null) return;
    setUPC14Mutation.mutate({ id, upc14 });
  };

  const handleAdjustQuantity = (id: number) => {
    const value = window.prompt("Enter quantity adjustment");
    if (value === null) return;
    const quantity = Number(value);
    if (isNaN(quantity)) {
      alert("Quantity must be a number");
      return;
    }
    const purchaseDate = window.prompt(
      "Enter purchase date (ISO, optional)"
    );
    adjustQuantityMutation.mutate({
      id,
      quantity,
      purchaseDate: purchaseDate || undefined,
    });
  };

  const handleToggleFavorite = (id: number, current: boolean) => {
    setFavoriteMutation.mutate({ id, isFavorite: !current });
  };

  const extraActions = (row: any) => (
    <Box sx={{ display: "flex", gap: 0.5, flexWrap: "wrap" }}>
      <Button
        size="small"
        onClick={() => handleChangeCategory(row.itemID)}
      >
        Category
      </Button>
      <Button size="small" onClick={() => handleUPC12(row.itemID)}>
        UPC12
      </Button>
      <Button size="small" onClick={() => handleUPC14(row.itemID)}>
        UPC14
      </Button>
      <Button
        size="small"
        onClick={() => handleAdjustQuantity(row.itemID)}
      >
        Qty
      </Button>
      <Button
        size="small"
        onClick={() =>
          handleToggleFavorite(row.itemID, row.isFavorite as boolean)
        }
      >
        {row.isFavorite ? "Unfav" : "Fav"}
      </Button>
    </Box>
  );

  return (
    <Box>
      <DataTable
        title="Items"
        rows={listQuery.data ?? []}
        isLoading={listQuery.isLoading}
        error={listQuery.error as Error | null}
        onCreate={handleCreate}
        onEdit={handleEdit}
        onDelete={handleDelete}
        extraActions={extraActions}
      />
      <CrudDialog
        open={dialogOpen}
        title={isCreate ? "Create Item" : "Edit Item"}
        fields={itemFields}
        values={dialogData}
        onClose={() => setDialogOpen(false)}
        onSave={handleSave}
      />
    </Box>
  );
}
