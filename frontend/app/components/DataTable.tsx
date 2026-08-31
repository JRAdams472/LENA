"use client";

import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import CircularProgress from "@mui/material/CircularProgress";
import Alert from "@mui/material/Alert";
import Paper from "@mui/material/Paper";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Typography from "@mui/material/Typography";
import IconButton from "@mui/material/IconButton";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import { ReactNode } from "react";

interface DataTableProps {
  title: string;
  rows: any[];
  isLoading: boolean;
  error: Error | null;
  onCreate: () => void;
  onEdit: (row: any) => void;
  onDelete: (row: any) => void;
  extraActions?: (row: any) => ReactNode;
}

export default function DataTable({
  title,
  rows,
  isLoading,
  error,
  onCreate,
  onEdit,
  onDelete,
  extraActions,
}: DataTableProps) {
  const columns = rows.length > 0 ? Object.keys(rows[0]) : [];

  return (
    <Box>
      <Box
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          mb: 2,
        }}
      >
        <Typography variant="h4" gutterBottom>
          {title}
        </Typography>
        <Button variant="contained" onClick={onCreate}>
          Create
        </Button>
      </Box>

      {isLoading && <CircularProgress />}
      {error && <Alert severity="error">{error.message}</Alert>}
      {!isLoading && !error && rows.length === 0 && (
        <Typography color="text.secondary">No data</Typography>
      )}
      {!isLoading && !error && rows.length > 0 && (
        <TableContainer component={Paper}>
          <Table size="small">
            <TableHead>
              <TableRow>
                {columns.map((col) => (
                  <TableCell key={col}>{col}</TableCell>
                ))}
                <TableCell>Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {rows.map((row, i) => (
                <TableRow key={i}>
                  {columns.map((col) => {
                    const value = row[col];
                    return (
                      <TableCell key={col}>
                        {value === null || value === undefined
                          ? ""
                          : typeof value === "object"
                          ? JSON.stringify(value)
                          : String(value)}
                      </TableCell>
                    );
                  })}
                  <TableCell>
                    <IconButton onClick={() => onEdit(row)} size="small">
                      <EditIcon />
                    </IconButton>
                    <IconButton onClick={() => onDelete(row)} size="small">
                      <DeleteIcon />
                    </IconButton>
                    {extraActions?.(row)}
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      )}
    </Box>
  );
}
