using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Vertex))]
public class FordFulkersonVertex : MonoBehaviour
{
    public Vertex meta {get; private set;}
    public override string ToString() => gameObject.name;

    public List<FordFulkersonEdge> edges = new List<FordFulkersonEdge>();


    /// <summary>
    /// [Gf] 還可以容納水通過的館子
    /// </summary>
    public List<FordFulkersonEdge> ResidualEdges => edges.FindAll(e => e.flow < e.meta.capacity);

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        meta = GetComponent<Vertex>();
        foreach(Edge e in meta.edges)
        {
            edges.Add(e.GetComponent<FordFulkersonEdge>());
        }
    }
}