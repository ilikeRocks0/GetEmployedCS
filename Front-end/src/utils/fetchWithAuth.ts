export async function fetchWithAuth(input: RequestInfo | URL, init?: RequestInit): Promise<Response> {
  const res = await fetch(input, { ...init, credentials: "include" });
  if (res.status === 401) {
    const { pathname } = window.location;
    if (pathname !== "/login" && pathname !== "/signup") {
      sessionStorage.setItem("session-expired", "true");
      window.location.href = "/login";
    }
    throw new Error("Unauthorized");
  }
  return res;
}
