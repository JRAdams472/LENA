"use client";

import { use, useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import Box from "@mui/material/Box";
import Paper from "@mui/material/Paper";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import Checkbox from "@mui/material/Checkbox";
import FormControlLabel from "@mui/material/FormControlLabel";
import IconButton from "@mui/material/IconButton";
import DeleteIcon from "@mui/icons-material/Delete";
import Alert from "@mui/material/Alert";
import CircularProgress from "@mui/material/CircularProgress";
import { api } from "@/lib/api";
import { GroceryList, GroceryListItem, Item } from "@/lib/types";

interface ManualForm {
  manualItemName: string;
  quantityNeeded: string;
  unitOfMeasure: string;
}

const SOURCE_ORDER = ["MealPlan", "Depleted", "Manual"];
const SOURCE_LABELS: Record<string, string> = {
  MealPlan: "From Menu",
  Depleted: "Depleted Stock",
  Manual: "Manual",
};

export function ItemRow({
  item,
  items,
  listId,
}: {
  item: GroceryListItem;
  items: Item[] | undefined;
  listId: number;
}) {
  const queryClient = useQueryClient();
  const toggleMutation = useMutation({
    mutationFn: () => api.toggleGroceryListItemChecked(item.groceryListItemID),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["groceryList", listId] });
    },
  });

  const deleteMutation = useMutation({
    mutationFn: () => api.deleteGroceryListItem(item.groceryListItemID),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["groceryList", listId] });
    },
  });

  const itemName =
    item.itemName ??
    item.manualItemName ??
    items?.find((i) => i.itemID === item.itemID)?.name ??
    `Item ${item.itemID}`;

  return (
    <Box
      sx={{
        display: "flex",
        alignItems: "center",
        gap: 1,
        p: 1,
        borderBottom: "1px solid",
        borderColor: "divider",
      }}
    >
      <FormControlLabel
        control={
          <Checkbox
            checked={item.isChecked}
            onChange={() => toggleMutation.mutate()}
          />
        }
        label={
          <Box>
            <Typography
              variant="body1"
              sx={{
                textDecoration: item.isChecked ? "line-through" : "none",
              }}
            >
              {itemName}
            </Typography>
            <Typography variant="caption" color="text.secondary">
              {Number(item.quantityNeeded).toFixed(2)} {item.unitOfMeasure}
            </Typography>
          </Box>
        }
      />
      <Box sx={{ flexGrow: 1 }} />
      <IconButton
        size="small"
        onClick={() => deleteMutation.mutate()}
        disabled={deleteMutation.isPending}
      >
        <DeleteIcon fontSize="small" />
      </IconButton>
    </Box>
  );
}

export default function GroceryListDetailPage({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = use(params);
  const listId = Number(id);
  const queryClient = useQueryClient();

  const listQuery = useQuery({
    queryKey: ["groceryList", listId],
    queryFn: () => api.getGroceryList(listId),
  });

  const itemsQuery = useQuery({
    queryKey: ["items"],
    queryFn: () => api.getItems(),
  });

  const [manual, setManual] = useState<ManualForm>({
    manualItemName: "",
    quantityNeeded: "",
    unitOfMeasure: "",
  });

  const addManualMutation = useMutation({
    mutationFn: () =>
      api.addGroceryListItem(listId, {
        itemID: null,
        manualItemName: manual.manualItemName,
        quantityNeeded: Number(manual.quantityNeeded),
        unitOfMeasure: manual.unitOfMeasure,
        source: "Manual",
        isChecked: false,
      } as Omit<GroceryListItem, "groceryListItemID" | "groceryListID" | "groceryList">),
    onSuccess: () => {
      setManual({ manualItemName: "", quantityNeeded: "", unitOfMeasure: "" });
      queryClient.invalidateQueries({ queryKey: ["groceryList", listId] });
    },
  });

  const handleAddManual = () => {
    if (manual.manualItemName.trim() !== "" && manual.quantityNeeded !== "") {
      addManualMutation.mutate();
    }
  };

  if (listQuery.isLoading) return <CircularProgress />;
  if (listQuery.error)
    return <Alert severity="error">{(listQuery.error as Error).message}</Alert>;
  if (!listQuery.data) return <Alert severity="warning">Grocery list not found</Alert>;

  const list = listQuery.data;
  const grouped = (list.groceryListItems ?? []).reduce(
    (acc, item) => {
      const source = item.source || "Manual";
      if (!acc[source]) acc[source] = [];
      acc[source].push(item);
      return acc;
    },
    {} as Record<string, GroceryListItem[]>
  );

  return (
    <Box>
      <Paper sx={{ p: 3, mb: 3 }}>
        <Typography variant="h4" gutterBottom>
          Grocery List
        </Typography>
        <Typography variant="body1" color="text.secondary" gutterBottom>
          Generated {list.generatedDate?.split("T")[0]}
          {list.mealPlanID ? ` for Meal Plan ${list.mealPlanID}` : ""}
        </Typography>
      </Paper>

      <Paper sx={{ p: 3, mb: 3 }}>
        <Typography variant="h5" gutterBottom>
          Add Manual Item
        </Typography>
        <Box sx={{ display: "flex", gap: 2, flexWrap: "wrap", mb: 2 }}>
          <TextField
            size="small"
            label="Item Name"
            value={manual.manualItemName}
            onChange={(e) =>
              setManual((m) => ({ ...m, manualItemName: e.target.value }))
            }
          />
          <TextField
            size="small"
            label="Qty"
            type="number"
            value={manual.quantityNeeded}
            onChange={(e) =>
              setManual((m) => ({ ...m, quantityNeeded: e.target.value }))
            }
          />
          <TextField
            size="small"
            label="Unit"
            value={manual.unitOfMeasure}
            onChange={(e) =>
              setManual((m) => ({ ...m, unitOfMeasure: e.target.value }))
            }
          />
          <Button
            variant="contained"
            onClick={handleAddManual}
            disabled={
              manual.manualItemName.trim() === "" || manual.quantityNeeded === ""
            }
          >
            Add
          </Button>
        </Box>
        {addManualMutation.error && (
          <Alert severity="error">
            {(addManualMutation.error as Error).message}
          </Alert>
        )}
      </Paper>

      {SOURCE_ORDER.map((source) => {
        const items = grouped[source] ?? [];
        if (items.length === 0) return null;
        return (
          <Paper key={source} sx={{ p: 3, mb: 3 }}>
            <Typography variant="h5" gutterBottom>
              {SOURCE_LABELS[source] ?? source}
            </Typography>
            {items.map((item) => (
              <ItemRow
                key={item.groceryListItemID}
                item={item}
                items={itemsQuery.data}
                listId={listId}
              />
            ))}
          </Paper>
        );
      })}
    </Box>
  );
}
