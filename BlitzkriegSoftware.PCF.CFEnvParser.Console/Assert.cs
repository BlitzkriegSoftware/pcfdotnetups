using System;
using System.Collections.Generic;
using System.Text;

namespace BlitzkriegSoftware.PCF.CFEnvParser.Console
{
    public static class Assert
    {
        public static void Equal(string s1, string s2)
        {
            bool ok = s1.Equals(s2);
            System.Console.WriteLine("{0}={1}, {2}", s1, s2, ok);
        }

        public static void Equal(int s1, int s2)
        {
            bool ok = s1.Equals(s2);
            System.Console.WriteLine("{0}={1}, {2}", s1, s2, ok);
        }

        public static void True(string title, bool expression)
        {
            System.Console.WriteLine("{0}, {1}", title, expression);
        }
    }
}
