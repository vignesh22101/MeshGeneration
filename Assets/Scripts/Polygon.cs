using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Polygon : MonoBehaviour
{
    #region Variables

    internal List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<int> verticesUsed = new List<int>();
    private List<int> nearestVertices = new List<int>();
    private List<float> distances = new List<float>();

    private Mesh mesh;
    private float zCoordinate;
    internal bool moveWithMouse;
    #endregion

    private void Start()
    {
        zCoordinate = Camera.main.WorldToScreenPoint(transform.position).z;

        mesh = new Mesh();
        if (GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();
        if (GetComponent<MeshFilter>() == null)
        {
            gameObject.AddComponent<MeshFilter>();
            GetComponent<MeshFilter>().mesh = mesh;
            mesh.Clear();
        }
    }

    private void Update()
    {
        if (moveWithMouse)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = zCoordinate;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = worldPos;
        }
    }

    internal void AddVertice(Vector3 globalPosition)
    {
        vertices.Add(globalPosition);
    }

    internal void ConstructPolygon(Material material)
    {
        if (vertices.Count < 3)
            return;

        for (int i = 0; i < vertices.Count; i++)
        {
            if (!verticesUsed.Contains(i))
            {
                FormTriangle(i);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        GetComponent<MeshRenderer>().material = material;
    }

    private int GetVerticeCount_Horizontal(int verticeIndex, bool isRightSide)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            if (i != verticeIndex)
            {
                if (isRightSide)
                {
                    if (vertices[verticeIndex].x < vertices[i].x)
                        return i;
                }
                else
                {
                    if (vertices[verticeIndex].x > vertices[i].x)
                        return i;
                }
            }
        }

        return -1;
    }

    private int GetVerticeCount_Vertical(int verticeIndex, bool isDownSide)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            if (i != verticeIndex)
            {
                if (isDownSide)
                {
                    if (vertices[verticeIndex].z > vertices[i].z)
                        return i;
                }
                else
                {
                    if (vertices[verticeIndex].z < vertices[i].z)
                        return i;
                }
            }
        }

        return -1;
    }

    private List<int> GetNearestVertices(int vertice)
    {
        distances.Clear();
        nearestVertices.Clear();

        foreach (var item in vertices)
        {
            distances.Add(Vector3.Distance(vertices[vertice], item));
        }

        int index = 0;
        for (int i = 0; i < distances.Count; i++)
        {
            index = distances.IndexOf(distances.Min());
            nearestVertices.Add(index);
            distances.RemoveAt(index);
        }

        return nearestVertices;
    }

    private void VerticeAdded(int vertice)
    {
        if (!verticesUsed.Contains(vertice))
            verticesUsed.Add(vertice);
    }

    private void FormTriangle(int vertex)
    {
        int firstVertex = vertex;
        int secondVertex = -1;
        int thirdVertex = -1;

        for (int i = 0; i < vertices.Count; i++)
        {
            if (firstVertex != i)
            {
                int rightVertex = GetVerticeCount_Horizontal(firstVertex, true);
                int leftVertex = GetVerticeCount_Horizontal(firstVertex, false);

                if (rightVertex != -1)
                {
                    int downVertex = GetVerticeCount_Vertical(rightVertex, true);
                    if (downVertex != -1)
                    {
                        secondVertex = rightVertex;
                        thirdVertex = downVertex;
                        break;
                    }
                }

                if (leftVertex != -1)
                {
                    int upperVertex = GetVerticeCount_Vertical(leftVertex, false);
                    if (upperVertex != -1)
                    {
                        secondVertex = leftVertex;
                        thirdVertex = upperVertex;
                        break;
                    }
                }
            }
        }

        if (secondVertex != -1 && firstVertex != -1)
        {
            print($"TriangleFormed,{firstVertex},{secondVertex},{thirdVertex}");
            triangles.AddRange(new List<int> { firstVertex, secondVertex, thirdVertex });
            VerticeAdded(firstVertex);
            VerticeAdded(secondVertex);
            VerticeAdded(thirdVertex);
        }
    }
}