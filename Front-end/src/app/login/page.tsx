"use client";

import { App, Layout, Card, Form, Input, Button, Typography, Divider } from "antd";
import { UserOutlined, LockOutlined } from "@ant-design/icons";
import { useRouter } from "next/navigation";
import { useLogin } from "@/context/LoginContext";

const { Content } = Layout;
const { Title, Text, Link } = Typography;

interface LoginFormValues {
  email: string;
  password: string;
}

function LoginPageContent() {
  const { notification } = App.useApp();
  const { login } = useLogin();
  const router = useRouter();

  const onFinish = async (values: LoginFormValues) => {
    try {
      await login(values.email, values.password);
      router.push("/");
    } catch (err) {
      notification.error({
        message: "Login Failed",
        description: err instanceof Error ? err.message : "Something went wrong. Please try again.",
      });
    }
  };

  return (
    <Layout style={{ minHeight: "100vh" }}>
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
          }}
        >
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
          <Title level={2} style={{ textAlign: "center" }}>
            Login
          </Title>

          <Form<LoginFormValues>
            name="login"
            onFinish={onFinish}
            layout="vertical"
          >
            <Form.Item
              name="email"
              rules={[{ required: true, message: "Please enter email" }]}
            >
              <Input
                prefix={<UserOutlined />}
                placeholder="Email"
                size="large"
              />
            </Form.Item>

            <Form.Item
              name="password"
              rules={[{ required: true, message: "Please enter password" }]}
            >
              <Input.Password
                prefix={<LockOutlined />}
                placeholder="Password"
                size="large"
              />
            </Form.Item>

            <Form.Item>
              <Button type="primary" htmlType="submit" block size="large">
                Log in
              </Button>
            </Form.Item>

            <Divider />

            <Text>
              Don&apos;t have an account? <Link href="/signup">Register now!</Link>
            </Text>
          </Form>
        </Card>
      </Content>
    </Layout>
  );
}

export default function LoginPage() {
  return (
    <App>
      <LoginPageContent />
    </App>
  );
}
