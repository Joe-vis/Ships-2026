using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GA.Collections
{
	public class LinkedList<T> : ICollection<T>
	{
		protected class Node
		{
			public T Value { get; set; }
			public Node Next { get; set; }
			public Node Previous { get; set; }

			public Node() : this(default(T))
			{
			}

			public Node(T value, Node next = null, Node previous = null)
			{
				Value = value;
				Next = next;
				Previous = previous;
			}
		}

		/// <summary>
		/// The head of the linked list. When the list is empty, this will be null.
		/// </summary>
		protected Node Head { get; set; } = null;
		protected Node Tail { get; set; } = null;
		public int Count { get; private set; } = 0;

		public virtual bool IsReadOnly => false;

		public void Add(T item)
		{
			if(IsReadOnly)
			{
				throw new NotSupportedException("");
			}

			Node node = new(item);

			if(Head == null)
			{
				Head = node;
				Tail = node;
			}
			else
			{
				Tail.Next = node;
				node.Previous = Tail;
				Tail = node;
			}
			Count++;

		}

		public void Clear()
		{
			if(IsReadOnly) throw new NotSupportedException("");

			Head = null;
			Tail = null;
			Count = 0;
		}

		public bool Contains(T item)
		{
			Node current = Head;
			while(current != null)
			{
				if(EqualityComparer<T>.Default.Equals(current.Value, item))
				{
					return true;
				}

				current = current.Next;
			}
			return false;
		}

		// for debugging
		public T GetHead()
		{
			return Head.Value;
		}

		// for debugging
		public T GetTail()
		{
			return Tail.Value;
		}

		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			if(IsReadOnly) throw new NotSupportedException("");
			if(arrayIndex > Count || arrayIndex < 0) throw new ArgumentOutOfRangeException("");

			if(arrayIndex == Count) // if tail just use add
			{
				foreach(T item in array)
				{
					Add(item);
				}
				return;
			}

			bool forward = arrayIndex < ((Count - 1) / 2);
			Node current = forward ? Head : Tail;
			int currentIndex = forward ? 0 : Count - 1;

			while(current != null)
			{
				if(currentIndex == arrayIndex)
				{
					for(int i = 0; i < array.Length; i++)
					{
                        Node node = new(array[i])
                        {
                            Previous = current.Previous,
                            Next = current
                        };

						if(current.Previous != null)
						{
							current.Previous.Next = node;
						}
						else
						{
							Head = node;
						}
                        current.Previous = node;
						Count++;
					}
					break;
				}
				currentIndex += forward ? 1 : -1;
				current = forward ? current.Next : current.Previous;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			Node current = Head;
			while (current != null)
			{
				yield return current.Value;
				current = current.Next;
			}
		}

		public bool Remove(T item)
		{
			if(IsReadOnly) throw new NotSupportedException();

			Node current = Head;
			while(current != null)
			{
				if(EqualityComparer<T>.Default.Equals(current.Value, item))
				{
					if(current.Next != null && current.Previous != null) // Removing body
					{
						current.Previous.Next = current.Next;
						current.Next.Previous = current.Previous;
					}
					else if (current.Previous != null) // Removing Tail
					{
						current.Previous.Next = null;
						Tail = current.Previous;
					}
					else if (current.Next != null) // Removing Head
					{
						current.Next.Previous = null;
						Head = current.Next;
					}
					else // Removing last element
					{
						Head = null;
						Tail = null;
					}

					Count--;
					return true;
				}
				current = current.Next;
			}

			return false;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	}
}