import { API_BASE_URL } from "@/config/config";
import { fetchWithAuth } from "@/utils/fetchWithAuth";
import { User } from "@/types/User";

export interface UserSearchParams {
  searchTerm?: string;
  employer?: boolean;
  pageNumber?: number;
}

export async function searchUsers(params: UserSearchParams, signal?: AbortSignal): Promise<User[]> {
  const query = new URLSearchParams();
  if (params.searchTerm) query.set("searchTerm", params.searchTerm);
  if (params.employer !== undefined) query.set("employer", String(params.employer));
  if (params.pageNumber) query.set("pageNumber", String(params.pageNumber));

  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/search?${query.toString()}`, { signal });
  if (!res.ok) throw new Error(`Failed to search users: ${res.status}`);
  return res.json();
}
