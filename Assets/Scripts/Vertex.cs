using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    public int id;

    /// <summary>
    /// 指出去的 edges
    /// </summary>
    /// <typeparam name="Edge"></typeparam>
    /// <returns></returns>
    public List<Edge> edges = new List<Edge>();

    /// <summary>
    /// 被指向的 edges
    /// </summary>
    /// <typeparam name="Edge"></typeparam>
    /// <returns></returns>
    public List<Edge> reverseEdges = new List<Edge>();


    [HideInInspector] public Color color = Color.white;

    Rect boundries => GameManager.Instance.boundries;
    public override string ToString() => gameObject.name;
    SpriteRenderer spriteRenderer;
    TextMesh id_text;


    // Start is called before the first frame update
    void Start()
    {
        id_text = transform.GetChild(0).GetComponent<TextMesh>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        id_text.text = id.ToString();
        spriteRenderer.color = color;
    }

    public void AddEdge(Edge edge)
    {
        edges.Add(edge);

        Edge reverseEdge = edge.dest.edges.Find(e=>e.dest == this);
        if(reverseEdge != null && edge.offsetY == 0) // bi-directional
        {
            edge.SetOffsetY(1);
            reverseEdge.SetOffsetY(-1);
        }
    }

    public void AddReverseEdge(Edge edge)
    {
        reverseEdges.Add(edge);
    }

    public void SetRandomPos()
    {    
        transform.position = new Vector3(
            Random.Range(boundries.xMin, boundries.xMax),
            Random.Range(boundries.yMin, boundries.yMax),
            0
        );

        // FIXME for 測資用
        switch(id)
        {
            case 1: transform.position = new Vector3(-16.21f, 1.45f, 0); break;
            case 2: transform.position = new Vector3(-14.37f, 0.86f, 0); break;
            case 3: transform.position = new Vector3(-16.13f, -3.66f, 0); break;
            case 4: transform.position = new Vector3(-14.18f, -2.74f); break;
            case 5: transform.position = new Vector3(-11.93f, 1.28f, 0); break;
            case 6: transform.position = new Vector3(-8.08f, 1.12f, 0); break;
            case 7: transform.position = new Vector3(-12.38f, -2.78f, 0); break;
            case 8: transform.position = new Vector3(-8.33f, -2.05f, 0); break;
        }
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }
}
