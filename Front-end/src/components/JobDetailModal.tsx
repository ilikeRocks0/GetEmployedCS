"use client";

import { Modal, Button, Avatar, Tag, Typography, Divider } from "antd";
import type { Job } from "@/types/Job";

const { Text, Title } = Typography;

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

interface JobDetailModalState {
  job: Job | null;
  open: boolean;
  onClose: () => void;
}

export default function JobDetailModal({ job, open, onClose }: JobDetailModalState) {
  if (!job) return null;

  return (
    <Modal
      open={open}
      onCancel={onClose}
      footer={null}
      centered
      width={600}
    >
      <div style={{ display: "flex", alignItems: "center", gap: 16, borderLeft: "4px solid #1677ff", paddingLeft: 16, marginBottom: 4 }}>
        <Avatar
          size={56}
          src={job.logo}
          style={{ backgroundColor: job.logo ? undefined : avatarColor(job.company), flexShrink: 0, fontWeight: 700 }}
        >
          {!job.logo && job.company.slice(0, 2).toUpperCase()}
        </Avatar>
        <div>
          <Title level={4} style={{ margin: 0, lineHeight: 1.3 }}>{job.company}</Title>
          <Text type="secondary" style={{ fontSize: 15 }}>{job.position}</Text>
          <div style={{ marginTop: 6, display: "flex", gap: 6 }}>
            <Tag>{job.language}</Tag>
            <Tag color={TYPE_COLORS[job.employment_type] ?? "default"}>{job.employment_type}</Tag>
          </div>
        </div>
      </div>

      <Divider />

      <Title level={5}>Job Description</Title>
      <Text type="secondary">{job.description}</Text>

      <div style={{ marginTop: 24, display: "flex", justifyContent: "flex-end" }}>
        <Button type="primary" size="large">Apply Now</Button>
      </div>
    </Modal>
  );
}