using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using valheimEnhancments.commands;

namespace valheimEnhancments.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var commands = valheimEnhancmentsPlugin.GetCommands();

            var output = new List<string>();

            foreach (var currentCommand in valheimEnhancmentsPlugin.GetCommands())
            {
                output.Add(currentCommand.ToString());
            }

            File.WriteAllLines("", output);
        }
    }
}
