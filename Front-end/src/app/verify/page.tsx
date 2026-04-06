"use client";

import { Suspense, useEffect, useState } from "react";
import { useSearchParams } from "next/navigation";
import { App, Layout, Card, Button, Result, Spin } from "antd";
import { verifyEmail } from "@/api/users";

function VerifyContent() {
  const searchParams = useSearchParams();
  const token = searchParams.get("token");
  const [status, setStatus] = useState<"loading" | "success" | "error">(token ? "loading" : "error");

  useEffect(() => {
    if (!token) return;
    verifyEmail(token)
      .then(() => setStatus("success"))
      .catch(() => setStatus("error"));
  }, [token]);

  return (
    <Card style={{ width: 400, boxShadow: "0 4px 12px rgba(0,0,0,0.1)", borderRadius: "8px" }}>
      {status === "loading" && (
        <div style={{ textAlign: "center", padding: "40px 0" }}>
          <Spin size="large" />
          <p style={{ marginTop: 16, color: "#888" }}>Verifying your email…</p>
        </div>
      )}
      {status === "success" && (
        <Result
          status="success"
          title="Email Verified!"
          subTitle="Your account is now active. You can log in."
          extra={
            <Button type="primary" href="/login" size="large">
              Go to Login
            </Button>
          }
        />
      )}
      {status === "error" && (
        <Result
          status="error"
          title="Verification Failed"
          subTitle="This link is invalid or has already been used."
          extra={
            <Button href="/signup" size="large">
              Back to Sign Up
            </Button>
          }
        />
      )}
    </Card>
  );
}

export default function VerifyPage() {
  return (
    <App>
      <Layout style={{ minHeight: "100vh", background: "#f0f2f5" }}>
        <Layout.Content style={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
          <Suspense fallback={<Spin size="large" />}>
            <VerifyContent />
          </Suspense>
        </Layout.Content>
      </Layout>
    </App>
  );
}
