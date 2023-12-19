using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc
{
    public struct Vector3DRange : IEquatable<Vector3DRange>, IEnumerable<Vector3D>
    {
        public Vector3DRange(Vector3D min, Vector3D max)
        {
            Min = min;
            Max = max;
        }

        public Vector3DRange(Vector3D max)
            : this(Vector3D.Zero, max)
        {
        }

        public Vector3DRange(int x, int y, int z)
            : this(new(x, y, z))
        {
        }

        public Vector3D Min { get; }
        public Vector3D Max { get; }

        public int Width  => Max.x - Min.x + 1;
        public int Height => Max.y - Min.y + 1;
        public int Depth  => Max.z - Min.z + 1;
        public int Count  => Width * Height * Depth;

        public long LongCount =>
            (long)Width * Height * Depth;

        public readonly override bool Equals(object obj) =>
            obj is Vector3DRange other && Equals(other);

        public readonly bool Equals(Vector3DRange other) =>
            Min.Equals(other.Min) &&
            Max.Equals(other.Max);

        public readonly override int GetHashCode() =>
            HashCode.Combine(Min, Max);

        public readonly override string ToString() =>
            $"{Min},{Max}";

        public readonly void Deconstruct(out Vector3D min, out Vector3D max)
        {
            min = Min;
            max = Max;
        }

        public readonly IEnumerator<Vector3D> GetEnumerator()
        {
            for (var z = Min.z; z <= Max.z; z++)
                for (var y = Min.y; y <= Max.y; y++)
                    for (var x = Min.x; x <= Max.x; x++)
                        yield return new(x, y, z);
        }

        readonly IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public readonly bool IsMatch(Vector3D vector) =>
            vector.x >= Min.x && vector.x <= Max.x &&
            vector.y >= Min.y && vector.y <= Max.y &&
            vector.z >= Min.z && vector.z <= Max.z;

        public readonly bool IsMatch(Vector3DRange other) =>
            other.Min.x <= Max.x && other.Max.x >= Min.x &&
            other.Min.y <= Max.y && other.Max.y >= Min.y &&
            other.Min.z <= Max.z && other.Max.z >= Min.z;

        public readonly bool Contains(Vector3D vector) =>
            IsMatch(vector);

        public readonly bool Contains(Vector3DRange other) =>
            other.Min.x >= Min.x && other.Max.x <= Max.x &&
            other.Min.y >= Min.y && other.Max.y <= Max.y &&
            other.Min.z >= Min.z && other.Max.z <= Max.z;

        public readonly Vector3DRange Union(Vector3DRange other) =>
            new(min: Vector3D.Min(Min, other.Min), max: Vector3D.Max(Max, other.Max));

        public static Vector3DRange Union(Vector3DRange left, Vector3DRange right) =>
            left.Union(right);

        public readonly Vector3DRange Intersect(Vector3DRange other) =>
            new(min: Vector3D.Max(Min, other.Min), max: Vector3D.Min(Max, other.Max));

        public static Vector3DRange Intersect(Vector3DRange left, Vector3DRange right) =>
            left.Intersect(right);

        public readonly Vector3DRange SplitWest(int width) =>
            new(Min, new(Min.x + width, Max.y, Max.z));

        public readonly Vector3DRange SplitEast(int width) =>
            new(new(Min.x + width, Min.y, Min.z), Max);

        public readonly Vector3DRange SplitNorth(int height) =>
            new(Min, new(Max.x, Min.y + height, Max.z));

        public readonly Vector3DRange SplitSouth(int height) =>
            new(new(Min.x, Min.y + height, Min.z), Max);

        public readonly Vector3DRange SplitUp(int depth) =>
            new(Min, new(Max.x, Max.y, Min.z + depth));

        public readonly Vector3DRange SplitDown(int depth) =>
            new(new(Min.x, Min.y, Min.z + depth), Max);

        public static implicit operator (Vector3D min, Vector3D max)(Vector3DRange value) =>
            (value.Min, value.Max);

        public static implicit operator Vector3DRange((Vector3D min, Vector3D max) value) =>
            new(value.min, value.max);

        public static bool operator ==(Vector3DRange left, Vector3DRange right) =>
            left.Equals(right);

        public static bool operator !=(Vector3DRange left, Vector3DRange right) =>
            !left.Equals(right);
    }
}
