﻿using System.Collections.Generic;
using UnityEngine;

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
        cellLength = cellWidth;

        print("单元格尺寸:" + cellWidth);

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
                //划横线
                if (i != pointWidthCount - 1)
                {
                    Point p = _points[i, j];
                    Point p2 = _points[i + 1, j];
                    if (p2.gameObject.GetComponent<MeshRenderer>().enabled &&
                        p.gameObject.GetComponent<MeshRenderer>().enabled)
                        Debug.DrawLine(new Vector3(p.x, p.y, 0), new Vector3(p2.x, p2.y, 0), Color.white);
                }

                //画竖线
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

        for (int i = 1; i < pointWidthCount; i += 2)
        {
            for (int j = 1; j < pointLengthCount; j += 2)
            {
                Point p = _points[i, j];
                Point p2 = new Point();
                Vector3 position = p.gameObject.transform.position;
                Vector3 position2;
                //暂且设置最大射线距离为二倍的网格间距
                float maxDistance = 2 * cellWidth;
                RaycastHit hit;

                //右上
                if (i + 1 < pointWidthCount && j + 1 < pointLengthCount)
                {
                    p2 = _points[i + 1, j + 1];
                    position2 = p2.gameObject.transform.position;
                    if (Physics.Raycast(p.gameObject.transform.position, (position2 - position).normalized, out hit,
                        maxDistance))
                    {
                        //抗锯齿
                        if (!hit.transform.gameObject.CompareTag("barrier"))
                        {
                            if (p2.gameObject.GetComponent<MeshRenderer>().enabled &&
                                p.gameObject.GetComponent<MeshRenderer>().enabled)
                                Debug.DrawLine(new Vector3(p.x, p.y, 0), new Vector3(p2.x, p2.y, 0), Color.white);
                        }
                    }
                }

                //右下
                if (i + 1 < pointWidthCount && j - 1 >= 0)
                {
                    p2 = _points[i + 1, j - 1];
                    position2 = p2.gameObject.transform.position;
                    if (Physics.Raycast(p.gameObject.transform.position, (position2 - position).normalized, out hit,
                        maxDistance))
                    {
                        if (!hit.transform.gameObject.CompareTag("barrier"))
                        {
                            if (p2.gameObject.GetComponent<MeshRenderer>().enabled &&
                                p.gameObject.GetComponent<MeshRenderer>().enabled)
                                Debug.DrawLine(new Vector3(p.x, p.y, 0), new Vector3(p2.x, p2.y, 0), Color.white);
                        }
                    }
                }

                //左上
                if (i - 1 >= 0 && j + 1 < pointLengthCount)
                {
                    p2 = _points[i - 1, j + 1];
                    position2 = p2.gameObject.transform.position;
                    if (Physics.Raycast(p.gameObject.transform.position, (position2 - position).normalized, out hit,
                        maxDistance))
                    {
                        if (!hit.transform.gameObject.CompareTag("barrier"))
                        {
                            if (p2.gameObject.GetComponent<MeshRenderer>().enabled &&
                                p.gameObject.GetComponent<MeshRenderer>().enabled)
                                Debug.DrawLine(new Vector3(p.x, p.y, 0), new Vector3(p2.x, p2.y, 0), Color.white);
                        }
                    }
                }

                //左下
                if (i - 1 < pointWidthCount && j - 1 <= pointLengthCount)
                {
                    p2 = _points[i - 1, j - 1];
                    position2 = p2.gameObject.transform.position;
                    if (Physics.Raycast(p.gameObject.transform.position, (position2 - position).normalized, out hit,
                        maxDistance))
                    {
                        if (!hit.transform.gameObject.CompareTag("barrier"))
                        {
                            if (p2.gameObject.GetComponent<MeshRenderer>().enabled &&
                                p.gameObject.GetComponent<MeshRenderer>().enabled)
                                Debug.DrawLine(new Vector3(p.x, p.y, 0), new Vector3(p2.x, p2.y, 0), Color.white);
                        }
                    }
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

    public void CreateArray()
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

                //distance unlimited
                _points[i, j].distanceFromStart = 30000;
            }
        }


        _points[0, 1].distanceFromStart = cellWidth;
        _points[1, 0].distanceFromStart = cellWidth;
        //point 1 checked
        Slack(_points[0, 0]);


        print("最终最短路径值: " + _points[3, 3].distanceFromStart);
    }

    /// <summary>
    /// 松弛
    /// </summary>
    /// <param name="point"></param>
    private void Slack(Point point)
    {
        //当所有点都被标记为确定值之后结束递归
        bool over = true;
        int count = 0;
        for (int i = 0; i < pointWidthCount; i++)
        {
            for (int j = 0; j < pointLengthCount; j++)
            {
                count += 1;
                if (!_points[i, j].isMark)
                {
                    over = false;
                    break;
                }
            }
        }

        print("count" + count);

        if (over)
        {
            return;
        }

        //因为挑选的点是当前已知的距初始点最近的所以列为确定值
        point.isMark = true;
        int indexX = point.indexX;
        int indexY = point.indexY;
        List<Point> outPoint = new List<Point>();
        //松弛首先要找出该点的最多八个相邻点
        if (indexX + 1 < pointWidthCount)
        {
            outPoint.Add(_points[indexX + 1, indexY]);
        }

        if (indexX - 1 >= 0)
        {
            outPoint.Add(_points[indexX - 1, indexY]);
        }

        if (indexY + 1 < pointLengthCount)
        {
            outPoint.Add(_points[indexX, indexY + 1]);
        }

        if (indexY - 1 >= 0)
        {
            outPoint.Add(_points[indexX, indexY - 1]);
        }

        if (indexX + 1 < pointWidthCount && indexY + 1 < pointLengthCount)
        {
            outPoint.Add(_points[indexX + 1, indexY + 1]);
        }

        if (indexX - 1 >= 0 && indexY + 1 < pointLengthCount)
        {
            outPoint.Add(_points[indexX - 1, indexY + 1]);
        }

        if (indexX + 1 < pointWidthCount && indexY - 1 >= 0)
        {
            outPoint.Add(_points[indexX + 1, indexY - 1]);
        }

        if (indexX - 1 >= 0 && indexY - 1 >= 0)
        {
            outPoint.Add(_points[indexX - 1, indexY - 1]);
        }

        print("当前检查点:" + indexX + "," + indexY);

        float sortestPointDistance = int.MaxValue;

        //已经标记过，最短距离确定过的点不考虑
        for (int i = 0; i < outPoint.Count; i++)
        {
            if (outPoint[i].isMark)
            {
                continue;
            }

            //当前点到第一个点的距离
            float distanceCurrent = point.distanceFromStart;
            //到相邻点的距离
            float distanceToNext;
            //如果是斜的相邻点
            if (IsTilt(indexX, indexY, outPoint[i].indexX, outPoint[i].indexY))
            {
                distanceToNext = distanceCurrent + Mathf.Sqrt(2) * cellLength;
            }
            //如果是普通的相邻点
            else
            {
                distanceToNext = distanceCurrent + 1 * cellLength;
            }

            //相邻点到初始点的距离
            float distanceNextFromStart = outPoint[i].distanceFromStart;
            //刷新相邻点到初始点的距离
            if (distanceNextFromStart != int.MaxValue)
            {
                outPoint[i].distanceFromStart = Mathf.Min(distanceToNext, distanceNextFromStart);
            }
            else
            {
                outPoint[i].distanceFromStart = distanceToNext;
            }

            print(outPoint[i].distanceFromStart);

            sortestPointDistance = Mathf.Min(outPoint[i].distanceFromStart, sortestPointDistance);
        }

        //松弛之后找出最近点
        if (sortestPointDistance != int.MaxValue)
        {
            for (int i = 0; i < outPoint.Count; i++)
            {
                if (outPoint[i].isMark)
                    continue;
                if (outPoint[i].distanceFromStart == sortestPointDistance)
                {
                    Slack(outPoint[i]);
                }
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

    private bool IsTilt(int x1, int y1, int x2, int y2)
    {
        if (x1 + 1 == x2 && y1 + 1 == y2)
        {
            return true;
        }

        if (x1 + 1 == x2 && y1 - 1 == y2)
        {
            return true;
        }

        if (x1 - 1 == x2 && y1 + 1 == y2)
        {
            return true;
        }

        if (x1 - 1 == x2 && y1 - 1 == y2)
        {
            return true;
        }

        return false;
    }
}