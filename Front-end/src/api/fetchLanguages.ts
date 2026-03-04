import { API_BASE_URL } from "@/config/config";

export async function fetchLanguages(): Promise<string[]> {
  const res = await fetch(`${API_BASE_URL}/api/jobs/languages`);
  if (!res.ok) throw new Error(`Failed to fetch languages: ${res.status}`);
  return res.json();
}