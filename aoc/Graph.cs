using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    public abstract class GraphBase<TSelf, TEdges, TValue> : ICloneable, IEnumerable<int>
        where TSelf : GraphBase<TSelf, TEdges, TValue>
        where TEdges : new()
    {
        protected readonly TEdges[] outgoing;
        protected readonly TEdges[] incoming;

        protected GraphBase(Func<int, int, TValue> getValue, int count)
        {
            outgoing = CreateEdges(getValue, count);
            incoming = CreateEdges((i, j) => getValue(j, i), count);
        }

        protected GraphBase(int count)
        {
            outgoing = new TEdges[count];
            incoming = new TEdges[count];
            for (int i = 0; i < count; i++)
            {
                outgoing[i] = new();
                incoming[i] = new();
            }
        }

        protected GraphBase(TSelf graph)
        {
            outgoing = new TEdges[graph.outgoing.Length];
            incoming = new TEdges[graph.incoming.Length];
            for (int i = 0; i < incoming.Length; i++)
            {
                outgoing[i] = CloneEdges(graph.outgoing[i]);
                incoming[i] = CloneEdges(graph.incoming[i]);
            }
        }

        public abstract TSelf Clone();

        object ICloneable.Clone() =>
            Clone();

        public IEnumerator<int> GetEnumerator() =>
            Enumerable.Range(0, outgoing.Length).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public abstract TValue GetValue(int index, int index2);
        public abstract void SetValue(int index, int index2, TValue value);

        public TValue this[int index, int index2]
        {
            get => GetValue(index, index2);
            set => SetValue(index, index2, value);
        }

        private TEdges[] CreateEdges(Func<int, int, TValue> getValue, int count) =>
            Enumerable.Range(0, count)
                .Select(i => CreateEdges(Enumerable.Range(0, count)
                    .Select(j => (j, value: getValue(i, j)))
                    .Where(t => !Equals(t.value, default(TValue)))))
                .ToArray();

        protected abstract TEdges CreateEdges(IEnumerable<(int key, TValue value)> source);
        protected abstract TEdges CloneEdges(TEdges edges);
    }

    public sealed class Graph : GraphBase<Graph, HashSet<int>, bool>
    {
        public Graph(Func<int, int, bool> predicate, int count)
            : base(predicate, count)
        {
        }

        public Graph(int count)
            : base(count)
        {
        }

        public Graph(Graph graph)
            : base(graph)
        {
        }

        public override Graph Clone() =>
            new(this);

        public bool Add(int index1, int index2) =>
            outgoing[index1].Add(index2) &&
            incoming[index2].Add(index1);

        public bool Remove(int index1, int index2) =>
            outgoing[index1].Remove(index2) &&
            incoming[index2].Remove(index1);

        public override bool GetValue(int index, int index2) =>
            outgoing[index].Contains(index2);

        public override void SetValue(int index, int index2, bool value)
        {
            if (value)
                Add(index, index2);
            else
                Remove(index, index2);
        }

        public IReadOnlyList<IReadOnlySet<int>> Outgoing =>
            outgoing;

        public IReadOnlyList<IReadOnlySet<int>> Incoming =>
            incoming;

        protected override HashSet<int> CreateEdges(IEnumerable<(int key, bool value)> source) =>
            new(source.Select(t => t.key));

        protected override HashSet<int> CloneEdges(HashSet<int> edges) =>
            new(edges);
    }

    public sealed class Graph<T> : GraphBase<Graph<T>, Dictionary<int, T>, T>
    {
        public Graph(Func<int, int, T> getValue, int count)
            : base(getValue, count)
        {
        }

        public Graph(int count)
            : base(count)
        {
        }

        public Graph(Graph<T> graph)
            : base(graph)
        {
        }

        public override Graph<T> Clone() =>
            new(this);

        public bool Add(int index1, int index2, T value) =>
            outgoing[index1].TryAdd(index2, value) &&
            incoming[index2].TryAdd(index1, value);

        public bool Remove(int index1, int index2) =>
            outgoing[index1].Remove(index2) &&
            incoming[index2].Remove(index1);

        public override T GetValue(int index, int index2) =>
            outgoing[index][index2];

        public override void SetValue(int index1, int index2, T value)
        {
            outgoing[index1][index2] = value;
            incoming[index2][index1] = value;
        }

        public IReadOnlyList<IReadOnlyDictionary<int, T>> Outgoing =>
            outgoing;

        public IReadOnlyList<IReadOnlyDictionary<int, T>> Incoming =>
            incoming;

        protected override Dictionary<int, T> CreateEdges(IEnumerable<(int key, T value)> source) =>
            source.ToDictionary(t => t.key, t => t.value);

        protected override Dictionary<int, T> CloneEdges(Dictionary<int, T> edges) =>
            new(edges);
    }
}
