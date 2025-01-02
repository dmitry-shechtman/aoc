using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Numerics.BitOperations;
using Store = System.Int64;

namespace aoc
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public struct BitSet : ICloneable, IEquatable<BitSet>, IComparable<BitSet>, IReadOnlyCollection<int>
    {
        private const Store Zero  = 0;
        private const Store One   = 1;
        private const int   Shift = 6;
        private const int   Mask  = (1 << Shift) - 1;

        private readonly Store[] _bits;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BitSet(int count)
            : this(count, new Store[(count + Mask) >> Shift])
        {
        }

        public BitSet(params Store[] bits)
            : this(bits.Length << Shift, bits)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BitSet(int count, params Store[] bits)
        {
            Count = count;
            _bits = bits;
        }

        public BitSet(int count, bool value)
            : this(count)
        {
            if (value)
                SetAll();
        }

        public BitSet(bool[] values)
            : this(values.Length)
        {
            for (int i = 0, k = 0; i < _bits.Length; i++)
            {
                Store store = Zero;
                for (int j = 0; j < (1 << Shift) && k < values.Length; j++, k++)
                    store |= values[k] ? One << j : Zero;
                _bits[i] = store;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BitSet(BitSet from)
            : this(from.Count)
        {
            from._bits.CopyTo(_bits, 0);
        }

        public int Count { get; }

        public bool this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get =>
                (_bits[index >> Shift] & One << (index & 0x3F)) != 0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                var (i, v) = (index >> Shift, One << (index & 0x3F));
                _bits[i] = value ? _bits[i] | v : _bits[i] & ~v;
            }
        }

        public readonly BitSet Clone()
        {
            return new(this);
        }

        object ICloneable.Clone() => Clone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override bool Equals(object obj) =>
            obj is BitSet other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(BitSet other)
        {
            for (int i = 0; i < _bits.Length; i++)
                if (_bits[i] != other._bits[i])
                    return false;
            return true;
        }

        public readonly override int GetHashCode()
        {
            HashCode hashCode = new();
            for (int i = 0; i < _bits.Length; i++)
                hashCode.Add(_bits[i]);
            return hashCode.ToHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            StringBuilder sb = new();
            for (int i = _bits.Length - 1; i >= 0; i--)
                sb.AppendFormat($"{{0:x0{1 << (Shift - 2)}}}", _bits[i]);
            return sb.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(BitSet other)
        {
            for (int i = _bits.Length - 1; i >= 0; i--)
                if (_bits[i] > other._bits[i])
                    return 1;
                else if (_bits[i] < other._bits[i])
                    return -1;
            return 0;
        }

        public readonly int CountSet()
        {
            int count = 0;
            for (int i = 0; i < _bits.Length; i++)
                count += PopCount((ulong)_bits[i]);
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool AnySet()
        {
            for (int i = 0; i < _bits.Length; i++)
                if (_bits[i] != 0)
                    return true;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int FirstSet()
        {
            return FirstSet(out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int FirstSet(out Store store)
        {
            for (int i = 0; i < _bits.Length; i++)
                if ((store = _bits[i]) != 0)
                    return i << Shift | TrailingZeroCount(store);
            store = 0;
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int FirstSet(int index, out Store store)
        {
            if ((store = _bits[index >> Shift] & ~((One << index) - 1)) != 0)
                return index & ~Mask | TrailingZeroCount(store);
            for (int i = (index >> Shift) + 1; i < _bits.Length; i++)
                if ((store = _bits[i]) != 0)
                    return i << Shift | TrailingZeroCount(store);
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int LastSet()
        {
            return LastSet(out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int LastSet(out Store store)
        {
            for (int i = _bits.Length - 1; i >= 0; i--)
                if ((store = _bits[i]) != 0)
                    return i << Shift | Log2(store);
            store = 0;
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int LastSet(int index, out Store store)
        {
            if ((store = _bits[index >> Shift] & ((One << index) - 1)) != 0)
                return index & ~Mask | Log2(store);
            for (int i = (index >> Shift) - 1; i >= 0; i--)
                if ((store = _bits[i]) != 0)
                    return i << Shift | Log2(store);
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int NextSet(int index)
        {
            var store = _bits[index >> Shift] & ~((One << index) - 1);
            return NextSet(index, ref store);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int NextSet(int index, ref Store store)
        {
            if ((store &= ~(One << index)) != 0)
                return index & ~Mask | TrailingZeroCount(store);
            for (int i = (index >> Shift) + 1; i < _bits.Length; i++)
                if ((store = _bits[i]) != 0)
                    return i << Shift | TrailingZeroCount(store);
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int PreviousSet(int index)
        {
            var store = _bits[index >> Shift];
            return PreviousSet(index, ref store);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int PreviousSet(int index, ref Store store)
        {
            if ((store &= (One << index) - 1) != 0)
                return index & ~Mask | Log2(store);
            for (int i = (index >> Shift) - 1; i >= 0; i--)
                if ((store = _bits[i]) != 0)
                    return i << Shift | Log2(store);
            return -1;
        }

        public BitSet And(BitSet other)
        {
            for (int i = 0; i < _bits.Length; i++)
                _bits[i] &= other._bits[i];
            return this;
        }

        public BitSet AndNot(BitSet other)
        {
            for (int i = 0; i < _bits.Length; i++)
                _bits[i] &= ~other._bits[i];
            return this;
        }

        public BitSet Or(BitSet other)
        {
            for (int i = 0; i < _bits.Length; i++)
                _bits[i] |= other._bits[i];
            return this;
        }

        public BitSet Xor(BitSet other)
        {
            for (int i = 0; i < _bits.Length; i++)
                _bits[i] ^= other._bits[i];
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BitSet ThenSet(int index, bool value)
        {
            this[index] = value;
            return this;
        }

        public BitSet ThenSet(int index)
        {
            return ThenSet(index, true);
        }

        public BitSet ThenClear(int index)
        {
            return ThenSet(index, false);
        }

        public void SetAll(bool value)
        {
            if (value)
                SetAll();
            else
                ClearAll();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAll()
        {
            _bits.Fill(~Zero);
            int trail = Count & Mask;
            if (trail != 0)
                _bits[^1] = (One << trail) - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearAll()
        {
            _bits.Fill(Zero);
        }

        public readonly IEnumerator<int> GetEnumerator()
        {
            for (int index = FirstSet(out Store store); index >= 0; index = NextSet(index, ref store))
                yield return index;
        }

        public readonly IEnumerator<int> GetReverseEnumerator()
        {
            for (int index = LastSet(out Store store); index >= 0; index = PreviousSet(index, ref store))
                yield return index;
        }

        readonly IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Log2(long store) =>
            System.Numerics.BitOperations.Log2((ulong)store);

        private readonly string GetDebuggerDisplay()
        {
            return $"0x{this}";
        }

        public static bool operator ==(BitSet left, BitSet right) =>
            left.Equals(right);

        public static bool operator !=(BitSet left, BitSet right) =>
            !left.Equals(right);

        public static bool operator <(BitSet left, BitSet right) =>
            left.CompareTo(right) < 0;

        public static bool operator >(BitSet left, BitSet right) =>
            left.CompareTo(right) > 0;

        public static bool operator <=(BitSet left, BitSet right) =>
            left.CompareTo(right) <= 0;

        public static bool operator >=(BitSet left, BitSet right) =>
            left.CompareTo(right) >= 0;
    }
}
