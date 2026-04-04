
import { useState } from "react";
import Link from "next/link";
import { Button, Avatar, Typography, Spin, Input } from "antd";
import { CommentOutlined, SendOutlined } from "@ant-design/icons";
import type { Comment } from "@/types/JobComment";

const { Text } = Typography;
const { TextArea } = Input;

interface CommentListProps {
  getComments: () => Promise<Comment[]>;
  createComment: (comment: string) => Promise<Comment>;
}

export default function CommentList({ getComments, createComment }: CommentListProps) {
    const [showComments, setShowComments] = useState(false);
    const [comments, setComments] = useState<Comment[]>([]);
    const [commentsLoading, setCommentsLoading] = useState(false);
    const [newComment, setNewComment] = useState("");
    const [submitting, setSubmitting] = useState(false);

    async function handleToggleComments() {
    if (!showComments && comments.length === 0) {
      setCommentsLoading(true);
      try {
        const fetched = await getComments();
        setComments(fetched);
      } finally {
        setCommentsLoading(false);
      }
    }
    setShowComments((prev) => !prev);
  }

  async function handleSubmitComment() {
    if (!newComment.trim()) return;
    setSubmitting(true);
    try {
      const posted = await createComment(newComment.trim());
      setComments((prev) => [...prev, posted]);
      setNewComment("");
    } finally {
      setSubmitting(false);
    }
  }

  return(
    <div>
        <Button
        icon={<CommentOutlined />}
        onClick={handleToggleComments}
        loading={commentsLoading}
      >
        {showComments ? "Hide Comments" : "View Comments"}
      </Button>

      {showComments && (
        <div style={{ marginTop: 16 }}>
          {commentsLoading ? (
            <Spin style={{ display: "block" }} />
          ) : comments.length === 0 ? (
            <Text type="secondary">No comments yet.</Text>
          ) : (
            <div style={{ display: "flex", flexDirection: "column", gap: 12, marginBottom: 16 }}>
              {comments.map((c, i) => (
                <div key={i} style={{ display: "flex", alignItems: "flex-start", gap: 12 }}>
                  <Link href={`/profile/${c.posterUsername}`}>
                    <Avatar style={{ cursor: "pointer" }}>{(c.posterUsername ?? "?").slice(0, 2).toUpperCase()}</Avatar>
                  </Link>
                  <div>
                    {c.posterUsername && (
                      <Link href={`/profile/${c.posterUsername}`} style={{ color: "#1677ff", fontWeight: 600, display: "block" }}>
                        {c.posterUsername}
                      </Link>
                    )}
                    <Text>{c.comment}</Text>
                  </div>
                </div>
              ))}
            </div>
          )}

          <div style={{ display: "flex", gap: 8, marginTop: 12 }}>
            <TextArea
              value={newComment}
              onChange={(e) => setNewComment(e.target.value)}
              placeholder="Add a comment..."
              autoSize={{ minRows: 1, maxRows: 4 }}
              style={{ flex: 1 }}
            />
            <Button
              type="primary"
              icon={<SendOutlined />}
              loading={submitting}
              disabled={!newComment.trim()}
              onClick={handleSubmitComment}
            />
          </div>
        </div>
      )}
    </div>
  )
}
