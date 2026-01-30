import { useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { useAuth } from "@/contexts/AuthContext";

export function LoginSuccessPage() {
  const { setTokenFromCallback } = useAuth();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const token = searchParams.get("token");

  useEffect(() => {
    if (token) {
      setTokenFromCallback(token);
      navigate("/", { replace: true });
    } else {
      navigate("/login", { replace: true });
    }
  }, [token, setTokenFromCallback, navigate]);

  return (
    <div className="flex min-h-svh flex-col items-center justify-center gap-4 p-4">
      <p className="text-muted-foreground">Entrando…</p>
    </div>
  );
}
