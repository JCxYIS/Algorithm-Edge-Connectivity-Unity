using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// JCxYIS 2021.06.14
/// ===References===
/// - 中大資工 何錦文 Ch.10 Maximum Flow
/// - Ford Fulkerson algorithm for Max Flow https://youtu.be/hmIrJCGPPG4
/// </summary>
public class FordFulkerson : Algorithm
{

    /// <summary>
    /// [Gf] 圖(G)中，還可以容納水通過的館子的圖
    /// 這裡直接拿 G 來用 可改進
    /// </summary>
    List<FordFulkersonVertex> residualGraph = new List<FordFulkersonVertex>();

    /// <summary>
    /// [p] Gf中，可以從起點流到終點的路徑
    /// residualCapacity: p中最小容量的邊的 capacity
    /// </summary>
    List<FordFulkersonEdge> augmentingPath;

    /// <summary>
    /// [Fm]
    /// </summary>
    int totalFlow = 0;


    protected override void Init()
    {
        edges.ForEach(e=>e.gameObject.AddComponent<FordFulkersonEdge>());
        foreach(var v in vertices.Values)
        {
            residualGraph.Add(
                v.gameObject.AddComponent<FordFulkersonVertex>()
            );
        }
    }

    protected override IEnumerator Play()
    {
        

        


        Log("讓我們開始吧");
        
            // find Gf 
            // residualGraph = FindResidualGraph();

            
        // O(n) max flow computations are sufficient for finding a min cut. (ex. 26.2-11)
        // Fix a node v, iterating all possible w != v and compute maximum flow
        Log($"鎖定點 {residualGraph[0]} 為出發點，尋找其至每個點的 Max Flow / Min Cut");
        int max_maxFlow = 0;        
        for(int i = 0; i < residualGraph.Count; i++)
        {
            // clear all flow
            residualGraph.ForEach(vertex=>{
                vertex.edges.ForEach(e=>e.flow = 0);
                vertex.meta.SetColor(Color.white);
            });
            totalFlow = 0;
            residualGraph[0].meta.SetColor(Color.green);
            residualGraph[i].meta.SetColor(Color.red);
            yield return new WaitForSeconds(1);
            

            Log($"第 {i}/{residualGraph.Count-1} 找！{residualGraph[0]} 至 {residualGraph[i]}");
            while(true) // exists p in Gf
            {
                Log($"尋找 {residualGraph[0]} 至 {residualGraph[i]} 的 Augmenting Path");
                augmentingPath = FindArgumentingPath(residualGraph[0], residualGraph[i]);

                // if desnt exists p, say bye
                if(augmentingPath == null || augmentingPath.Count == 0)
                {
                    Log("找不到可以增廣的 Augmenting Path，本找結束");
                    if(totalFlow > max_maxFlow)
                    {
                        max_maxFlow = totalFlow;
                        Log($"刷新最大 max flow 為 {max_maxFlow}");
                    }
                    else
                    {
                        Log($"max flow 為 {totalFlow}");
                    }
                    yield return new WaitForSeconds(1);
                    break;
                }
                else
                {
                    string log = "為 ";                
                    augmentingPath.ForEach(e=>{
                        e.meta.SetColor(Color.magenta);
                        log += e.ToString()+" ";
                    });
                    Log(log);
                    yield return new WaitForSeconds(1);
                    augmentingPath.ForEach(e=>e.meta.SetColor(Color.white));
                }

                // find smallest capacity
                Log("尋找 Augmenting Path 途中最小的管徑");
                int cfp = FindSmallestCapacity(augmentingPath); // Cf(p) = smallest capcity on p
                yield return new WaitForSeconds(1);

                Log($"把沿途所經的路徑流量、以及總流量，全部加上此值 {cfp}");
                totalFlow += cfp; // Fm += Cf(p)
                foreach(FordFulkersonEdge e in augmentingPath) // foreach edge e in p
                {
                    e.flow += cfp; // cf(u, v) -= Cf(p)
                    // // cf(v, u) += Cf(p)
                }                

                Log("完成一輪，進行下一輪");
                yield return new WaitForSeconds(1);
            }            
        }
        Log("演算法結束。最大Max Flow 為 "+max_maxFlow);

        // throw new System.NotImplementedException();
    }


