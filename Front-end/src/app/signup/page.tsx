"use client";

import { Layout, Card, Form, Input, Button, Checkbox, Typography, Divider } from "antd";
import { UserOutlined, LockOutlined } from "@ant-design/icons";

const { Content } = Layout;
const { Title, Text, Link } = Typography;

interface SignUpFormValues {
  firstname: string;
  lastname: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export default function HomePage() {
  const onFinish = (values: SignUpFormValues) => {
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
            Sign Up
          </Title>

          <Form<SignUpFormValues>
            name="signup"
            initialValues={{ remember: true }}
            onFinish={onFinish}
            layout="vertical"
          >

            <Form.Item
              name="firstName"
              rules={[{ required: true, message: "Please enter your first name." }]}
            >
              <Input
                prefix={<UserOutlined />}
                placeholder="First Name"
                size="large"
              />
            </Form.Item>

            <Form.Item
              name="lastName"
              rules={[{ required: true, message: "Please enter your last name." }]}
            >
              <Input
                prefix={<UserOutlined />}
                placeholder="Last Name"
                size="large"
              />
            </Form.Item>
            
            <Form.Item
              name="email"
              rules={[{ required: true, message: "Please enter Email." }]}
            >
              <Input
                prefix={<UserOutlined />}
                placeholder="Email"
                size="large"
              />
            </Form.Item>

            <Form.Item
              name="password"
              rules={[{ required: true, message: "Please enter password." }]}
            >
              <Input.Password
                prefix={<LockOutlined />}
                placeholder="Password"
                size="large"
              />
            </Form.Item>

            <Form.Item
              name="confirmPassword"
              rules={[
                { required: true, message: "Please enter password." },
                ({ getFieldValue }) => ({
                    validator(_, value) {
                      if (!value || getFieldValue("password") === value) {
                        return Promise.resolve();
                      }
                      return Promise.reject(
                        new Error("Passwords do not match.")
                      );
                    },
                  }),
            ]}
            >
              <Input.Password
                prefix={<LockOutlined />}
                placeholder="Confirm Password"
                size="large"
              />
            </Form.Item>

            <Divider></Divider>

            <Form.Item>
              <Button type="primary" htmlType="submit" block size="large">
                Register
              </Button>
            </Form.Item>


          </Form>
        </Card>
      </Content>
    </Layout>
  );
}