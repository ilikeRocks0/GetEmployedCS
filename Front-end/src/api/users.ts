import { API_BASE_URL } from "@/config/config";

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
    body: JSON.stringify(payload),
  });
  if (!res.ok) throw new Error(`Registration failed: ${res.status}`);
}
