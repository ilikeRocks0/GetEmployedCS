"use client";

import { createContext, useState } from "react";
import { DEFAULT_FILTERS, JobFilters } from "@/types/JobFilters";

interface FiltersContextType {
  filters: JobFilters;
  update: (filters: JobFilters) => void;
}

export const FiltersContext = createContext<FiltersContextType>({filters: DEFAULT_FILTERS, update: () => {}});

export function FiltersProvider({ children }: { children: React.ReactNode }) {
  const [filters, setFilters] = useState(DEFAULT_FILTERS)
  
  return (
    <FiltersContext.Provider value={{filters, update: setFilters}}>
      {children}
    </FiltersContext.Provider>
  );
}
