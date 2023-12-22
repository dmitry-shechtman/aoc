using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    public sealed class Graph : ICloneable, IEnumerable<int>
    {
        private readonly HashSet<int>[] outgoing;
        private readonly HashSet<int>[] incoming;

        public Graph(Func<int, int, bool> predicate, int count)
        {
            outgoing = CreateEdges(predicate, count);
            incoming = CreateEdges((i, j) => predicate(j, i), count);
        }

        public Graph(Graph graph)
        {
            outgoing = new HashSet<int>[graph.outgoing.Length];
            incoming = new HashSet<int>[graph.incoming.Length];
            for (int i = 0; i < incoming.Length; i++)
            {
                outgoing[i] = new(graph.outgoing[i]);
                incoming[i] = new(graph.incoming[i]);
            }
        }

        public Graph Clone() =>
            new(this);

        object ICloneable.Clone() =>
            Clone();

        public IEnumerator<int> GetEnumerator() =>
            Enumerable.Range(0, outgoing.Length).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public bool Add(int index1, int index2) =>
            outgoing[index1].Add(index2) &&
            incoming[index2].Add(index1);

        public bool Remove(int index1, int index2) =>
            outgoing[index1].Remove(index2) &&
            incoming[index2].Remove(index1);

        public IReadOnlyList<IReadOnlySet<int>> Outgoing =>
            outgoing;

        public IReadOnlyList<IReadOnlySet<int>> Incoming =>
            incoming;

        private static HashSet<int>[] CreateEdges(Func<int, int, bool> predicate, int count) =>
            Enumerable.Range(0, count)
                .Select(i => Enumerable.Range(0, count)
                    .Where(j => predicate(i, j))
                    .ToHashSet())
                .ToArray();
    }
}
