"use client";

import { Avatar, Card, Tag, Typography } from "antd";
import Link from "next/link";
import { User } from "@/types/User";

const { Text, Title } = Typography;

const AVATAR_COLORS = ["#1677ff", "#52c41a", "#fa8c16", "#eb2f96", "#722ed1"];
function avatarColor(name: string) {
  const code = (name?.charCodeAt(0) ?? 0) + (name?.charCodeAt(1) ?? 0);
  return AVATAR_COLORS[code % AVATAR_COLORS.length];
}

interface UserCardProps {
  user: User;
}

export default function UserCard({ user }: UserCardProps) {
  const displayName = user.isEmployer
    ? user.employerName ?? user.username
    : (user.firstName || user.lastName)
      ? `${user.firstName ?? ""} ${user.lastName ?? ""}`.trim()
      : user.username;

  return (
    <Link href={`/profile/${user.username}`}>
      <Card
        hoverable
        style={{ borderTop: "4px solid #1677ff", cursor: "pointer", height: "100%" }}
        styles={{ body: { padding: "20px", display: "flex", flexDirection: "column", alignItems: "center", gap: 8, textAlign: "center" } }}
      >
        <Avatar
          size={64}
          style={{ backgroundColor: avatarColor(user.username), fontWeight: 700 }}
        >
          {user.username.slice(0, 2).toUpperCase()}
        </Avatar>

        <div>
          <Title level={5} style={{ margin: 0, lineHeight: 1.3 }}>
            {displayName}
          </Title>
          <Text type="secondary" style={{ fontSize: 12 }}>@{user.username}</Text>
        </div>

        <Tag color={user.isEmployer ? "blue" : "green"}>
          {user.isEmployer ? "Employer" : "Job Seeker"}
        </Tag>

        {user.bio && (
          <Text
            type="secondary"
            style={{ fontSize: 12, display: "-webkit-box", WebkitLineClamp: 2, WebkitBoxOrient: "vertical", overflow: "hidden" }}
          >
            {user.bio}
          </Text>
        )}
      </Card>
    </Link>
  );
}
