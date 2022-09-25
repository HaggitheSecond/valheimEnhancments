using System.Collections.Generic;

namespace valheimEnhancments.extensions
{
    public static class ZLogHelper
    {
        public static void Log<T>(IEnumerable<T> items, string seperator = "\r\n")
        {
            ZLog.Log(string.Join(seperator, items));
        }
    }
}
