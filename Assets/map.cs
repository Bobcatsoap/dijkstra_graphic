using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class map : MonoBehaviour
{
    private Point[,] _points;
    private float _mapWidth, _mapLength;
    [Range(0, 1)] public float gridWidthDensity;
    private int pointWidthCount, pointLengthCount;
    private List<Point> _knowedPoints;
    [Range(0, .5f)] public float sphereSize;
    public GameObject cube;
    public GameObject spherePrefab;
    public GameObject endPoint;
    public GameObject startPoint;
    private float cellWidth;
    private float cellLength;
    public Material lineMaterial;

    class Point
    {
        //坐标
        public float x;
        public float y;
        public int indexX;
        public int indexY;
        public GameObject gameObject;
        public float distanceFromStart;

        public Point prePoint;

        public Point nextPoint;

        //已检查
        public bool isMark;
    }

    // Use this for initialization
    void Start()
    {
        _mapWidth = Mathf.Abs(endPoint.transform.position.x - startPoint.transform.position.x);
        _mapLength = Mathf.Abs(endPoint.transform.position.y - startPoint.transform.position.y);

        _knowedPoints = new List<Point>();
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
                    if (p2.gameObject.GetComponent<MTrigger>().visiable &&
                        p.gameObject.GetComponent<MTrigger>().visiable)
                        if (!IsCrossTheBarrier(p, p2, cellWidth))
                        {
                            Debug.DrawLine(new Vector3(p.x, p.y, 0), new Vector3(p2.x, p2.y, 0), Color.white);
                        }
                }

                //画竖线
                if (j != pointLengthCount - 1)
                {
                    Point p = _points[i, j];
                    Point p2 = _points[i, j + 1];
                    if (p2.gameObject.GetComponent<MTrigger>().visiable &&
                        p.gameObject.GetComponent<MTrigger>().visiable)
                        if (!IsCrossTheBarrier(p, p2, cellWidth))
                        {
                            Debug.DrawLine(new Vector3(p.x, p.y, 0), new Vector3(p2.x, p2.y, 0), Color.white);
                        }
                }
            }
        }

        for (int i = 0; i < pointWidthCount; i += 1)
        {
            for (int j = 0; j < pointLengthCount; j += 1)
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
                            if (p2.gameObject.GetComponent<MTrigger>().visiable &&
                                p.gameObject.GetComponent<MTrigger>().visiable)
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
                            if (p2.gameObject.GetComponent<MTrigger>().visiable &&
                                p.gameObject.GetComponent<MTrigger>().visiable)
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
                            if (p2.gameObject.GetComponent<MTrigger>().visiable &&
                                p.gameObject.GetComponent<MTrigger>().visiable)
                                Debug.DrawLine(new Vector3(p.x, p.y, 0), new Vector3(p2.x, p2.y, 0), Color.white);
                        }
                    }
                }

                //左下
                if (i - 1 >= 0 && j - 1 >= 0)
                {
                    p2 = _points[i - 1, j - 1];
                    position2 = p2.gameObject.transform.position;
                    if (Physics.Raycast(p.gameObject.transform.position, (position2 - position).normalized, out hit,
                        maxDistance))
                    {
                        if (!hit.transform.gameObject.CompareTag("barrier"))
                        {
                            if (p2.gameObject.GetComponent<MTrigger>().visiable &&
                                p.gameObject.GetComponent<MTrigger>().visiable)
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
                _points[i, j].distanceFromStart = int.MaxValue;

                //不统计初始点
                if (i == 0 && j == 0)
                {
                    _points[i, j].distanceFromStart = 0;
                }

                _points[i, j].isMark = false;
            }
        }

        _points[0, 1].distanceFromStart = cellWidth;
        _points[1, 0].distanceFromStart = cellWidth;
        _points[1, 1].distanceFromStart = cellWidth * Mathf.Sqrt(2);
        Slack(_points[0, 0]);
        print("最终最短路径值: " + _points[pointWidthCount - 1, pointLengthCount - 1].distanceFromStart);
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Line");
        for (int i = 0; i < lines.Length; i++)
        {
            Destroy(lines[i]);
        }

        StartCoroutine(RendererLine(_points[pointWidthCount - 1, pointLengthCount - 1]));
    }

    /// <summary>
    /// 松弛
    /// </summary>
    /// <param name="point"></param>
    private void Slack(Point point)
    {
        //最后一个点的值确定后结束递归
        if (_points[pointWidthCount - 1, pointLengthCount - 1].isMark)
        {
            return;
        }

        //距离为确定值
        point.isMark = true;
        _knowedPoints.Remove(point);

        int indexX = point.indexX;
        int indexY = point.indexY;

        List<Point> outPoint = new List<Point>();

        //松弛首先要找出该点的最多八个相邻点,并从小到大排序

        //右
        if (indexX + 1 < pointWidthCount)
        {
            Point p2 = _points[indexX + 1, indexY];
            if (p2.gameObject.GetComponent<MTrigger>().visiable)
            {
                if (!IsCrossTheBarrier(point, p2, cellWidth))
                {
                    outPoint.Add(p2);
                }
            }
        }

        //左
        if (indexX - 1 >= 0)
        {
            Point p2 = _points[indexX - 1, indexY];
            if (p2.gameObject.GetComponent<MTrigger>().visiable)
            {
                if (!IsCrossTheBarrier(point, p2, cellWidth))
                {
                    if (outPoint.Count == 0)
                    {
                        outPoint.Add(p2);
                    }
                    else
                    {
                        Point temp = outPoint[outPoint.Count - 1];
                        if (p2.distanceFromStart <= temp.distanceFromStart)
                        {
                            outPoint.Add(p2);
                        }
                        else
                        {
                            outPoint[outPoint.Count - 1] = p2;
                            outPoint.Add(temp);
                        }
                    }
                }
            }
        }

        //上
        if (indexY + 1 < pointLengthCount)
        {
            Point p2 = _points[indexX, indexY + 1];
            if (p2.gameObject.GetComponent<MTrigger>().visiable)
            {
                if (!IsCrossTheBarrier(point, p2, cellWidth))
                {
                    if (outPoint.Count == 0)
                    {
                        outPoint.Add(p2);
                    }
                    else
                    {
                        Point temp = outPoint[outPoint.Count - 1];
                        if (_points[indexX, indexY + 1].distanceFromStart <= temp.distanceFromStart)
                        {
                            outPoint.Add(p2);
                        }
                        else
                        {
                            outPoint[outPoint.Count - 1] = p2;
                            outPoint.Add(temp);
                        }
                    }
                }
            }
        }

        //下
        if (indexY - 1 >= 0)
        {
            Point p2 = _points[indexX, indexY - 1];
            if (p2.gameObject.GetComponent<MTrigger>().visiable)
            {
                if (!IsCrossTheBarrier(point, p2, cellWidth))
                {
                    if (outPoint.Count == 0)
                    {
                        outPoint.Add(p2);
                    }
                    else
                    {
                        Point temp = outPoint[outPoint.Count - 1];
                        if (_points[indexX, indexY - 1].distanceFromStart <= temp.distanceFromStart)
                        {
                            outPoint.Add(p2);
                        }
                        else
                        {
                            outPoint[outPoint.Count - 1] = p2;
                            outPoint.Add(temp);
                        }
                    }
                }
            }
        }

        //右上
        if (indexX + 1 < pointWidthCount && indexY + 1 < pointLengthCount)
        {
            Point p2 = _points[indexX + 1, indexY + 1];
            if (p2.gameObject.GetComponent<MTrigger>().visiable)
            {
                //没穿过障碍物的两个点才能连斜线
//                if (!IsTiltCrossTheBarrier(point, p2))
//                {
                if (outPoint.Count == 0)
                {
                    outPoint.Add(p2);
                }
                else
                {
                    Point temp = outPoint[outPoint.Count - 1];
                    if (p2.distanceFromStart <= temp.distanceFromStart)
                    {
                        outPoint.Add(p2);
                    }
                    else
                    {
                        outPoint[outPoint.Count - 1] = p2;
                        outPoint.Add(temp);
                    }
                }

//                }
            }
        }

        //左上
        if (indexX - 1 >= 0 && indexY + 1 < pointLengthCount)
        {
            Point p2 = _points[indexX - 1, indexY + 1];
            if (p2.gameObject.GetComponent<MTrigger>().visiable)
            {
                //没穿过障碍物的两个点才能连斜线
//                if (!IsTiltCrossTheBarrier(point, p2))
//                {
                if (outPoint.Count == 0)
                {
                    outPoint.Add(p2);
                }
                else
                {
                    Point temp = outPoint[outPoint.Count - 1];
                    if (p2.distanceFromStart <= temp.distanceFromStart)
                    {
                        outPoint.Add(p2);
                    }
                    else
                    {
                        outPoint[outPoint.Count - 1] = p2;
                        outPoint.Add(temp);
                    }
                }

//                }
            }
        }

        //右下
        if (indexX + 1 < pointWidthCount && indexY - 1 >= 0)
        {
            Point p2 = _points[indexX + 1, indexY - 1];
            if (p2.gameObject.GetComponent<MTrigger>().visiable)
            {
//                if (!IsTiltCrossTheBarrier(point, p2))
//                {
                if (outPoint.Count == 0)
                {
                    outPoint.Add(p2);
                }
                else
                {
                    Point temp = outPoint[outPoint.Count - 1];
                    if (p2.distanceFromStart <= temp.distanceFromStart)
                    {
                        outPoint.Add(p2);
                    }
                    else
                    {
                        outPoint[outPoint.Count - 1] = p2;
                        outPoint.Add(temp);
                    }
                }

//                }
            }
        }

        //左下
        if (indexX - 1 >= 0 && indexY - 1 >= 0)
        {
            Point p2 = _points[indexX - 1, indexY - 1];
            if (p2.gameObject.GetComponent<MTrigger>().visiable)
            {
//                if (!IsTiltCrossTheBarrier(point, p2))
//                {
                if (outPoint.Count == 0)
                {
                    outPoint.Add(p2);
                }
                else
                {
                    Point temp = outPoint[outPoint.Count - 1];
                    if (p2.distanceFromStart <= temp.distanceFromStart)
                    {
                        outPoint.Add(p2);
                    }
                    else
                    {
                        outPoint[outPoint.Count - 1] = p2;
                        outPoint.Add(temp);
                    }
                }

//                }
            }
        }

        //已经标记过，最短距离确定过的点不考虑
        for (int i = 0; i < outPoint.Count; i++)
        {
            if (outPoint[i].isMark)
            {
                continue;
            }

            //当前点到起始点的距离
            float distanceCurrent = point.distanceFromStart;

            //当前点到起始点的距离+到当前点到出点的距离
            float distanceToNext;

            if (IsTilt(indexX, indexY, outPoint[i].indexX, outPoint[i].indexY))
            {
                //斜向出点距离=根号 2 * 网格尺寸
                distanceToNext = distanceCurrent + Mathf.Sqrt(2) * cellLength;
            }
            else
            {
                //普通出点距离=网格尺寸
                distanceToNext = distanceCurrent + 1 * cellLength;
            }

            //出点到初始点的距离
            float distanceNextFromStart = outPoint[i].distanceFromStart;

            //刷新出点到初始点的距离

            outPoint[i].distanceFromStart = Mathf.Min(distanceToNext, distanceNextFromStart);


            if (distanceToNext <= distanceNextFromStart)
            {
                point.nextPoint = outPoint[i];
                outPoint[i].prePoint = point;
            }
        }

        for (int i = outPoint.Count - 1; i >= 0; i--)
        {
            if (outPoint[i].isMark)
            {
                continue;
            }

            Slack(outPoint[i]);
        }
    }

    private IEnumerator RendererLine(Point p)
    {
        while (p.prePoint != null)
        {
            GameObject line = new GameObject();
            LineRenderer l = line.AddComponent<LineRenderer>();
            line.tag = "Line";
            l.material = lineMaterial;
            l.startWidth = .3f;
            l.endWidth = .3f;
            l.SetPosition(0, p.gameObject.transform.position);
            l.SetPosition(1, p.prePoint.gameObject.transform.position);
            Instantiate(line);
            p = p.prePoint;
            yield return new WaitForSeconds(.1f);
        }
    }


    //是否斜向相邻
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

    //连线是否穿过了障碍物
    private bool IsCrossTheBarrier(Point p, Point p2, float maxDistance)
    {
        Vector3 position = p.gameObject.transform.position;
        Vector3 position2 = p2.gameObject.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(p.gameObject.transform.position, (position2 - position).normalized, out hit,
            maxDistance))
        {
            return hit.collider.CompareTag("barrier");
        }

        return false;
    }
}