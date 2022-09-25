
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using valheimEnhancments.extensions;

namespace valheimEnhancments.helpers
{
    public static class CsvWriter
    {
        public static void Write(string filePath, List<string> headers, List<List<object>> lines)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("filePath cannot be null or whitespacee");

            if(lines == null || lines.Count == 0)
                throw new ArgumentException("lines cannot be null or empty");

            var builder = new StringBuilder();

            if (headers.Count != 0)
                builder.AppendLine(string.Join(",", headers));

            foreach (var currentLine in lines)
            {
                foreach (var currentValue in currentLine)
                {
                    string actualValue;
                    if (currentValue is string stringValue)
                        actualValue = $"\"{stringValue.EnsureOneLiner()}\"";
                    else
                        actualValue = currentValue.ToString();

                    builder.Append($"{actualValue},");
                }

                builder.AppendLine();
            }

            var directory = Path.GetDirectoryName(filePath);
            if (Directory.Exists(directory) == false)
                Directory.CreateDirectory(directory);

            File.WriteAllText(filePath + (filePath.EndsWith(".csv") ? string.Empty : ".csv"), builder.ToString());
        }
    }
}
