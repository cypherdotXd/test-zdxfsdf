using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph<T>
{
    private readonly Dictionary<Node<T>, List<Node<T>>> Nodes = new();
    public int Count => Nodes.Count;

    public void AddEdge(Node<T> node1, Node<T> node2)
    {
        if (!Nodes.ContainsKey(node1))
            Nodes.Add(node1, new List<Node<T>>());
        if(!Nodes[node1].Contains(node2))
            Nodes[node1].Add(node2);

        if (!Nodes.ContainsKey(node2))
            Nodes.Add(node2, new List<Node<T>>());
        if(!Nodes[node2].Contains(node1))
            Nodes[node2].Add(node1);
    }

    public void Print()
    {
        var output = $"Count: {Count} => ";
        Nodes.Keys.ToList().ForEach(node => output += " " + node.Data);
        Debug.Log(output);
    }
    
    public void ForEachDfs(Node<T> start, Action<Node<T>> action, Predicate<Node<T>> terminate = null)
    {
        var visited = new HashSet<Node<T>>();
        var current = start;
        if (current == null)
            return;
        // ExploreDfs(current, ref visited, action, terminate);
        ExploreDfs(current, visited, action, terminate);
    }

    private void ExploreDfs(Node<T> start, HashSet<Node<T>> visited, Action<Node<T>> action = null, Predicate<Node<T>> terminate = null)
    {
        if (visited.Contains(start))
            return;
        action?.Invoke(start);
        visited.Add(start);
        
        var res = terminate?.Invoke(start);
        if (res is false) return;
        
        var neighbors = Nodes[start];
        foreach (var neighbor in neighbors)
            ExploreDfs(neighbor, visited, action);
    }
}

public class Node<T>
{
    public readonly T Data;
    
    public Node(T data)
    {
        Data = data;
    }

    public override bool Equals(object obj)
    {
        return obj is Node<T> node && Data.Equals(node.Data);
    }

    public override int GetHashCode()
    {
        return Data.GetHashCode();
    }
}
