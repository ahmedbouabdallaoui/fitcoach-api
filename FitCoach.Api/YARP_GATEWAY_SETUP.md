# YARP API Gateway Configuration for FitCoach

This document provides the complete setup for configuring YARP (Yet Another Reverse Proxy) as an API gateway for the FitCoach backend.

## Overview

YARP will proxy requests from the frontend to your FitCoach API backend. The backend exposes three main controllers:

- **ChatController** - AI chat interactions
- **ProfileController** - User profile management  
- **ConversationController** - Conversation history and management

## 1. Install YARP Packages

In your Gateway project, add the YARP NuGet packages:

```bash
dotnet add package Yarp.ReverseProxy
```

## 2. appsettings.json Configuration

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://0.0.0.0:8080"
      },
      "Https": {
        "Url": "https://0.0.0.0:8443",
        "Certificate": {
          "Path": "<path-to-cert.pfx>",
          "Password": "<cert-password>"
        }
      }
    }
  },

  "ReverseProxy": {
    "Routes": [
      {
        "RouteId": "fitCoachChat",
        "ClusterId": "fitCoachApi",
        "Match": {
          "Path": "api/chat/{**remainder}"
        },
        "Transforms": [
          { "PathRemovePrefix": "/api" }
        ]
      },
      {
        "RouteId": "fitCoachProfile",
        "ClusterId": "fitCoachApi",
        "Match": {
          "Path": "api/profile/{**remainder}"
        },
        "Transforms": [
          { "PathRemovePrefix": "/api" }
        ]
      },
      {
        "RouteId": "fitCoachConversations",
        "ClusterId": "fitCoachApi",
        "Match": {
          "Path": "api/conversations/{**remainder}"
        },
        "Transforms": [
          { "PathRemovePrefix": "/api" }
        ]
      }
    ],
    "Clusters": [
      {
        "ClusterId": "fitCoachApi",
        "Destinations": {
          "fitCoachApi.destination": {
            "Address": "https://localhost:5001"
          }
        }
      }
    ]
  }
}
```

### Important Notes:
- Update the `Address` in `fitCoachApi.destination` to your actual backend URL
- Configure SSL certificates for production (or use HTTP only for development)
- The `PathRemovePrefix` transform removes `/api` prefix before forwarding to backend (since your backend routes already include `/api/...`)

## 3. Program.cs Configuration

```csharp
using Microsoft.AspNetCore.HttpOverrides;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(builderContext =>
    {
        // Optional: Add custom transforms here
        return builderContext;
    });

// Optional: Configure HttpClient for the backend
builder.Services.AddHttpClient("fitCoachApi", client =>
{
    // Configure timeout, default headers, etc.
    client.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

// Forwarded headers (important if behind another proxy/load balancer)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Enable routing
app.MapReverseProxy();

app.Run();
```

## 4. Development vs Production

### Development (HTTP only)
```json
"Kestrel": {
  "Endpoints": {
    "Http": {
      "Url": "http://0.0.0.0:8080"
    }
  }
}
```

### Production (HTTPS)
- Obtain SSL certificates
- Update the certificate path and password
- Consider using environment variables for secrets

## 5. Environment Variables (Optional)

You can also configure destinations via environment variables:

```json
"ReverseProxy": {
  "Clusters": {
    "fitCoachApi": {
      "Destinations": {
        "fitCoachApi.destination": {
          "Address": "${FITCOACH_API_URL}"
        }
      }
    }
  }
}
```

## 6. Testing the Gateway

Start your gateway and test each endpoint:

```bash
# Gateway should run on port 8080 (HTTP) or 8443 (HTTPS)
# Backend should run on port 5001

# Test chat endpoint
curl -X POST http://localhost:8080/api/chat \
  -H "Content-Type: application/json" \
  -d '{"message": "Hello", "tag": "test"}'

# Test profile endpoint
curl http://localhost:8080/api/profile

# Test conversations endpoint
curl http://localhost:8080/api/conversations
```

## 7. Frontend Integration

Your frontend should now point to the YARP gateway instead of directly to the backend:

- **Gateway URL**: `http://localhost:8080` (dev) or your production gateway URL
- **API Base**: `${GATEWAY_URL}/api/`

Example frontend API client configuration:
```javascript
const API_BASE = 'http://localhost:8080/api';

// Use endpoints:
// - POST ${API_BASE}/chat
// - GET ${API_BASE}/profile
// - GET/PUT/DELETE ${API_BASE}/conversations
```

## Endpoint Reference

| Method | Route | Proxy Path | Backend Path |
|--------|-------|------------|--------------|
| POST | `/api/chat` | `api/chat` | `/api/chat` |
| GET | `/api/profile` | `api/profile | `/api/profile` |
| PUT | `/api/profile` | `api/profile` | `/api/profile` |
| GET | `/api/conversations` | `api/conversations` | `/api/conversations` |
| GET | `/api/conversations/{id}` | `api/conversations/{id}` | `/api/conversations/{id}` |
| DELETE | `/api/conversations/{id}` | `api/conversations/{id}` | `/api/conversations/{id}` |
| PUT | `/api/conversations/{id}/title` | `api/conversations/{id}/title` | `/api/conversations/{id}/title` |

## Troubleshooting

1. **404 errors**: Check route patterns match the incoming request path
2. **502 errors**: Backend is not running or wrong destination address
3. **Path issues**: The `PathRemovePrefix` transform removes `/api` before forwarding. Remove it if your backend doesn't expect that.
4. **SSL errors**: Development certificates may need to be trusted or disabled for testing

## Additional Resources

- [YARP Official Documentation](https://microsoft.github.io/reverse-proxy/)
- [YARP GitHub Repository](https://github.com/microsoft/reverse-proxy)
