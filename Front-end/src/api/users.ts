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

export interface UpdateUserRequest {
  username?: string;
  about?: string;
  firstName?: string;
  lastName?: string;
  employerName?: string;
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

export async function fetchUser( username: string, signal?: AbortSignal ): Promise<User> {
    const res = await fetchWithAuth(`${API_BASE_URL}/api/users/${username}`, { signal });
    if (!res.ok) throw new Error(`Failed to retrieve user: ${res.status}`);
    return res.json();
}

export async function updateUser(payload: UpdateUserRequest): Promise<User> {
    const res = await fetchWithAuth(`${API_BASE_URL}/api/users`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload),
    });
    if (!res.ok) throw new Error(`Failed to update user: ${res.status}`);
    return res.json();
}

export async function saveJob(jobId: number): Promise<void> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/save?JobId=${jobId}`, { method: "POST" });
  if (!res.ok) throw new Error(`Failed to save job: ${res.status}`);
}

export async function unsaveJob(jobId: number): Promise<void> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/unsave?JobId=${jobId}`, { method: "POST" });
  if (!res.ok) throw new Error(`Failed to unsave job: ${res.status}`);
}

export async function fetchFollowing(signal?: AbortSignal): Promise<User[]> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/following`, { signal });
  if (!res.ok) throw new Error(`Failed to fetch following: ${res.status}`);
  return res.json();
}

export async function checkIfUserIsEmployer(): Promise<boolean> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/check-employer`, { method: "GET" });
  if (!res.ok) throw new Error(`Failed to check employer status: ${res.status}`);
  return res.json();
}

export async function followUser(username: string): Promise<void> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/follow/${username}`, { method: "POST" });
  if (!res.ok) throw new Error(`Failed to follow user: ${res.status}`);
}

export async function unfollowUser(username: string): Promise<void> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/unfollow/${username}`, { method: "POST" });
  if (!res.ok) throw new Error(`Failed to unfollow user: ${res.status}`);
}
