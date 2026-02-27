"use client";

import { useState, useEffect } from "react";
import { Layout, Input, Select, Space, Spin, Pagination } from "antd";
import JobCard from "@/components/JobCard";
import type { Job } from "@/types/Job";
import SiteHeader from "@/components/SiteHeader";
import ALL_JOBS, { JOB_TYPES, LANGUAGES } from "@/data/JobsStub";
import { JobFilters, DEFAULT_FILTERS } from "@/types/JobFilters";

const { Content } = Layout;

// 20 Jobs per page
const PAGE_SIZE = 20;

// Stub API: swap the body for a real fetch call when the backend is ready
async function fetchJobs(
  filters: JobFilters,
  page: number,
  pageSize: number,
  signal: AbortSignal

  // Simulate network delay
): Promise<{ data: Job[]; total: number }> {
  await new Promise<void>((resolve, reject) => {
    const t = setTimeout(resolve, 300);
    // If the filters change before the 300ms delay finishes, the promise gets rejected.
    signal.addEventListener("abort", () => {
      clearTimeout(t);
      reject(new DOMException("Aborted", "AbortError"));
    });
  });

  const query = filters.searchText.toLowerCase();

  // Run filter on all the jobs. (Stub data for now)
  const filtered = ALL_JOBS.filter((job) => {
    const matchesSearch =
      !query ||
      job.company.toLowerCase().includes(query) ||
      job.position.toLowerCase().includes(query);
    const matchesType = !filters.selectedType || job.employment_type === filters.selectedType;
    const matchesLanguage = filters.selectedLanguages.length === 0 || filters.selectedLanguages.includes(job.language);
    return matchesSearch && matchesType && matchesLanguage;
  });

  // Slice filtered data for the current page (used for pagination)
  const start = (page - 1) * pageSize;
  return {
    data: filtered.slice(start, start + pageSize),
    total: filtered.length,
  };
}

export default function JobsPage() {
  const [filters, setFilters] = useState<JobFilters>(DEFAULT_FILTERS);
  const [page, setPage] = useState(1);
  const [jobs, setJobs] = useState<Job[]>(ALL_JOBS.slice(0, PAGE_SIZE));
  const [total, setTotal] = useState(ALL_JOBS.length);
  const [loading, setLoading] = useState(false);

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
  }, [filters, page]);

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
            options={LANGUAGES.map((l) => ({ label: l, value: l }))}
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
