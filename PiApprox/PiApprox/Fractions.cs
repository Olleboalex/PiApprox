using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Data.Common;

namespace Fractions
{
    public struct Fraction
    {
        public BigInteger top;
        public BigInteger bot;

        public Fraction(BigInteger top, BigInteger bot)
        {
            this.top = top;
            this.bot = bot;
        }

        public BigInteger GCD(BigInteger a, BigInteger b)
        {
            while(b != 0)
            {
                BigInteger temp = a;
                a = b;
                b = temp % b;
            }
            return a;
        }

        public void Simplify()
        {
            BigInteger gcd = GCD(top, bot);
            top /= gcd;
            bot /= gcd;
        }

        public static Fraction operator +(Fraction left, Fraction right) => new Fraction(left.top * right.bot + right.top * left.bot, left.bot * right.bot);
        public static Fraction operator -(Fraction a) => new Fraction(-a.top, a.bot);
        public static Fraction operator -(Fraction left, Fraction right) => left + (-right);
        public static Fraction operator *(Fraction left, Fraction right) => new Fraction(left.top * right.top, left.bot * right.bot);
        public static Fraction operator /(Fraction left, Fraction right) => new Fraction(left.top * right.bot, left.bot * right.top);

        public static implicit operator Fraction((BigInteger, BigInteger) a) => new Fraction(a.Item1, a.Item2);
    }
}