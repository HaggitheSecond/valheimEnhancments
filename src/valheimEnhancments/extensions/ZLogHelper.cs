using System.Collections.Generic;
using System.Linq;

namespace valheimEnhancments.extensions
{
    public static class ZLogHelper
    {
        public static void Log<T>(IEnumerable<T> items, string seperator = "\r\n")
        {
            ZLog.Log(string.Join(seperator, items));
        }

        public static void LogProperties(List<(string property, string value)> items, string seperator = "\r\n")
        {
            ZLog.Log(string.Join(seperator, items.Select(f => $"{f.property} => {f.value}")));
        }
    }
}
