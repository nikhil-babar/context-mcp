namespace ContextMcp.Api.Utility
{
    public static class KnowledgebaseRegistry
    {
        private static readonly Dictionary<string, Type> _types = new();

        public static void Register(Type type, string discriminator)
        {
            _types[discriminator] = type;
        }

        public static Type? Resolve(string discriminator)
        {
            return _types.TryGetValue(discriminator, out var t) ? t : null;
        }
    }
}
