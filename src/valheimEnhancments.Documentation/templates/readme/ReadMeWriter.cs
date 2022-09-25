
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using valheimEnhancments.Shared;

namespace valheimEnhancments.Documentation.templates.readme
{
    public class ReadMeWriter : IDocumentationWriter
    {
        public void Write()
        {
            var template = string.Empty;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("valheimEnhancments.Documentation.templates.readme.ReadMe.txt"))
            using (var streamReader = new StreamReader(stream))
            {
                template = streamReader.ReadToEnd();
            }

            template = template.Replace("<commands>", string.Join(Environment.NewLine, valheimEnhancmentsPlugin.GetCommands().OrderBy(f => f.Name)));

            File.WriteAllText(Paths.valheimEnhancmentsDocumentation.ReadmeFileLocation, template);
        }
    }
}
