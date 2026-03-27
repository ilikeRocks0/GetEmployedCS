"use client";

import { App, Layout, Typography } from "antd";
import SiteHeader from "@/components/SiteHeader";
import { FiltersProvider } from "@/context/FiltersContext";
import { JobsProvider } from "@/context/JobsContext";
import { FilterOptions } from "@/components/FilterOptions";
import { LanguageProvider } from "@/context/LanguageContext";
import { JobContainer } from "@/components/JobContainer";

const { Title } = Typography;
const { Content } = Layout

function JobsPageContent() {
  return (
    <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
      <SiteHeader selectedKey="search-jobs" />
      <Content style={{ padding: "40px 80px" }}>
        <div style={{ marginBottom: 24 }}>
          <Title level={1} style={{ margin: 0 }}>Search Jobs</Title>
        </div>
        <FilterOptions/>
        <JobContainer/>
      </Content>
    </Layout>
  );
}

export default function JobsPage() {
  return (

    <App>
      <FiltersProvider>
        <JobsProvider>
          <LanguageProvider>
            <JobsPageContent/>
          </LanguageProvider>
        </JobsProvider>
      </FiltersProvider>
    </App>
  );
}
