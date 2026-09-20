using System;
using System.Collections;
using System.Collections.Generic;

namespace GA.Collections
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> _data = new List<T>();

        public int Count => _data.Count;

        public void Enqueue(T item)
        {
            _data.Add(item);
            int childIdx = Count - 1;

            while (childIdx > 0)
            {
                int parentIdx = (childIdx - 1) / 2;
                if(_data[childIdx].CompareTo(_data[parentIdx]) >= 0)
                {
                    break;
                }

                _data.Swap(childIdx, parentIdx);
                childIdx = parentIdx;
            }
        }

        public T Dequeue()
        {
            if(Count == 0)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            // Save first item
            T result = _data[0];

            // Move last item to the first position
            int lastIdx = Count - 1;
            _data[0] = _data[lastIdx];
            _data.RemoveAt(lastIdx);
            lastIdx--;

            // Bubble down the first item to its correct position
            int parentIdx = 0;

            while (true)
            {
                // Left child index
                int childIdx = parentIdx * 2 + 1;

                //  Making sure there are more children
                if(childIdx > lastIdx) break;

                // Getting right child index
                int rightChildIdx = childIdx + 1;

                // Checking children for their order
                if(rightChildIdx <= lastIdx && _data[rightChildIdx].CompareTo(_data[childIdx]) < 0)
                {
                    childIdx = rightChildIdx;
                }

                if(_data[parentIdx].CompareTo(_data[childIdx]) <= 0)
                {
                    break;
                }
                _data.Swap(parentIdx, childIdx);
                parentIdx = childIdx;
            }

            return result;
        }

        public T Peek()
        {
            if(Count == 0)
            {
                throw new InvalidOperationException("Queue is empty");
            }
            return _data[0];
        }

        public void Clear() => _data.Clear();
        public bool Contains(T item) => _data.Contains(item);

        public bool IsConsistant()
        {
            if(_data.Count == 0) return true;

            int lastIdx = Count - 1;

            for(int parentIdx = 0; parentIdx < Count; parentIdx++)
            {
                int leftChild   = parentIdx * 2 + 1;
                int rightChild  = parentIdx * 2 + 2;

                if(leftChild <= lastIdx && _data[parentIdx].CompareTo(_data[leftChild]) > 0)
                {
                    return false;
                }

                if(rightChild <= lastIdx && _data[parentIdx].CompareTo(_data[rightChild]) > 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}