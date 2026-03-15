// src/MyMcpServer.Api/Program.cs
using ContextMcp.Api.Tools;

var builder = WebApplication.CreateBuilder(args);

// Register application services

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
    .WithHttpTransport()        // ← enables SSE over HTTP
    .WithTools<CalculatorTool>();

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseCors();
app.MapMcp("/mcp");   // ← mounts SSE at /mcp/sse, POST at /mcp/message

app.Run();