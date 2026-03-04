"use client";

import { createContext, useContext } from "react";
import { fetchLanguages } from "@/api/fetchJobs";

export type FetchLanguages = () => Promise<string[]>;

const FiltersContext = createContext<FetchLanguages>(fetchLanguages);

export function FiltersProvider({ children }: { children: React.ReactNode }) {
  return (
    <FiltersContext.Provider value={fetchLanguages}>
      {children}
    </FiltersContext.Provider>
  );
}

export function useFilters(): FetchLanguages {
  return useContext(FiltersContext);
}
