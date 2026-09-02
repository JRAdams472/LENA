"use client";

import { GoogleLogin } from "@react-oauth/google";
import Box from "@mui/material/Box";
import Paper from "@mui/material/Paper";
import Typography from "@mui/material/Typography";
import { useAuth } from "@/app/auth/AuthProvider";

export default function LoginScreen() {
  const { signIn } = useAuth();

  return (
    <Box
      sx={{
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        minHeight: "100vh",
        px: 2,
      }}
    >
      <Paper
        elevation={3}
        sx={{
          p: 4,
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          gap: 3,
          maxWidth: 400,
          width: "100%",
          textAlign: "center",
        }}
      >
        <Typography variant="h4" component="h1">
          LENA
        </Typography>
        <Typography color="text.secondary">
          Sign in to manage inventory, recipes, and meal plans.
        </Typography>
        <GoogleLogin
          onSuccess={(response) => {
            if (response.credential) {
              signIn(response.credential);
            }
          }}
          onError={() => {
            // noop
          }}
        />
      </Paper>
    </Box>
  );
}
