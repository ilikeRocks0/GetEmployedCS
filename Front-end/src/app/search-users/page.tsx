"use client";

import { useEffect, useState } from "react";
import { App, Input, Layout, Select, Spin, Typography, Empty } from "antd";
import SiteHeader from "@/components/SiteHeader";
import UserCard from "@/components/UserCard";
import { searchUsers } from "@/api/searchUsers";
import { User } from "@/types/User";

const { Title } = Typography;
const { Content } = Layout;

const EMPLOYER_OPTIONS = [
  { label: "All users", value: "" },
  { label: "Employers", value: "true" },
  { label: "Job Seekers", value: "false" },
];

function SearchUsersContent() {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");
  const [employerFilter, setEmployerFilter] = useState<string>("");
  const { notification } = App.useApp();

  useEffect(() => {
    const controller = new AbortController();
    setLoading(true);

    const timer = setTimeout(async () => {
      try {
        const results = await searchUsers(
          {
            searchTerm: searchTerm || undefined,
            employer: employerFilter === "" ? undefined : employerFilter === "true",
          },
          controller.signal
        );
        setUsers(results);
      } catch (err) {
        if ((err as DOMException).name !== "AbortError") {
          notification.error({ message: "Failed to load users. Please try again." });
        }
      } finally {
        setLoading(false);
      }
    }, 300);

    return () => {
      clearTimeout(timer);
      controller.abort();
    };
  }, [searchTerm, employerFilter, notification]);

  return (
    <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
      <SiteHeader selectedKey="/search-users" />
      <Content style={{ padding: "40px 80px" }}>
        <div style={{ marginBottom: 24 }}>
          <Title level={1} style={{ margin: 0 }}>Search Users</Title>
        </div>

        <div style={{ display: "flex", gap: 12, marginBottom: 24, flexWrap: "wrap" }}>
          <Input.Search
            placeholder="Search by name, username, or email"
            allowClear
            style={{ width: 320 }}
            onChange={(e) => setSearchTerm(e.target.value)}
            onSearch={(value) => setSearchTerm(value)}
          />
          <Select
            value={employerFilter}
            style={{ width: 160 }}
            options={EMPLOYER_OPTIONS}
            onChange={(value) => setEmployerFilter(value)}
          />
        </div>

        <Spin spinning={loading}>
          {!loading && users.length === 0 ? (
            <Empty description="No users found" />
          ) : (
            <div style={{ display: "grid", gridTemplateColumns: "repeat(3, 1fr)", gap: 16 }}>
              {users.map((user) => <UserCard key={user.userId} user={user} />)}
            </div>
          )}
        </Spin>
      </Content>
    </Layout>
  );
}

export default function SearchUsersPage() {
  return (
    <App>
      <SearchUsersContent />
    </App>
  );
}
