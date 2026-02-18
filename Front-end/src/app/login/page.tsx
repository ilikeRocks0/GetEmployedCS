"use client";

import { Layout, Card, Form, Input, Button, Checkbox, Typography, Divider } from "antd";
import { UserOutlined, LockOutlined } from "@ant-design/icons";
 
const { Content } = Layout;
const { Title, Text, Link } = Typography;

interface LoginFormValues {
  email: string;
  password: string;
  remember?: boolean;
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
          <Title level={2} style={{ textAlign: "center" }}>
            Login
          </Title>

          <Form<LoginFormValues>
            name="login"
            initialValues={{ remember: true }}
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
              <div
                style={{
                  display: "flex",
                  justifyContent: "space-between",
                  alignItems: "center",
                }}
              >
                <Checkbox>Remember me</Checkbox>
                <Link href="#">Forgot password?</Link>
              </div>
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