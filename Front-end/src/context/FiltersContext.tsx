"use client";

import { createContext, useState } from "react";
import { DEFAULT_FILTERS, JobFilters } from "@/types/JobFilters";


export const FiltersContext = createContext({filters: DEFAULT_FILTERS, update: (_filters: JobFilters) => {},});

export function FiltersProvider({ children }: { children: React.ReactNode }) {
  const [filters, setFilters] = useState(DEFAULT_FILTERS)
  
  return (
    <FiltersContext.Provider value={{filters, update: setFilters}}>
      {children}
    </FiltersContext.Provider>
  );
}
