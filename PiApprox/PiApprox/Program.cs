using Fractions;
namespace PiApprox
{
    internal class Program
    {
        public static Fraction Approx(int n)
        {
            Fraction result = new Fraction(1, 1);
            for (int i = 1; i <= n; i++)
            {
                result += i % 2 == 0 ? new Fraction(1, 2 * i + 1) : new Fraction(-1, 2 * i + 1);
            }
            return new Fraction(4, 1) * result;
        }
        static void Main(string[] args)
        {
            Fraction res = Approx(100000);
            res.Simplify();
            Console.WriteLine($"{res.top} / {res.bot}");
        }
    }
}