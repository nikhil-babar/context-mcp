using ContextMcp.Api.Models;
using System.Reflection;

namespace ContextMcp.Api.Utility
{
    public class KnowledgeBaseTypeScanner
    {
        public static void RegisterFromAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(t => !t.IsAbstract && typeof(KnowledgeBaseContent).IsAssignableFrom(t));

            foreach (var type in types)
            {
                var instance = (KnowledgeBaseContent)Activator.CreateInstance(type)!;

                KnowledgebaseRegistry.Register(type, instance.Type);
            }
        }
    }
}
