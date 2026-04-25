# FitCoach Backend Improvements Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Enhance the FitCoach API with caching, circuit breaker, health monitoring, CQRS patterns, security upgrades, WebSocket/STOMP real-time messaging, and code quality improvements

**Architecture:** Extend existing layered architecture (Domain/Infrastructure/Services/Controllers) with new infrastructure components while maintaining backward compatibility with REST endpoints

**Tech Stack:**
- .NET 10 (C#)
- MongoDB.Driver 3.7.0
- RabbitMQ.Client 7.2.1 with STOMP plugin enabled
- StackExchange.Redis for caching
- Polly for circuit breaker and retry patterns
- ASP.NET Core WebSockets
- xUnit for testing

---

## Phase 1: Infrastructure Enhancements (Weeks 1-3)

### Task 1: Redis Caching Infrastructure

**Files:**
- Create: `FitCoach.Api/Infrastructure/Caching/ICacheService.cs`
- Create: `FitCoach.Api/Infrastructure/Caching/RedisCacheService.cs`
- Create: `FitCoach.Api.Tests/Infrastructure/Caching/RedisCacheServiceTests.cs`
- Modify: `FitCoach.Api/Program.cs` (register Redis and cache service)

- [ ] **Step 1: Write failing test for RedisCacheService.GetAsync**
- [ ] **Step 2: Run test - expect FAIL (RedisCacheService doesn't exist)**
- [ ] **Step 3: Implement ICacheService interface**
- [ ] **Step 4: Implement RedisCacheService with GetAsync, SetAsync, RemoveAsync, ExistsAsync**
- [ ] **Step 5: Run test - expect PASS**
- [ ] **Step 6: Add Redis configuration to appsettings.json (Redis:Configuration)**
- [ ] **Step 7: Register Redis and cache service in Program.cs**
- [ ] **Step 8: Run test - ensure DI works**
- [ ] **Step 9: Commit: "feat: add Redis caching infrastructure"**

---

### Task 2: Circuit Breaker Pattern for ML Service Calls

**Files:**
- Create: `FitCoach.Api.Tests/Infrastructure/HttpClients/MLServiceClientCircuitBreakerTests.cs`
- Modify: `FitCoach.Api/Infrastructure/HttpClients/MLServiceClient.cs`

- [ ] **Step 1: Write failing test for MLServiceClient with circuit breaker**
- [ ] **Step 2: Run test - expect FAIL (circuit breaker not implemented)**
- [ ] **Step 3: Add Polly using statements to MLServiceClient.cs**
- [ ] **Step 4: Add AsyncCircuitBreakerPolicy and AsyncRetryPolicy fields**
- [ ] **Step 5: Initialize policies in constructor with 3 retries, 30s circuit break**
- [ ] **Step 6: Wrap _httpClient.PostAsync calls in retry and circuit breaker policies**
- [ ] **Step 7: Update PostAsync and PostStringAsync methods**
- [ ] **Step 8: Run test - expect PASS**
- [ ] **Step 9: Commit: "feat: add circuit breaker and retry for ML service resilience"**

---

### Task 3: Health Monitoring System

**Files:**
- Create: `FitCoach.Api/Infrastructure/Health/HealthStatus.cs`
- Create: `FitCoach.Api/Infrastructure/Health/HealthCheckService.cs`
- Create: `FitCoach.Api/Infrastructure/Health/Components/IHealthCheck.cs`
- Create: `FitCoach.Api/Infrastructure/Health/Components/MongoHealthCheck.cs`
- Create: `FitCoach.Api/Infrastructure/Health/Components/RabbitMQHealthCheck.cs`
- Create: `FitCoach.Api/Infrastructure/Health/Components/CacheHealthCheck.cs` (optional)
- Create: `FitCoach.Api.Tests/Infrastructure/Health/HealthCheckServiceTests.cs`
- Create: `FitCoach.Api.Tests/Infrastructure/Health/Components/*UnitTests.cs`
- Create: `FitCoach.Api/Controllers/HealthController.cs`
- Modify: `FitCoach.Api/Program.cs`

- [ ] **Step 1: Write tests for HealthCheckService.CheckAllAsync**
- [ ] **Step 2: Write tests for each health check component**
- [ ] **Step 3: Run tests - expect FAIL (implementations don't exist)**
- [ ] **Step 4: Implement HealthStatus model**
- [ ] **Step 5: Implement IHealthCheck interface**
- [ ] **Step 6: Implement HealthCheckService with CheckAllAsync method**
- [ ] **Step 7: Implement MongoHealthCheck (test DB connection)**
- [ ] **Step 8: Implement RabbitMQHealthCheck (test connection)**
- [ ] **Step 9: Implement CacheHealthCheck (test Redis if configured)**
- [ ] **Step 10: Run component tests - expect PASS**
- [ ] **Step 11: Implement HealthController with /health, /health/live, /health/ready endpoints**
- [ ] **Step 12: Register all health checks in Program.cs**
- [ ] **Step 13: Run all health tests - expect PASS**
- [ ] **Step 14: Commit: "feat: add comprehensive health monitoring system"**

---

## Phase 2: Service Layer Improvements (Weeks 4-5)

### Task 4: Implement CQRS Pattern for Conversations

**Files:**
- Create: `FitCoach.Api/Domain/Queries/GetConversationsQuery.cs`
- Create: `FitCoach.Api/Domain/Queries/GetConversationByIdQuery.cs`
- Create: `FitCoach.Api/Domain/Commands/UpdateConversationCommand.cs`
- Create: `FitCoach.Api/Domain/Commands/DeleteConversationCommand.cs`
- Create: `FitCoach.Api/Services/Queries/IConversationQueryService.cs`
- Create: `FitCoach.Api/Services/Queries/ConversationQueryService.cs`
- Create: `FitCoach.Api/Services/Commands/IConversationCommandService.cs`
- Create: `FitCoach.Api/Services/Commands/ConversationCommandService.cs`
- Modify: `FitCoach.Api/Controllers/ConversationController.cs` (use query/command services)
- Modify: `FitCoach.Api/Program.cs` (register new services)

- [ ] **Step 1: Define conversation query DTOs (ConversationSummaryDto, ConversationDetailDto)**
- [ ] **Step 2: Write tests for ConversationQueryService.GetByUserIdAsync**
- [ ] **Step 3: Implement query handlers with projection to reduce data transfer**
- [ ] **Step 4: Write tests for ConversationCommandService.UpdateAsync**
- [ ] **Step 5: Implement command handlers with proper validation**
- [ ] **Step 6: Update ConversationController to use query/command pattern**
- [ ] **Step 7: Run all conversation tests - expect PASS**
- [ ] **Step 8: Commit: "feat: implement CQRS pattern for conversation operations"**

---

### Task 5: MongoDB Projection Optimization

**Files:**
- Modify: `FitCoach.Api/Infrastructure/Repositories/ConversationRepository.cs`
- Modify: `FitCoach.Api/Infrastructure/Repositories/UserProfileRepository.cs`
- Modify: `FitCoach.Api/Infrastructure/Repositories/TrainingPlanRepository.cs`
- Create: `FitCoach.Api/Domain/Queries/ProjectionDefinitions.cs`

- [ ] **Step 1: Analyze current queries in repositories**
- [ ] **Step 2: Create projection definitions for each repository**
- [ ] **Step 3: Modify GetByUserIdAsync in ConversationRepository to use projection**
- [ ] **Step 4: Modify GetByIdAsync to use projection for specific fields**
- [ ] **Step 5: Update repository tests to verify projections**
- [ ] **Step 6: Run all tests - expect PASS**
- [ ] **Step 7: Commit: "perf: optimize MongoDB queries with projection"**

---

### Task 6: Transaction Management for Batch Operations

**Files:**
- Modify: `FitCoach.Api/Infrastructure/Repositories/ConversationRepository.cs`
- Modify: `FitCoach.Api/Infrastructure/MongoDB/MongoDbContext.cs`

- [ ] **Step 1: Add IClientSessionHandle field to MongoDbContext**
- [ ] **Step 2: Add BeginTransactionAsync and CommitAsync methods to MongoDbContext**
- [ ] **Step 3: Update ConversationRepository to use transactions for CreateAsync + MessageAdd**
- [ ] **Step 4: Write integration test for transactional batch operations**
- [ ] **Step 5: Run integration tests - expect PASS**
- [ ] **Step 6: Commit: "feat: add transaction support for batch operations"**

---

## Phase 3: Security Enhancements (Weeks 6-7)

### Task 7: JWT Security Hardening

**Files:**
- Create: `FitCoach.Api/Security/RefreshTokenService.cs`
- Create: `FitCoach.Api/Security/ITokenBlacklistService.cs`
- Create: `FitCoach.Api/Security/TokenBlacklistService.cs`
- Modify: `FitCoach.Api/Program.cs` (JWT configuration)
- Modify: `FitCoach.Api/Controllers/AuthController.cs` (if exists, otherwise create)

- [ ] **Step 1: Research and design refresh token rotation strategy**
- [ ] **Step 2: Write tests for refresh token generation and validation**
- [ ] **Step 3: Implement RefreshTokenService with rotation logic**
- [ ] **Step 4: Implement token blacklist for revocation**
- [ ] **Step 5: Add JwtBearer events to validate blacklist in Program.cs**
- [ ] **Step 6: Add refresh token endpoint to AuthController**
- [ ] **Step 7: Add token revocation endpoint**
- [ ] **Step 8: Update ChatController and ProfileController with [Authorize]**
- [ ] **Step 9: Run security tests - expect PASS**
- [ ] **Step 10: Commit: "feat: add refresh token rotation and blacklist support"**

---

### Task 8: Rate Limiting Implementation

**Files:**
- Create: `FitCoach.Api/Middleware/RateLimitMiddleware.cs`
- Modify: `FitCoach.Api/Program.cs`
- Create: `FitCoach.Api.Tests/Middleware/RateLimitMiddlewareTests.cs`

- [ ] **Step 1: Write tests for rate limiting middleware (per IP, per user)**
- [ ] **Step 2: Implement in-memory rate limiter with sliding window**
- [ ] **Step 3: Add middleware to pipeline in Program.cs**
- [ ] **Step 4: Apply rate limiting to auth endpoints (10 req/min)**
- [ ] **Step 5: Apply rate limiting to chat endpoints (60 req/min)**
- [ ] **Step 6: Add configuration options for limits**
- [ ] **Step 7: Run rate limit tests - expect PASS**
- [ ] **Step 8: Commit: "feat: implement rate limiting for API endpoints"**

---

### Task 9: Input Validation Enhancement

**Files:**
- Modify: `FitCoach.Api/DTOs/Requests/ChatRequest.cs`
- Modify: `FitCoach.Api/DTOs/Requests/UpdateProfileRequest.cs`
- Modify: `FitCoach.Api/DTOs/Requests/UpdateConversationTitleRequest.cs`
- Create: `FitCoach.Api/Validators/ChatRequestValidator.cs`
- Create: `FitCoach.Api/Validators/UpdateProfileRequestValidator.cs`
- Create: `FitCoach.Api.Tests/Validators/ValidatorsTests.cs`

- [ ] **Step 1: Add [Required], [StringLength], [Range] attributes to DTOs**
- [ ] **Step 2: Write FluentValidation validators for complex rules**
- [ ] **Step 3: Register validators in Program.cs**
- [ ] **Step 4: Add validation filter to controller pipeline**
- [ ] **Step 5: Write tests for validation rules**
- [ ] **Step 6: Run validation tests - expect PASS**
- [ ] **Step 7: Commit: "feat: add comprehensive input validation"**

---

## Phase 4: Real-Time Messaging with WebSocket/STOMP (Weeks 8-10)

### Task 10: WebSocket Infrastructure Setup

**Files:**
- Create: `FitCoach.Api/Infrastructure/WebSockets/WebSocketConnectionManager.cs`
- Create: `FitCoach.Api/Infrastructure/WebSockets/WebSocketConnection.cs`
- Modify: `FitCoach.Api/Program.cs` (enable WebSocket)
- Create: `FitCoach.Api/Controllers/WebSocketController.cs`
- Create: `FitCoach.Api.Tests/Infrastructure/WebSockets/WebSocketConnectionManagerTests.cs`

- [ ] **Step 1: Write tests for connection management (add, remove, get by userId)**
- [ ] **Step 2: Implement WebSocketConnection class (Id, UserId, Socket, ConnectedAt)**
- [ ] **Step 3: Implement WebSocketConnectionManager with thread-safe ConcurrentDictionary**
- [ ] **Step 4: Enable WebSocket in Program.cs (app.UseWebSockets())**
- [ ] **Step 5: Create WebSocketController with /ws endpoint**
- [ ] **Step 6: Implement WebSocket handshake with JWT validation**
- [ ] **Step 7: Implement connection lifecycle (accept, add to manager, cleanup)**
- [ ] **Step 8: Add ping/pong heartbeat mechanism**
- [ ] **Step 9: Run WebSocket tests - expect PASS**
- [ ] **Step 10: Commit: "feat: add WebSocket connection infrastructure"**

---

### Task 11: STOMP Protocol Implementation

**Files:**
- Create: `FitCoach.Api/Infrastructure/WebSockets/Stomp/StompFrame.cs`
- Create: `FitCoach.Api/Infrastructure/WebSockets/Stomp/StompCommand.cs`
- Create: `FitCoach.Api/Infrastructure/WebSockets/Stomp/IStompMessageHandler.cs`
- Create: `FitCoach.Api/Infrastructure/WebSockets/Stomp/StompMessageHandler.cs`
- Modify: `FitCoach.Api/Controllers/WebSocketController.cs` (integrate STOMP)
- Create: `FitCoach.Api.Tests/Infrastructure/WebSockets/Stomp/StompMessageHandlerTests.cs`

- [ ] **Step 1: Define STOMP frame structure (command, headers, body)**
- [ ] **Step 2: Write tests for parsing STOMP frames from byte streams**
- [ ] **Step 3: Implement StompFrame parsing logic**
- [ ] **Step 4: Write tests for STOMP command handling (CONNECT, SUBSCRIBE, SEND, DISCONNECT)**
- [ ] **Step 5: Implement IStompMessageHandler interface**
- [ ] **Step 6: Implement StompMessageHandler with command routing**
- [ ] **Step 7: Integrate STOMP handler into WebSocketController**
- [ ] **Step 8: Update WebSocketConnectionManager to track STOMP sessions**
- [ ] **Step 9: Run STOMP tests - expect PASS**
- [ ] **Step 10: Commit: "feat: add STOMP protocol support over WebSocket"**

---

### Task 12: STOMP-to-RabbitMQ Integration

**Files:**
- Create: `FitCoach.Api/Infrastructure/WebSockets/Stomp/StompBrokerBridge.cs`
- Modify: `FitCoach.Api/Infrastructure/Messaging/InjuryAlertPublisher.cs` (extend)
- Create: `FitCoach.Api/Infrastructure/WebSockets/Stomp/StompDestinationRouter.cs`
- Create: `FitCoach.Api.Tests/Infrastructure/WebSockets/Stomp/StompBrokerBridgeTests.cs`

- [ ] **Step 1: Write tests for STOMP destination routing**
- [ ] **Step 2: Implement StompDestinationRouter (map /topic/* to RabbitMQ exchanges)**
- [ ] **Step 3: Implement StompBrokerBridge to forward STOMP messages to RabbitMQ**
- [ ] **Step 4: Configure RabbitMQ to enable STOMP plugin**
- [ ] **Step 5: Add STOMP connection factory (port 61613 or 61614)**
- [ ] **Step 6: Connect StompBrokerBridge to RabbitMQ on startup**
- [ ] **Step 7: Handle RabbitMQ messages and forward to WebSocket clients**
- [ ] **Step 8: Implement message persistence to RabbitMQ**
- [ ] **Step 9: Add dead-letter queue handling**
- [ ] **Step 10: Run STOMP broker bridge tests - expect PASS**
- [ ] **Step 11: Add RabbitMQ health check integration**
- [ ] **Step 12: Commit: "feat: integrate STOMP with RabbitMQ message broker"**

---

### Task 13: Real-Time Message Types and Destinations

**Files:**
- Create: `FitCoach.Api/Domain/Events/InjuryAlertEvent.cs`
- Create: `FitCoach.Api/Domain/Events/WorkoutProgressEvent.cs`
- Create: `FitCoach.Api/Domain/Events/ConversationMessageEvent.cs`
- Create: `FitCoach.Api/Domain/Events/GoalAchievedEvent.cs`
- Modify: `FitCoach.Api/Infrastructure/WebSockets/Stomp/StompDestinationRouter.cs`
- Create: `FitCoach.Api/Services/RealTimeNotificationService.cs`
- Create: `FitCoach.Api/Interfaces/IRealTimeNotificationService.cs`
- Modify: `FitCoach.Api/Services/InjuryPredictionService.cs` (publish real-time alerts)
- Modify: `FitCoach.Api/Services/ChatService.cs` (publish messages in real-time)

- [ ] **Step 1: Define event types with serializable properties**
- [ ] **Step 2: Write tests for RealTimeNotificationService**
- [ ] **Step 3: Implement RealTimeNotificationService to send via WebSocket**
- [ ] **Step 4: Add destination routing rules for each event type**
- [ ] **Step 5: Update InjuryPredictionService to publish real-time alerts**
- [ ] **Step 6: Update ChatService to broadcast conversation messages**
- [ ] **Step 7: Add subscription management (user joins/leaves conversation topics)**
- [ ] **Step 8: Implement message queuing for offline users**
- [ ] **Step 9: Run real-time notification tests - expect PASS**
- [ ] **Step 10: Commit: "feat: add real-time event publishing and subscription management"**

---

### Task 14: Fallback to Long Polling

**Files:**
- Create: `FitCoach.Api/Infrastructure/WebSockets/LongPollingHandler.cs`
- Create: `FitCoach.Api/Controllers/LongPollingController.cs`
- Modify: `FitCoach.Api/Program.cs` (register long polling endpoint)

- [ ] **Step 1: Design fallback strategy (WebSocket fails → HTTP long poll)**
- [ ] **Step 2: Write tests for LongPollingHandler**
- [ ] **Step 3: Implement LongPollingHandler with 30s timeout**
- [ ] **Step 4: Create LongPollingController with /poll endpoint**
- [ ] **Step 5: Integrate with same RealTimeNotificationService**
- [ ] **Step 6: Add heartbeat and reconnection logic**
- [ ] **Step 7: Test fallback from WebSocket to long polling**
- [ ] **Step 8: Commit: "feat: add long polling fallback for real-time messaging"**

---

### Task 15: WebSocket Security

**Files:**
- Modify: `FitCoach.Api/Controllers/WebSocketController.cs`
- Modify: `FitCoach.Api/Infrastructure/WebSockets/WebSocketConnectionManager.cs`
- Create: `FitCoach.Api/Middleware/WebSocketRateLimitMiddleware.cs`

- [ ] **Step 1: Enforce WSS (secure WebSocket) in production**
- [ ] **Step 2: Validate JWT during WebSocket handshake**
- [ ] **Step 3: Implement per-user connection limits (max 3 connections)**
- [ ] **Step 4: Add global connection pool limit (1000 concurrent)**
- [ ] **Step 5: Implement message rate limiting (500 msg/sec/user)**
- [ ] **Step 6: Add connection timeout (10 min idle disconnect)**
- [ ] **Step 7: Sanitize all incoming STOMP messages**
- [ ] **Step 8: Write security integration tests**
- [ ] **Step 9: Commit: "feat: harden WebSocket/STOMP security"**

---

## Phase 5: Code Quality & Testing (Weeks 11-12)

### Task 16: Controller Improvements

**Files:**
- Modify: `FitCoach.Api/Controllers/ChatController.cs`
- Modify: `FitCoach.Api/Controllers/ProfileController.cs`
- Modify: `FitCoach.Api/Controllers/ConversationController.cs`

- [ ] **Step 1: Remove hardcoded userId in ChatController**
- [ ] **Step 2: Inject user ID via controller base or JWT claims**
- [ ] **Step 3: Add [Authorize] attribute to all protected controllers**
- [ ] **Step 4: Add try-catch with logging in all endpoints**
- [ ] **Step 5: Commit: "refactor: centralize userId, add auth and error handling"**

---

### Task 17: Service Layer Improvements

**Files:**
- Modify: `FitCoach.Api/Services/ChatService.cs`
- Modify: `FitCoach.Api/Services/ProfileService.cs`
- Modify: `FitCoach.Api/Services/ConversationService.cs`

- [ ] **Step 1: Ensure all async methods are properly async**
- [ ] **Step 2: Add exception handling with logging**
- [ ] **Step 3: Add circuit breaker already implemented in MLServiceClient**
- [ ] **Step 4: Add Redis caching where appropriate**
- [ ] **Step 5: Commit: "refactor: improve async patterns and error handling"**

---

### Task 18: DTO Validation

**Files:**
- Modify: `FitCoach.Api/DTOs/Requests/ChatRequest.cs`
- Modify: `FitCoach.Api/DTOs/Requests/UpdateProfileRequest.cs`
- Modify: `FitCoach.Api/DTOs/Requests/UpdateConversationTitleRequest.cs`
- Modify: `FitCoach.Api/DTOs/Requests/*.cs` (all request DTOs)

- [ ] **Step 1: Add [Required] to all non-nullable fields**
- [ ] **Step 2: Add [StringLength] with reasonable max lengths**
- [ ] **Step 3: Add [Range] for numeric fields**
- [ ] **Step 4: Add custom validation for business rules (age > 0, etc.)**
- [ ] **Step 5: Write validation tests**
- [ ] **Step 6: Commit: "refactor: add comprehensive DTO validation"**

---

### Task 19: Repository Performance

**Files:**
- Modify: `FitCoach.Api/Infrastructure/Repositories/ConversationRepository.cs:30-40`
- Modify: `FitCoach.Api/Infrastructure/Repositories/UserProfileRepository.cs`
- Modify: `FitCoach.Api/Infrastructure/Repositories/TrainingPlanRepository.cs`

- [ ] **Step 1: Use projection in GetByUserIdAsync (only return needed fields for lists)**
- [ ] **Step 2: Add pagination support (skip, take)**
- [ ] **Step 3: Add index hints for frequently queried fields**
- [ ] **Step 4: Add async optimization (use async methods properly)**
- [ ] **Step 5: Write performance benchmarks**
- [ ] **Step 6: Commit: "perf: optimize repository queries with projection and pagination"**

---

### Task 20: Comprehensive Testing

**Files:**
- Create: `FitCoach.Api.Tests/Integration/FullWorkflowTests.cs`
- Create: `FitCoach.Api.Tests/Integration/RealTimeMessagingTests.cs`
- Create: `FitCoach.Api.Tests/E2E/ApiEndToEndTests.cs`
- Modify: `FitCoach.Api.Tests/UnitTests/*` (add missing coverage)

- [ ] **Step 1: Write integration tests for full conversation flow**
- [ ] **Step 2: Write integration tests with real Redis cache**
- [ ] **Step 3: Write end-to-end tests with Testcontainers (MongoDB, RabbitMQ)**
- [ ] **Step 4: Write real-time messaging integration tests**
- [ ] **Step 5: Run all tests and fix coverage gaps**
- [ ] **Step 6: Add 80%+ unit test coverage**
- [ ] **Step 7: Add 60%+ integration test coverage**
- [ ] **Step 8: Commit: "test: comprehensive test coverage"**

---

## Phase 6: Documentation & Deployment (Weeks 13-14)

### Task 21: API Documentation Updates

**Files:**
- Create: `docs/api/real-time-messaging.md`
- Update: `docs/api/README.md` (if exists)
- Modify: `FitCoach.Api/Controllers/WebSocketController.cs` (XML comments)
- Modify: `FitCoach.Api/Controllers/HealthController.cs` (XML comments)

- [ ] **Step 1: Document WebSocket connection protocol**
- [ ] **Step 2: Document STOMP message format and destinations**
- [ ] **Step 3: Document health check endpoints and expected responses**
- [ ] **Step 4: Add XML comments to all new public APIs**
- [ ] **Step 5: Generate API documentation with Swagger/Scalar**
- [ ] **Step 6: Commit: "docs: add real-time messaging and health monitoring docs"**

---

### Task 22: Environment Configuration

**Files:**
- Create: `docker-compose.override.yml` (adds Redis, RabbitMQ STOMP)
- Update: `FitCoach.Api/appsettings.json`
- Update: `FitCoach.Api/appsettings.Development.json`
- Create: `.env.example` (environment variables)

- [ ] **Step 1: Add Redis configuration to docker-compose**
- [ ] **Step 2: Enable STOMP plugin in RabbitMQ service**
- [ ] **Step 3: Add environment variables for Redis and STOMP ports**
- [ ] **Step 4: Configure health check intervals**
- [ ] **Step 5: Configure circuit breaker thresholds**
- [ ] **Step 6: Add rate limit configuration**
- [ ] **Step 7: Document configuration in README**
- [ ] **Step 8: Commit: "feat: add Redis and STOMP configuration"**

---

### Task 23: Monitoring & Logging

**Files:**
- Modify: `FitCoach.Api/Program.cs` (structured logging)
- Create: `FitCoach.Api/Infrastructure/Logging/SerilogConfiguration.cs` (optional)
- Modify: `FitCoach.Api/Infrastructure/HttpClients/MLServiceClient.cs` (add metrics)
- Modify: `FitCoach.Api/Infrastructure/WebSockets/*.cs` (add connection metrics)
- Create: `FitCoach.Api/Metrics/MetricsCollector.cs`

- [ ] **Step 1: Add structured logging with Serilog/ILogger**
- [ ] **Step 2: Log WebSocket connection/disconnection events**
- [ ] **Step 3: Log STOMP message routing**
- [ ] **Step 4: Add circuit breaker state change logging**
- [ ] **Step 5: Add cache hit/miss metrics**
- [ ] **Step 6: Create Prometheus metrics endpoint (optional)**
- [ ] **Step 7: Commit: "feat: add comprehensive logging and metrics"**

---

## Implementation Notes

**Prerequisites:**
- Redis server running on localhost:6379
- RabbitMQ with STOMP plugin enabled (port 61613/61614)
- MongoDB instance accessible

**Testing Strategy:**
- Use Testcontainers for integration tests
- Mock external services with Moq for unit tests
- Use in-memory Redis for test environment

**Rollout Plan:**
1. Deploy infrastructure changes (Redis, STOMP) first
2. Deploy caching and circuit breaker (canary release)
3. Deploy health monitoring
4. Deploy CQRS pattern with feature flag
5. Deploy WebSocket/STOMP alongside REST (gradual traffic shift)
6. Deploy security improvements

**Risk Mitigation:**
- All changes backward compatible with REST
- WebSocket optional - existing clients unaffected
- Circuit breaker includes fallback behavior
- Health checks enable monitoring during rollout
- Feature flags for CQRS and WebSocket enable quick rollback

**Estimated Time:** 13-14 weeks with full testing and documentation

---

**Next Step:** Save this plan and ask user to review before implementation with subagent-driven development.