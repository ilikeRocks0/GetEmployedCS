"use client";

import { useEffect, useState } from "react";
import { App, Layout, Spin, Typography, Empty } from "antd";
import SiteHeader from "@/components/SiteHeader";
import UserCard from "@/components/UserCard";
import { fetchFollowing } from "@/api/users";
import { User } from "@/types/User";

const { Title } = Typography;
const { Content } = Layout;

function FollowingContent() {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  useEffect(() => {
    const controller = new AbortController();

    fetchFollowing(controller.signal)
      .then(setUsers)
      .catch(() => {})
      .finally(() => setLoading(false));

    return () => controller.abort();
  }, []);

  return (
    <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
      <SiteHeader selectedKey="/saved-users" />
      <Content style={{ padding: "40px 80px" }}>
        <div style={{ marginBottom: 24 }}>
          <Title level={1} style={{ margin: 0 }}>Following</Title>
        </div>

        <Spin spinning={loading}>
          {!loading && users.length === 0 ? (
            <Empty description="You havent found anyone to follow yet!" />
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

export default function FollowingPage() {
  return (
    <App>
      <FollowingContent />
    </App>
  );
}
