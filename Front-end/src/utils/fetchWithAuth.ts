export async function fetchWithAuth(input: RequestInfo | URL, init?: RequestInit): Promise<Response> {
  const res = await fetch(input, { ...init, credentials: "include" });
  if (res.status === 401) {
    sessionStorage.setItem("session-expired", "true");
    window.location.href = "/login";
    throw new Error("Unauthorized");
  }
  return res;
}
