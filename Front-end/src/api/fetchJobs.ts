import { API_BASE_URL } from "@/config/config";
import type { Job } from "@/types/Job";
import type { JobFilters } from "@/types/JobFilters";
import { mapJob, ApiJob } from "@/utils/ApiJobMapper";
import { fetchWithAuth } from "@/utils/fetchWithAuth";

function buildParams(filters: JobFilters, page?: number): URLSearchParams {
  const params = new URLSearchParams();
  if (filters.searchText) params.set("searchTerm", filters.searchText);
  if (filters.selectedType) params.set("employmentTypes", filters.selectedType);
  filters.selectedLanguages.forEach((lang) => params.append("languages", lang));
  if (page != null) params.set("pageNumber", String(page));
  return params;
}

export async function fetchLanguages(): Promise<string[]> {
  const res = await fetchWithAuth(`${API_BASE_URL}/api/jobs/languages`);
  if (!res.ok) throw new Error(`Failed to fetch languages: ${res.status}`);
  return res.json();
}

export async function fetchJobs(
  filters: JobFilters,
  page: number,
  _pageSize: number,
  signal: AbortSignal
): Promise<{ data: Job[]; total: number }> {
  const params = buildParams(filters, page);
  const countParams = buildParams(filters);

  const [jobsRes, countRes] = await Promise.all([
    fetchWithAuth(`${API_BASE_URL}/api/jobs?${params}`, { signal }),
    fetchWithAuth(`${API_BASE_URL}/api/jobs/number?${countParams}`, { signal }),
  ]);

  if (!jobsRes.ok) throw new Error(`Failed to fetch jobs: ${jobsRes.status}`);
  if (!countRes.ok) throw new Error(`Failed to fetch job count: ${countRes.status}`);

  const apiJobs: ApiJob[] = await jobsRes.json();
  const total: number = await countRes.json();

  return { data: apiJobs.map(mapJob), total };
}

export async function deleteJob(id: number) {
  const res =  await fetchWithAuth(`${API_BASE_URL}/api/jobs/${id}`, {
    method: "DELETE",
  });
  if (!res.ok) throw new Error(`Failed to fetch jobs: ${res.status}`);
}

export async function fetchSavedSublist(filters: JobFilters, page: number, signal: AbortSignal): Promise<Set<number>> {
  const params = buildParams(filters, page);
  const res = await fetchWithAuth(`${API_BASE_URL}/api/jobs/saved/sublist?${params}`, { signal });
  if (!res.ok) throw new Error(`Failed to fetch saved sublist: ${res.status}`);
  const apiJobs: ApiJob[] = await res.json();
  return new Set(apiJobs.map((j) => mapJob(j).id));
}

export async function fetchSavedJobs(
  filters: JobFilters,
  page: number,
  _pageSize: number,
  signal: AbortSignal
): Promise<{ data: Job[]; total: number }> {
  const params = buildParams(filters, page);
  const countParams = buildParams(filters);

  const [jobsRes, countRes] = await Promise.all([
    fetchWithAuth(`${API_BASE_URL}/api/jobs/saved?${params}`, { signal }),
    fetchWithAuth(`${API_BASE_URL}/api/jobs/saved/number?${countParams}`, { signal }),
  ]);

  if (!jobsRes.ok) throw new Error(`Failed to fetch saved jobs: ${jobsRes.status}`);
  if (!countRes.ok) throw new Error(`Failed to fetch saved job count: ${countRes.status}`);

  const apiJobs: ApiJob[] = await jobsRes.json();
  const total: number = await countRes.json();

  return { data: apiJobs.map(mapJob), total };
}
