const STUB_USERS: Record<string, { password: string; userId: string; username: string; seekerId: string }> = {
  "johnstubman@gmail.com": { password: "12345", userId: "14", username: "johnstubman", seekerId: "1" },
};

export async function login(email: string, password: string): Promise<void> {
  const user = STUB_USERS[email];
  if (!user || user.password !== password) {
    throw new Error("Invalid email or password.");
  }

  const payload = btoa(JSON.stringify({ userId: user.userId, seekerId: user.seekerId, username: user.username, email }));
  const expires = new Date(Date.now() + 24 * 60 * 60 * 1000).toUTCString();
  document.cookie = `session=${payload}; expires=${expires}; path=/; SameSite=Strict`;
}

export async function logout(): Promise<void> {
  document.cookie = "session=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
}
