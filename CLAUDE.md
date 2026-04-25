# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

### Build
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

### Run Application
```bash
dotnet run
```

### Check Dependencies
```bash
dotnet restore
```

### Run Single Test
```bash
dotnet test --filter "FitCoach.Api.Tests"
```

## Architecture

The codebase follows a layered architecture with these key components:

1. **Domain Layer**
   - Contains entity classes (e.g., `Seance.cs`, `Exercice.cs`) representing core business objects
   - Includes value objects and domain services

2. **Infrastructure Layer**
   - **Repositories**: Implement data access patterns (e.g., `TrainingPlanRepository.cs`, `ConversationRepository.cs`)
   - **HttpClients**: Interface with external services (e.g., `MLServiceClient.cs`, `EStoreServiceClient.cs`)
   - **Messaging**: Handles event-driven communication (e.g., `InjuryAlertPublisher.cs`)

3. **Services Layer**
   - Business logic implementations (e.g., `ConversationService.cs`, `TrainingPlanService.cs`)
   - Integrates with domain and infrastructure components

4. **Controllers**
   - API endpoints for user interaction (e.g., `ChatController.cs`, `ProfileController.cs`)

5. **Mappers**
   - Convert between domain models and API DTOs (e.g., `ConversationMapper.cs`)

## Notes

- **MongoDB Integration**: Uses `MongoDB.Driver` for data persistence
- **ML Services**: Integrates with external ML models via `MLServiceClient`
- **Conversation Context**: Tracks conversation state using `ConversationContext.cs`
- **No README.md**: No existing documentation found in repository
- **Cursor Rules**: No cursor rules found in `.cursor/` directory
- **Copilot Instructions**: No copilot instructions found in `.github/` directory

This architecture supports a conversational AI system for fitness coaching with real-time injury prediction and