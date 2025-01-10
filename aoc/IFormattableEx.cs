using System;

namespace aoc
{
    public interface IFormattableEx : IFormattable
    {
        string ToString(IFormatProvider? provider);
    }
}
