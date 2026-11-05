using System;
using System.Collections.Generic;

namespace ClinicApp.Models
{
    public class WaitingQueue<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();

        public int Count => _queue.Count;
        public bool IsEmpty => _queue.Count == 0;

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
        }

        public T Dequeue()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Черга очікування порожня. Немає кого приймати.");

            return _queue.Dequeue();
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Черга очікування порожня. Немає першого пацієнта.");

            return _queue.Peek();
        }

        public T[] ToArray()
        {
            return _queue.ToArray();
        }
    }
}
