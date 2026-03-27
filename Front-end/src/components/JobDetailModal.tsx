"use client";

import { Modal, Button, Avatar, Tag, Typography, Divider } from "antd";
import type { Job } from "@/types/Job";
import CommentList from "./CommentList";

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
  isCurrentUsers: boolean;
}

export default function JobDetailModal({ job, open, onClose, isCurrentUsers }: JobDetailModalState) {
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
          style={{ backgroundColor: job.logo ? undefined : avatarColor(job.company || "?"), flexShrink: 0, fontWeight: 700 }}
        >
          {!job.logo && (job.company || "?").slice(0, 2).toUpperCase()}
        </Avatar>
        <div>
          <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
            <Title level={4} style={{ margin: 0, lineHeight: 1.3 }}>{job.position}</Title>
            <Tag color={job.employerPoster ? "blue" : "purple"}>{job.employerPoster ? "Employer Posting" : "Community Posting"}</Tag>
          </div>
          <Text type="secondary" style={{ fontSize: 14 }}>Posted by {job.posterName ?? "Unknown"}</Text>
          <div style={{ marginTop: 6, display: "flex", gap: 6, flexWrap: "wrap" }}>
            {job.languages.map((lang) => <Tag key={lang}>{lang}</Tag>)}
            <Tag color={TYPE_COLORS[job.employment_type] ?? "default"}>{job.employment_type}</Tag>
          </div>
        </div>
      </div>

      <Divider />

      <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "12px 24px", marginBottom: 20 }}>
        {job.locations.length > 0 && (
          <div>
            <Text type="secondary" style={{ fontSize: 12, display: "block" }}>Locations</Text>
            <Text>{job.locations.join(", ")}</Text>
          </div>
        )}
        <div>
          <Text type="secondary" style={{ fontSize: 12, display: "block" }}>Work Mode</Text>
          <div style={{ display: "flex", gap: 4, marginTop: 4 }}>
            {job.isRemote && <Tag color="cyan">Remote</Tag>}
            {job.isHybrid && <Tag color="geekblue">Hybrid</Tag>}
            {!job.isRemote && !job.isHybrid && <Tag>On-site</Tag>}
          </div>
        </div>
        <div>
          <Text type="secondary" style={{ fontSize: 12, display: "block" }}>Position Type</Text>
          <Text>{job.position_type}</Text>
        </div>
        {job.deadline && (
          <div>
            <Text type="secondary" style={{ fontSize: 12, display: "block" }}>Deadline</Text>
            <Text>{new Date(job.deadline).toLocaleDateString("en-US", { year: "numeric", month: "long", day: "numeric" })}</Text>
          </div>
        )}
      </div>

      <Title level={5}>Job Description</Title>
      <Text type="secondary">{job.description}</Text>
      <Divider />
      
      <CommentList jobId={job.id} />

      {!isCurrentUsers && (
        <div style={{ marginTop: 24, display: "flex", justifyContent: "flex-end" }}>
          <Button type="primary" size="large" href={job.applicationLink} target="_blank" rel="noopener noreferrer">Apply Now</Button>
        </div>
      )}
    </Modal>
  );
}