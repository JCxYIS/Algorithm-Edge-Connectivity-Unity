using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Edge : MonoBehaviour
{
    // Algo var
    /// <summary>
    /// 是否帶有方向？
    /// </summary>
    public bool isDirected {get; protected set;}
    
    /// <summary>
    /// 起點
    /// </summary>
    public Vertex from {get; protected set;}

    /// <summary>
    /// 終點
    /// </summary>
    public Vertex dest {get; protected set;}

    /// <summary>
    /// 館子最大流量。無權重圖為 1
    /// </summary>
    public int weight {get; protected set;}



    // var
    public enum Behavior {Idle, Flash}
    public Behavior behavior = Behavior.Idle;
    public Color color = Color.white;

    // GAME OBJECTS
    LineRenderer lineRenderer;
    [HideInInspector] public TextMesh weight_Text;


    /// <summary>
    /// (Flow Chart) 管子最大容量
    /// </summary>
    public int capacity => weight;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        weight_Text = transform.GetChild(0).GetComponent<TextMesh>();
    }
    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, from.transform.position);
        lineRenderer.SetPosition(1, dest.transform.position);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        
        // TODO directed

        weight_Text.transform.position = (from.transform.position + dest.transform.position) / 2f + Vector3.up * 0.48763f;
        weight_Text.text = weight.ToString();
        weight_Text.color = color;
    }



    public void Init(bool directed, Vertex from, Vertex dest, int weight = 1)
    {
        this.isDirected = directed;
        this.from = from;
        this.dest = dest;
        this.weight = weight;

        from.AddEdge(this);        
        if(!isDirected)
            dest.AddEdge(this);
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }

    public void SetBehavior(Behavior behavior)
    {
        this.behavior = behavior;
    }
}
