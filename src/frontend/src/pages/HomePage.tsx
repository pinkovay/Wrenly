import { Link } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

export function HomePage() {
  const { isAuthenticated, logout } = useAuth();

  return (
    <div className="home">
      
      <header className="home__header">
        <span className="home__brand">Wrenly</span>
        {isAuthenticated ? (
          <button type="button" onClick={logout} className="home__btn">
            Sair
          </button>
        ) : (
          <nav className="home__nav">
            <Link to="/login" className="home__link">Entrar</Link>
            <Link to="/register" className="home__link home__link--primary">
              Criar conta
            </Link>
          </nav>
        )}
      </header>

      <main className="home__main">
        {isAuthenticated ? (
          <h1 className="home__title">Olá, você está conectado.</h1>
        ) : (
          <>
            <h1 className="home__title">Bem-vindo ao Wrenly</h1>
            <p className="home__subtitle">
              <Link to="/login" className="home__link">Entrar</Link>
              {' ou '}
              <Link to="/register" className="home__link">criar conta</Link>
              {' para continuar.'}
            </p>
          </>
        )}
      </main>
    </div>
  );
}
