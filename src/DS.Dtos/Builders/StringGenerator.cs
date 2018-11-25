using System;
using System.Linq;

namespace DS.Dtos.Builders
{
    public static class StringGenerator
    {
        private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly static Random _random = new Random();

        public static string Random(int length)
        {
            return new string
            (
                Enumerable.Repeat(CHARS, length).Select(c => c[_random.Next(c.Length)]).ToArray()
            );
        }
    }
}
