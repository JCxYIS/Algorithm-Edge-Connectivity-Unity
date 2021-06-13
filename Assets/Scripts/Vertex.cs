using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    public int id;
    public List<Edge> edges = new List<Edge>();

    Rect boundries => GameManager.Instance.boundries;
    TextMesh id_text;


    // Start is called before the first frame update
    void Start()
    {
        id_text = transform.GetChild(0).GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        id_text.text = id.ToString();
    }

    public void AddEdge(Edge dest)
    {
        edges.Add(dest);
    }

    public void SetRandomPos()
    {    
        transform.position = new Vector3(
            Random.Range(boundries.xMin, boundries.xMax),
            Random.Range(boundries.yMin, boundries.yMax),
            0
        );
    }
}
