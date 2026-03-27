import { FiltersContext } from "@/context/FiltersContext";
import { useLanguage } from "@/context/LanguageContext";
import { App, Input, Select, Space } from "antd";

const EMPLOYMENT_TYPES = ["Co-op", "Contract", "Full-time", "Internship", "Part-time"];
import { useContext, useEffect, useState } from "react";

export function FilterOptions(){
    const [languages, setLanguages] = useState<string[]>([]);
    const fetchLanguages = useLanguage();
    const {filters, update: setFilters} = useContext(FiltersContext);
    const { notification } = App.useApp();
    
    useEffect(() => {
      fetchLanguages().then(setLanguages).catch(() =>
        notification.error({ message: "Failed to load language filters. Please refresh the page." })
      );
    }, [fetchLanguages, notification]);

    return(
        <Space style={{ marginBottom: 24, flexWrap: "wrap" }} size="middle">
          <Input.Search
            placeholder="Search by company or position"
            allowClear
            style={{ width: 280 }}
            onChange={(e) => { setFilters({ ...filters, searchText: e.target.value });}}
            onSearch={(value) => { setFilters({ ...filters, searchText: value });}}
          />
          <Select
            placeholder="Job type"
            allowClear
            style={{ width: 160 }}
            options={EMPLOYMENT_TYPES.map((t) => ({ label: t, value: t }))}
            onChange={(value) => { setFilters({ ...filters, selectedType: value ?? null });}}
          />
          <Select
            mode="multiple"
            placeholder="Language / tool"
            allowClear
            style={{ minWidth: 180 }}
            options={languages.map((l) => ({ label: l, value: l }))}
            onChange={(value: string[]) => { setFilters({ ...filters, selectedLanguages: value });}}
          />
        </Space>
    )
}