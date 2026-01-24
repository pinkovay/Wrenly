import { Link } from 'react-router-dom';
import { useAuth } from '@/contexts/AuthContext';
import { Button } from '@/components/ui/button';
import { ThemeToggle } from '@/components/ThemeToggle';

export function HomePage() {
  const { isAuthenticated, logout } = useAuth();

  return (
    <div className="min-h-svh bg-transparent">
      <header className="flex items-center justify-between border-b border-border px-4 py-3">
        <span className="text-lg font-semibold">Wrenly</span>
        <div className="flex items-center gap-2">
          <ThemeToggle />
        {isAuthenticated ? (
          <Button variant="outline" size="sm" onClick={logout}>
            Sair
          </Button>
        ) : (
          <nav className="flex gap-3">
            <Button variant="ghost" asChild>
              <Link to="/login">Entrar</Link>
            </Button>
            <Button asChild>
              <Link to="/register">Criar conta</Link>
            </Button>
          </nav>
        )}
        </div>
      </header>
      <main className="mx-auto max-w-2xl px-4 py-16 text-center">
        {isAuthenticated ? (
          <h1 className="text-2xl font-bold">Olá, você está conectado.</h1>
        ) : (
          <>
            <h1 className="text-2xl font-bold">Bem-vindo ao Wrenly</h1>
            <p className="mt-2 text-muted-foreground">
              <Button variant="link" className="p-0 h-auto" asChild>
                <Link to="/login">Entrar</Link>
              </Button>
              {' ou '}
              <Button variant="link" className="p-0 h-auto" asChild>
                <Link to="/register">criar conta</Link>
              </Button>
              {' para continuar.'}
            </p>
          </>
        )}
      </main>
    </div>
  );
}
