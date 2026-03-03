"use client";

import { useState } from "react";
import { Card, Tag, Avatar, Typography } from "antd";
import JobDetailModal from "./JobDetailModal";
import type { Job } from "@/types/Job";

export type { Job };

const { Text, Title } = Typography;

interface JobCardState {
  job: Job;
}

const TYPE_COLORS: Record<string, string> = {
  "Full-time": "green",
  "Co-op": "blue",
  "Internship": "blue",
  Contract: "orange",
};

const AVATAR_COLORS = ["#1677ff", "#52c41a", "#fa8c16", "#eb2f96", "#722ed1"];
function avatarColor(name: string) {
  const code = name.charCodeAt(0) + (name.charCodeAt(1) ?? 0);
  return AVATAR_COLORS[code % AVATAR_COLORS.length];
}

export default function JobCard({ job }: JobCardState) {
  const [modalOpen, setModalOpen] = useState(false);

  return (
    <>
      <Card
        hoverable
        onClick={() => setModalOpen(true)}
        style={{
          width: "100%",
          marginBottom: 12,
          borderLeft: "4px solid #1677ff",
          cursor: "pointer",
        }}
        styles={{
          body: {
            display: "flex",
            alignItems: "center",
            gap: 16,
            padding: "16px 20px",
          },
        }}
      >
        <Avatar
          size={48}
          src={job.logo}
          style={{ backgroundColor: job.logo ? undefined : avatarColor(job.company), flexShrink: 0, fontWeight: 700 }}
        >
          {!job.logo && job.company.slice(0, 2).toUpperCase()}
        </Avatar>

        <div style={{ flex: 1, minWidth: 0 }}>
          <Title level={5} style={{ margin: 0, lineHeight: 1.3 }}>
            {job.company}
          </Title>
          <Text type="secondary">{job.position}</Text>
          <div style={{ marginTop: 4, display: "flex", gap: 12, flexWrap: "wrap" }}>
            {job.locations.length > 0 && (
              <Text type="secondary" style={{ fontSize: 12 }}>
                {job.locations[0]}{job.locations.length > 1 ? ` +${job.locations.length - 1}` : ""}
              </Text>
            )}
            {job.deadline && (
              <Text type="secondary" style={{ fontSize: 12 }}>
                {new Date(job.deadline).toLocaleDateString()}
              </Text>
            )}
          </div>
        </div>

        <div style={{ display: "flex", alignItems: "center", gap: 8, flexShrink: 0, flexWrap: "wrap", justifyContent: "flex-end" }}>
          {job.languages.map((lang) => <Tag key={lang}>{lang}</Tag>)}
          <Tag color={TYPE_COLORS[job.employment_type] ?? "default"}>{job.employment_type}</Tag>
          {job.isRemote && <Tag color="cyan">Remote</Tag>}
          {job.isHybrid && <Tag color="geekblue">Hybrid</Tag>}
        </div>
      </Card>

      <JobDetailModal
        job={job}
        open={modalOpen}
        onClose={() => setModalOpen(false)}
      />
    </>
  );
}
