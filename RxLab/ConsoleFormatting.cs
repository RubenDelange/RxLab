﻿// code was found at the C# Examples Site: http://www.csharp-examples.net/indent-string-with-spaces/

namespace RxLab
{
    public class ConsoleFormatting
    {
        public static string Indent(int count)
        {
            return "".PadLeft(count);
        }
    }
}
