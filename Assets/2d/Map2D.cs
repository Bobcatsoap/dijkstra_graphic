using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Map2D : MonoBehaviour
{
    private Point[,] _points;
    private float _mapWidth, _mapLength;
    public int gridWidthDensity;
    private int pointWidthCount, pointLengthCount;
    private float sphereSize;
    public GameObject cube;
    public GameObject spherePrefab;
    private int[,] _shortPoints;
    public GameObject endPoint;
    public GameObject startPoint;
    private float cellWidth;
    private float cellLength;

    class Point
    {
        //坐标
        public float x;
        public float y;
        public GameObject gameObject;
        public int shortPath;
    }

    // Use this for initialization
    void Start()
    {
        _mapWidth = Mathf.Abs(endPoint.transform.position.x - startPoint.transform.position.x);
        _mapLength = Mathf.Abs(endPoint.transform.position.y - startPoint.transform.position.y);
       
        int gridLengthDensity = (int) (gridWidthDensity * (_mapLength / _mapWidth));
        sphereSize = 20 / (float)gridWidthDensity;
        
        pointLengthCount = gridLengthDensity + 1;
        pointWidthCount = gridWidthDensity + 1;
        
        cellWidth = _mapWidth / gridWidthDensity;
        cellLength = _mapLength / gridLengthDensity;
        
        _points = new Point[pointWidthCount, pointLengthCount];
        CreateSphere();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < pointWidthCount; i++)
        {
            for (int j = 0; j < pointLengthCount; j++)
            {
                if (i != pointWidthCount - 1)
                {
                    Point p = _points[i, j];
                    Point p2 = _points[i + 1, j];
                    if (p2.gameObject.GetComponent<SpriteRenderer>().enabled &&
                        p.gameObject.GetComponent<SpriteRenderer>().enabled)
                        Debug.DrawLine(new Vector3(p.x, p.y, 0), new Vector3(p2.x, p2.y, 0), Color.white);
                }

                if (j != pointLengthCount - 1)
                {
                    Point p = _points[i, j];
                    Point p2 = _points[i, j + 1];
                    if (p2.gameObject.GetComponent<SpriteRenderer>().enabled &&
                        p.gameObject.GetComponent<SpriteRenderer>().enabled)
                        Debug.DrawLine(new Vector3(p.x, p.y, 0), new Vector3(p2.x, p2.y, 0), Color.white);
                }
            }
        }
    }

    public void GenerateSphere()
    {
        GameObject[] spheres = GameObject.FindGameObjectsWithTag("sphere");
        for (int i = 0; i < spheres.Length; i++)
        {
            Destroy(spheres[i]);
        }

        CreateSphere();
    }

    public void CreateSphere()
    {
        for (int i = 0; i < pointWidthCount; i++)
        {
            for (int j = 0; j < pointLengthCount; j++)
            {
                Point p = new Point();
                p.x = i * cellWidth + startPoint.transform.position.x;
                p.y = j * cellLength + startPoint.transform.position.y;
                _points[i, j] = p;
                GameObject circle = Instantiate(spherePrefab);
                circle.tag = "sphere";
                circle.transform.localScale = Vector3.one * sphereSize;
                circle.transform.position = new Vector3(p.x, p.y, 0);
                p.gameObject = circle;
            }
        }
    }

    private void CreateArray()
    {
    }

    /// <summary>
    /// 辗转相除
    /// </summary>
    /// <param name="f1"></param>
    /// <param name="f2"></param>
    private int Division(int f1, int f2)
    {
        f1 = Mathf.Max(f1, f2);
        f2 = Mathf.Min(f1, f2);
        if (f1 % f2 == 0)
            return f2;
        int t = f2;
        f2 = f1 % f2;
        f1 = t;
        return Division(f1, f2);
    }
}