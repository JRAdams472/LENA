"use client";

import { useQuery } from "@tanstack/react-query";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import Alert from "@mui/material/Alert";
import Paper from "@mui/material/Paper";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";

export default function ItemsPage() {
  const apiUrl =
    process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";

  const {
    data: items,
    isLoading,
    error,
  } = useQuery<any[]>({
    queryKey: ["items"],
    queryFn: async () => {
      const res = await fetch(`${apiUrl}/api/Item/items`);
      if (!res.ok) throw new Error("Failed to fetch items");
      return res.json();
    },
  });

  const columns = items && items.length > 0 ? Object.keys(items[0]) : [];

  return (
    <Box>
      <Typography variant="h4" gutterBottom>
        Items
      </Typography>
      <Paper sx={{ p: 2 }}>
        {isLoading && <CircularProgress />}
        {error && (
          <Alert severity="error">{(error as Error).message}</Alert>
        )}
        {items && (
          <TableContainer>
            <Table size="small">
              <TableHead>
                <TableRow>
                  {columns.map((col) => (
                    <TableCell key={col}>{col}</TableCell>
                  ))}
                </TableRow>
              </TableHead>
              <TableBody>
                {items.map((row, i) => (
                  <TableRow key={i}>
                    {columns.map((col) => (
                      <TableCell key={col}>
                        {String(row[col] ?? "")}
                      </TableCell>
                    ))}
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        )}
      </Paper>
    </Box>
  );
}
