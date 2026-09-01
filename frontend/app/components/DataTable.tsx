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
import FormControl from "@mui/material/FormControl";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import Select from "@mui/material/Select";
import { ReactNode } from "react";

interface DataTableProps<T extends object> {
  title: string;
  rows: T[];
  isLoading: boolean;
  error: Error | null;
  onCreate: () => void;
  onEdit: (row: T) => void;
  onDelete: (row: T) => void;
  extraActions?: (row: T) => ReactNode;
  page?: number;
  pageSize?: number;
  totalCount?: number;
  onPageChange?: (page: number) => void;
  onPageSizeChange?: (pageSize: number) => void;
  pagination?: {
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
    onPageChange: (page: number) => void;
    onPageSizeChange: (size: number) => void;
  };
}

export default function DataTable<T extends object>({
  title,
  rows,
  isLoading,
  error,
  onCreate,
  onEdit,
  onDelete,
  extraActions,
  page,
  pageSize,
  totalCount,
  onPageChange,
  onPageSizeChange,
  pagination,
}: DataTableProps<T>) {
  const columns = rows.length > 0 ? Object.keys(rows[0]) : [];

  const paginationData =
    pagination ??
    (page !== undefined && pageSize !== undefined && totalCount !== undefined && onPageChange && onPageSizeChange
      ? {
          pageNumber: page,
          pageSize,
          totalCount,
          totalPages: Math.max(1, Math.ceil(totalCount / pageSize)),
          onPageChange,
          onPageSizeChange,
        }
      : null);

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
                    const value = (row as Record<string, unknown>)[col];
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
      {paginationData && (
        <Box sx={{ display: "flex", justifyContent: "flex-end", mt: 1 }}>
          <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
            <FormControl size="small" sx={{ minWidth: 120 }}>
              <InputLabel id="rows-per-page-label">Rows per page</InputLabel>
              <Select
                labelId="rows-per-page-label"
                value={paginationData.pageSize}
                label="Rows per page"
                onChange={(e) => paginationData.onPageSizeChange(Number(e.target.value))}
              >
                <MenuItem value={10}>10</MenuItem>
                <MenuItem value={25}>25</MenuItem>
                <MenuItem value={50}>50</MenuItem>
                <MenuItem value={100}>100</MenuItem>
              </Select>
            </FormControl>
            <Typography variant="body2" color="text.secondary">
              Page {paginationData.pageNumber} of {Math.max(paginationData.totalPages, 1)} ({paginationData.totalCount} total)
            </Typography>
            <Button
              variant="outlined"
              size="small"
              onClick={() => paginationData.onPageChange(paginationData.pageNumber - 1)}
              disabled={paginationData.pageNumber <= 1}
            >
              &lt;
            </Button>
            <Button
              variant="outlined"
              size="small"
              onClick={() => paginationData.onPageChange(paginationData.pageNumber + 1)}
              disabled={paginationData.pageNumber >= Math.max(paginationData.totalPages, 1)}
            >
              &gt;
            </Button>
          </Box>
        </Box>
      )}
    </Box>
  );
}
