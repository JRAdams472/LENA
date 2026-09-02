"use client";

import {
  createContext,
  useContext,
  useEffect,
  useState,
  useCallback,
  ReactNode,
  useRef,
} from "react";
import {
  setAuthTokenGetter,
  setOnUnauthorized,
} from "@/lib/api";

interface GoogleJwtPayload {
  email?: string;
  sub?: string;
  exp?: number;
}

export interface AuthUser {
  email: string;
  sub?: string;
}

interface AuthContextValue {
  token: string | null;
  user: AuthUser | null;
  isAuthenticated: boolean;
  signIn: (credential: string) => void;
  signOut: () => void;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

const TOKEN_KEY = "lena_id_token";

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

function isTokenExpired(token: string): boolean {
  const payload = decodeJwtPayload(token);
  if (typeof payload?.exp !== "number") return false;
  return payload.exp < Math.floor(Date.now() / 1000);
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [token, setToken] = useState<string | null>(null);
  const [user, setUser] = useState<AuthUser | null>(null);
  const tokenRef = useRef<string | null>(null);

  const signIn = useCallback((credential: string) => {
    localStorage.setItem(TOKEN_KEY, credential);
    tokenRef.current = credential;
    const payload = decodeJwtPayload(credential);
    setToken(credential);
    setUser(
      payload?.email
        ? { email: payload.email, sub: payload.sub }
        : null
    );
  }, []);

  const signOut = useCallback(() => {
    localStorage.removeItem(TOKEN_KEY);
    tokenRef.current = null;
    setToken(null);
    setUser(null);
  }, []);

  useEffect(() => {
    const stored = localStorage.getItem(TOKEN_KEY);
    if (stored) {
      if (isTokenExpired(stored)) {
        localStorage.removeItem(TOKEN_KEY);
      } else {
        signIn(stored);
      }
    }
  }, [signIn]);

  useEffect(() => {
    setAuthTokenGetter(() => tokenRef.current);
    setOnUnauthorized(signOut);
  }, [signOut]);

  useEffect(() => {
    tokenRef.current = token;
  }, [token]);

  const value: AuthContextValue = {
    token,
    user,
    isAuthenticated: !!token && !!user,
    signIn,
    signOut,
  };

  return (
    <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
  );
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
}
