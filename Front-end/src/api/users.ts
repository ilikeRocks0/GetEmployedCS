import { API_BASE_URL } from "@/config/config";
import { User } from "@/types/User";
import { fetchWithAuth } from "@/utils/fetchWithAuth";

export interface RegisterUserRequest {
  username: string;
  password: string;
  firstName: string;
  lastName: string;
  employerName: string;
  isEmployer: boolean;
  email: string;
}

export async function registerUser(payload: RegisterUserRequest): Promise<void> {
  const res = await fetch(`${API_BASE_URL}/api/users`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(payload),
  });
  if (!res.ok) throw new Error(`Registration failed: ${res.status}`);
}

export async function fetchUser( userId: number, signal: AbortSignal ): Promise<User> {
    const res = await fetchWithAuth(`${API_BASE_URL}/api/users/${userId}`, { signal });
    if (!res.ok) throw new Error(`Failed to retrieve user: ${res.status}`);

    const user: User = await res.json();
    return user;
}
