"use client";

import { useState, useEffect } from "react";
import { Card, Tag, Avatar, Typography, Button, message, Popconfirm } from "antd";
import { HeartOutlined, HeartFilled, DeleteOutlined } from "@ant-design/icons";
import JobDetailModal from "./JobDetailModal";
import type { Job } from "@/types/Job";
import { saveJob, unsaveJob } from "@/api/users";
import { useIsSavedList } from "@/context/IsSavedListContext";

export type { Job };

const { Text, Title } = Typography;

interface JobCardState {
  job: Job;
  onRemove?: () => void;
  isSaved?: boolean;
  isCurrentUsers?: boolean;
  onDelete?: (id: number) => void;
}

const TYPE_COLORS: Record<string, string> = {
  "Full-time": "green",
  "Co-op": "blue",
  "Internship": "blue",
  Contract: "orange",
};

const AVATAR_COLORS = ["#1677ff", "#52c41a", "#fa8c16", "#eb2f96", "#722ed1"];
function avatarColor(name: string) {
  const code = (name?.charCodeAt(0) ?? 0) + (name?.charCodeAt(1) ?? 0);
  return AVATAR_COLORS[code % AVATAR_COLORS.length];
}

const REMOVE_DURATION = 350;

export default function JobCard({ job, onRemove, isSaved: isSavedProp, isCurrentUsers, onDelete }: JobCardState) {
  const isSavedList = useIsSavedList();
  const [modalOpen, setModalOpen] = useState(false);
  const [saved, setSaved] = useState(isSavedProp ?? isSavedList);
  const [saving, setSaving] = useState(false);
  const [removing, setRemoving] = useState(false);

  useEffect(() => {
    if (isSavedProp !== undefined) setSaved(isSavedProp);
  }, [isSavedProp]);

  async function handleSaveToggle(e: React.MouseEvent) {
    e.stopPropagation();
    setSaving(true);
    try {
      if (saved) {
        await unsaveJob(job.id);
        if (isSavedList && onRemove) {
          setRemoving(true);
          setTimeout(onRemove, REMOVE_DURATION);
        } else {
          setSaved(false);
        }
      } else {
        await saveJob(job.id);
        setSaved(true);
      }
    } catch {
      message.error(saved ? "Failed to unsave job" : "Failed to save job");
    } finally {
      setSaving(false);
    }
  }

  return (
    <div style={{
      overflow: "hidden",
      opacity: removing ? 0 : 1,
      maxHeight: removing ? 0 : 200,
      marginBottom: removing ? 0 : 12,
      transition: `opacity ${REMOVE_DURATION}ms ease, max-height ${REMOVE_DURATION}ms ease, margin-bottom ${REMOVE_DURATION}ms ease`,
    }}>
      <Card
        hoverable
        onClick={() => setModalOpen(true)}
        style={{
          width: "100%",
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
          style={{ backgroundColor: job.logo ? undefined : avatarColor(job.company || "?"), flexShrink: 0, fontWeight: 700 }}
        >
          {!job.logo && (job.company || "?").slice(0, 2).toUpperCase()}
        </Avatar>

        <div style={{ flex: 1, minWidth: 0 }}>
          <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
            <Title level={5} style={{ margin: 0, lineHeight: 1.3 }}>
              {job.position}
            </Title>
            <Tag color={job.employerPoster ? "blue" : "purple"}>{job.employerPoster ? "Employer" : "Community"}</Tag>
          </div>
          <Text type="secondary">Posted by {job.posterName ?? "Unknown"}</Text>
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
          {!isCurrentUsers && (
            <Button
              type="text"
              shape="circle"
              loading={saving}
              icon={saved ? <HeartFilled style={{ color: "#1677ff" }} /> : <HeartOutlined />}
              onClick={handleSaveToggle}
            />
          )}
        </div>

        {isCurrentUsers && onDelete && (
          <Popconfirm
              title="Delete job posting?"
              description="Are you sure you want to remove this?"
              onConfirm={() => onDelete(job.id)}
              okText="Yes"
              cancelText="No"
          >
            <Button
                type="text"
                size="small"
                danger
                icon={<DeleteOutlined />}
            />
          </Popconfirm>
        )}
      </Card>

      <JobDetailModal
        job={job}
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        isCurrentUsers={isCurrentUsers || false}
      />
    </div>
  );
}
