import ALL_JOBS from "@/data/JobsStub";
import type { Job } from "@/types/Job";
import type { JobFilters } from "@/types/JobFilters";

// Stub API: swap the body for a real fetch call when the backend is ready
export async function fetchJobs(
  filters: JobFilters,
  page: number,
  pageSize: number,
  signal: AbortSignal
): Promise<{ data: Job[]; total: number }> {
  // Simulate network delay
  await new Promise<void>((resolve, reject) => {
    const t = setTimeout(resolve, 300);
    // If the filters change before the 300ms delay finishes, the promise gets rejected.
    signal.addEventListener("abort", () => {
      clearTimeout(t);
      reject(new DOMException("Aborted", "AbortError"));
    });
  });

  const query = filters.searchText.toLowerCase();

  // Run filter on all the jobs. (Stub data for now)
  const filtered = ALL_JOBS.filter((job) => {
    const matchesSearch =
      !query ||
      job.company.toLowerCase().includes(query) ||
      job.position.toLowerCase().includes(query);
    const matchesType = !filters.selectedType || job.employment_type === filters.selectedType;
    const matchesLanguage =
      filters.selectedLanguages.length === 0 ||
      filters.selectedLanguages.some((lang) => job.languages.includes(lang));
    return matchesSearch && matchesType && matchesLanguage;
  });

  // Slice filtered data for the current page (used for pagination)
  const start = (page - 1) * pageSize;
  return {
    data: filtered.slice(start, start + pageSize),
    total: filtered.length,
  };
}
