"use client";

import { useQuery } from "@tanstack/react-query";
import { api } from "@/lib/api";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import Alert from "@mui/material/Alert";
import Paper from "@mui/material/Paper";

export default function Dashboard() {
  const {
    data: items,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["items"],
    queryFn: () => api.getItems(1, 1000).then((r) => r.items),
  });

  return (
    <Box>
      <Typography variant="h4" gutterBottom>
        Dashboard
      </Typography>
      <Paper sx={{ p: 2 }}>
        {isLoading && <CircularProgress />}
        {error && (
          <Alert severity="error">{(error as Error).message}</Alert>
        )}
        {items && (
          <pre>{JSON.stringify(items, null, 2)}</pre>
        )}
      </Paper>
    </Box>
  );
}