    /// <summary>
    /// Find Gf
    /// </summary>
    // List<Edge> FindResidualGraph()
    // {
    //     List<Edge> gf = new List<Edge>();
    //     foreach(Edge e in edges)
    //     {
    //         if( fe.flow < fe.edge.capacity )
    //         {
    //             gf.Add(e);
    //         }
    //     }
    //     return gf;
    // }

    List<FordFulkersonEdge> FindArgumentingPath(FordFulkersonVertex source, FordFulkersonVertex sink)
    {        
        List<FordFulkersonEdge> result = new List<FordFulkersonEdge>();
        result = BFSFindPath(source, sink);
        return result;
    }

    /// <summary>
    /// 在Ford Fulkson 使用 BFS 就是 Edmond Karp
    /// </summary>
    List<FordFulkersonEdge> BFSFindPath(FordFulkersonVertex source, FordFulkersonVertex sink)
    {

        // bfs for software engineers!
        Queue<FordFulkersonVertex> bfsqueue = new Queue<FordFulkersonVertex>();
        bfsqueue.Enqueue(source);

        // 在BFS時，某點是從哪個點爬過來的？
        Dictionary<FordFulkersonVertex, FordFulkersonEdge> parent = new Dictionary<FordFulkersonVertex, FordFulkersonEdge>();

        // visited dict
        Dictionary<FordFulkersonVertex, bool> visited = new Dictionary<FordFulkersonVertex, bool>();
        foreach(var v in residualGraph)
        {
            visited.Add(v, false);
        }

        // loop
        while(bfsqueue.Count != 0)
        {
            FordFulkersonVertex now = bfsqueue.Dequeue();
            foreach(FordFulkersonEdge nowedge in now.ResidualEdges)
            {
                FordFulkersonVertex dest = nowedge.meta.dest.GetComponent<FordFulkersonVertex>();
                if(!visited[dest])
                {
                    visited[dest] = true;
                    parent[dest] = nowedge;
                    bfsqueue.Enqueue(dest);
                }
            }            
        }

        // sink 沿著 parent 爬回來
        if(!parent.ContainsKey(sink))
            return null;
        List<FordFulkersonEdge> path = new List<FordFulkersonEdge>();
        for(FordFulkersonVertex now = sink; now != source; )
        {
            path.Add(parent[now]);
            now = parent[now].meta.from.GetComponent<FordFulkersonVertex>();
        }
        return path;
    }

    /// <summary>
    /// 也做了 DFS，後來發現藥用 Edmond Karp...
    /// </summary>
    /// <param name="source"></param>
    /// <param name="sink"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    List<FordFulkersonEdge> DFSFindPath(FordFulkersonVertex source, FordFulkersonVertex sink, List<FordFulkersonEdge> path)
    {
        // 已經到終點了
        if(source == sink)
            return path;
        
        // 找 source 所有能出去的 edge
        foreach(FordFulkersonEdge e in source.ResidualEdges)
        {
            if(!path.Contains(e)) // 不是 back edge
            {
                path.Add(e);
                path = DFSFindPath(e.meta.dest.GetComponent<FordFulkersonVertex>(), sink, path); // DFS，讓下一個點遞迴                
                return path; // 可能是 null
            }
        }

        // 找不到能出去的 edge
        return null;
    }

    /// <summary>
    /// 尋找 Augmenting Path 途中，最小的管徑
    /// </summary>
    int FindSmallestCapacity(List<FordFulkersonEdge> path)
    {
        int currentMin = int.MaxValue;
        
        foreach(FordFulkersonEdge edge in path)
        {   
            if(currentMin > edge.meta.capacity)
            {
                currentMin = edge.meta.capacity;
            }
        }

        return currentMin;
    }
}
