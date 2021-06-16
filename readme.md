# Program 5 - Edge Connectivity
###### tags: `演算法`, `109-2`

## 題目簡述
給我們一份無向、無權重的 connected graph，要找到他的邊連通度 (edge connectivity)，以及其 edge。  

如以下圖，邊連通度為 2，可以在 (2-5) 以及 (4-7) 畫兩刀達成。  
![](https://i.imgur.com/yoCmMw2.png)

作業要求使用 Ford-Fulkerson + Edmonds-Karp 演算法來達成。  

## 構想
要求邊連通度，相當於求出這整張圖的最小最小割 (minimum-cut)，也就是求出這整張圖所有任兩個點間最小的 min-cut。但是根據作業提示(ex. 26.2-11)，O(n) max flow computations are sufficient for finding a min cut，也就是說，我們只要求出一個點到其他點之間最小的 min-cut 即可。  

求 min-cut 可以利用最大流最小割原理取得，這裡作業直接要求使用 Ford-Fulkerson + Edmonds-Karp 演算法。  

使用該演算法得到 maximum-flow 之後，判斷該 max-flow 是否為最小，如果是，那麼其 min-cut 也會為最小，把 min-cut 求出之後儲存起來，等待下一個更小的出現。  

最後印出儲存的 min-cut 數及其分別連接的點，完成！  

## 演算法設計

由於 Ford Fulkerson + Edmonds-Karp 演算法是設計給有向圖的，而且會有「反向流量」的情形發生，因此，我們先把無向無權圖的每個 edge 拆成 正反 兩個方向，並且每個管子賦予他一個最大流量 1  
![](https://i.imgur.com/2XsIWxV.jpg)

進入演算法，我們鎖定輸入的第一個點為起點 source，開始找與其他點的 max-flow  
<!-- ![](https://i.imgur.com/dt25yT5.jpg) -->
![](https://i.imgur.com/6oN7RDz.jpg)


開始 Ford Fulkerson / Edmonds-Karp，踏上尋找 max-flow 的旅程。  
第一步，尋找 Augmenting Path，這個 path 有以下幾個規則
- 從 source 走到 sink 
- 經過的管子不能是滿的 (residualPath: flow < capacity 才能通過)
- 不能重複走同樣的管子

這裡我們採用 BFS 來取得此路徑，也就是 Edmonds-Karp 演算法。  

![](https://i.imgur.com/7dQDDVd.jpg)
  


第二步，尋找此路徑間最窄小的管徑(在這裡通常是 1)，並將這之間所有的管子按以下規則灌水  
- 流經管子的 flow 會增加 1
- 流經管子的反向，flow 會減少 1，允許負數產生
- 紀錄總流量增加 1，等下會用到

<!-- ![](https://i.imgur.com/FhpL0ZN.png) -->
![](https://i.imgur.com/4FMlctf.jpg)


重複上面的步驟直到找不到 Argumenting Path。
![](https://i.imgur.com/y1kkwRF.jpg)
![](https://i.imgur.com/XPwFYaK.jpg)
![](https://i.imgur.com/ooiFSKc.jpg)

完成之後就可以直接得到 max-flow (就是前面紀錄的總流量)，並算出 min-cut 了。  
當然，如果 max-flow 是我們目前求過的最小值，我們才需要去算 min-cut。如果不是的話，就直接進到下一個點。  
min-cut 是由以下規則求出，遞迴圖中：
- 從 source 出發 
- 經過的管子不能是滿的 (residualPath: flow < capacity 才能通過)
- 不能重複走同樣的管子

完成後，記錄經過的所有 Vertex 點 中
- 有被遞迴過
- 但其某條 edge 指向的 vertex 沒被遞迴過

的所有這種 edge，這個概念很像是海峽兩岸，你一路沿著陸地(管子沒滿)的路上到了海邊，發現所有的跨海大橋都堵住了(管子滿了)。總之，這些 edge 即為 min-cut，把他存下來吧！  
這裡我們使用的是 DFS  
![](https://i.imgur.com/4H0tUVk.jpg)

到這裡，這個點的計算就結束了，可以進行下一個點的計算：重新開始算 max-flow，繼續努力。   
或是，如果我們已經把所有點爬過了，到這裡我們就可以印出我們儲存的最小 max-flow 與 min-cut 了
![](https://i.imgur.com/jz48D0m.jpg)

## not-so-pseudo pseudo codes

### 演算法
```csharp
int min_maxFlow = int.max
List<Edge> min_minCut
for(int i = 0; i < graph.vertex.count; i++)
    if(i == 0)
        continue;
    
    while(true)
        argumentingPath = FindArgumentingPath(graph.vertex[0], graph.vertex[i])
        if(argumentingPath == null)
            if(totalFlow < min_maxFlow)
                min_maxFlow = totalFlow
                min_minCut = FindMinCut(graph.vertex[0])
        
        int flow = FindSmallestResidual(argumentingPath)
        totalFlow += cfp; // Fm += Cf(p)
        foreach Edge e in augmentingPath: 
            e.flow += flow;
            e.flow.reverse -= flow

```

### FindArgumentingPath

```csharp
FindArgumentingPath(source, sink)
    從 source 到 sink 進行 BFS，其中只能經過 residualPath，取得遞迴順序陣列 parent[]
    
    List<Edge> result
    從 sink，沿著 parent 加進 result
    return result    
```
### FindSmallestResidual

```csharp
FindSmallestResidual(augmentingPath)
    int minFlow = int.max
    foreach Edge e in augmentingPath: 
        if(minFlow > e.capacity)
            minFlow = e.capacity

    return minFlow
```

### FindMinCut

```csharp
FindMinCut(source)
    從 source 進行 DFS，其中只能經過 residualPath，取得點是否被遞迴陣列 visited[]
    
    List<Edge> result
    foreach(Vertex v in graph)
        foreach(Edge e in v.edges)
            if(visted[v] && !visited[v.e.dest])
                result.Add(e)
    return result
    
```

## 心得
當初在看作業要求時沒看到有限制要用什麼語言寫，我就拿 Unity 來硬幹啦  
編輯器 繪圖 演算法 全部自己來  
除了痛苦的理解演算法是要怎麼跑的過程外，製作過程其實還滿快樂的。  
看來我果然不是製作演算法的料ww

Anyway, 如果之後有時間有閒，我可以試著把其他演算法這樣 visualize，這樣應該比較好理解 (還是我專題要做這個? hmmmmmmmm

<!-- 喔對 空白鍵可以加速 左Shift減速 -->

打到這裡想起來還有加分題沒做....

## References
- Unit 10 Maximum Flow https://ncueeclass.ncu.edu.tw/media/doc/28212
- Ford Fulkerson algorithm for Max Flow https://youtu.be/hmIrJCGPPG4

- 福特-富爾克森算法 https://zh.wikipedia.org/wiki/%E7%A6%8F%E7%89%B9-%E5%AF%8C%E5%B0%94%E5%85%8B%E6%A3%AE%E7%AE%97%E6%B3%95

- 埃德蒙兹-卡普算法 https://zh.wikipedia.org/wiki/%E5%9F%83%E5%BE%B7%E8%92%99%E5%85%B9-%E5%8D%A1%E6%99%AE%E7%AE%97%E6%B3%95

- Find minimum s-t cut in a flow network https://www.geeksforgeeks.org/minimum-cut-in-a-directed-graph/

- Algorithm - Ch5 網路流 與 最大流最小割定理 Network Flow and Maximum Flow Minimum Cut Theorem https://mropengate.blogspot.com/2015/01/algorithm-ch4-network-flow.html


- Finding edge connectivity of a network by using Maximum Flow algorithm https://stackoverflow.com/questions/16384151/finding-edge-connectivity-of-a-network-by-using-maximum-flow-algorithm

- Minimum Cuts in Near-Linear Time https://arxiv.org/abs/cs/9812007

- Edge connectivity / Vertex connectivity https://cp-algorithms.com/graph/edge_vertex_connectivity.html

- Finding the max flow of an undirected graph with Ford-Fulkerson https://math.stackexchange.com/questions/677743/finding-the-max-flow-of-an-undirected-graph-with-ford-fulkerson