"use client";

import { App, Layout } from "antd";
import { FiltersProvider } from "@/context/FiltersContext";
import { SavedJobsProvider } from "@/context/SavedJobsContext";
import { Typography } from "antd";
import SiteHeader from "@/components/SiteHeader";
import { FilterOptions } from "@/components/FilterOptions";
import { JobContainer } from "@/components/JobContainer";
import { LanguageProvider } from "@/context/LanguageContext";

const { Title } = Typography;
const { Content } = Layout

function SaveJobsPageContent() {
  return (
    <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
      <SiteHeader selectedKey="search-jobs" />
      <Content style={{ padding: "40px 80px" }}>
        <Title level={1}>Saved Jobs</Title>
        <FilterOptions/>
        <JobContainer/>
      </Content>
    </Layout>
  );
}

export default function SaveJobPage() {
  return (
    <App>
      <FiltersProvider>
        <SavedJobsProvider>
          <LanguageProvider>
            <SaveJobsPageContent />
          </LanguageProvider>
        </SavedJobsProvider>
      </FiltersProvider>
    </App>
  );
}