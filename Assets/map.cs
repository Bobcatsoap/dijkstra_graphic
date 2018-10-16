using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class map : MonoBehaviour
{
    private Point[,] _points;

    public int cellSize;
    public int gridSize;

    public int sphereSize;

//    public Vector3 cubeCenter;
//    public float cubeSize;
    public GameObject cube;


    class Point
    {
        public int x;
        public int y;
        public GameObject gameObject;
    }

    // Use this for initialization
    void Start()
    {
        _points = new Point[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Point p = new Point();
                p.x = i * cellSize;
                p.y = j * cellSize;
                _points[i, j] = p;
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.tag = "sphere";
                sphere.transform.localScale = Vector3.one * sphereSize;
                sphere.transform.position = new Vector3(p.x, p.y, 0);
                p.gameObject = sphere;
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (i != gridSize - 1)
                {
                    Point p = _points[i, j];
                    Point p2 = _points[i + 1, j];
                    if (p2.gameObject.activeSelf && p.gameObject.activeSelf)
                        Debug.DrawLine(new Vector3(p.x, p.y, 0), new Vector3(p2.x, p2.y, 0), Color.white);
                }

                if (j != gridSize - 1)
                {
                    Point p = _points[i, j];
                    Point p2 = _points[i, j + 1];
                    if (p2.gameObject.activeSelf && p.gameObject.activeSelf)
                        Debug.DrawLine(new Vector3(p.x, p.y, 0), new Vector3(p2.x, p2.y, 0), Color.white);
                }
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                GameObject sphere = _points[i, j].gameObject;
                float cubeEdgeLeft = cube.transform.position.x - cube.transform.localScale.x / 2;
                float cubeEdgeRight = cube.transform.position.x + cube.transform.localScale.x / 2;
                float cubeEdgeTop = cube.transform.position.y + cube.transform.localScale.y / 2;
                float cubeEdgeBottom = cube.transform.position.y - cube.transform.localScale.y / 2;
                Point p = _points[i, j];
                Vector3 pPosition = p.gameObject.transform.position;
                if (pPosition.x + sphereSize / 2f >= cubeEdgeLeft && pPosition.x - sphereSize / 2f <= cubeEdgeRight &&
                    pPosition.y + sphereSize / 2f >= cubeEdgeBottom && pPosition.y - sphereSize / 2f <= cubeEdgeTop)
                {
                    p.gameObject.SetActive(false);
                }
                else
                {
                    p.gameObject.SetActive(true);
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

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Point p = new Point();
                p.x = i * cellSize;
                p.y = j * cellSize;
                _points[i, j] = p;
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.tag = "sphere";
                sphere.transform.localScale = Vector3.one * sphereSize;
                sphere.transform.position = new Vector3(p.x, p.y, 0);
            }
        }
    }
}