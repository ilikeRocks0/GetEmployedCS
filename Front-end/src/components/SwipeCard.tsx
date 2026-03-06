"use client";

import { useState, useRef, forwardRef, useImperativeHandle } from "react";
import { Tag, Avatar, Typography } from "antd";
import type { Job } from "@/types/Job";

const { Title, Text } = Typography;

const TYPE_COLORS: Record<string, string> = {
  "Full-time": "green",
  "Co-op": "blue",
  Internship: "blue",
  Contract: "orange",
};

const AVATAR_COLORS = ["#1677ff", "#52c41a", "#fa8c16", "#eb2f96", "#722ed1"];
function avatarColor(name: string) {
  const code = name.charCodeAt(0) + (name.charCodeAt(1) ?? 0);
  return AVATAR_COLORS[code % AVATAR_COLORS.length];
}

const SWIPE_THRESHOLD = 100;
const FLY_DURATION = 350;

export interface SwipeCardHandle {
  swipe: (direction: "left" | "right") => void;
}

interface SwipeCardState {
  initialJob: Job;
  onAccept: (jobId: number) => Promise<Job | null>;
  onReject: (jobId: number) => Promise<Job | null>;
  onSwipeStart?: () => void;
  onSwiped: (direction: "left" | "right") => void;
}

