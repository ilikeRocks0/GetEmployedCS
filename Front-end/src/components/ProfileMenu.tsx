"use client";

import React from "react";
import { Avatar, Dropdown, MenuProps } from "antd";
import {
  UserOutlined,
  SettingOutlined,
  FileTextOutlined,
  LogoutOutlined,
} from "@ant-design/icons";
import { useRouter } from "next/navigation";
import { useLogin } from "@/context/LoginContext";

const ProfileMenu: React.FC = () => {
  const router = useRouter();
  const { logout } = useLogin();
  const items: MenuProps["items"] = [
    {
      key: "profile",
      icon: <UserOutlined />,
      label: "Profile",
    },
    {
      key: "applications",
      icon: <FileTextOutlined />,
      label: "My Applications",
    },
    {
      key: "settings",
      icon: <SettingOutlined />,
      label: "Settings",
    },
    {
      type: "divider",
    },
    {
      key: "logout",
      icon: <LogoutOutlined />,
      danger: true,
      label: "Logout",
    },
  ];
  
  const handleClick: MenuProps["onClick"] = async (e) => {
    switch (e.key) {
      case "profile": {
        const user = JSON.parse(localStorage.getItem("user") ?? "{}");
        if (user.username) router.push(`/profile/${user.username}`);
        break;
      }
      case "applications":
        router.push("/applications");
        break;
      case "settings":
        break;
      case "logout":
        await logout();
        router.push("/login");
        break;
    }
  };

  return (
    <Dropdown
      menu={{ items, onClick: handleClick }}
      placement="bottomRight"
      trigger={["click"]}
    >
      <Avatar
        size="large"
        icon={<UserOutlined />}
        style={{
          cursor: "pointer",
          backgroundColor: "#111",
        }}
      />
    </Dropdown>
  );
};

export default ProfileMenu;