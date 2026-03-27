"use client";

import { useState, useEffect, useRef } from "react";
import { Layout, Spin, Button } from "antd";
import { CloseOutlined, CheckOutlined } from "@ant-design/icons";
import SiteHeader from "@/components/SiteHeader";
import { JobGameProvider, useJobGame } from "@/context/JobGameContext";
import { UserGameProvider, useUserGame } from "@/context/UserGameContext";
import JobSwipeCard, { type JobSwipeCardHandle } from "@/components/JobSwipeCard";
import SeekerSwipeCard, { type SeekerSwipeCardHandle } from "@/components/SeekerSwipeCard";
import { checkIfUserIsEmployer } from "@/api/users";
import type { User } from "@/types/User";

const { Content } = Layout;

function JobSeekerContent() {
  const { initJobGame, acceptJob, rejectJob } = useJobGame();
  const [initialJob, setInitialJob] = useState<Awaited<ReturnType<typeof initJobGame>>>(null);
  const [accepted, setAccepted] = useState(0);
  const [rejected, setRejected] = useState(0);
  const [loading, setLoading] = useState(true);
  const [swiping, setSwiping] = useState(false);
  const cardRef = useRef<JobSwipeCardHandle>(null);

  useEffect(() => {
    initJobGame()
      .then((job) => { setInitialJob(job); setLoading(false); })
      .catch(() => setLoading(false));
  }, [initJobGame]);

  function onSwiped(direction: "left" | "right") {
    setSwiping(false);
    if (direction === "right") setAccepted((a) => a + 1);
    else setRejected((r) => r + 1);
  }

  if (loading) return <Spin size="large" />;

  return (
    <>
      <div style={{ display: "flex", gap: 32, marginBottom: 32, fontSize: 15, fontWeight: 500 }}>
        <span style={{ color: "#ff4d4f" }}>✕ {rejected} Passed</span>
        <span style={{ color: "#52c41a" }}>✓ {accepted} Saved</span>
      </div>

      {initialJob && (
        <JobSwipeCard
          ref={cardRef}
          initialJob={initialJob}
          onAccept={acceptJob}
          onReject={rejectJob}
          onSwipeStart={() => setSwiping(true)}
          onSwiped={onSwiped}
        />
      )}

      <div style={{ display: "flex", gap: 40, marginTop: 36 }}>
        <Button danger shape="circle" size="large" icon={<CloseOutlined />}
          style={{ width: 60, height: 60, fontSize: 22 }}
          disabled={swiping} onClick={() => cardRef.current?.swipe("left")} />
        <Button shape="circle" size="large" icon={<CheckOutlined />}
          style={{ width: 60, height: 60, fontSize: 22, background: "#52c41a", borderColor: "#52c41a", color: "#fff" }}
          disabled={swiping} onClick={() => cardRef.current?.swipe("right")} />
      </div>
    </>
  );
}

function EmployerContent() {
  const { initUserGame, acceptUser, rejectUser } = useUserGame();
  const [initialSeeker, setInitialSeeker] = useState<User | null>(null);
  const [accepted, setAccepted] = useState(0);
  const [rejected, setRejected] = useState(0);
  const [loading, setLoading] = useState(true);
  const [swiping, setSwiping] = useState(false);
  const cardRef = useRef<SeekerSwipeCardHandle>(null);
  const currentSeekerRef = useRef<User | null>(null);

  useEffect(() => {
    initUserGame()
      .then((seeker) => { setInitialSeeker(seeker); currentSeekerRef.current = seeker; setLoading(false); })
      .catch(() => setLoading(false));
  }, [initUserGame]);

  async function onAccept(): Promise<User | null> {
    const next = await acceptUser(currentSeekerRef.current!.username);
    currentSeekerRef.current = next;
    return next;
  }

  async function onReject(): Promise<User | null> {
    const next = await rejectUser(currentSeekerRef.current!.username);
    currentSeekerRef.current = next;
    return next;
  }

  function onSwiped(direction: "left" | "right") {
    setSwiping(false);
    if (direction === "right") setAccepted((a) => a + 1);
    else setRejected((r) => r + 1);
  }

  if (loading) return <Spin size="large" />;

  return (
    <>
      <div style={{ display: "flex", gap: 32, marginBottom: 32, fontSize: 15, fontWeight: 500 }}>
        <span style={{ color: "#ff4d4f" }}>✕ {rejected} Passed</span>
        <span style={{ color: "#52c41a" }}>✓ {accepted} Interested</span>
      </div>

      {initialSeeker && (
        <SeekerSwipeCard
          ref={cardRef}
          initialSeeker={initialSeeker}
          onAccept={onAccept}
          onReject={onReject}
          onSwipeStart={() => setSwiping(true)}
          onSwiped={onSwiped}
        />
      )}

      <div style={{ display: "flex", gap: 40, marginTop: 36 }}>
        <Button danger shape="circle" size="large" icon={<CloseOutlined />}
          style={{ width: 60, height: 60, fontSize: 22 }}
          disabled={swiping} onClick={() => cardRef.current?.swipe("left")} />
        <Button shape="circle" size="large" icon={<CheckOutlined />}
          style={{ width: 60, height: 60, fontSize: 22, background: "#52c41a", borderColor: "#52c41a", color: "#fff" }}
          disabled={swiping} onClick={() => cardRef.current?.swipe("right")} />
      </div>
    </>
  );
}

function QuickHirePageContent() {
  const [isEmployer, setIsEmployer] = useState<boolean | null>(null);

  useEffect(() => {
    checkIfUserIsEmployer()
      .then(setIsEmployer)
      .catch(() => setIsEmployer(false));
  }, []);

  return (
    <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
      <SiteHeader selectedKey="/quick-hire" />
      <Content style={{ display: "flex", flexDirection: "column", alignItems: "center", justifyContent: "center", padding: "40px 20px" }}>
        {isEmployer === null ? (
          <Spin size="large" />
        ) : isEmployer ? (
          <EmployerContent />
        ) : (
          <JobSeekerContent />
        )}
      </Content>
    </Layout>
  );
}

export default function QuickHirePage() {
  return (
    <JobGameProvider>
      <UserGameProvider>
        <QuickHirePageContent />
      </UserGameProvider>
    </JobGameProvider>
  );
}
