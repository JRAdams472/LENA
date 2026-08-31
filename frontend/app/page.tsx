"use client";

import { useQuery } from "@tanstack/react-query";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import Alert from "@mui/material/Alert";
import Paper from "@mui/material/Paper";

interface Item {
  itemID: number;
  name: string;
  unit: string;
}

export default function Dashboard() {
  const apiUrl =
    process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";

  const {
    data: items,
    isLoading,
    error,
  } = useQuery<Item[]>({
    queryKey: ["items"],
    queryFn: async () => {
      const res = await fetch(`${apiUrl}/api/Item/items`);
      if (!res.ok) throw new Error("Failed to fetch items");
      return res.json();
    },
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
