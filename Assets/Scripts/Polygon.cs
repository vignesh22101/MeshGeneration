using System.Collections.Generic;
using UnityEngine;

public class Polygon : MonoBehaviour
{
    #region Variables

    internal List<Vector3> vertices = new List<Vector3>();
    internal List<int> triangles = new List<int>();

    private Mesh mesh;

    #endregion

    private void Start()
    {
        mesh = new Mesh();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshFilter>().mesh.Clear();
        //GetComponent<MeshRenderer>().material = new Material();
    }

    internal void AddVertice(Vector3 globalPosition)
    {
        vertices.Add(globalPosition);
    }

    internal void ConstructPolygon()
    {
        if (vertices.Count < 3) 
            return;

        for (int i = 0; i < vertices.Count; i++)
        {
            int firstVertice = i;
            int secondVertice = GetVerticeCount_RightSide(i);
            int thirdVertice = GetVerticeCount_DownSide(firstVertice,secondVertice);

            triangles.Add(firstVertice);
            triangles.Add(secondVertice);
            triangles.Add(thirdVertice);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    private int GetVerticeCount_RightSide(int currentVerticeCount)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            if (i != currentVerticeCount)
            {
                if (vertices[currentVerticeCount].x < vertices[i].x)
                    return i;
            }
        }

        return 0;
    }

    private int GetVerticeCount_DownSide(int firstVertice,int secondVertice)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            if (i != firstVertice && i!=secondVertice)
            {
                if (vertices[secondVertice].z > vertices[i].z)
                    return i;
            }
        }

        return 0;
    }
}