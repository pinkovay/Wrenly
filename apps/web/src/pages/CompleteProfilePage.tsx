import { useState, useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { useAuth } from "@/contexts/AuthContext";
import { AuthLayout } from "@/components/AuthLayout";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

export function CompleteProfilePage() {
  const { finalizeSocialProfile } = useAuth();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const email = searchParams.get("email") ?? "";
  const provider = searchParams.get("provider") ?? "";
  const key = searchParams.get("key") ?? "";

  const [displayName, setDisplayName] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (!email || !provider || !key) {
      navigate("/login", { replace: true });
    }
  }, [email, provider, key, navigate]);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);
    setLoading(true);
    try {
      await finalizeSocialProfile(email, provider, key, displayName);
      navigate("/", { replace: true });
    } catch (err) {
      setError(err instanceof Error ? err.message : "Erro ao finalizar cadastro.");
    } finally {
      setLoading(false);
    }
  }

  if (!email || !provider || !key) {
    return null;
  }

  return (
    <AuthLayout title="Finalizar cadastro">
      <form onSubmit={handleSubmit} className="space-y-4">
        <p className="text-sm text-muted-foreground">
          Você entrou com Google. Escolha um nome de exibição para concluir.
        </p>
        {error && (
          <div
            role="alert"
            className="rounded-md border border-destructive/50 bg-destructive/10 px-3 py-2 text-sm text-destructive"
          >
            {error}
          </div>
        )}
        <div className="space-y-2">
          <Label htmlFor="complete-email">Email</Label>
          <Input
            id="complete-email"
            type="email"
            value={email}
            readOnly
            className="bg-muted/50"
          />
        </div>
        <div className="space-y-2">
          <Label htmlFor="complete-username">Username</Label>
          <Input
            id="complete-username"
            type="text"
            autoComplete="username"
            placeholder="Como você quer ser chamado?"
            value={displayName}
            minLength={3}
            maxLength={50}
            onChange={(e) => setDisplayName(e.target.value)}
            required
          />
          <p className="text-xs text-muted-foreground">
            Entre 3 e 50 caracteres.
          </p>
        </div>
        <Button type="submit" className="w-full" disabled={loading}>
          {loading ? "Salvando…" : "Continuar"}
        </Button>
      </form>
    </AuthLayout>
  );
}
