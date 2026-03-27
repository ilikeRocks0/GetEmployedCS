
import { FiltersContext } from "@/context/FiltersContext";
import { JobsContext } from "@/context/JobsContext";
import { useContext, useEffect, useState } from "react";
import JobCard, { Job } from "./JobCard";
import { PAGE_SIZE } from "@/config/config";
import { Pagination, Spin } from "antd";
import { fetchSavedSublist } from "@/api/fetchJobs";
import { useIsSavedList } from "@/context/IsSavedListContext";


export function JobContainer(){
    const fetchJobs = useContext(JobsContext);
    const {filters:filters} = useContext(FiltersContext);
    const isSavedList = useIsSavedList();
    const [page, setPage] = useState(1);
    const [jobs, setJobs] = useState<Job[]>([]);
    const [total, setTotal] = useState(0);
    const [loading, setLoading] = useState(false);
    const [savedIds, setSavedIds] = useState<Set<number>>(new Set());

    useEffect(() => {
        setPage(1);
    }, [filters]);

    useEffect(() => {
        const controller = new AbortController();
        setLoading(true);

        const timer = setTimeout(async () => {
            try {
                const promises: [ReturnType<typeof fetchJobs>, Promise<Set<number>>?] = [
                    fetchJobs(filters, page, PAGE_SIZE, controller.signal),
                    !isSavedList ? fetchSavedSublist(filters, page, controller.signal) : undefined,
                ];
                const [result, sublist] = await Promise.all(promises);
                setJobs(result.data);
                setTotal(result.total);
                if (sublist) setSavedIds(sublist);
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
    }, [filters, page, fetchJobs, isSavedList]);

    return (
        <div>
            <Spin spinning={loading}>
            {jobs.length === 0 ? (
                <p style={{ color: "#888" }}>No jobs match your filters.</p>
                ) : (
                    jobs.map((job) => (
                        <JobCard key={job.id} job={job} isSaved={isSavedList || savedIds.has(job.id)} onRemove={() => setJobs(prev => prev.filter(j => j.id !== job.id))} />
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
        </div>
    )

}