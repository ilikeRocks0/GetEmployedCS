import { User } from "@/types/User";
import { UpdateUserRequest } from "@/api/users";
import { Form, Input, Modal } from "antd";
import { useEffect } from "react";

interface ProfileEditModalProps {
    open: boolean;
    user: User;
    loading: boolean;
    onClose: () => void;
    onSave: (values: UpdateUserRequest) => Promise<void>;
};

export default function ProfileEditModal({ open, user, loading, onClose, onSave }: ProfileEditModalProps) {
    const [form] = Form.useForm();

    useEffect(() => {
        if (open) form.setFieldsValue({
            username: user.username,
            about: user.bio,
            firstName: user.firstName,
            lastName: user.lastName,
            employerName: user.employerName,
        });
    }, [open, user, form]);

    const handleOk = () => {
        form.validateFields().then((values) => {
            onSave(values);
        });
    };

    return (
        <Modal
            title="Edit Profile"
            open={open}
            onCancel={onClose}
            onOk={handleOk}
            confirmLoading={loading}
        >
            <Form form={form} layout="vertical">
                <Form.Item name="username" label="Username">
                    <Input />
                </Form.Item>
                {user.isEmployer ? (
                    <Form.Item name="employerName" label="Company Name">
                        <Input />
                    </Form.Item>
                ) : (
                    <>
                        <Form.Item name="firstName" label="First Name">
                            <Input />
                        </Form.Item>
                        <Form.Item name="lastName" label="Last Name">
                            <Input />
                        </Form.Item>
                    </>
                )}
                <Form.Item name="about" label="About Me">
                    <Input.TextArea rows={4} />
                </Form.Item>
            </Form>
        </Modal>
    );
}