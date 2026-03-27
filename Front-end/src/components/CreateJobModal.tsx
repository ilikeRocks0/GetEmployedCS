"use client";

import { useEffect, useState } from "react";
import { Modal, Form, Input, Select, DatePicker, Checkbox, Button, Row, Col, App } from "antd";
import { createJob, type NewJobRequest } from "@/api/createJob";
import { useLanguage } from "@/context/LanguageContext";
import type { Dayjs } from "dayjs";

const { TextArea } = Input;

const EMPLOYMENT_TYPES = ["Co-op", "Contract", "Full-time", "Internship", "Part-time"];
const POSITION_TYPES = ["AI", "Back-end", "Data", "Design", "Embedded", "Front-end", "Full stack", "Game dev", "Mobile"];

interface CreateJobModalProps {
  open: boolean;
  onClose: () => void;
  onCreated?: () => void;
}

interface FormValues {
  position: string;
  description: string;
  applicationLink: string;
  employment_type: string;
  position_type: string;
  locations?: string[];
  languages?: string[];
  deadline?: Dayjs;
  isRemote?: boolean;
  isHybrid?: boolean;
}

export default function CreateJobModal({ open, onClose, onCreated }: CreateJobModalProps) {
  const [form] = Form.useForm<FormValues>();
  const [submitting, setSubmitting] = useState(false);
  const [languages, setLanguages] = useState<string[]>([]);
  const fetchLanguages = useLanguage();
  const { message } = App.useApp();

  useEffect(() => {
    fetchLanguages().then(setLanguages).catch(() => {});
  }, [fetchLanguages]);

  async function handleSubmit(values: FormValues) {
    setSubmitting(true);
    try {
      const payload: NewJobRequest = {
        title: values.position,
        jobDescription: values.description,
        applicationLink: values.applicationLink,
        employmentType: values.employment_type,
        positionType: values.position_type,
        locations: values.locations ?? [],
        programmingLanguages: values.languages ?? [],
        deadline: values.deadline ? values.deadline.format("YYYY-MM-DD") : null,
        hasRemote: values.isRemote ?? false,
        hasHybrid: values.isHybrid ?? false,
      };
      await createJob(payload);
      message.success("Job posted successfully.");
      form.resetFields();
      onCreated?.();
      onClose();
    } catch {
      message.error("Failed to post job. Please try again.");
    } finally {
      setSubmitting(false);
    }
  }

  function handleCancel() {
    form.resetFields();
    onClose();
  }

  return (
    <Modal
      open={open}
      onCancel={handleCancel}
      title="Post a New Job"
      centered
      width={680}
      footer={null}
    >
      <Form
        form={form}
        layout="vertical"
        onFinish={handleSubmit}
        style={{ marginTop: 16 }}
      >
        <Form.Item name="position" label="Position" rules={[{ required: true, message: "Required" }]}>
          <Input placeholder="Job title" />
        </Form.Item>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item name="employment_type" label="Employment Type" rules={[{ required: true, message: "Required" }]}>
              <Select placeholder="Select type" options={EMPLOYMENT_TYPES.map((t) => ({ label: t, value: t }))} />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item name="position_type" label="Position Type" rules={[{ required: true, message: "Required" }]}>
              <Select placeholder="Select level" options={POSITION_TYPES.map((t) => ({ label: t, value: t }))} />
            </Form.Item>
          </Col>
        </Row>

        <Form.Item name="description" label="Job Description" rules={[{ required: true, message: "Required" }]}>
          <TextArea rows={4} placeholder="Describe the role, responsibilities, and requirements..." />
        </Form.Item>

        <Form.Item name="applicationLink" label="Application Link" rules={[{ required: true, message: "Required" }, { type: "url", message: "Enter a valid URL" }]}>
          <Input placeholder="https://..." />
        </Form.Item>

        <Row gutter={16}>
          <Col span={12}>
            <Form.Item name="languages" label="Languages / Tools">
              <Select
                mode="multiple"
                placeholder="Select languages"
                options={languages.map((l) => ({ label: l, value: l }))}
              />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item
              name="locations"
              label="Locations"
              rules={[{
                validator: (_, value: string[]) => {
                  if (!value || value.length === 0) return Promise.resolve();
                  const invalid = value.filter(v => v.split(',').filter(p => p.trim()).length !== 3);
                  return invalid.length === 0
                    ? Promise.resolve()
                    : Promise.reject(new Error("Each location must be in \"City, State, Country\" format"));
                }
              }]}
            >
              <Select mode="tags" placeholder="City, State, Country (press Enter)" tokenSeparators={[]} />
            </Form.Item>
          </Col>
        </Row>

        <Form.Item name="deadline" label="Application Deadline">
          <DatePicker style={{ width: "100%" }} />
        </Form.Item>

        <Form.Item label="Work Mode">
          <Row gutter={16}>
            <Col>
              <Form.Item name="isRemote" valuePropName="checked" noStyle>
                <Checkbox>Remote</Checkbox>
              </Form.Item>
            </Col>
            <Col>
              <Form.Item name="isHybrid" valuePropName="checked" noStyle>
                <Checkbox>Hybrid</Checkbox>
              </Form.Item>
            </Col>
          </Row>
        </Form.Item>

        <Form.Item style={{ marginBottom: 0, textAlign: "right" }}>
          <Button onClick={handleCancel} style={{ marginRight: 8 }}>Cancel</Button>
          <Button type="primary" htmlType="submit" loading={submitting}>Post Job</Button>
        </Form.Item>
      </Form>
    </Modal>
  );
}
