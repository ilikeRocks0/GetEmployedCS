"use client";

import { useState, useEffect } from "react";
import { App, Layout, Input, Select, Space, Spin, Pagination } from "antd";
import JobCard from "@/components/JobCard";
import type { Job } from "@/types/Job";
import SiteHeader from "@/components/SiteHeader";
import { JOB_TYPES } from "@/data/JobsStub";
import { JobFilters, DEFAULT_FILTERS } from "@/types/JobFilters";
import { JobsProvider, useJobs } from "@/context/JobsContext";
import { PAGE_SIZE } from "@/config/config";
import { FiltersProvider, useFilters } from "@/context/FiltersContext";

const { Content } = Layout;

function JobsPageContent() {
  const { notification } = App.useApp();
  const fetchJobs = useJobs();
  const fetchFilters = useFilters();

  const [filters, setFilters] = useState<JobFilters>(DEFAULT_FILTERS);
  const [page, setPage] = useState(1);
  const [jobs, setJobs] = useState<Job[]>([]);
  const [total, setTotal] = useState(0);
  const [loading, setLoading] = useState(false);
  const [languages, setLanguages] = useState<string[]>([]);

  useEffect(() => {
    fetchFilters().then(setLanguages).catch(() =>
      notification.error({ message: "Failed to load language filters. Please refresh the page." })
    );
  }, [fetchFilters]);

  useEffect(() => {
    const controller = new AbortController();
    setLoading(true);

    const timer = setTimeout(async () => {
      try {
        const result = await fetchJobs(filters, page, PAGE_SIZE, controller.signal);
        setJobs(result.data);
        setTotal(result.total);
      } catch (err) {
        if ((err as DOMException).name !== "AbortError") throw err;
      } finally {
        setLoading(false);
      }
    }, 300);

    return () => {
      clearTimeout(timer);
      controller.abort();
    };
  }, [filters, page, fetchJobs]);

  return (
    <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
      <SiteHeader selectedKey="search-jobs" />
      <Content style={{ padding: "40px 80px" }}>
        <Space style={{ marginBottom: 24, flexWrap: "wrap" }} size="middle">
          <Input.Search
            placeholder="Search by company or position"
            allowClear
            style={{ width: 280 }}
            onChange={(e) => { setFilters({ ...filters, searchText: e.target.value }); setPage(1); }}
            onSearch={(value) => { setFilters({ ...filters, searchText: value }); setPage(1); }}
          />
          <Select
            placeholder="Job type"
            allowClear
            style={{ width: 160 }}
            options={JOB_TYPES.map((t) => ({ label: t, value: t }))}
            onChange={(value) => { setFilters({ ...filters, selectedType: value ?? null }); setPage(1); }}
          />
          <Select
            mode="multiple"
            placeholder="Language / tool"
            allowClear
            style={{ minWidth: 180 }}
            options={languages.map((l) => ({ label: l, value: l }))}
            onChange={(value: string[]) => { setFilters({ ...filters, selectedLanguages: value }); setPage(1); }}
          />
        </Space>

        <Spin spinning={loading}>
          {jobs.length === 0 ? (
            <p style={{ color: "#888" }}>No jobs match your filters.</p>
          ) : (
            jobs.map((job) => (
              <JobCard key={job.id} job={job} />
            ))
          )}
        </Spin>

        <div style={{ marginTop: 24, display: "flex", justifyContent: "flex-end" }}>
          <Pagination
            current={page}
            pageSize={PAGE_SIZE}
            total={total}
            onChange={setPage}
            showTotal={(t) => `${t} jobs`}
          />
        </div>
      </Content>
    </Layout>
  );
}

export default function JobsPage() {
  return (

    <App>
      <FiltersProvider>
        <JobsProvider>
          <JobsPageContent />
        </JobsProvider>
      </FiltersProvider>
    </App>
  );
}
