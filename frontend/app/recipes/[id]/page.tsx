"use client";

import { useState } from "react";
import { useParams } from "next/navigation";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import Alert from "@mui/material/Alert";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import CircularProgress from "@mui/material/CircularProgress";
import Divider from "@mui/material/Divider";
import FormControl from "@mui/material/FormControl";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import Paper from "@mui/material/Paper";
import Select from "@mui/material/Select";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TextField from "@mui/material/TextField";
import Typography from "@mui/material/Typography";
import { api } from "@/lib/api";
import { RecipeStep } from "@/lib/types";

export default function RecipeDetailPage() {
  const params = useParams<{ id: string }>();
  const recipeId = Number(params.id);
  const queryClient = useQueryClient();

  const [itemId, setItemId] = useState<number | "">("");
  const [portion, setPortion] = useState("");
  const [unit, setUnit] = useState("");

  const [stepNumber, setStepNumber] = useState("");
  const [instruction, setInstruction] = useState("");
  const [editingStepId, setEditingStepId] = useState<number | null>(null);

  const recipeQuery = useQuery({
    queryKey: ["recipe", recipeId],
    queryFn: () => api.getRecipe(recipeId),
    enabled: !isNaN(recipeId),
  });

  const itemsQuery = useQuery({
    queryKey: ["items"],
    queryFn: api.getItems,
  });

  const recipeItemsQuery = useQuery({
    queryKey: ["recipe-items", recipeId],
    queryFn: () => api.getRecipeItems(recipeId),
    enabled: !isNaN(recipeId),
  });

  const recipeStepsQuery = useQuery({
    queryKey: ["recipe-steps", recipeId],
    queryFn: () => api.getRecipeSteps(recipeId),
    enabled: !isNaN(recipeId),
  });

  const invalidateItems = () =>
    queryClient.invalidateQueries({ queryKey: ["recipe-items", recipeId] });
  const invalidateSteps = () =>
    queryClient.invalidateQueries({ queryKey: ["recipe-steps", recipeId] });

  const addItemMutation = useMutation({
    mutationFn: (payload: {
      itemId: number;
      portion: number;
      unit: string | null;
    }) => api.addRecipeItem(recipeId, payload),
    onSuccess: () => {
      setItemId("");
      setPortion("");
      setUnit("");
      return invalidateItems();
    },
  });

  const removeItemMutation = useMutation({
    mutationFn: (id: number) => api.removeRecipeItem(recipeId, id),
    onSuccess: invalidateItems,
  });

  const addStepMutation = useMutation({
    mutationFn: (payload: { stepNumber: number; instruction: string }) =>
      api.addRecipeStep(recipeId, payload),
    onSuccess: () => {
      resetStepForm();
      return invalidateSteps();
    },
  });

  const updateStepMutation = useMutation({
    mutationFn: ({
      stepId,
      ...payload
    }: {
      stepId: number;
      stepNumber: number;
      instruction: string;
    }) => api.updateRecipeStep(recipeId, stepId, payload),
    onSuccess: () => {
      resetStepForm();
      return invalidateSteps();
    },
  });

  const deleteStepMutation = useMutation({
    mutationFn: (stepId: number) => api.deleteRecipeStep(recipeId, stepId),
    onSuccess: invalidateSteps,
  });

  const resetStepForm = () => {
    setEditingStepId(null);
    setStepNumber("");
    setInstruction("");
  };

  const handleAddItem = () => {
    if (itemId === "" || portion === "") return;
    addItemMutation.mutate({
      itemId: Number(itemId),
      portion: Number(portion),
      unit: unit === "" ? null : unit,
    });
  };

  const handleSaveStep = () => {
    if (stepNumber === "" || instruction.trim() === "") return;
    if (editingStepId === null) {
      addStepMutation.mutate({
        stepNumber: Number(stepNumber),
        instruction,
      });
    } else {
      updateStepMutation.mutate({
        stepId: editingStepId,
        stepNumber: Number(stepNumber),
        instruction,
      });
    }
  };

  const handleEditStep = (step: RecipeStep) => {
    setEditingStepId(step.recipeStepID);
    setStepNumber(String(step.stepNumber));
    setInstruction(step.instruction);
  };

  const handleDeleteStep = (step: RecipeStep) => {
    if (window.confirm("Delete this step?")) {
      deleteStepMutation.mutate(step.recipeStepID);
    }
  };

  const itemName = (id: number) =>
    itemsQuery.data?.find((item) => item.itemID === id)?.name ?? String(id);

  const sortedSteps = [...(recipeStepsQuery.data ?? [])].sort(
    (a, b) => a.stepNumber - b.stepNumber
  );

  if (isNaN(recipeId)) {
    return <Alert severity="error">Invalid recipe id</Alert>;
  }

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Paper sx={{ p: 3 }}>
        {recipeQuery.isLoading && <CircularProgress />}
        {recipeQuery.error && (
          <Alert severity="error">
            {(recipeQuery.error as Error).message}
          </Alert>
        )}
        {recipeQuery.data && (
          <>
            <Typography variant="h4" gutterBottom>
              {recipeQuery.data.recipeName}
            </Typography>
            <Typography color="text.secondary" gutterBottom>
              {recipeQuery.data.description ?? "No description"}
            </Typography>
            <Typography variant="body2">
              Servings: {recipeQuery.data.servings ?? "-"}
            </Typography>
          </>
        )}
      </Paper>

      <Paper sx={{ p: 3 }}>
        <Typography variant="h5" gutterBottom>
          Ingredients
        </Typography>

        <Box sx={{ display: "flex", gap: 2, flexWrap: "wrap", mb: 2 }}>
          <FormControl size="small" sx={{ minWidth: 220 }}>
            <InputLabel id="recipe-item-label">Item</InputLabel>
            <Select
              labelId="recipe-item-label"
              label="Item"
              value={itemId === "" ? "" : String(itemId)}
              onChange={(e) =>
                setItemId(e.target.value === "" ? "" : Number(e.target.value))
              }
            >
              {(itemsQuery.data ?? []).map((item) => (
                <MenuItem key={item.itemID} value={String(item.itemID)}>
                  {item.name}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          <TextField
            size="small"
            label="Portion"
            type="number"
            value={portion}
            onChange={(e) => setPortion(e.target.value)}
          />
          <TextField
            size="small"
            label="Unit"
            value={unit}
            onChange={(e) => setUnit(e.target.value)}
          />
          <Button
            variant="contained"
            onClick={handleAddItem}
            disabled={itemId === "" || portion === ""}
          >
            Add Item
          </Button>
        </Box>

        {addItemMutation.error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {(addItemMutation.error as Error).message}
          </Alert>
        )}
        {removeItemMutation.error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {(removeItemMutation.error as Error).message}
          </Alert>
        )}

        {recipeItemsQuery.isLoading && <CircularProgress />}
        {recipeItemsQuery.error && (
          <Alert severity="error">
            {(recipeItemsQuery.error as Error).message}
          </Alert>
        )}
        {!recipeItemsQuery.isLoading &&
          !recipeItemsQuery.error &&
          (recipeItemsQuery.data ?? []).length === 0 && (
            <Typography color="text.secondary">No ingredients</Typography>
          )}
        {(recipeItemsQuery.data ?? []).length > 0 && (
          <TableContainer>
            <Table size="small">
              <TableHead>
                <TableRow>
                  <TableCell>Item</TableCell>
                  <TableCell>Portion</TableCell>
                  <TableCell>Unit</TableCell>
                  <TableCell>Actions</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {(recipeItemsQuery.data ?? []).map((recipeItem) => (
                  <TableRow key={recipeItem.itemID}>
                    <TableCell>{itemName(recipeItem.itemID)}</TableCell>
                    <TableCell>{recipeItem.quantity}</TableCell>
                    <TableCell>{recipeItem.unitOfMeasure ?? ""}</TableCell>
                    <TableCell>
                      <Button
                        size="small"
                        color="error"
                        onClick={() =>
                          removeItemMutation.mutate(recipeItem.itemID)
                        }
                      >
                        Remove
                      </Button>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        )}
      </Paper>

      <Paper sx={{ p: 3 }}>
        <Typography variant="h5" gutterBottom>
          Steps
        </Typography>

        <Box sx={{ display: "flex", gap: 2, flexWrap: "wrap", mb: 2 }}>
          <TextField
            size="small"
            label="Step Number"
            type="number"
            value={stepNumber}
            onChange={(e) => setStepNumber(e.target.value)}
          />
          <TextField
            size="small"
            label="Instruction"
            sx={{ flexGrow: 1, minWidth: 260 }}
            value={instruction}
            onChange={(e) => setInstruction(e.target.value)}
          />
          <Button
            variant="contained"
            onClick={handleSaveStep}
            disabled={stepNumber === "" || instruction.trim() === ""}
          >
            {editingStepId === null ? "Add Step" : "Save Step"}
          </Button>
          {editingStepId !== null && (
            <Button onClick={resetStepForm}>Cancel</Button>
          )}
        </Box>

        {addStepMutation.error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {(addStepMutation.error as Error).message}
          </Alert>
        )}
        {updateStepMutation.error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {(updateStepMutation.error as Error).message}
          </Alert>
        )}
        {deleteStepMutation.error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {(deleteStepMutation.error as Error).message}
          </Alert>
        )}

        {recipeStepsQuery.isLoading && <CircularProgress />}
        {recipeStepsQuery.error && (
          <Alert severity="error">
            {(recipeStepsQuery.error as Error).message}
          </Alert>
        )}
        {!recipeStepsQuery.isLoading &&
          !recipeStepsQuery.error &&
          sortedSteps.length === 0 && (
            <Typography color="text.secondary">No steps</Typography>
          )}
        {sortedSteps.map((step) => (
          <Box key={step.recipeStepID}>
            <Box
              sx={{
                display: "flex",
                alignItems: "center",
                gap: 2,
                py: 1,
              }}
            >
              <Typography sx={{ minWidth: 32 }}>{step.stepNumber}.</Typography>
              <Typography sx={{ flexGrow: 1 }}>{step.instruction}</Typography>
              <Button size="small" onClick={() => handleEditStep(step)}>
                Edit
              </Button>
              <Button
                size="small"
                color="error"
                onClick={() => handleDeleteStep(step)}
              >
                Delete
              </Button>
            </Box>
            <Divider />
          </Box>
        ))}
      </Paper>
    </Box>
  );
}
