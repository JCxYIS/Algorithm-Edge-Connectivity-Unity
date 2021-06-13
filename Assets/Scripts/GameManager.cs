using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Prefabs")]
    public GameObject vertexPrefab;
    public GameObject edgePrefab;

    [Header("Variables")]
    public InputField inputField;
    public Rect boundries;    

    [Header("Runtime")]
    public Dictionary<int, Vertex> vertex = new Dictionary<int, Vertex>();
    public List<GameObject> edges = new List<GameObject>();


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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Clear()
    {
        foreach(var v in vertex)
            Destroy(v.Value.gameObject);
        foreach(var e in edges)
            Destroy(e);

        vertex = new Dictionary<int, Vertex>();
        edges = new List<GameObject>();
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

                if(!vertex.ContainsKey(from))
                {
                    CreateVertex(from);
                }
                if(!vertex.ContainsKey(dest))
                {
                    CreateVertex(dest);
                }
             
                // Debug.Log("Create Edge ");
                CreateEdge(from, dest);                
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
        g.name = $"Vertice {id}";

        Vertex v = g.GetComponent<Vertex>();
        v.id = id;
        v.SetRandomPos();
        vertex.Add(id, v);

        Debug.Log($"Create {g.name}");
        return v;
    }

    private void CreateEdge(int from, int dest)
    {        
        GameObject g = Instantiate(edgePrefab);
        g.name = $"Edge {from}-{dest}";

        Edge e = g.GetComponent<Edge>();
        e.Init(vertex[from], vertex[dest]);

        Debug.Log($"Create {g.name}");
    }

    
}
