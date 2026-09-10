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
    public void TestRemove()
    {
        GC.LinkedList<int> list = new GC.LinkedList<int>();
        list.Add(1);
        list.Add(-2);
        list.Add(8);

        Assert.True(list.Remove(8));
        Assert.DoesNotContain<int>(8, list);
        Assert.Contains<int>(-2, list);
        Assert.Equal(2, list.Count);
    }
}