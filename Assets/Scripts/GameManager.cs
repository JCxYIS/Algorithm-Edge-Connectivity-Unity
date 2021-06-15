using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Prefabs")]
    public GameObject vertexPrefab;
    public GameObject edgePrefab;

    [Header("GO Variables")]
    public InputField inputField;
    public Rect boundries;    
    public Text[] text_lt;
    public Text text_rb;


    [Header("Runtime")]
    public Dictionary<int, Vertex> vertices = new Dictionary<int, Vertex>();
    public List<Edge> edges = new List<Edge>();


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(Text t in text_lt)
            t.text = "";
        text_rb.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
            Time.timeScale = 3;
        else if(Input.GetKey(KeyCode.LeftShift))
            Time.timeScale = 0.33f;
        else
            Time.timeScale = 1;

    }

    private void Clear()
    {
        foreach(var v in vertices)
            Destroy(v.Value.gameObject);
        foreach(var e in edges)
            Destroy(e.gameObject);

        vertices = new Dictionary<int, Vertex>();
        edges = new List<Edge>();
    }

    public void HandleInput()
    {
        try
        {
            Clear();

            string[] inputs = inputField.text.Split('\n');
            Debug.Log("測資數"+(inputs.Length-1));

            for(int i = 0; i < inputs.Length; i++)
            {
                string s = inputs[i];

                if(i == 0) // 不須那種東西
                    continue;
                if(!s.Contains(" "))
                    continue;

                string[] value = s.Split(' ');
                int from = int.Parse(value[0]);
                int dest = int.Parse(value[1]);

                if(!vertices.ContainsKey(from))
                {
                    CreateVertex(from);
                }
                if(!vertices.ContainsKey(dest))
                {
                    CreateVertex(dest);
                }
             
                // Debug.Log("Create Edge ");
                // undirected; create both
                CreateEdge(from, dest);                
                CreateEdge(dest, from);                
            }
        }
        catch(Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    private Vertex CreateVertex(int id)
    {
        GameObject g = Instantiate(vertexPrefab);
        g.name = $"Vertex {id}";

        Vertex v = g.GetComponent<Vertex>();
        v.id = id;
        v.SetRandomPos();
        vertices.Add(id, v);

        Debug.Log($"Create {g.name}");
        return v;
    }

    private void CreateEdge(int from, int dest)
    {        
        GameObject g = Instantiate(edgePrefab);
        g.name = $"Edge {from}-{dest}";

        Edge e = g.GetComponent<Edge>();
        e.Init(vertices[from], vertices[dest], 1); // 1: undirected 
        edges.Add(e);

        Debug.Log($"Create {g.name}");
    }

    
    public void PlayFordFulker()
    {
        gameObject.AddComponent<FordFulkerson>();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }
}
