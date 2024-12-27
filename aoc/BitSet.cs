﻿using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Store = System.UInt64;

namespace aoc
{
    public struct BitSet : IReadOnlyCollection<int>
    {
        private const Store Zero  = 0;
        private const Store One   = 1;
        private const int   Shift = 6;
        private const int   Mask  = (1 << Shift) - 1;

        private readonly Store[] _bits;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BitSet(int count)
        {
            Count = count;
            _bits = new Store[(count + Mask) >> Shift];
        }

        public BitSet(int count, bool value)
            : this(count)
        {
            if (value)
                SetAll();
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

        public readonly int CountSet()
        {
            int count = 0;
            for (int i = 0; i < _bits.Length; i++)
                count += BitOperations.PopCount(_bits[i]);
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
                    return i << Shift | BitOperations.TrailingZeroCount(store);
            store = 0;
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int FirstSet(int index, out Store store)
        {
            if ((store = _bits[index >> Shift] & ~((One << index) - 1)) != 0)
                return index & ~Mask | BitOperations.TrailingZeroCount(store);
            for (int i = (index >> Shift) + 1; i < _bits.Length; i++)
                if ((store = _bits[i]) != 0)
                    return i << Shift | BitOperations.TrailingZeroCount(store);
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
                return index & ~Mask | BitOperations.TrailingZeroCount(store);
            for (int i = (index >> Shift) + 1; i < _bits.Length; i++)
                if ((store = _bits[i]) != 0)
                    return i << Shift | BitOperations.TrailingZeroCount(store);
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

        public BitSet ThenSet(int index, bool value)
        {
            this[index] = value;
            return this;
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

        public IEnumerator<int> GetEnumerator()
        {
            for (int index = FirstSet(out Store store); index >= 0; index = NextSet(index, ref store))
                yield return index;
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}