"use client";

import { useState, useEffect } from "react";
import { Layout, Input, Select, Space, Spin, Pagination } from "antd";
import JobCard, { Job } from "@/components/JobCard";
import SiteHeader from "@/components/SiteHeader";
import ALL_JOBS from "@/data/jobs";

const { Content } = Layout;

// 20 Jobs per page
const PAGE_SIZE = 20;

const JOB_TYPES = [...new Set(ALL_JOBS.map((j) => j.type))];
const LANGUAGES = [...new Set(ALL_JOBS.map((j) => j.language))];

// Stub API: swap the body for a real fetch call when the backend is ready
async function fetchJobs(
  params: { search: string; type: string | null; language: string | null; page: number; pageSize: number },
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

  const query = params.search.toLowerCase();

  // Run filter on all the jobs. (Stub data for now)
  const filtered = ALL_JOBS.filter((job) => {
    const matchesSearch =
      !query ||
      job.company.toLowerCase().includes(query) ||
      job.position.toLowerCase().includes(query);
    const matchesType = !params.type || job.type === params.type;
    const matchesLanguage = !params.language || job.language === params.language;
    return matchesSearch && matchesType && matchesLanguage;
  });

  // Slice filtered data for the current page (used for pagination)
  const start = (params.page - 1) * params.pageSize;
  return {
    data: filtered.slice(start, start + params.pageSize),
    total: filtered.length,
  };
}

export default function JobsPage() {
  const [searchText, setSearchText] = useState("");
  const [selectedType, setSelectedType] = useState<string | null>(null);
  const [selectedLanguage, setSelectedLanguage] = useState<string | null>(null);
  const [page, setPage] = useState(1);
  const [jobs, setJobs] = useState<Job[]>(ALL_JOBS.slice(0, PAGE_SIZE));
  const [total, setTotal] = useState(ALL_JOBS.length);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const controller = new AbortController();
    setLoading(true);

    const timer = setTimeout(async () => {
      try {
        const result = await fetchJobs(
          { search: searchText, type: selectedType, language: selectedLanguage, page, pageSize: PAGE_SIZE },
          controller.signal
        );
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
  }, [searchText, selectedType, selectedLanguage, page]);

  return (
    <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
      <SiteHeader selectedKey="search-jobs" />
      <Content style={{ padding: "40px 80px" }}>
        <Space style={{ marginBottom: 24, flexWrap: "wrap" }} size="middle">
          <Input.Search
            placeholder="Search by company or position"
            allowClear
            style={{ width: 280 }}
            onChange={(e) => { setSearchText(e.target.value); setPage(1); }}
            onSearch={(value) => { setSearchText(value); setPage(1); }}
          />
          <Select
            placeholder="Job type"
            allowClear
            style={{ width: 160 }}
            options={JOB_TYPES.map((t) => ({ label: t, value: t }))}
            onChange={(value) => { setSelectedType(value ?? null); setPage(1); }}
          />
          <Select
            placeholder="Language / tool"
            allowClear
            style={{ width: 180 }}
            options={LANGUAGES.map((l) => ({ label: l, value: l }))}
            onChange={(value) => { setSelectedLanguage(value ?? null); setPage(1); }}
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
