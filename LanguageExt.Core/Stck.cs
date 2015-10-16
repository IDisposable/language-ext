﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace LanguageExt
{
    [Serializable]
    public class Stck<T> : IEnumerable<T>, IEnumerable, IAppendable<Stck<T>>
    {
        public readonly static Stck<T> Empty = new Stck<T>();

        readonly T value;
        readonly Stck<T> tail;

        internal Stck()
        {
        }

        internal Stck(T value, Stck<T> tail)
        {
            Count = tail.Count + 1;
            this.tail = tail;
            this.value = value;
        }

        internal Stck(IEnumerable<T> initial)
        {
            tail = new Stck<T>();
            foreach (var item in initial)
            {
                value = item;
                tail = tail.Push(item);
                Count++;
            }
            tail = tail.Pop();
        }

        internal Stck(Lst<T> initial)
        {
            tail = new Stck<T>();
            foreach (var item in initial)
            {
                value = item;
                tail = tail.Push(item);
                Count++;
            }
            tail = tail.Pop();
        }

        public int Count
        {
            get;
            private set;
        }

        public Stck<T> Reverse()
        {
            var s = new Stck<T>();
            foreach (var item in this)
            {
                s = s.Push(item);
            }
            return s;
        }

        public bool IsEmpty => 
            Count == 0;

        public Stck<T> Clear() =>
            Empty;

        public IEnumerator<T> GetEnumerator() =>
            AsEnumerable().GetEnumerator();

        public IEnumerable<T> AsEnumerable()
        {
            var self = this;
            while (self.Count != 0)
            {
                yield return self.value;
                self = self.tail;
            }
        }

        public T Peek()
        {
            if (Count > 0)
            {
                return value;
            }
            else
            {
                throw new InvalidOperationException("Stack is empty");
            }
        }

        public Stck<T> Pop()
        {
            if (Count > 0)
            {
                return tail;
            }
            else
            {
                throw new InvalidOperationException("Stack is empty");
            }
        }

        public Stck<T> Pop(out T outValue)
        {
            if (Count > 0)
            {
                outValue = value;
                return tail;
            }
            else
            {
                throw new InvalidOperationException("Stack is empty");
            }
        }

        public Tuple<Stck<T>, Option<T>> TryPop() =>
            Count > 0
                ? Tuple.Create(tail, Prelude.Some(value))
                : Tuple.Create<Stck<T>, Option<T>>(this, Prelude.None);

        public Option<T> TryPeek() =>
            Count > 0
                ? Prelude.Some(value)
                : Prelude.None;

        public Stck<T> Push(T value) =>
            new Stck<T>(value, this);

        IEnumerator IEnumerable.GetEnumerator() =>
            AsEnumerable().GetEnumerator();

        public static Stck<T> operator +(Stck<T> lhs, Stck<T> rhs) =>
            lhs.Append(rhs);

        public Stck<T> Append(Stck<T> rhs)
        {
            var self = this;
            foreach (var item in rhs)
            {
                self = self.Push(item);
            }
            return self;
        }
    }
}