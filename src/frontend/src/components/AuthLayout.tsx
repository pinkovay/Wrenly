import { Link, useLocation } from 'react-router-dom';
import type { ReactNode } from 'react';

interface AuthLayoutProps {
  title: string;
  children: ReactNode;
}

export function AuthLayout({ title, children }: AuthLayoutProps) {
  const { pathname } = useLocation();
  const isLogin = pathname === '/login';

  return (
    <div className="auth-layout">
      <div className="auth-layout__card">
        <div className="auth-layout__brand">
          <span className="auth-layout__logo">W</span>
          <h1 className="auth-layout__title">Wrenly</h1>
        </div>
        <h2 className="auth-layout__heading">{title}</h2>
        {children}
        <p className="auth-layout__switch">
          {isLogin ? (
            <>
              Não tem conta?{' '}
              <Link to="/register" className="auth-layout__link">
                Criar conta
              </Link>
            </>
          ) : (
            <>
              Já tem conta?{' '}
              <Link to="/login" className="auth-layout__link">
                Entrar
              </Link>
            </>
          )}
        </p>
      </div>
    </div>
  );
}
