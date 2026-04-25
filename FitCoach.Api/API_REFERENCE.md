# FitCoach API Reference

A complete reference for frontend developers to consume the FitCoach backend. This document describes all available endpoints, request payload shapes, and response structures.

**Base URL:** `https://your-api-domain.com`  
**Auth:** JWT Bearer token (temporary — will be handled by YARP gateway)

---

## Table of Contents

1. [Chat](#chat)
   - [POST /api/chat](#post-apichat)
2. [Profile](#profile)
   - [GET /api/profile](#get-apiprofile)
   - [PUT /api/profile](#put-apiprofile)
3. [Conversations](#conversations)
   - [GET /api/conversations](#get-apiconversations)
   - [GET /api/conversations/{conversationId}](#get-apiconversationsconversationid)
   - [DELETE /api/conversations/{conversationId}](#delete-apiconversationsconversationid)

---

## Chat

### POST /api/chat

Send a message to the FitCoach AI coach.

**Headers:**  
`Authorization: Bearer <jwt-token>`  
`Content-Type: application/json`

**Request Body:**

```json
{
  "message": "How do I improve my bench press?",
  "conversationId": null,
  "tag": "training"
}
```

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `message` | string | yes | The user's message to the AI coach (max 2000 characters) |
| `conversationId` | string \| null | no | `null` creates a new conversation; existing string continues one |
| `tag` | string \| null | no | RAG context filter. Options: `"training"`, `"nutrition"`, `"injury"`, or `null` for general chat |

**Response:**

```json
{
  "conversationId": "65f1c2d4e7b8a1b2c3d4e5f6",
  "message": "To improve your bench press, focus on progressive overload...",
  "messageType": "training",
  "isGenerating": false,
  "productSuggestions": null
}
```

| Property | Type | Description |
|----------|------|-------------|
| `conversationId` | string | Unique conversation identifier |
| `message` | string | AI's reply (Groq-formatted response) |
| `messageType` | string | Type of response: `"question"`, `"plan"`, `"nutrition"`, `"injury"`, `"chat"` |
| `isGenerating` | boolean | `true` while ML services are processing (async follow-up) |
| `productSuggestions` | `EStoreProductCard[] \| null` | Optional product recommendations |

**Product Card** (when present):
```json
{
  "productId": "prod_123",
  "name": "Adjustable Dumbbell Set",
  "price": "$299.99",
  "rating": 4.5,
  "imageUrl": "https://...",
  "url": "https://store.fascoach.com/..."
}
```

---

## Profile

### GET /api/profile

Retrieve the authenticated user's fitness profile.

**Headers:**  
`Authorization: Bearer <jwt-token>`

**Response (200 OK):**

```json
{
  "id": "65f1c2d4e7b8a1b2c3d4e5f6",
  "userId": "test-user-123",
  "age": 28,
  "weightKg": 78.5,
  "heightCm": 180.0,
  "gender": "Male",
  "bodyFatPercentage": 17.3,
  "fitnessLevel": "intermediate",
  "isComplete": true,
  "createdAt": "2025-01-15T10:30:00Z",
  "updatedAt": "2025-01-16T14:45:00Z"
}
```

| Property | Type | Description |
|----------|------|-------------|
| `id` | string | MongoDB document ID |
| `userId` | string | User identifier (from auth) |
| `age` | int \| null | Optional |
| `weightKg` | double \| null | Optional, in kilograms |
| `heightCm` | double \| null | Optional, in centimeters |
| `gender` | string \| null | Optional, e.g., `"Male"` / `"Female"` |
| `bodyFatPercentage` | double \| null | Optional |
| `fitnessLevel` | string \| null | Optional: `"beginner"`, `"intermediate"`, `"advanced"` |
| `isComplete` | boolean | `true` when required profile fields are filled |
| `createdAt` | datetime (ISO) | Creation timestamp |
| `updatedAt` | datetime? (ISO) | Last update timestamp |

---

### PUT /api/profile

Update the user's profile. Only provided fields are modified.

**Headers:**  
`Authorization: Bearer <jwt-token>`  
`Content-Type: application/json`

**Request Body:**

```json
{
  "age": 29,
  "weightKg": 77.0,
  "heightCm": 180.0,
  "gender": "Male",
  "fitnessLevel": "advanced",
  "bodyFatPercentage": 16.5
}
```

All fields are optional; omit those you don't want to change.

| Property | Type | Description |
|----------|------|-------------|
| `age` | int \| null | Update age |
| `weightKg` | double \| null | Update weight (kg) |
| `heightCm` | double \| null | Update height (cm) |
| `gender` | string \| null | Update gender |
| `fitnessLevel` | string \| null | Update fitness level |
| `bodyFatPercentage` | double \| null | Update body fat % |

**Response (200 OK):** Returns the full updated `UserProfile` object (same shape as GET response).

---

## Conversations

All conversation endpoints use the authenticated user's context — no conversationId is passed to associate ownership.

### GET /api/conversations

List all conversations for the current user.

**Headers:**  
`Authorization: Bearer <jwt-token>`

**Response (200 OK):**

```json
[
  {
    "id": "65f1c2d4e7b8a1b2c3d4e5f6",
    "title": "Bench press technique",
    "messages": [
      {
        "role": "user",
        "content": "How do I improve my bench press?",
        "tag": "training",
        "timestamp": "2025-01-15T10:30:00Z"
      },
      {
        "role": "assistant",
        "content": "To improve your bench press, focus on progressive overload...",
        "tag": "training",
        "timestamp": "2025-01-15T10:30:15Z"
      }
    ],
    "createdAt": "2025-01-15T10:30:00Z",
    "updatedAt": "2025-01-15T10:30:15Z"
  }
]
```

`ConversationResponse` properties:

| Property | Type | Description |
|----------|------|-------------|
| `id` | string | Conversation identifier |
| `title` | string | Auto-generated summary (first user message truncated) |
| `messages` | `MessageResponse[]` | Full message history |
| `createdAt` | datetime (ISO) | When conversation started |
| `updatedAt` | datetime? (ISO) | Last message time |

`MessageResponse`:

| Property | Type | Description |
|----------|------|-------------|
| `role` | string | `"user"` or `"assistant"` |
| `content` | string | Message text |
| `tag` | string \| null | Optional RAG tag that was used |
| `timestamp` | datetime (ISO) | Time the message was sent |

---

### GET /api/conversations/{conversationId}

Fetch a single conversation with its full message history.

**Headers:**  
`Authorization: Bearer <jwt-token>`

**Path Parameters:**

| Parameter | Type | Required |
|-----------|------|----------|
| `conversationId` | string | yes |

**Response (200 OK):** Returns a single `ConversationResponse` object.

**Response (404):** `{ "message": "Conversation not found." }` if `conversationId` doesn't exist or doesn't belong to user.

---

### DELETE /api/conversations/{conversationId}

Permanently delete a conversation.

**Headers:**  
`Authorization: Bearer <jwt-token>`

**Path Parameters:**

| Parameter | Type | Required |
|-----------|------|----------|
| `conversationId` | string | yes |

**Response (204 No Content):** Success, no body.

---

## Notes

- **userId** is currently hardcoded to `"test-user-123"` for development; JWT extraction coming post-YARP gateway.
- **Rate limits:** None currently — but be polite.
- **Environments:** Base URL differs by environment; configure at build time.

---

## TypeScript Definitions (Copy-Paste)

For convenience, here are corresponding TypeScript interfaces:

```ts
export type ChatTag = "training" | "nutrition" | "injury" | null;
export type MessageRole = "user" | "assistant";
export type MessageType = "question" | "plan" | "nutrition" | "injury" | "chat";

export interface ChatRequest {
  message: string;
  conversationId: string | null;
  tag: ChatTag;
}

export interface ChatResponse {
  conversationId: string;
  message: string;
  messageType: MessageType;
  isGenerating: boolean;
  productSuggestions?: EStoreProductCard[] | null;
}

export interface EStoreProductCard {
  productId: string;
  name: string;
  price: string;
  rating: number;
  imageUrl: string;
  url: string;
}

export interface MessageResponse {
  role: MessageRole;
  content: string;
  tag: string | null;
  timestamp: string; // ISO 8601
}

export interface ConversationResponse {
  id: string;
  title: string;
  messages: MessageResponse[];
  createdAt: string;
  updatedAt: string | null;
}

export interface UserProfile {
  id: string;
  userId: string;
  age: number | null;
  weightKg: number | null;
  heightCm: number | null;
  gender: string | null;
  bodyFatPercentage: number | null;
  fitnessLevel: string | null;
  isComplete: boolean;
  createdAt: string;
  updatedAt: string | null;
}

export interface UpdateProfileRequest {
  age: number | null;
  weightKg: number | null;
  heightCm: number | null;
  gender: string | null;
  fitnessLevel: string | null;
  bodyFatPercentage: number | null;
}
```

---

Last updated: 2026-04-02  
Backend version: `main` branch (commit `e551e04`)
