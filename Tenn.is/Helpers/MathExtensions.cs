namespace Tennis.Helpers
{
    public static class MathExtensions
    {
        public static int Modulo(int x, int m)
        {
            return (x % m + m) % m;
        }
    }
}
