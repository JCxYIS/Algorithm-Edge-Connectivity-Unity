using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Edge : MonoBehaviour
{
    // Algo var
    /// <summary>
    /// 是否帶有方向？ ALWAYS TRUE NOW
    /// </summary>
    // public bool isDirected {get; protected set;}
    
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
    public short offsetY = 0;

    // GAME OBJECTS
    LineRenderer lineRenderer;
    public TextMesh weight_Text;
    public SpriteRenderer arrow_sprite;


    /// <summary>
    /// (Flow Chart) 管子最大容量
    /// </summary>
    public int capacity => weight;

    public override string ToString() => gameObject.name;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        weight_Text = transform.GetChild(0).GetComponent<TextMesh>();
        arrow_sprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 pos1 = from.transform.position + (Vector3.right + Vector3.up) * offsetY * 0.1f;
        Vector3 pos2 = dest.transform.position + (Vector3.right + Vector3.up) * offsetY * 0.1f;
        lineRenderer.SetPosition(0, pos1);
        lineRenderer.SetPosition(1, pos2);
        // arrow_sprite.transform.position = Vector3.Lerp(pos1, pos2, 0.87f);

        // TODO arrow and that stuffs
        // arrow_sprite.transform.rotation = Quaternion.identity;
        // arrow_sprite.transform.Rotate(transform.position, Vector3.Angle(pos1, pos2));
        
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        

        weight_Text.transform.position = (from.transform.position + dest.transform.position) / 2f + Vector3.up * offsetY * 0.48763f;
        weight_Text.text = weight.ToString();
        weight_Text.color = color;
    }



    public void Init(Vertex from, Vertex dest, int weight = 1)
    {
        // this.isDirected = directed;
        this.from = from;
        this.dest = dest;
        this.weight = weight;

        from.AddEdge(this);        
        dest.AddReverseEdge(this);
        // if(!isDirected)
        //     dest.AddEdge(this);
    }

    /// <summary>
    /// -1~1
    /// </summary>
    /// <param name="offsetY"></param>
    public void SetOffsetY(short offsetY)
    {
        this.offsetY = offsetY;
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
