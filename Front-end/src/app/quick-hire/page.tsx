"use client";

import { useState, useEffect, useRef } from "react";
import { Layout, Spin, Button } from "antd";
import { CloseOutlined, CheckOutlined } from "@ant-design/icons";
import SiteHeader from "@/components/SiteHeader";
import { JobGameProvider, useJobGame } from "@/context/JobGameContext";
import SwipeCard, { type SwipeCardHandle } from "@/components/SwipeCard";

const { Content } = Layout;

function QuickHirePageContent() {
  const { initJobGame, acceptJob, rejectJob } = useJobGame();
  const [initialJob, setInitialJob] = useState<Awaited<ReturnType<typeof initJobGame>>>(null);
  const [accepted, setAccepted] = useState(0);
  const [rejected, setRejected] = useState(0);
  const [initialLoading, setInitialLoading] = useState(true);
  const [swiping, setSwiping] = useState(false);
  const cardRef = useRef<SwipeCardHandle>(null);

  useEffect(() => {
    initJobGame()
      .then((job) => {
        setInitialJob(job);
        setInitialLoading(false);
      })
      .catch(() => setInitialLoading(false));
  }, [initJobGame]);

  function onSwiped(direction: "left" | "right") {
    setSwiping(false);
    if (direction === "right") setAccepted((a) => a + 1);
    else setRejected((r) => r + 1);
  }

  return (
    <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
      <SiteHeader selectedKey="/quick-hire" />
      <Content
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          justifyContent: "center",
          padding: "40px 20px",
        }}
      >
        {initialLoading ? (
          <Spin size="large" />
        ) : (
          <>
            <div style={{ display: "flex", gap: 32, marginBottom: 32, fontSize: 15, fontWeight: 500 }}>
              <span style={{ color: "#ff4d4f" }}>✕ {rejected} Passed</span>
              <span style={{ color: "#52c41a" }}>✓ {accepted} Saved</span>
            </div>

            {initialJob && (
              <SwipeCard
                ref={cardRef}
                initialJob={initialJob}
                onAccept={acceptJob}
                onReject={rejectJob}
                onSwipeStart={() => setSwiping(true)}
                onSwiped={onSwiped}
              />
            )}

            <div style={{ display: "flex", gap: 40, marginTop: 36 }}>
              <Button
                danger
                shape="circle"
                size="large"
                icon={<CloseOutlined />}
                style={{ width: 60, height: 60, fontSize: 22 }}
                disabled={swiping}
                onClick={() => cardRef.current?.swipe("left")}
              />
              <Button
                shape="circle"
                size="large"
                icon={<CheckOutlined />}
                style={{
                  width: 60,
                  height: 60,
                  fontSize: 22,
                  background: "#52c41a",
                  borderColor: "#52c41a",
                  color: "#fff",
                }}
                disabled={swiping}
                onClick={() => cardRef.current?.swipe("right")}
              />
            </div>
          </>
        )}
      </Content>
    </Layout>
  );
}

export default function QuickHirePage() {
  return (
    <JobGameProvider>
      <QuickHirePageContent />
    </JobGameProvider>
  );
}
