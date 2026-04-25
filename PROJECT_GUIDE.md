# FitCoach.Api Project Guide

## 1. Controllers Layer
### ChatController.cs
**Class:** ChatController
**Base Class:** ControllerBase
**Key Endpoints:**
- `POST /api/chat` - SendMessage: Handles user messages to AI assistant
  - Uses ChatRequest DTO for input
  - Returns ChatResponse via IChatService
**Dependencies:**
  - IChatService (business logic)
  - ILogger (logging)
**Key Variables:**
  - `userId`, `userName` (user context)

### ProfileController.cs
**Class:** ProfileController
**Endpoints:**
- `GET /api/profile` - GetProfile: Retrieves user profile
- `PUT /api/profile` - UpdateProfile: Updates profile fields
**DTOs:**
- UpdateProfileRequest (input)
**Dependencies:**
  - IProfileService
  - ILogger

### ConversationController.cs
**Class:** ConversationController
**Endpoints:**
- `GET /api/conversations` - GetConversations: Lists user conversations
- `GET /api/conversations/{id}` - GetConversation: Fetches specific conversation
- `DELETE /api/conversations/{id}` - DeleteConversation
**Key Logic:**
  - Uses ConversationMapper to convert domain entities to DTOs
**Dependencies:**
  - IConversationService
  - ILogger

## 2. Domain Entities
### Conversation.cs
**Class:** Represents a conversation with:
  - ID, UserId, Messages (list of Message entities)
  - Methods for message persistence

### Message.cs
**Class:** Represents a conversation message with:
  - ID, Content, Timestamp, SenderId

### TrainingPlan.cs
**Class:** Manages fitness training plans with:
  - ID, UserId, Exercises, Goals

### UserProfile.cs
**Class:** Stores user profile data including:
  - Age, WeightKg, HeightCm, Gender, FitnessLevel

## 3. DTOs Layer
### ChatRequest.cs
**Purpose:** Input data structure for SendMessage endpoint
**Properties:** Content, Tag (optional context)

### ChatResponse.cs
**Purpose:** Output data structure for API responses
**Properties:** Processed content, AI response details

### UpdateProfileRequest.cs
**Purpose:** Input for profile updates
**Properties:** Age, WeightKg, HeightCm, etc. (nullable)

## 4. Infrastructure Layer
### Services
- **ChatService.cs**: Handles message processing workflow
- **ProfileService.cs**: Manages profile creation/update
- **TrainingPlanService.cs**: Defines plan-related business logic

### Repositories
- **ConversationRepository.cs**: Database operations for conversations
- **UserProfileRepository.cs**: Profile data persistence
**Interfaces:**
  - Define contracts for service/repo interactions

### HttpClients
- **EStoreServiceClient.cs**: Integrates with external services
- **MLServiceClient.cs**: Handles machine learning predictions

## 5. Mappers
### ConversationMapper.cs
**Purpose:** Converts between Conversation DTO and domain entity
**Methods:** ToResponse(), ToDomain()

### UserProfileMapper.cs
**Purpose:** Handles profile data transformation

## 6. Security & Utilities
### EncryptionService.cs
**Function:** Data encryption/decryption

### JwtExtensions.cs
**Function:** JWT token generation/validation

### Shared Patterns
- Dependency injection via constructor parameters
- Logger usage for audit trails

## Key Relationships
1. Controllers → Services → Repositories (data flow)
2. DTOs act as API contract between layers
3. Mappers handle domain-UI data transformation
4. Security layer ensures data protection