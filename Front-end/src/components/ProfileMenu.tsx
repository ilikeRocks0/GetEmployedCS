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

const ProfileMenu: React.FC = () => {
  const router = useRouter();

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

  const handleClick: MenuProps["onClick"] = (e) => {
    switch (e.key) {
      case "profile":
        //router.push("/profile");
        break;
      case "applications":
        //router.push("/applications");
        break;
      case "settings":
        //router.push("/settings");
        break;
      case "logout":
        //console.log("Handle logout logic here");
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