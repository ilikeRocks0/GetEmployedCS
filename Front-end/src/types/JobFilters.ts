export interface JobFilters {
  searchText: string;
  selectedType: string | null;
  selectedLanguages: string[];
}

export const DEFAULT_FILTERS: JobFilters = {
  searchText: "",
  selectedType: null,
  selectedLanguages: [],
};
