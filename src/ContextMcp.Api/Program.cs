// src/MyMcpServer.Api/Program.cs
using ContextMcp.Api.Config;
using ContextMcp.Api.Infrastructure;
using ContextMcp.Api.Models;
using ContextMcp.Api.Services.VoyageService;
using ContextMcp.Api.Tools;
using ContextMcp.Api.Utility;
using ModelContextProtocol;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

KnowledgeBaseTypeScanner.RegisterFromAssembly(typeof(KnowledgeBaseContent).Assembly);

var jsonSerializerOptions = new JsonSerializerOptions(McpJsonUtilities.DefaultOptions);
jsonSerializerOptions.Converters.Add(new KnowledgeBaseContentJsonConverter());


// Register MCP with SSE transport
builder.Services
    .AddMcpServer(options =>
    {
        options.ServerInfo = new()
        {
            Name = "context-mcp",
            Version = "1.0.0"
        };
    })
    .WithHttpTransport()
    .WithTools<PostKnowledgeBaseTool>(jsonSerializerOptions);
  
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
    

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddVoyageEmbeddingService(builder.Configuration);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.UseCors();
app.MapMcp("/mcp");   // ← mounts SSE at /mcp/sse, POST at /mcp/message

app.Run();