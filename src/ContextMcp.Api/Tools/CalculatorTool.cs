using ModelContextProtocol.Server;
using System.ComponentModel;

namespace ContextMcp.Api.Tools;

[McpServerToolType]
public sealed class CalculatorTool
{
    [McpServerTool, Description("Add two numbers")]
    public double Add(
        [Description("First number")] double a,
        [Description("Second number")] double b) => a + b;

    [McpServerTool, Description("Divide two numbers")]
    public string Divide(double a, double b) =>
        b == 0 ? "Error: division by zero" : (a / b).ToString();
}