const SwipeCard = forwardRef<SwipeCardHandle, SwipeCardState>(
  ({ initialJob, onAccept, onReject, onSwipeStart, onSwiped }, ref) => {
    const [currentJob, setCurrentJob] = useState<Job>(initialJob);
    const [dragX, setDragX] = useState(0);
    const [dragging, setDragging] = useState(false);
    const [flying, setFlying] = useState<"left" | "right" | null>(null);
    const [entering, setEntering] = useState(false);
    const startX = useRef(0);
    const cardRef = useRef<HTMLDivElement>(null);

    async function swipe(direction: "left" | "right") {
      if (flying) return;
      setFlying(direction);
      setDragging(false);
      onSwipeStart?.();

      try {
        const [nextJob] = await Promise.all([
          direction === "right" ? onAccept(currentJob.id) : onReject(currentJob.id),
          new Promise((resolve) => setTimeout(resolve, FLY_DURATION)),
        ]);

        if (nextJob) setCurrentJob(nextJob);
        setFlying(null);
        setDragX(0);
        setEntering(true);
        requestAnimationFrame(() => requestAnimationFrame(() => {
          setEntering(false);
          onSwiped(direction);
        }));
      } catch {
        setFlying(null);
        setDragX(0);
      }
    }

    useImperativeHandle(ref, () => ({ swipe }));

    function onPointerDown(e: React.PointerEvent) {
      if (flying) return;
      startX.current = e.clientX;
      setDragging(true);
      cardRef.current?.setPointerCapture(e.pointerId);
    }

    function onPointerMove(e: React.PointerEvent) {
      if (!dragging || flying) return;
      setDragX(e.clientX - startX.current);
    }

    function onPointerUp() {
      if (!dragging) return;
      setDragging(false);
      if (dragX > SWIPE_THRESHOLD) swipe("right");
      else if (dragX < -SWIPE_THRESHOLD) swipe("left");
      else setDragX(0);
    }

    let cardTransform: string;
    let cardOpacity: number;
    let cardTransition: string;

    if (flying === "right") {
      cardTransform = "translateX(600px) rotate(30deg)";
      cardOpacity = 0;
      cardTransition = `transform ${FLY_DURATION}ms ease, opacity ${FLY_DURATION}ms ease`;
    } else if (flying === "left") {
      cardTransform = "translateX(-600px) rotate(-30deg)";
      cardOpacity = 0;
      cardTransition = `transform ${FLY_DURATION}ms ease, opacity ${FLY_DURATION}ms ease`;
    } else if (entering) {
      cardTransform = "scale(0.95)";
      cardOpacity = 0;
      cardTransition = "none";
    } else {
      cardTransform = `translateX(${dragX}px) rotate(${dragX / 20}deg)`;
      cardOpacity = 1 - Math.min(0.5, Math.abs(dragX) / 400);
      cardTransition = dragging ? "none" : "transform 0.3s ease, opacity 0.3s ease";
    }

    const stampOpacity = Math.min(1, Math.abs(dragX) / SWIPE_THRESHOLD);
    const isAccepting = dragX > 0;
    const stampColor = isAccepting ? "#52c41a" : "#ff4d4f";
    const stampText = isAccepting ? "SAVE" : "PASS";
    const avatarName = currentJob.company || currentJob.position;
    const avatarBg = currentJob.logo ? undefined : avatarColor(avatarName);

    return (
      <div
        ref={cardRef}
        onPointerDown={onPointerDown}
        onPointerMove={onPointerMove}
        onPointerUp={onPointerUp}
        style={{
          width: 560,
          height: 500,
          background: "#fff",
          borderRadius: 20,
          boxShadow: "0 8px 40px rgba(0,0,0,0.12)",
          padding: 40,
          display: "flex",
          flexDirection: "column",
          cursor: flying ? "default" : dragging ? "grabbing" : "grab",
          userSelect: "none",
          transform: cardTransform,
          opacity: cardOpacity,
          transition: cardTransition,
          position: "relative",
          zIndex: 1,
        }}
      >
        {/* Stamp */}
        <div
          style={{
            position: "absolute",
            top: 24,
            right: 24,
            border: `3px solid ${stampColor}`,
            borderRadius: 8,
            padding: "4px 12px",
            color: stampColor,
            fontWeight: 700,
            fontSize: 20,
            opacity: stampOpacity,
            transform: "rotate(12deg)",
            pointerEvents: "none",
          }}
        >
          {stampText}
        </div>

        {/* Company + position */}
        <div style={{ display: "flex", alignItems: "center", gap: 16, marginBottom: 20 }}>
          <Avatar
            size={56}
            src={currentJob.logo}
            style={{ backgroundColor: avatarBg, fontWeight: 700, flexShrink: 0 }}
          >
            {!currentJob.logo && avatarName.slice(0, 2).toUpperCase()}
          </Avatar>
          <div>
            <Title level={4} style={{ margin: 0 }}>
              {currentJob.company}
            </Title>
            <Text type="secondary" style={{ fontSize: 15 }}>
              {currentJob.position}
            </Text>
            <div style={{ marginTop: 4, display: "flex", gap: 12, flexWrap: "wrap" }}>
              {currentJob.locations.length > 0 && (
                <Text type="secondary" style={{ fontSize: 12 }}>
                  {currentJob.locations[0]}{currentJob.locations.length > 1 ? ` +${currentJob.locations.length - 1}` : ""}
                </Text>
              )}
              {currentJob.deadline && (
                <Text type="secondary" style={{ fontSize: 12 }}>
                  {new Date(currentJob.deadline).toLocaleDateString()}
                </Text>
              )}
            </div>
          </div>
        </div>

        {/* Tags */}
        <div style={{ display: "flex", flexWrap: "wrap", gap: 6, marginBottom: 20 }}>
          {currentJob.languages.map((lang) => (
            <Tag key={lang}>{lang}</Tag>
          ))}
          <Tag color={TYPE_COLORS[currentJob.employment_type] ?? "default"}>
            {currentJob.employment_type}
          </Tag>
          {currentJob.isRemote && <Tag color="cyan">Remote</Tag>}
          {currentJob.isHybrid && <Tag color="geekblue">Hybrid</Tag>}
        </div>

        {/* Description */}
        <div style={{ flex: 1, overflowY: "auto", minHeight: 0 }}>
          <Text type="secondary" style={{ fontSize: 14, lineHeight: 1.7, display: "block" }}>
            {currentJob.description}
          </Text>
        </div>
      </div>
    );
  }
);

SwipeCard.displayName = "SwipeCard";

export default SwipeCard;
