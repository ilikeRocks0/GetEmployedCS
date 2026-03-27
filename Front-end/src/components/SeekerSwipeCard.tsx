"use client";

import { useState, useRef, forwardRef, useImperativeHandle } from "react";
import { Avatar, Typography, Divider } from "antd";
import type { User } from "@/types/User";

const { Title, Text } = Typography;

const AVATAR_COLORS = ["#1677ff", "#52c41a", "#fa8c16", "#eb2f96", "#722ed1"];
function avatarColor(name: string) {
  const code = name.charCodeAt(0) + (name.charCodeAt(1) ?? 0);
  return AVATAR_COLORS[code % AVATAR_COLORS.length];
}

const SWIPE_THRESHOLD = 100;
const FLY_DURATION = 350;

export interface SeekerSwipeCardHandle {
  swipe: (direction: "left" | "right") => void;
}

interface SeekerSwipeCardProps {
  initialSeeker: User;
  onAccept: (seekerId: number) => Promise<User | null>;
  onReject: (seekerId: number) => Promise<User | null>;
  onSwipeStart?: () => void;
  onSwiped: (direction: "left" | "right") => void;
}

const SeekerSwipeCard = forwardRef<SeekerSwipeCardHandle, SeekerSwipeCardProps>(
  ({ initialSeeker, onAccept, onReject, onSwipeStart, onSwiped }, ref) => {
    const [currentSeeker, setCurrentSeeker] = useState<User>(initialSeeker);
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
        const [nextSeeker] = await Promise.all([
          direction === "right" ? onAccept(currentSeeker.userId) : onReject(currentSeeker.userId),
          new Promise((resolve) => setTimeout(resolve, FLY_DURATION)),
        ]);

        if (nextSeeker) setCurrentSeeker(nextSeeker);
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
    const stampText = isAccepting ? "INTERESTED" : "PASS";

    const displayName = currentSeeker.firstName && currentSeeker.lastName
      ? `${currentSeeker.firstName} ${currentSeeker.lastName}`
      : currentSeeker.username;

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

        {/* Name + username */}
        <div style={{ display: "flex", alignItems: "center", gap: 16, marginBottom: 20 }}>
          <Avatar
            size={56}
            style={{ backgroundColor: avatarColor(displayName), fontWeight: 700, flexShrink: 0 }}
          >
            {displayName.slice(0, 2).toUpperCase()}
          </Avatar>
          <div>
            <Title level={4} style={{ margin: 0 }}>{displayName}</Title>
            <Text type="secondary" style={{ fontSize: 13 }}>@{currentSeeker.username}</Text>
          </div>
        </div>

        {/* Bio */}
        {currentSeeker.bio && (
          <Text style={{ fontSize: 14, lineHeight: 1.6, marginBottom: 16, display: "block" }}>
            {currentSeeker.bio}
          </Text>
        )}

        {/* Experiences */}
        {currentSeeker.experiences && currentSeeker.experiences.length > 0 && (
          <>
            <Divider style={{ margin: "12px 0" }} />
            <div style={{ flex: 1, overflowY: "auto", minHeight: 0 }}>
              <Text type="secondary" style={{ fontSize: 12, display: "block", marginBottom: 8 }}>EXPERIENCE</Text>
              <div style={{ display: "flex", flexDirection: "column", gap: 10 }}>
                {currentSeeker.experiences.map((exp, i) => (
                  <div key={exp.experienceId || i}>
                    <Text strong style={{ display: "block" }}>{exp.positionTitle}</Text>
                    <Text type="secondary" style={{ fontSize: 13 }}>{exp.companyName}</Text>
                    {exp.jobDescription && (
                      <Text type="secondary" style={{ fontSize: 12, display: "block", marginTop: 2 }}>
                        {exp.jobDescription}
                      </Text>
                    )}
                  </div>
                ))}
              </div>
            </div>
          </>
        )}
      </div>
    );
  }
);

SeekerSwipeCard.displayName = "SeekerSwipeCard";

export default SeekerSwipeCard;
