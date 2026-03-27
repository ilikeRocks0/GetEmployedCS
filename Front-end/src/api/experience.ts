import { API_BASE_URL } from "@/config/config";
import { fetchWithAuth } from "@/utils/fetchWithAuth";
import { ExperienceValues } from "@/types/Experience";

export async function createExperience(payload: ExperienceValues): Promise<number> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/experiences`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });
  if (!res.ok) throw new Error(`Failed to create experience: ${res.status}`);
  return res.json();
}

export async function updateExperience(oldExperience: ExperienceValues, newExperience: ExperienceValues): Promise<void> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/experiences`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ oldExperience, newExperience }),
  });
  if (!res.ok) throw new Error(`Failed to update experience: ${res.status}`);
}

export async function deleteExperience(experience: ExperienceValues): Promise<void> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/users/experiences`, {
    method: "DELETE",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(experience),
  });
  if (!res.ok) throw new Error(`Failed to delete experience: ${res.status}`);
}
