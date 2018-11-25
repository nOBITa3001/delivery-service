using System;

namespace DS.Dtos.Builders
{
    public static class IntGenerator
    {
        private static readonly Random _random = new Random();

        public static int RandomCost() => _random.Next(1, 30);
    }
}
