using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using valheimEnhancments.Documentation.templates;

namespace valheimEnhancments.Documentation
{
    class Program
    {
        static void Main(string[] args)
        {
            var writers = Assembly.GetAssembly(typeof(Program))
                .GetTypes()
                .ToList()
                .Where(f => f.IsClass && typeof(IDocumentationWriter).IsAssignableFrom(f))
                .Select(f => Activator.CreateInstance(f))
                .Cast<IDocumentationWriter>();

            foreach (var currentWriter in writers)
            {
                currentWriter.Write();
            }
        }
    }
}
