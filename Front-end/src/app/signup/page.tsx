"use client";

import { useState } from "react";
import { App, Layout, Card, Form, Input, Button, Typography, Divider, Result, Tooltip } from "antd";
import { UserOutlined, LockOutlined, TeamOutlined, SolutionOutlined, ArrowLeftOutlined } from "@ant-design/icons";
import { UsersProvider, useUser } from "@/context/UserContext";

const { Content } = Layout;
const { Title, Text } = Typography;

interface SignUpFormValues {
  username: string;
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
  employerName?: string;
}

function SignUpPageContent() {
  const { notification } = App.useApp();
  const { registerUser } = useUser();
  const [isEmployer, setIsEmployer] = useState<boolean | null>(null);
  const [dir, setDir] = useState<"forward" | "back">("forward");
  const [registered, setRegistered] = useState(false);

  function selectRole(employer: boolean) {
    setDir("forward");
    setIsEmployer(employer);
  }

  function goBack() {
    setDir("back");
    setIsEmployer(null);
  }

  const onFinish = async (values: SignUpFormValues) => {
    try {
      await registerUser({
        username: values.username,
        password: values.password,
        firstName: values.firstName,
        lastName: values.lastName,
        employerName: values.employerName ?? "",
        isEmployer: isEmployer ?? false,
        email: values.email.toLowerCase(),
      });
      setDir("forward");
      setRegistered(true);
    } catch (err) {
      notification.error({
        message: "Registration Failed",
        description: err instanceof Error ? err.message : "Something went wrong. Please try again.",
      });
    }
  };

  return (
    <Layout style={{ minHeight: "100vh" }}>
      <style>{`
        @keyframes slideInRight {
          from { opacity: 0; transform: translateX(32px); }
          to   { opacity: 1; transform: translateX(0); }
        }
        @keyframes slideInLeft {
          from { opacity: 0; transform: translateX(-32px); }
          to   { opacity: 1; transform: translateX(0); }
        }
      `}</style>
      <Content
        style={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          padding: "40px",
          background: "#f0f2f5",
        }}
      >
        <Card
          style={{
            width: 400,
            boxShadow: "0 4px 12px rgba(0,0,0,0.1)",
            borderRadius: "8px",
            overflow: "hidden",
          }}
        >
          <div
            key={registered ? "success" : isEmployer === null ? "select" : isEmployer ? "employer" : "jobseeker"}
            style={{
              animation: `${dir === "forward" ? "slideInRight" : "slideInLeft"} 0.25s ease`,
            }}
          >
            {registered ? (
              <Result
                status="success"
                title="Almost there!"
                subTitle="We've sent a verification email to your inbox. Please verify your email to activate your account."
                extra={
                  <Button type="primary" href="/login" size="large">
                    Go to Login
                  </Button>
                }
              />
            ) : isEmployer === null ? (
              <>
                <Tooltip title="Back to login">
                  <Button type="text" href="/login" icon={<ArrowLeftOutlined />} size="large" style={{position: "absolute"}}/>
                </Tooltip>
                <div style={{ textAlign: "center", marginBottom: 8 }}>
                  <span style={{
                    fontSize: "28px",
                    fontWeight: 700,
                    letterSpacing: "-0.5px",
                    background: "linear-gradient(90deg, #111, #555)",
                    WebkitBackgroundClip: "text",
                    WebkitTextFillColor: "transparent",
                  }}>
                    GetEmployed.cs
                  </span>
                </div>
                <Title level={2} style={{ textAlign: "center", marginBottom: 8 }}>
                  Sign Up
                </Title>
                <Text type="secondary" style={{ display: "block", textAlign: "center", marginBottom: 32 }}>
                  I am registering as a...
                </Text>
                <div style={{ display: "flex", flexDirection: "column", gap: 16 }}>
                  <Button
                    size="large"
                    icon={<SolutionOutlined style={{ fontSize: 20 }} />}
                    onClick={() => selectRole(false)}
                    style={{ height: 72, fontSize: 16, display: "flex", alignItems: "center", gap: 12 }}
                  >
                    Job Seeker
                  </Button>
                  <Button
                    size="large"
                    icon={<TeamOutlined style={{ fontSize: 20 }} />}
                    onClick={() => selectRole(true)}
                    style={{ height: 72, fontSize: 16, display: "flex", alignItems: "center", gap: 12 }}
                  >
                    Employer
                  </Button>
                </div>
              </>
            ) : (
              <>
                <div style={{ display: "flex", alignItems: "center", marginBottom: 8 }}>
                  <Button
                    type="text"
                    icon={<ArrowLeftOutlined />}
                    onClick={goBack}
                    style={{ padding: 0, marginRight: 8 }}
                  />
                  <Title level={2} style={{ margin: 0, flex: 1, textAlign: "center" }}>
                    {isEmployer ? "Employer" : "Job Seeker"} Sign Up
                  </Title>
                </div>

                <Form<SignUpFormValues>
                  name="signup"
                  onFinish={onFinish}
                  layout="vertical"
                  style={{ marginTop: 24 }}
                >
                  <Form.Item
                    name="username"
                    rules={[{ required: true, message: "Please enter a username." }]}
                  >
                    <Input prefix={<UserOutlined />} placeholder="Username" size="large" />
                  </Form.Item>

                  {isEmployer && (
                    <Form.Item
                      name="employerName"
                      rules={[{ required: true, message: "Please enter your company name." }]}
                    >
                      <Input prefix={<TeamOutlined />} placeholder="Company Name" size="large" />
                    </Form.Item>
                  )}

                  {!isEmployer && (
                    <>
                      <Form.Item
                        name="firstName"
                        rules={[{ required: true, message: "Please enter your first name." }]}
                      >
                        <Input prefix={<UserOutlined />} placeholder="First Name" size="large" />
                      </Form.Item>
                      <Form.Item
                        name="lastName"
                        rules={[{ required: true, message: "Please enter your last name." }]}
                      >
                        <Input prefix={<UserOutlined />} placeholder="Last Name" size="large" />
                      </Form.Item>
                    </>
                  )}

                  <Form.Item
                    name="email"
                    rules={[{ required: true, message: "Please enter Email." }]}
                  >
                    <Input prefix={<UserOutlined />} placeholder="Email" size="large" />
                  </Form.Item>

                  <Form.Item
                    name="password"
                    rules={[{ required: true, message: "Please enter password." }]}
                  >
                    <Input.Password prefix={<LockOutlined />} placeholder="Password" size="large" />
                  </Form.Item>

                  <Form.Item
                    name="confirmPassword"
                    rules={[
                      { required: true, message: "Please confirm your password." },
                      ({ getFieldValue }) => ({
                        validator(_, value) {
                          if (!value || getFieldValue("password") === value) {
                            return Promise.resolve();
                          }
                          return Promise.reject(new Error("Passwords do not match."));
                        },
                      }),
                    ]}
                  >
                    <Input.Password prefix={<LockOutlined />} placeholder="Confirm Password" size="large" />
                  </Form.Item>

                  <Divider />

                  <Form.Item>
                    <Button type="primary" htmlType="submit" block size="large">
                      Register
                    </Button>
                  </Form.Item>
                </Form>
              </>
            )}
          </div>
        </Card>
      </Content>
    </Layout>
  );
}

export default function SignUpPage() {
  return (
    <App>
      <UsersProvider>
        <SignUpPageContent />
      </UsersProvider>
    </App>
  );
}
