"use client";

import { Layout, Card, Form, Input, Button, Typography, Divider } from "antd";
import { UserOutlined, LockOutlined } from "@ant-design/icons";
 
const { Content } = Layout;
const { Title, Text, Link } = Typography;


interface LoginFormValues {
  email: string;
  password: string;
}

export default function HomePage() {
  const onFinish = (values: LoginFormValues) => {
    console.log("Login values:", values);
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
              Don’t have an account? <Link href="/signup">Register now!</Link>
            </Text>
          </Form>
        </Card>
      </Content>
    </Layout>
  );
}