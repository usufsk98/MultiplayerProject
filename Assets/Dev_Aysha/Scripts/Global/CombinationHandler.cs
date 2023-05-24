using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CombinationHandler : MonoBehaviour
{
    [SerializeField] List<string> combinationList;
    private void Start()
    {
        //var sequence = new[] { 0, 1, 2, 3, 4 };
        //sequence.Combinations(3);
        //foreach (var combination in sequence.Combinations(3))
        //{
        //    combinationList.Add(string.Join(",", combination));
        //    Debug.Log(string.Join(",", combination));
        //}
    }
}




// An immutable stack
//
// Note that the class is abstract with a private 
// constructor; this ensures that only nested classes
// may be derived classes.

abstract class ImmutableStack<T> : IEnumerable<T>
{
    public static readonly ImmutableStack<T> Empty = new EmptyStack();

    private ImmutableStack() { }

    public abstract ImmutableStack<T> Pop();
    public abstract T Top { get; }
    public abstract bool IsEmpty { get; }

    public IEnumerator<T> GetEnumerator()
    {
        var current = this;
        while (!current.IsEmpty)
        {
            yield return current.Top;
            current = current.Pop();
        }
    }

    IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

    public ImmutableStack<T> Push(T value)
    {
        return new NonEmptyStack(value, this);
    }

    private class EmptyStack : ImmutableStack<T>
    {
        public override ImmutableStack<T> Pop()
        {
            throw new InvalidOperationException();
        }

        public override T Top
        {
            get { throw new InvalidOperationException(); }
        }

        public override bool IsEmpty
        {
            get { return true; }
        }
    }

    private class NonEmptyStack : ImmutableStack<T>
    {
        private readonly T head;
        private readonly ImmutableStack<T> tail;

        public NonEmptyStack(T head, ImmutableStack<T> tail)
        {
            this.head = head;
            this.tail = tail;
        }

        public override ImmutableStack<T> Pop()
        {
            return this.tail;
        }

        public override T Top
        {
            get { return this.head; }
        }

        public override bool IsEmpty
        {
            get { return false; }
        }
    }
}

static class ExtensionsCombination
{
    // A handy extension method which takes a sequence
    // of items and a corresponding sequence of bools,
    // and then produces a new sequence where the bools
    // select which items to take out of the original
    // sequence. This could be built out of Zip and
    // Where but it is easy to simply write the code
    // out directly.

    public static IEnumerable<T> ZipWhere<T>(
      this IEnumerable<T> items,
      IEnumerable<bool> selectors)
    {
        if (items == null)
            throw new ArgumentNullException("items");

        if (selectors == null)
            throw new ArgumentNullException("selectors");

        return ZipWhereIterator<T>(items, selectors);
    }

    private static IEnumerable<T> ZipWhereIterator<T>(
      IEnumerable<T> items,
      IEnumerable<bool> selectors)
    {
        using (var e1 = items.GetEnumerator())
        using (var e2 = selectors.GetEnumerator())
            while (e1.MoveNext() && e2.MoveNext())
                if (e2.Current)
                    yield return e1.Current;
    }

    // An extension method which takes a sequence of items
    // and produces all subsequences of that sequence of the
    // given size.

    public static IEnumerable<IEnumerable<T>> Combinations<T>(
      this IEnumerable<T> items, int k)
    {
        if (k < 0)
            throw new InvalidOperationException();

        if (items == null)
            throw new ArgumentNullException("items");

        return
          from combination in Combinations(items.Count(), k)
          select items.ZipWhere(combination);
    }

    // Takes two numbers n and k where both are positive.
    // Produces all sequences of n bits with k true bits
    // and n-k false bits.
    private static IEnumerable<ImmutableStack<bool>> Combinations(
      int n, int k)
    {
        // Base case: if n and k are both zero then the sequence
        // is easy: the sequence of zero bits with zero true bits
        // is the empty sequence.

        if (k == 0 && n == 0)
        {
            yield return ImmutableStack<bool>.Empty;
            yield break;
        }

        // Base case: if n < k then there are no such sequences.
        if (n < k)
            yield break;

        // Otherwise, produce first all the sequences that start with
        // true, and then all the sequences that start with false.

        // At least one of n or k is not zero, and 0 <= k <= n,
        // therefore n is not zero. But k could be.

        if (k > 0)
            foreach (var r in Combinations(n - 1, k - 1))
                yield return r.Push(true);

        foreach (var r in Combinations(n - 1, k))
            yield return r.Push(false);
    }
}

