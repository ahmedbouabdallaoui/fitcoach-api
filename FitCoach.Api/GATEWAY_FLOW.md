# How the Frontend → API Gateway → Backend Flow Works

## Current Architecture (Temporary)

```
Frontend ──HTTP/JSON──▶ FitCoach.API (:5001)
                          ├── JWT validated locally (Program.cs L20-35)
                          └── userId hardcoded to "test-user-123"
```

The backend does both JWT validation and business logic. The frontend points directly at the API. Not a problem for development, but not production-ready.

---

## Target Architecture

```
Frontend ──HTTPS──▶ YARP Gateway (:5000) ──HTTPS──▶ FitCoach.API (:5001)
                      ├── Validates JWT                    └── No JWT code
                      ├── Extracts user claims             └── No hardcoded userId
                      ├── Forwards as headers              └── Reads user from injected headers
                      ├── Routes to correct service
                      └── Handles CORS, rate limits, etc.
```

The gateway is a thin layer. It does not contain business logic. It handles infrastructure concerns (auth, routing, cross-cutting middleware) and passes clean requests downstream.

---

## Request Lifecycle (Step by Step)

### 1. Frontend makes a call

```ts
// Frontend sends request to the gateway — NOT directly to FitCoach.API
const response = await fetch("https://gateway.example.com/api/chat", {
  method: "POST",
  headers: {
    "Authorization": "Bearer <jwt-from-auth-provider>",
    "Content-Type": "application/json"
  },
  body: JSON.stringify({ message: "How do I bench?", tag: "training" })
});
```

The frontend only knows:
- The **gateway URL** (base URL)
- The **JWT token** (obtained from the login flow)
- The **API paths** (`/api/chat`, `/api/profile`, `/api/conversations`)

The frontend does **not** know the FitCoach.API exists.

### 2. Gateway receives the request

YARP intercepts the request. Before routing, it validates the JWT:

- **Valid JWT** → Extract claims (`userId`, `userName`, `role`, etc.) → Inject them as outbound HTTP headers
- **Invalid/missing JWT** → Returns `401 Unauthorized` directly, never touches FitCoach.API

### 3. Gateway injects user headers

After validating the JWT, YARP adds headers to the request before forwarding:

```
X-User-Id: sub-from-jwt-token
X-User-Name: name-from-jwt-token
X-User-Roles: role-from-jwt-token
```

These headers carry the authenticated user's identity downstream.

### 4. Gateway forwards the request

The original request body, method, and path are preserved. Only the URL changes (gateway routes it to the internal cluster):

```
https://gateway.example.com/api/chat   →   http://localhost:5001/api/chat
```

The forwarded request looks like:

```
POST /api/chat HTTP/1.1
Host: localhost:5001
Authorization: Bearer <jwt-token>          # forwarded as-is
X-User-Id: 507f1f77bcf86cd799439011       # injected by gateway
X-User-Name: Ahmed                          # injected by gateway
Content-Type: application/json

{ "message": "How do I bench?", "tag": "training", "conversationId": null }
```

### 5. FitCoach.API receives the request

FitCoach.API no longer performs JWT validation (removed from Program.cs). It reads `X-User-Id` from the incoming headers:

```csharp
// ChatController.cs
var userId = HttpContext.Request.Headers["X-User-Id"];    // "507f1f77..."
var userName = HttpContext.Request.Headers["X-User-Name"]; // "Ahmed"
// Not: var userId = "test-user-123";
```

Business logic executes normally. No auth code.

### 6. FitCoach.API returns response

```json
{
  "conversationId": "65f1c2d4e7b8a1b2c3d4e5f6",
  "message": "To improve your bench press...",
  "messageType": "training",
  "isGenerating": false
}
```

### 7. Gateway passes response through

YARP does not modify the response body. It simply relays it back to the frontend with the same status code.

### 8. Frontend receives the response

No difference in how the frontend consumes the response — the shape is identical. The frontend still calls `POST /api/chat` and gets the same `ChatResponse`.

---

## What Changes vs. What Stays the Same

| Component | Changes | Stays the Same |
|-----------|---------|----------------|
| **Frontend** | Base URL changes (now points to gateway) | JSON request/response shapes, headers (just `Authorization`), paths (`/api/...`) |
| **YARP Gateway** | Created new, handles JWT validation, routing, header injection | N/A |
| **FitCoach.API** | Removes JWT validation code, reads `X-User-*` headers, removes hardcoded userId | Business logic, DTOs, controllers, response shapes |

---

## Key Points for Frontend Developer

1. **Same API contract**: Everything in `API_REFERENCE.md` remains valid. No change to request shapes or response shapes.
2. **Different base URL**: Frontend changes its `API_BASE_URL` from the direct FitCoach.API URL to the gateway URL.
3. **Auth unchanged from frontend perspective**: Frontend still sends `Authorization: Bearer <token>`. The gateway handles validation.
4. **No new endpoints exposed**: The gateway only forwards existing routes. Frontend does not interact with any "gateway-specific" endpoints.

---

## What YARP Does NOT Do

- Does not modify request bodies
- Does not modify response bodies
- Does not implement business logic
- Does not generate new APIs
- Does not store state (no session management)

It is a transparent proxy with authentication middleware.

---

Last updated: 2026-04-02
