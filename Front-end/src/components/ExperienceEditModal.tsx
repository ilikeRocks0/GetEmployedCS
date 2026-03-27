import { Modal, Form, Input } from "antd";
import { Experience, ExperienceValues } from "@/types/Experience";
import { useEffect } from "react";

interface Props {
  open: boolean;
  initialValues?: Experience | null;
  onClose: () => void;
  onSave: (values: ExperienceValues) => void;
}

export default function ExperienceFormModal({ open, initialValues, onClose, onSave }: Props) {
  const [form] = Form.useForm();

  useEffect(() => {
    if (open) {
      form.setFieldsValue(initialValues || { companyName: '', positionTitle: '', jobDescription: '' });
    }
  }, [open, initialValues, form]);

  return (
    <Modal
      title={initialValues ? "Edit Experience" : "Add Experience"}
      open={open}
      onCancel={onClose}
      onOk={() => form.submit()}
    >
      <Form form={form} layout="vertical" onFinish={onSave}>
        <Form.Item name="companyName" label="Company" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="positionTitle" label="Role" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="jobDescription" label="Description" rules={[{ required: true }]}>
          <Input.TextArea rows={4} />
        </Form.Item>
      </Form>
    </Modal>
  );
}