using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Algorithm : MonoBehaviour
{
    protected GameManager gameManager;

    protected Dictionary<int, Vertex> vertices => gameManager.vertices;
    protected List<Edge> edges => gameManager.edges;



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        gameManager = GameManager.Instance;
        Init();
    }

    void Start()
    {
        StartCoroutine(Play());
    }

    protected void Log(string log)
    {
        Debug.Log($"<color=cyan>{log}</color>");
        // TODO 文字log
    }

    protected abstract void Init();
    protected abstract IEnumerator Play();    
}