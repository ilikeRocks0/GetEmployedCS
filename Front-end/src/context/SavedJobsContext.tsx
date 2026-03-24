"use client";

import { fetchSavedJobs } from "@/api/fetchJobs";
import { JobsContext } from "./JobsContext";
import { IsSavedListContext } from "./IsSavedListContext";

export function SavedJobsProvider({ children }: { children: React.ReactNode }) {
  return (
    <IsSavedListContext.Provider value={true}>
      <JobsContext.Provider value={fetchSavedJobs}>
        {children}
      </JobsContext.Provider>
    </IsSavedListContext.Provider>
  );
}