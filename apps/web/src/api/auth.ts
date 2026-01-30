const API_BASE = '/api';

/** Redireciona o usuário para o backend, que inicia OAuth Google e redireciona para o provedor. */
export function getGoogleSignInUrl(): string {
  return `${API_BASE}/auth/signin-Google`;
}

export interface FinalizeSocialRegisterRequest {
  email: string;
  provider: string;
  providerKey: string;
  displayName: string;
}

export interface FinalizeSocialRegisterResponse {
  requiresProfileCompletion: boolean;
  email: string;
  token: string;
}

export async function finalizeSocialRegister(
  email: string,
  provider: string,
  providerKey: string,
  displayName: string
): Promise<FinalizeSocialRegisterResponse> {
  const res = await fetch(`${API_BASE}/auth/finalize`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      email,
      provider,
      providerKey,
      displayName,
    } satisfies FinalizeSocialRegisterRequest),
  });
  const data = await res.json().catch(() => ({}));
  if (!res.ok) {
    const err = data as { errors?: string[] };
    throw new Error(
      Array.isArray(err?.errors) ? err.errors.join(' ') : 'Erro ao finalizar cadastro.'
    );
  }
  return data as FinalizeSocialRegisterResponse;
}

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
