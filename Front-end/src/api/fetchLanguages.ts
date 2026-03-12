import { API_BASE_URL } from "@/config/config";
import { fetchWithAuth } from "@/utils/fetchWithAuth";

export async function fetchLanguages(): Promise<string[]> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/jobs/languages`);
  if (!res.ok) throw new Error(`Failed to fetch languages: ${res.status}`);
  return res.json();
}