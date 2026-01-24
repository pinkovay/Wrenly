import { Link, useLocation } from 'react-router-dom';
import type { ReactNode } from 'react';
import { Card, CardContent, CardFooter, CardHeader, CardTitle} from '@/components/ui/card';
import { ThemeToggle } from '@/components/ThemeToggle';

interface AuthLayoutProps {
  title: string;
  children: ReactNode;
}

export function AuthLayout({ children }: AuthLayoutProps) {
  const { pathname } = useLocation();
  const isLogin = pathname === '/login';

  return (
    <div className="relative flex min-h-svh flex-col items-center justify-center bg-transparent p-4">
      <div className="absolute right-4 top-4">
        <ThemeToggle />
      </div>
      <Card className="w-full max-w-[350px] shadow-[0_24px_48px_rgba(0,0,0,0.4)]">
        <CardHeader className="space-y-1 text-center">
          <div className="mb-2 flex items-center justify-center gap-2">
            <span className="flex h-10 w-10 items-center justify-center rounded-lg bg-gradient-to-br from-[#6366f1] to-[#8b5cf6] text-lg font-bold text-primary-foreground">
              W
            </span>
            <CardTitle className="text-xl">Wrenly</CardTitle>
          </div>
          {/* <CardDescription>{title}</CardDescription> */}
        </CardHeader>
        <CardContent>{children}</CardContent>
        <CardFooter className="flex justify-center pt-4">
          <p className="text-center text-sm text-muted-foreground">
            {isLogin ? (
              <>
                Não tem conta?{' '}
                <Link
                  to="/register"
                  className="font-medium text-[#818cf8] underline-offset-4 hover:text-[#a5b4fc] hover:underline"
                >
                  Criar conta
                </Link>
              </>
            ) : (
              <>
                Já tem conta?{' '}
                <Link
                  to="/login"
                  className="font-medium text-[#818cf8] underline-offset-4 hover:text-[#a5b4fc] hover:underline"
                >
                  Entrar
                </Link>
              </>
            )}
          </p>
        </CardFooter>
      </Card>
    </div>
  );
}
