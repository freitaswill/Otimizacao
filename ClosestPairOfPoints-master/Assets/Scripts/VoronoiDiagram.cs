using UnityEngine;
using System.Collections.Generic;

using csDelaunay;

public class VoronoiDiagram : MonoBehaviour
{

    // The number of polygons/sites we want
    public int polygonNumber = 200;

    // This is where we will store the resulting data
    public Dictionary<Vector2f, Site> sites;
    private List<Edge> edges;
    private Vector2f V;
    public Voronoi voronoi;
    List<Site> Path;

    public void triggerVoronoi(List<Vector2f> points)
    {
        // Create your sites (lets call that the center of your polygons)
        //points = CreateRandomPoint();
        V = new Vector2f(20, 20);
        //List<Vector2f> points = new List<Vector2f>();
        //points.Add(new Vector2f(1.5, 5));
        //points.Add(new Vector2f(5.3, 0));
        //points.Add(new Vector2f(0, 0));
        //points.Add(new Vector2f(7.3, 0));
        //for (int i = 0; i < points.Count; i++)
        //{
        //    points[i] = points[i] * (19);
        //}

        // Create the bounds of the voronoi diagram
        // Use Rectf instead of Rect; it's a struct just like Rect and does pretty much the same,
        // but like that it allows you to run the delaunay library outside of unity (which mean also in another tread)
        Rectf bounds = new Rectf(0, 0, 512, 512);

        // There is a two ways you can create the voronoi diagram: with or without the lloyd relaxation
        // Here I used it with 2 iterations of the lloyd relaxation
        voronoi = new Voronoi(points, bounds);

        // But you could also create it without lloyd relaxtion and call that function later if you want
        //Voronoi voronoi = new Voronoi(points,bounds);
        //voronoi.LloydRelaxation(5);

        // Now retreive the edges from it, and the new sites position if you used lloyd relaxtion
        sites = voronoi.SitesIndexedByLocation;
        edges = voronoi.Edges;

        DisplayVoronoiDiagram();
    }

    private List<Vector2f> CreateRandomPoint()
    {
        // Use Vector2f, instead of Vector2
        // Vector2f is pretty much the same than Vector2, but like you could run Voronoi in another thread
        List<Vector2f> points = new List<Vector2f>();
        for (int i = 0; i < polygonNumber; i++)
        {
            //points.Add(new Vector2f(Random.Range(0, 26), Random.Range(0, 26)));
        }

        return points;
    }

    // Here is a very simple way to display the result using a simple bresenham line algorithm
    // Just attach this script to a quad
    private void DisplayVoronoiDiagram()
    {
        Texture2D tx = new Texture2D(512, 512);
        foreach (KeyValuePair<Vector2f, Site> kv in sites)
        {
            tx.SetPixel((int)kv.Key.x, (int)kv.Key.y, Color.red);
        }
        foreach (Edge edge in edges)
        {
            // if the edge doesn't have clippedEnds, if was not within the bounds, dont draw it
            if (edge.ClippedEnds == null) continue;

            DrawLine(edge.ClippedEnds[LR.LEFT], edge.ClippedEnds[LR.RIGHT], tx, Color.black);
        }

        List<Vector2f> neighbors = new List<Vector2f>();
        //List<Site> sitesNeighbors = new List<Site>();
        foreach (KeyValuePair<Vector2f, Site> kv in sites)
        {
            Vector2f v2 = new Vector2f(kv.Key.x, kv.Key.y);
            neighbors = voronoi.NeighborSitesForSite(v2);
            
            for(int i = 0; i < neighbors.Count; i++)
                DrawLine(v2, neighbors[i], tx, Color.red);
        }
            //DrawLine(edge.ClippedEnds[LR.LEFT], edge.ClippedEnds[LR.RIGHT], tx, Color.black);
        
        tx.Apply();

        tx.Apply();

        this.GetComponent<Renderer>().material.mainTexture = tx;
    }

    // Bresenham line algorithm
    private void DrawLine(Vector2f p0, Vector2f p1, Texture2D tx, Color c, int offset = 0)
    {
        int x0 = (int)p0.x;
        int y0 = (int)p0.y;
        int x1 = (int)p1.x;
        int y1 = (int)p1.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            tx.SetPixel(x0 + offset, y0 + offset, c);

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }
    public VoronoiDiagram getVoronoi()
    {
        return this;
    }
    public Dictionary<Vector2f, Site> getIndexedSites()
    {
        return voronoi.SitesIndexedByLocation;
    }
    public void SetPath(List<Site> P)
    {
        Path = P;
    }
    public void DrawPath()
    {
        Texture2D tx = new Texture2D(512, 512);
        
        //List<Site> sitesNeighbors = new List<Site>();
        
            for (int i = 0; i < Path.Count-1; i++)
                DrawLine(Path[i].Coord, Path[i+1].Coord, tx, Color.red);
        
        //DrawLine(edge.ClippedEnds[LR.LEFT], edge.ClippedEnds[LR.RIGHT], tx, Color.black);

        tx.Apply();

        tx.Apply();

        this.GetComponent<Renderer>().material.mainTexture = tx;
    }
}