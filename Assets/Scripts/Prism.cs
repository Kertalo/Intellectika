using System.Collections.Generic;
using UnityEngine;

public class Prism : Solid
{
    public Prism(GameObject gameObject, int[] values)
        : base(gameObject)
    {
        type = SolidType.Prism;
        nameValues = new string[] { "Радиус", "Кол-во граней", "Высота" };
        this.values = values;

        CreateSolid();
    }

    public new void CreateSolid()
    {
        Mesh mesh = new Mesh();
        int r = values[0];
        float y = values[2] / 2.0f;
        int count = values[1];
        float addAngle = 2 * Mathf.PI / count;

        Vector3[] vertices = new Vector3[count * 2];
        float angle = 0;
        for (int i = 0; i < count; i++)
        {
            float x = Mathf.Cos(angle) * r;
            float z = Mathf.Sin(angle) * r;
            vertices[i] = new(x, -y, z);
            vertices[count + i] = new(x, y, z);
            angle += addAngle;
        }
        mesh.vertices = vertices;

        List<int> triangles = new List<int>();
        for (int i = 0; i < count - 1; i++)
        {
            triangles.AddRange(new int[] { i + 1, i, count + i });
            triangles.AddRange(new int[] { i + 1, count + i, count + i + 1 });
        }
        {
            triangles.AddRange(new int[] { 0, count - 1, 2 * count - 1 });
            triangles.AddRange(new int[] { 0, 2 * count - 1, count });
        }
        {
            for (int i = 2; i < count; i++)
            {
                triangles.AddRange(new int[] { 0, i - 1, i });
                triangles.AddRange(new int[] { count, count + i, count + i - 1 });
            }
        }
        mesh.triangles = triangles.ToArray();
        meshFilter.mesh = mesh;
    }
}
