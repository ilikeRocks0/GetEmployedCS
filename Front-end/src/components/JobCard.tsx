"use client";

import { useState } from "react";
import { Card, Tag, Avatar, Typography } from "antd";
import JobDetailModal from "./JobDetailModal";

const { Text, Title } = Typography;

//the fields of the Job object need some more definition, added what I felt was needed for now.
export interface Job {
  id: number;
  company: string;
  position: string;
  language: string;
  type: string;
  logo?: string;
  description: string;
}

interface Props {
  job: Job;
}

const TYPE_COLORS: Record<string, string> = {
  "Full-Time": "green",
  Internship: "blue",
  Contract: "orange",
};

const AVATAR_COLORS = ["#1677ff", "#52c41a", "#fa8c16", "#eb2f96", "#722ed1"];
function avatarColor(name: string) {
  const code = name.charCodeAt(0) + (name.charCodeAt(1) ?? 0);
  return AVATAR_COLORS[code % AVATAR_COLORS.length];
}

export default function JobCard({ job }: Props) {
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
        </div>

        <div style={{ display: "flex", alignItems: "center", gap: 8, flexShrink: 0 }}>
          <Tag>{job.language}</Tag>
          <Tag color={TYPE_COLORS[job.type] ?? "default"}>{job.type}</Tag>
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
