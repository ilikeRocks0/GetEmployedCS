"use client";

import { createContext, useContext } from "react";
import { fetchLanguages } from "@/api/fetchJobs";

export type FetchLanguages = () => Promise<string[]>;

const LanguageContext = createContext<FetchLanguages>(fetchLanguages);

export function LanguageProvider({ children }: { children: React.ReactNode }) {
  return (
    <LanguageContext.Provider value={fetchLanguages}>
      {children}
    </LanguageContext.Provider>
  );
}

export function useLanguage(): FetchLanguages {
  return useContext(LanguageContext);
}
