using GC = GA.Collections;
using Xunit;
using System.ComponentModel;


public class LinkedListTests
{
    [Fact]
    public void TestBasicAdd()
    {
        GC.LinkedList<int> list = new GC.LinkedList<int>();
        list.Add(1);
        list.Add(-2);
        list.Add(8);
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void TestBasicContains()
    {
        GC.LinkedList<int> list = new GC.LinkedList<int>();
        list.Add(1);
        list.Add(-2);
        list.Add(8);
#pragma warning disable xUnit2017 // Do not use Contains() to check if a value exists in a collection
        Assert.True(list.Contains(-2));
#pragma warning restore xUnit2017 // Do not use Contains() to check if a value exists in a collection
    }

    [Fact]
    public void TestListOrder()
    {
        GC.LinkedList<int> list = [1, -2, 8];
        Assert.Equal(list, new List<int>() { 1, -2, 8});
    }

    [Fact]
    public void TestClear()
    {
        GC.LinkedList<int> list = new GC.LinkedList<int>();
        list.Add(1);
        list.Add(-2);
        list.Add(8);
        list.Clear();

        Assert.Empty(list);
    }

    [Fact]
    public void TestRemoveBody()
    {
        GC.LinkedList<int> list = [1, -2, 4, 8];

        Assert.True(list.Remove(-2));
        Assert.DoesNotContain<int>(-2, list);
        Assert.Contains<int>(4, list);
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void TestRemoveHead()
    {
        GC.LinkedList<int> list = [1, -2, 4, 8];

        Assert.True(list.Remove(1));
        Assert.DoesNotContain<int>(1, list);
        Assert.Contains<int>(-2, list);
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void TestRemoveTail()
    {
        GC.LinkedList<int> list = [1, -2, 4, 8];
        Assert.True(list.Remove(8));
        Assert.DoesNotContain<int>(8, list);
        Assert.Contains<int>(-2, list);
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void TestRemoveOnlyInstance()
    {
        GC.LinkedList<int> list = [1];
        Assert.True(list.Remove(1));
        Assert.DoesNotContain<int>(1, list);
        Assert.Empty(list);
    }

    [Fact]
    public void TestCloneTo()
    {
        GC.LinkedList<int> list = [1, 2, 3, 4];
        list.CopyTo([7,8,9], 1);

        Assert.Equal(list, new List<int>() { 1, 7, 8, 9, 2, 3, 4});

    }

    [Fact]
    public void TestCloneToTail()
    {
        GC.LinkedList<int> list = [1, 2, 3, 4];
        list.CopyTo([7,8,9], 4);

        Assert.Equal(list, new List<int>() { 1, 2, 3, 4, 7, 8, 9});
        Assert.Equal(1, list.GetHead());
        Assert.Equal(9, list.GetTail());
    }

    [Fact]
    public void TestCloneToHead()
    {
        GC.LinkedList<int> list = [1, 2, 3, 4];
        list.CopyTo([7,8,9], 0);

        Assert.Equal(list, new List<int>() {7, 8, 9, 1, 2, 3, 4});
        Assert.Equal(7, list.GetHead());
        Assert.Equal(4, list.GetTail());
    }
}