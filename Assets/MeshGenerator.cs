using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;
    float minY = 0;
    float maxY = 0;

    public int xSize;
    public int zSize;
    public float variance;
    public Color bottom;
    public Color middle;
    public Color top;

    public MeshGenerator(int x, int z, float v, Color b, Color m, Color t)
    {
        xSize = x;
        zSize = z;
        variance = v;
        bottom = b;
        middle = m;
        top = t;
    }

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        CalcColors();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * variance;
                minY = Mathf.Min(minY, y);
                maxY = Mathf.Max(maxY, y);
                vertices[i] = (x == 0 || x == xSize || z == 0 || z == zSize) ? new Vector3(x, 0, z) : new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void CalcColors()
    {
        for (var i = 0; i < colors.Length; i++)
        {
            float normalizedY = ((vertices[i].y - minY) / (maxY - minY));
            if (normalizedY <= 0.33) colors[i] = bottom;
            else if (normalizedY > 0.33 && normalizedY <= 0.50) colors[i] = middle;
            else colors[i] = top;

            colors[i] = Color.Lerp(bottom, top, normalizedY);
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        CalcColors();
        mesh.colors = colors;
        mesh.RecalculateNormals();
        // optionally, add a mesh collider (As suggested by Franku Kek via Youtube comments).
        // To use this, your MeshGenerator GameObject needs to have a mesh collider
        // component added to it.  Then, just re-enable the code below.
        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }
}