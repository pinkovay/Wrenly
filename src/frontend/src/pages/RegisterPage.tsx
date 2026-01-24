import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { AuthLayout } from '../components/AuthLayout';
import { FormField } from '../components/FormField';

export function RegisterPage() {
  const { signup } = useAuth();
  const navigate = useNavigate();
  const [displayName, setDisplayName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);
    setLoading(true);
    try {
      await signup(displayName, email, password);
      navigate('/login', { replace: true });
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao criar conta.');
    } finally {
      setLoading(false);
    }
  }

  return (
    <AuthLayout title="Criar conta">
      <form onSubmit={handleSubmit} className="auth-form">
        {error && (
          <div className="auth-form__error" role="alert">
            {error}
          </div>
        )}
        <FormField
          id="register-displayName"
          label="Nome de exibição"
          type="text"
          placeholder="Digite seu nome de exibição"
          autoComplete="name"
          value={displayName}
          minLength={3}
          onChange={(e) => setDisplayName(e.target.value)}
          required
        />
        <FormField
          id="register-email"
          label="Email"
          type="email"
          placeholder="Digite seu email"
          autoComplete="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <FormField
          id="register-password"
          label="Senha"
          type="password"
          placeholder="Digite sua senha"
          autoComplete="new-password"
          minLength={8}
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <p className="auth-form__hint">
          Mín. 8 caracteres, maiúsculas, minúsculas, números e especiais.
        </p>
        <button
          type="submit"
          className="auth-form__submit"
          disabled={loading}
        >
          {loading ? 'Criando conta…' : 'Criar conta'}
        </button>
      </form>
    </AuthLayout>
  );
}
