using GC = GA.Collections;
using Xunit;
using System.ComponentModel;


public class PriorityQueueTests
{
    [Fact]
    public void Create_IsEmpty()
    {
        var queue = new GC.PriorityQueue<int>();
        Assert.Equal(0, queue.Count);
    }

    [Fact]
    public void Enqueue_MultipleNumbers()
    {
        var queue = new GC.PriorityQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.Enqueue(4);

        Assert.Equal(4, queue.Count);
        Assert.True(queue.IsConsistant());
    }

    [Fact]
    public void Enqueue_MultipleSameNumbers()
    {
        var queue = new GC.PriorityQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(1);
        queue.Enqueue(1);
        queue.Enqueue(1);
        queue.Enqueue(1);

        Assert.Equal(5, queue.Count);
        Assert.True(queue.IsConsistant());
    }

    /// <summary>
    /// You can insert a null value
    /// which can't be compared so they breaks the queue
    /// </summary>
    [Fact]
    public void Enqueue_null()
    {
        var queue = new GC.PriorityQueue<string?>();
        queue.Enqueue("a");
        queue.Enqueue(null);

        Assert.Equal(2, queue.Count);
        Assert.True(queue.IsConsistant());
    }

    [Fact]
    public void Enqueue_String()
    {
        var queue = new GC.PriorityQueue<string>();
        queue.Enqueue("b");
        queue.Enqueue("a");
        queue.Enqueue("c");
        queue.Enqueue("adawd");
        queue.Enqueue("adawd");


        Assert.Equal(5, queue.Count);
        Assert.True(queue.IsConsistant());
        Assert.Equal("a", queue.Peek());

    }

    [Fact]
    public void Enqueue_NegativeNumbers()
    {
        var queue = new GC.PriorityQueue<int>();
        queue.Enqueue(-3);
        queue.Enqueue(-2);

        Assert.Equal(2, queue.Count);
        Assert.True(queue.IsConsistant());
        Assert.Equal(-3, queue.Peek());

    }

    [Fact]
    public void Peek_SortedAddition_ReturnsHighestPrio()
    {
        var queue = new GC.PriorityQueue<int>();

        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.Enqueue(4);

        Assert.Equal(1, queue.Peek());
    }

    [Fact]
    public void Peek_UnsortedAddition_ReturnsHighestPrio()
    {
        var queue = new GC.PriorityQueue<int>();

        queue.Enqueue(3);
        queue.Enqueue(2);
        queue.Enqueue(1);
        queue.Enqueue(4);

        Assert.Equal(1, queue.Peek());
    }

    [Fact]
    public void Dequeue_UnsortedAddition_ReturnsHighestPrio()
    {
        var queue = new GC.PriorityQueue<int>();
        queue.Enqueue(3);
        queue.Enqueue(2);
        queue.Enqueue(1);
        queue.Enqueue(4);

        Assert.True(queue.IsConsistant(), "1");
        Assert.Equal(1, queue.Dequeue());
        Assert.True(queue.IsConsistant(), "2");
        Assert.Equal(2, queue.Dequeue());
        Assert.True(queue.IsConsistant(), "3");
        Assert.Equal(3, queue.Dequeue());
        Assert.True(queue.IsConsistant(), "4");
        Assert.Equal(4, queue.Dequeue());

    }
}