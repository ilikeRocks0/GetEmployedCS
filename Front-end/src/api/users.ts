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

export async function saveJob(jobId: number): Promise<void> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/save?JobId=${jobId}`, { method: "POST" });
  if (!res.ok) throw new Error(`Failed to save job: ${res.status}`);
}

export async function unsaveJob(jobId: number): Promise<void> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/unsave?JobId=${jobId}`, { method: "POST" });
  if (!res.ok) throw new Error(`Failed to unsave job: ${res.status}`);
}

export async function checkIfUserIsEmployer(): Promise<boolean> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/check-employer`, { method: "GET" });
  if (!res.ok) throw new Error(`Failed to check employer status: ${res.status}`);
  return res.json();
}
