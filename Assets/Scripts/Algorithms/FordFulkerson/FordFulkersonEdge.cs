using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Edge))]
public class FordFulkersonEdge : MonoBehaviour
{    
    public Edge meta;
    public override string ToString() => gameObject.name;

    /// <summary>
    /// [cf] 目前的流量
    /// </summary>
    public int flow = 0;    



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        meta = GetComponent<Edge>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        meta.weight_Text.text = flow + "/1";
    }
}