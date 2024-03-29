﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace valheimEnhancments.extensions
{
    public static class StringExtensions
    {
        public static string EnsureOneLiner(this string self, string replacement = "")
        {
            if (string.IsNullOrWhiteSpace(self))
                return self;

            return self.Replace("\r", replacement).Replace("\n", replacement).Replace(Environment.NewLine, replacement);
        }

        public static string EnsureLenght(this string self, char filler, int lenght)
        {
            if (self is null)
                self = string.Empty;

            if (self.Length >= lenght)
                return self;

            self += new string(filler, lenght - self.Length);

            return self;
        }

        public static string CapitalizeFirstLetter(this string self, bool forceToLower = true)
        {
            if (string.IsNullOrWhiteSpace(self))
                return self;

            var firstLetter = char.ToUpper(self[0]);

            var rest = string.Empty;
            if (self.Length > 1)
            {
                rest = self.Substring(1);

                if (forceToLower)
                    rest = rest.ToLower();
            }

            return firstLetter + rest;
        }

        public static string Repeat(this string self, int count)
        {
            var output = string.Empty;

            for (int i = 0; i < count; i++)
            {
                output += self;
            }

            return output;
        }

        public static string FormatCoordinate(this float self)
        {
            return self.ToString("F0");
        }
    }
}
