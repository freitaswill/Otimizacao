using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct Distance
{
    public float distance;
    public GameObject firstPoint;
    public GameObject secondPoint;

    public Distance(GameObject firstPoint, GameObject secondPoint)
    {
        this.firstPoint = firstPoint;
        this.secondPoint = secondPoint;

        distance = Vector2.Distance(firstPoint.transform.position, secondPoint.transform.position);
    }

    public Distance(GameObject singlePoint)
    {
        firstPoint = singlePoint;
        secondPoint = null;

        distance = Mathf.Infinity;
    }
}



public class ClosestPairOfPoints
{
    private float getDistance(Vector3 firstPoint, Vector3 secondPoint)
    {
        return Mathf.Pow(firstPoint.x - secondPoint.x, 2) +
                   Mathf.Pow(firstPoint.y - secondPoint.y, 2);
    }

    Distance BruteForce(List<GameObject> points)
    {
        Distance closestPair = new Distance(points[0]);
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = 0; j < points.Count; j++)
            {
                if (points[i].transform.position == points[j].transform.position)
                {
                    continue;
                }

                float pointsDistance = Vector2.Distance(points[i].transform.position, points[j].transform.position);
                if (pointsDistance < closestPair.distance)
                {
                    Debug.Log("oie");
                    closestPair.firstPoint = points[i];
                    closestPair.secondPoint = points[j];
                    closestPair.distance = Vector2.Distance(closestPair.firstPoint.transform.position, closestPair.secondPoint.transform.position);
                }
                   // closestPair.Distance = pointsDistance;
                
            }
        }

        return closestPair;
    }

    public Distance FindClosestPair(List<GameObject> points)
    {
        Debug.Log(points.Count);
            return BruteForce(points);
        

        Debug.Log("Points: " + points.Count);
        List<GameObject> points1 = points.GetRange(0, points.Count / 2);
        List<GameObject> points2 = points.GetRange(points.Count / 2, points.Count / 2);

        Distance closestPairOnTheLeft = FindClosestPair(points1);
        Distance closestPairOnTheRight = FindClosestPair(points2);

        Distance closestPair;

        if (closestPairOnTheLeft.distance < closestPairOnTheRight.distance)
        {
            closestPair = closestPairOnTheLeft;
        }
        else
        {
            closestPair = closestPairOnTheRight;
        }

        GameObject middlePoint = points[points.Count / 2];

        List<GameObject> verticalStrip = new List<GameObject>();

        foreach (GameObject point in points)
        {
            if (Vector3.Distance(point.transform.position, middlePoint.transform.position) < closestPair.distance)
            {
                verticalStrip.Add(point);
            }
        }

        Distance closestPairInVerticalStrip = FindClosestPairInStrip(verticalStrip, closestPair.distance);

        if (closestPairInVerticalStrip.distance < closestPair.distance)
        {
            return closestPairInVerticalStrip;
        }

        return closestPair;
    }

    private Distance FindClosestPairInStrip(List<GameObject> points, float minDistance)
    {
        Debug.Log(points.Count);
        //points.Sort((p1, p2) => p1.position.y.CompareTo(p2.position.y));
        Distance closestPair = new Distance(points[0]);
        for (int i = 0; i < points.Count; ++i)
        {
            for (int j = i + 1; j < points.Count && (points[j].transform.position.y - points[i].transform.position.y) < minDistance; ++j)
            {
                float pointsDistance = Vector3.Distance(points[i].transform.position, points[j].transform.position);
                if (pointsDistance < minDistance)
                {
                    if (pointsDistance < closestPair.distance)
                    {
                        closestPair.firstPoint = points[i];
                        closestPair.secondPoint = points[j];
                    }
                }
            }
        }

        return closestPair;
    }
}
