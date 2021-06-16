using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class Algorithm : MonoBehaviour
{
    protected GameManager gameManager;

    protected Dictionary<int, Vertex> vertices => gameManager.vertices;
    protected List<Edge> edges => gameManager.edges;

    private Text[] text_lt => gameManager.text_lt;
    private Text text_rb => gameManager.text_rb;



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

    protected IEnumerator Log(string log)
    {
        Debug.Log($"<color=cyan>{log}</color>");

        for(int i = text_lt.Length-1; i > 0 ; i--)
        {            
            text_lt[i].text = text_lt[i-1].text;
        }
        text_lt[0].text = log;
        yield return new WaitForSeconds(1);
    }

    protected void LogRB(string text)
    {
        Debug.Log($"<color=green>{text}</color>");
        text_rb.text = text;
    }

    protected abstract void Init();
    protected abstract IEnumerator Play();    
}