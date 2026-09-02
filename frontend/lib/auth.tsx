"use client";

import {
  createContext,
  useContext,
  useEffect,
  useState,
  useCallback,
  ReactNode,
} from "react";

interface GoogleJwtPayload {
  email?: string;
  sub?: string;
}

interface AuthUser {
  email: string;
  idToken: string;
}

interface AuthContextValue {
  user: AuthUser | null;
  signIn: (credential: string) => void;
  signOut: () => void;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

function decodeJwtPayload(token: string): GoogleJwtPayload | null {
  try {
    const payload = token.split(".")[1];
    const base64 = payload.replace(/-/g, "+").replace(/_/g, "/");
    const padding = "=".repeat((4 - (base64.length % 4)) % 4);
    const json = atob(base64 + padding);
    return JSON.parse(json) as GoogleJwtPayload;
  } catch {
    return null;
  }
}

function getStoredUser(): AuthUser | null {
  if (typeof window === "undefined") return null;
  const token = localStorage.getItem("lena_id_token");
  if (!token) return null;
  const payload = decodeJwtPayload(token);
  if (!payload?.email) {
    localStorage.removeItem("lena_id_token");
    return null;
  }
  return { idToken: token, email: payload.email };
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<AuthUser | null>(null);

  useEffect(() => {
    setUser(getStoredUser());
  }, []);

  const signIn = useCallback((credential: string) => {
    localStorage.setItem("lena_id_token", credential);
    const payload = decodeJwtPayload(credential);
    if (payload?.email) {
      setUser({ idToken: credential, email: payload.email });
    }
  }, []);

  const signOut = useCallback(() => {
    localStorage.removeItem("lena_id_token");
    setUser(null);
  }, []);

  return (
    <AuthContext.Provider value={{ user, signIn, signOut }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth(): AuthContextValue {
  const value = useContext(AuthContext);
  if (!value) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return value;
}
