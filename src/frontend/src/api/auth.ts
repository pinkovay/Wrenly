const API_BASE = '/api';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  tokenType: string;
  accessToken: string;
  expiresIn: number;
}

export interface SignupRequest {
  displayName: string;
  email: string;
  password: string;
}

export interface SignupSuccess {
  message: string;
}

export interface SignupError {
  errors: string[];
}

export async function login(
  email: string,
  password: string
): Promise<LoginResponse> {
  const res = await fetch(`${API_BASE}/auth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password } satisfies LoginRequest),
  });
  if (!res.ok) {
    const err = await res.json().catch(() => ({}));
    throw new Error(
      Array.isArray(err?.errors) ? err.errors.join(' ') : 'Login falhou. Verifique email e senha.'
    );
  }
  return res.json() as Promise<LoginResponse>;
}

export async function signup(
  displayName: string,
  email: string,
  password: string
): Promise<SignupSuccess> {
  const res = await fetch(`${API_BASE}/auth/signup`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      displayName,
      email,
      password,
    } satisfies SignupRequest),
  });
  const data = (await res.json().catch(() => ({}))) as SignupSuccess | SignupError;
  if (!res.ok) {
    const err = data as SignupError;
    throw new Error(
      Array.isArray(err?.errors) ? err.errors.join(' ') : 'Não foi possível criar a conta.'
    );
  }
  return data as SignupSuccess;
}
