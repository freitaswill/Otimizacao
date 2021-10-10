using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using csDelaunay;

public struct Node
{
    public Vector2f Coord;
    public float cost;
    public bool startBool, finishBool;
    public Node(Vector2f c, bool s, bool f, float cos, float d, List<Vector2f> N)
    {
        Coord = c;
        startBool = s;
        finishBool = f;
        cost = cos;
    }

    public void setCost(float coss)
    {
        cost = coss;
    }

}



public class PathFinding : MonoBehaviour
{
    Vector2f myStart;
        Vector2f myFinish;
    private int startIndex, finishIndex;
    Site site;
    bool first;

    List<Site> path = new List<Site>();

    public VoronoiDiagram VD;
    List<Node> nodes = new List<Node>();
    // Start is called before the first frame update
    void Start()
    {
        //VD = this.GetComponent<VoronoiDiagram>();
       // nodes ;
    }

    // Update is called once per frame
    void Update()
    {

    }

   public void setNodes(Dictionary<Vector2f, Site> sites){
        int i = 0;
        foreach(KeyValuePair<Vector2f, Site> kv in sites){
            Vector2f Vec = new Vector2f(kv.Key.x, kv.Key.y);
            if ( Vec == myStart)
            {
                startIndex = i;
                //Debug.Log(kv.Key.x);
                Node nod = new Node(Vec, true, false, 0, 0, VD.voronoi.NeighborSitesForSite(Vec));
                //Debug.Log(nod.Coord.x);
                nodes.Add(nod);
                Debug.Log("start");
            }
            else if (Vec == myFinish)
            {
                //nodes[i].finishBool = true;
                Node nod = new Node(Vec, true, false, 100, 5000, VD.voronoi.NeighborSitesForSite(Vec));
                //Debug.Log(nod.Coord.x);
                nodes.Add(nod);
                nodes[i] = new Node(nodes[i].Coord, nodes[i].startBool, nodes[i].finishBool, 50, 5000, VD.voronoi.NeighborSitesForSite(Vec));
                Debug.Log(nodes[i].cost);
                Debug.Log("finish");
                finishIndex = i;
            }
            else
            {
                nodes.Add(new Node(Vec, true, false, 100, 5000, VD.voronoi.NeighborSitesForSite(Vec)));
                Node nod = new Node(Vec, true, false, 100, 5000, VD.voronoi.NeighborSitesForSite(Vec));
                //Debug.Log(nod.Coord.x);
                nodes.Add(nod);
            }
            i++;
        }
        first = true;
        FindPath(nodes[startIndex].Coord);
        ShortestPath(nodes[finishIndex].Coord);
}
    public List<Site> TriggerPathFinding(Vector2f start, Vector2f finish, VoronoiDiagram V)
    {
        myStart = start;
        myFinish = finish;
        VD = V;
        setNodes(V.sites);
        return path;
    }
    public void FindPath(Vector2f coord)
    {

        if (VD.getIndexedSites().TryGetValue(coord, out site))
        {
            if (first)
            {
                site.costSum = 0;
                first = false;
            }

            List<Site> neighbors = site.NeighborSites();
            //Debug.Log(site.Coord);
            foreach (Site neighbor in neighbors)
            {
                if (site.costSum + neighbor.cost < neighbor.costSum)
                {
                    neighbor.costSum = site.costSum + site.cost;
                    FindPath(neighbor.Coord);
                    //Debug.Log("Caminho mais curto");
                }
            }
            
        }



        path.Clear();


        //    foreach (Vector2f neighbor in neighbors) {
        //        if(neighbor + Site.Coord + Site.cost)

        //            }
    }
    public void ShortestPath(Vector2f coord)
    {
        
        

        if (VD.getIndexedSites().TryGetValue(coord, out site))
        {
            path.Add(site);
            Debug.Log(site.Coord);
            
            List<Site> neighbors = site.NeighborSites();
            //Debug.Log(site.Coord);
            foreach (Site neighbor in neighbors)
            {
                Debug.Log(site.costSum);
                Debug.Log(neighbor.costSum);
                if (site.costSum > neighbor.costSum)
                {
                    path.Add(neighbor);
                    ShortestPath(neighbor.Coord);
                    Debug.Log("Caminho mais curto");
                }
                if (neighbor.Coord == nodes[startIndex].Coord)
                {

                    Debug.Log("Cheguei no final");
                }
            }

        }

    }

}

