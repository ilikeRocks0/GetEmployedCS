function getSessionPayload(): Record<string, unknown> | null {
  const cookie = document.cookie.split("; ").find((c) => c.startsWith("session="));
  if (!cookie) return null;
  try {
    return JSON.parse(atob(cookie.slice("session=".length)));
  } catch {
    return null;
  }
}

export function getUserIdFromSession(): number | null {
  const payload = getSessionPayload();
  return payload?.userId != null ? Number(payload.userId) : null;
}

export function getSeekerIdFromSession(): number | null {
  const payload = getSessionPayload();
  return payload?.seekerId != null ? Number(payload.seekerId) : null;
}
