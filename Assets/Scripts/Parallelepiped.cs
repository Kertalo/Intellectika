using System.Collections.Generic;
using UnityEngine;

public class Parallelepiped : Solid
{
    public Parallelepiped(GameObject gameObject, int[] values)
        : base(gameObject)
    {
        type = SolidType.Parallelepiped;
        nameValues = new string[] { "Ширина", "Высота", "Длина" };
        this.values = values;

        CreateSolid();
    }

    public new void CreateSolid()
    {
        Mesh mesh = new Mesh();
        int x = values[0] / 2;
        float y = values[1] / 2.0f;
        int z = values[2] / 2;

        Vector3[] vertices =
        {
            new(-x, -y, -z), new(-x, y, -z),
            new(x, y, -z), new(x, -y, -z),
            new(-x, -y, z), new(-x, y, z),
            new(x, y, z), new(x, -y, z),
        };
        mesh.vertices = vertices;

        int[] triangles = new int[]
        {
            0, 4, 1, 1, 4, 5,
            1, 5, 2, 2, 5, 6,
            2, 6, 3, 3, 6, 7,
            3, 7, 0, 4, 0, 7,
            0, 1, 2, 2, 3, 0,
            4, 6, 5, 6, 4, 7
        };
        mesh.triangles = triangles;

        meshFilter.mesh = mesh;
    }
}
