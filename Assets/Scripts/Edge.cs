using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Edge : MonoBehaviour
{
    public Vertex from;
    public Vertex dest;

    LineRenderer line;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, from.transform.position);
        line.SetPosition(1, dest.transform.position);
    }

    public void Init(Vertex from, Vertex dest)
    {
        this.from = from;
        this.dest = dest;
        from.AddEdge(this);
        dest.AddEdge(this);
    }
}
