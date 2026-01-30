import {
  createContext,
  useCallback,
  useContext,
  useState,
  type ReactNode,
} from "react";
import * as authApi from "@/api/auth";

const TOKEN_KEY = "wrenly_access_token";

interface AuthContextValue {
  token: string | null;
  isAuthenticated: boolean;

  login: (email: string, password: string) => Promise<void>;
  signup: (
    displayName: string,
    email: string,
    password: string,
  ) => Promise<void>;
  logout: () => void;

  /** Redireciona para o backend OAuth Google (signin-Google). */
  loginWithGoogle: () => void;
  /** Finaliza cadastro social (complete-profile) e armazena o token. */
  finalizeSocialProfile: (
    email: string,
    provider: string,
    providerKey: string,
    displayName: string,
  ) => Promise<void>;
  /** Armazena token vindo da URL (login-success?token=...). */
  setTokenFromCallback: (token: string) => void;
}

const AuthContext = createContext<AuthContextValue | null>(null);

function loadToken(): string | null {
  return localStorage.getItem(TOKEN_KEY);
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [token, setToken] = useState<string | null>(loadToken);

  const login = useCallback(async (email: string, password: string) => {
    const res = await authApi.login(email, password);
    localStorage.setItem(TOKEN_KEY, res.accessToken);
    setToken(res.accessToken);
  }, []);

  const signup = useCallback(
    async (displayName: string, email: string, password: string) => {
      await authApi.signup(displayName, email, password);
    },
    [],
  );

  const logout = useCallback(() => {
    localStorage.removeItem(TOKEN_KEY);
    setToken(null);
  }, []);

  const loginWithGoogle = useCallback(() => {
    const base = typeof window !== "undefined" ? window.location.origin : "";
    window.location.href = `${base}${authApi.getGoogleSignInUrl()}`;
  }, []);

  const finalizeSocialProfile = useCallback(
    async (
      email: string,
      provider: string,
      providerKey: string,
      displayName: string,
    ) => {
      const res = await authApi.finalizeSocialRegister(
        email,
        provider,
        providerKey,
        displayName,
      );
      localStorage.setItem(TOKEN_KEY, res.token);
      setToken(res.token);
    },
    [],
  );

  const setTokenFromCallback = useCallback((t: string) => {
    localStorage.setItem(TOKEN_KEY, t);
    setToken(t);
  }, []);

  const value: AuthContextValue = {
    token,
    isAuthenticated: !!token,
    login,
    signup,
    logout,
    loginWithGoogle,
    finalizeSocialProfile,
    setTokenFromCallback,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}
