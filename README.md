# Google Developer Student Club (GDSC) — NCCUPass App Backend Engineer

**Role:** Backend Engineer (GDSC @ NCCU)  
**When:** 2023–2024 *(club project)*

> **Disclaimer:** This portfolio uses sanitized descriptions and **synthetic examples only**.  
> No proprietary code, credentials, student data, or internal configs are included.

---

## Problem
Campus apps need **reliable and discoverable REST APIs** for login, passes, and basic services. Without clear contracts and guardrails, teams face flaky integrations, slow incident response, and regressions when shipping new features.

---

## P1 — REST API Platform
**Solution:** Built an **ASP.NET Core Web API** with clean layering and OpenAPI docs to power NCCUPass features.

**Highlights:**
- **Architecture:** Controllers → Services → Repositories/Clients; DTOs for requests/responses; model validation.
- **Middleware:** Centralized exception handling and structured logs with correlation IDs.
- **Docs:** **Swagger/OpenAPI** for fast exploration and contract-first collaboration.
- **Config:** Environment-based `appsettings` and secrets; health and readiness endpoints.

**Impact:**
- **Faster iteration:** Clear contracts and mockable layers reduced integration friction.
- **Reliability:** Consistent error payloads and request tracing improved on-call triage.

**Pipeline (simplified)**
```mermaid
flowchart LR
  A[Client] --> B[API Controllers]
  B --> C[Services]
  C --> D[Repositories]
  D --> E[DB or External]
  B --> F[Swagger OpenAPI]
  B --> G[Middleware Logs]
