using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class map : MonoBehaviour
{
    private Point[,] _points;
    private float _mapWidth, _mapLength;
    [Range(0, 1)] public float gridWidthDensity;
    private int pointWidthCount, pointLengthCount;
    [Range(0, .5f)] public float sphereSize;
    public GameObject cube;
    public GameObject spherePrefab;
    public GameObject endPoint;
    public GameObject startPoint;
    private float cellWidth;
    private float cellLength;

    class Point
    {
        //坐标
        public float x;
        public float y;
        public int indexX;
        public int indexY;
        public GameObject gameObject;

        public float distanceFromStart;

        //已检查
        public bool isMark;
    }

    // Use this for initialization
    void Start()
    {
        _mapWidth = Mathf.Abs(endPoint.transform.position.x - startPoint.transform.position.x);
        _mapLength = Mathf.Abs(endPoint.transform.position.y - startPoint.transform.position.y);

        int gridWCount = (int) (_mapWidth / gridWidthDensity);

        int gridLCount = (int) (gridWCount * _mapLength / _mapWidth);

        pointLengthCount = gridLCount + 1;
        pointWidthCount = gridWCount + 1;

        cellWidth = _mapWidth / gridWCount;
        cellLength = _mapLength / gridLCount;

        sphereSize = gridWidthDensity * sphereSize;


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
                    if (p2.gameObject.GetComponent<MeshRenderer>().enabled &&
                        p.gameObject.GetComponent<MeshRenderer>().enabled)
                        Debug.DrawLine(new Vector3(p.x, p.y, 0), new Vector3(p2.x, p2.y, 0), Color.white);
                }

                if (j != pointLengthCount - 1)
                {
                    Point p = _points[i, j];
                    Point p2 = _points[i, j + 1];
                    if (p2.gameObject.GetComponent<MeshRenderer>().enabled &&
                        p.gameObject.GetComponent<MeshRenderer>().enabled)
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
                p.indexX = i;
                p.indexY = j;
                _points[i, j] = p;
                GameObject sphere = Instantiate(spherePrefab);
                sphere.tag = "sphere";
                sphere.transform.localScale = Vector3.one * sphereSize;
                sphere.transform.position = new Vector3(p.x, p.y, 0);
                sphere.isStatic = true;
                p.gameObject = sphere;
            }
        }
    }

    private void CreateArray()
    {
        for (int i = 0; i < pointWidthCount; i++)
        {
            for (int j = 0; j < pointLengthCount; j++)
            {
                //不统计初始点
                if (i == 0 && j == 0)
                {
                    continue;
                }

                if (i == j)
                {
                    //自己到自己为距离为0
                    _points[i, j].distanceFromStart = 0;
                    continue;
                }

                //-1代表无限远
                _points[i, j].distanceFromStart = -1;
            }
        }

        //第一个点检查过
        _points[0, 0].isMark = true;
        Slack(_points[1, 0], 1);
    }

    private void Slack(Point point, float distanceFromStart)
    {
        int indexX = point.indexX;
        int indexY = point.indexY;
        List<Point> outPoint = new List<Point>();
        if (indexX + 1 < pointWidthCount)
        {
            outPoint.Add(_points[indexX + 1, indexY]);
        }

        if (indexX - 1 >= 0)
        {
            outPoint.Add(_points[indexX - 1, indexY]);
        }

        if (indexY + 1 < pointWidthCount)
        {
            outPoint.Add(_points[indexX, indexY + 1]);
        }

        if (indexY - 1 >= 0)
        {
            outPoint.Add(_points[indexX, indexY - 1]);
        }

        //不考虑已经标记过的点
        for (int i = 0; i < outPoint.Count; i++)
        {
            if (outPoint[i].isMark)
            {
                continue;
            }

            
            
        }
        
        
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