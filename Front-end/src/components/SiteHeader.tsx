"use client";

import React from "react";
import { Layout, Menu } from "antd";
import type { MenuProps } from "antd";
import { useRouter } from "next/navigation";
import ProfileMenu from "./ProfileMenu";

const { Header } = Layout;

interface SiteHeaderState {
  selectedKey?: string;
}

const SiteHeader: React.FC<SiteHeaderState> = ({
  selectedKey = "home",
}) => {
  const router = useRouter();

  const items = [
    { key: "/", label: "Home" },
    { key: "/quick-hire", label: "Quick Hire"},
    { key: "/search-jobs", label: "Find Jobs" },
    { key: "/resume stuff", label: "Resume Helper Tools" }, // does not currently exist yet
  ];

    const handleClick: MenuProps["onClick"] = (e) => {
        router.push(e.key); // just push the key directly
    };

  return (
    <Header
      style={{
        position: "sticky",
        top: 0,
        zIndex: 1000,
        height: "80px",
        padding: "0 64px",
        display: "flex",
        alignItems: "center",
        justifyContent: "space-between",
        background: "rgba(255, 255, 255, 0.7)",
        backdropFilter: "blur(12px)",
        WebkitBackdropFilter: "blur(12px)",
        borderBottom: "1px solid rgba(0,0,0,0.06)",
        boxShadow: "0 4px 20px rgba(0,0,0,0.04)",
      }}
    >
      {/* LEFT SIDE */}
      <div
        style={{
          display: "flex",
          alignItems: "center",
          gap: "48px",
        }}
      >
        {/* Logo */}
        <div
          onClick={() => router.push("/")}
          style={{
            fontSize: "22px",
            fontWeight: 700,
            letterSpacing: "-0.5px",
            cursor: "pointer",
            background: "linear-gradient(90deg, #111, #555)",
            WebkitBackgroundClip: "text",
            WebkitTextFillColor: "transparent",
          }}
        >
          GetEmployed.cs
        </div>

        {/* Navigation */}
        <Menu
          mode="horizontal"
          overflowedIndicator={null}
          selectedKeys={[selectedKey]}
          onClick={handleClick}
          items={items}
          style={{
            borderBottom: "none",
            background: "transparent",
            fontSize: "15px",
            fontWeight: 500,
          }}
        />
      </div>

      <ProfileMenu />
    </Header>
  );
};

export default SiteHeader;