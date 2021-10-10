using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    private GameObject point;
    private LineRenderer line;
    private LineRenderer line3;
    private LineRenderer line4;
    private LineRenderer line5;
    private ClosestPairOfPoints closestPairOfPoints;
    private GrahamScan grahamScan;
    public GameObject[] gameObject;
    public Vector2[] myPoints;
    List<GameObject> points;
    List<Vector2> points2;

    private void Start()
    {
        point = Resources.Load<GameObject>("Prefabs/Point");
        line = GetComponent<LineRenderer>();
        closestPairOfPoints = new ClosestPairOfPoints();
        grahamScan = new GrahamScan();
        gameObject = new GameObject[16];
        myPoints = new Vector2[16];
        gameObject[1] = (GameObject)Instantiate(point, new Vector2(-13, -0.5f), Quaternion.identity, transform)   ;
        gameObject[2] = (GameObject)Instantiate(point, new Vector2(-10.5f, 11.5f), Quaternion.identity, transform);
        gameObject[3] = (GameObject)Instantiate(point, new Vector2(-10, -9), Quaternion.identity, transform)      ;
        gameObject[4] = (GameObject)Instantiate(point, new Vector2(-4.5f, 2), Quaternion.identity, transform)     ;
        gameObject[5] = (GameObject)Instantiate(point, new Vector2(-1, -8.5f), Quaternion.identity, transform)    ;
        gameObject[6] = (GameObject)Instantiate(point, new Vector2(0.5f, -6), Quaternion.identity, transform)     ;
        gameObject[7] = (GameObject)Instantiate(point, new Vector2(0.5f, 12), Quaternion.identity, transform)     ;
        gameObject[8] = (GameObject)Instantiate(point, new Vector2(2, -12.5f), Quaternion.identity, transform)    ;
        gameObject[9] = (GameObject)Instantiate(point, new Vector2(3.5f, -11), Quaternion.identity, transform)    ;
        gameObject[10] =(GameObject) Instantiate(point, new Vector2(5.5f, -3), Quaternion.identity, transform)    ;
        gameObject[11] =(GameObject) Instantiate(point, new Vector2(5.5f, 7), Quaternion.identity, transform)     ;
        gameObject[12] =(GameObject) Instantiate(point, new Vector2(5, -11.5f), Quaternion.identity, transform)   ;
        gameObject[13] =(GameObject) Instantiate(point, new Vector2(6.5f, -3.2f), Quaternion.identity, transform) ;
        gameObject[14] =(GameObject) Instantiate(point, new Vector2(7, 10), Quaternion.identity, transform)       ;
        gameObject[15] =(GameObject) Instantiate(point, new Vector2(9, 5), Quaternion.identity, transform)        ;
        gameObject[0] = (GameObject)Instantiate(point, new Vector2(11.5f, 4), Quaternion.identity, transform);
       
        points = new List<GameObject>();
        points2 = new List<Vector2>();

        for (int i = 0; i < 16; i++)
        {
            myPoints[i] = new Vector2(gameObject[i].transform.position.x, gameObject[i].transform.position.y);
            
            
            points.Add(gameObject[i]);
            points2.Add(myPoints[i]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DefaultPoints();
            Distance smallestDistance = closestPairOfPoints.FindClosestPair(points);

           //smallestDistance.firstPoint.GetComponent<SpriteRenderer>().color = Color.red;
           //smallestDistance.secondPoint.GetComponent<SpriteRenderer>().color = Color.red;

            line.SetPositions(new Vector3[]
            {
                smallestDistance.firstPoint.transform.position,
                smallestDistance.secondPoint.transform.position
            });
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RandomPoints();
            Distance smallestDistance = closestPairOfPoints.FindClosestPair(points);

            //smallestDistance.firstPoint.GetComponent<SpriteRenderer>().color = Color.red;
            //smallestDistance.secondPoint.GetComponent<SpriteRenderer>().color = Color.red;

            line.SetPositions(new Vector3[]
            {
                smallestDistance.firstPoint.transform.position,
                smallestDistance.secondPoint.transform.position
            });
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Clear();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            points.Sort((p1, p2) => p1.transform.position.y.CompareTo(p2.transform.position.y));
            points2.Sort((p1, p2) => p1.y.CompareTo(p2.y));
            points2 = grahamScan.triggerGrahamScan(points2, points);
            line.SetPositions(new Vector3[]
            {
                new Vector3(points2[0].x, points2[0].y, 0),
                 new Vector3(points2[1].x, points2[1].y, 0)
            });
            LineRenderer line2 = points[0].AddComponent<LineRenderer>();
            line2.SetPositions(new Vector3[]
            {
                new Vector3(points2[2].x, points2[2].y, 0),
                 new Vector3(points2[1].x, points2[1].y, 0)
            });

        }
    }

    private void DefaultPoints()
    {
        Clear();
        gameObject[1].transform.position = new Vector2(-13, -0.5f);
        gameObject[2].transform.position = new Vector2(-10.5f, 11.5f);
        gameObject[3].transform.position = new Vector2(-10, -9);
        gameObject[4].transform.position = new Vector2(-4.5f, 2);
        gameObject[5].transform.position = new Vector2(-1, -8.5f);
        gameObject[6].transform.position = new Vector2(0.5f, -6);
        gameObject[7].transform.position = new Vector2(0.5f, 12);
        gameObject[8].transform.position = new Vector2(2, -12.5f);
        gameObject[9].transform.position = new Vector2(3.5f, -11);
        gameObject[10].transform.position = new Vector2(5.5f, -3);
        gameObject[11].transform.position = new Vector2(5.5f, 7);
        gameObject[12].transform.position = new Vector2(5, -11.5f);
        gameObject[13].transform.position = new Vector2(6.5f, -3.2f);
        gameObject[14].transform.position = new Vector2(7, 10);
        gameObject[15].transform.position = new Vector2(9, 5);
        gameObject[0].transform.position = new Vector2(11.5f, 4);
    }

    private void RandomPoints()
    {
        Clear();

        for (int i = 0; i < 16; i++)
        {
            gameObject[i].transform.position = new Vector2(Random.Range(-15f, 15f), Random.Range(-15f, 15f));


            //Instantiate(point, randomPosition, Quaternion.identity, transform).name = "Point";
        }
    }

    //private List<Transform> GetPoints()
    //{
    //    List<Transform> points = new List<Transform>();
    //    points.Clear();
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        points.Add(transform.GetChild(i));
    //    }
    //    Debug.Log(points.Count);
    //    points = points.OrderBy(point => point.position.y).ToList();
    //    Debug.Log(points.Count);
    //    return points;
    //}

    private void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            //Destroy(transform.GetChild(i).gameObject);
        }
    }
}
