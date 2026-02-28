"use client";

import { createContext, useContext } from "react";
import type { Job } from "@/types/Job";
import type { JobFilters } from "@/types/JobFilters";
import { fetchJobs as stubFetchJobs } from "@/stub/fetchJobsStub";

export type FetchJobs = (
  filters: JobFilters,
  page: number,
  pageSize: number,
  signal: AbortSignal
) => Promise<{ data: Job[]; total: number }>;

const JobsContext = createContext<FetchJobs>(stubFetchJobs);

export function JobsProvider({ children }: { children: React.ReactNode }) {
  return (
    <JobsContext.Provider value={stubFetchJobs}>
      {children}
    </JobsContext.Provider>
  );
}

export function useJobs(): FetchJobs {
  return useContext(JobsContext);
}
