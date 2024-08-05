using System.Collections.Generic;
using UnityEngine;

public class Sphere : Solid
{
    public Sphere(GameObject gameObject, int[] values)
        : base(gameObject)
    {
        type = SolidType.Sphere;
        nameValues = new string[] { "Радиус", "Кол-во граней" };
        this.values = values;

        CreateSolid();
    }

    public new void CreateSolid()
    {
        Mesh mesh = new Mesh();
        int countTriangles = 5;
        int r = values[0];
        int count = values[1];

        float addYAngle = Mathf.PI / 2 / count;

        int allCount = count * countTriangles + 2;
        for (int i = count - 1; i > 0; i--)
            allCount += 2 * i * countTriangles;

        List<Vector3> vertices = new();

        float angleY = -Mathf.PI / 2;
        vertices.Add(new(0, -r, 0));
        for (int i = 0; i < 2 * count; i++)
        {
            float angle = 0;
            float countPoints = (-Mathf.Abs(i - count) + count) * countTriangles;
            float addAngle = 2 * Mathf.PI / countPoints;
            for (int j = 0; j < countPoints; j++)
            {
                float x = Mathf.Abs(Mathf.Cos(angleY)) * Mathf.Cos(angle) * r;
                float y = Mathf.Sin(angleY) * r;
                float z = Mathf.Abs(Mathf.Cos(angleY)) * Mathf.Sin(angle) * r;
                vertices.Add(new(x, y, z));
                angle += addAngle;
            }
            angleY += addYAngle;
        }
        vertices.Add(new(0, r, 0));
        mesh.vertices = vertices.ToArray();

        List<int> triangles = new List<int>();
        int index1 = 0;
        int index2 = 1;
        int bound1 = 1;
        int bound2 = 2;
        int addBound = 0;
        int loop = 0;
        bool isHardTriangle = false;
        while (loop < 2 * countTriangles * count)
        {
            bool isEnd = false;
            if (loop < count * countTriangles)
            {
                int z = index2 + 1;
                if (isHardTriangle && z == bound2)
                {
                    z -= countTriangles * (addBound + 1);
                    isEnd = true;
                }
                triangles.AddRange(new int[] { index1, index2, z });
                index2++;
            }
            else
            {
                int z = index1 + 1;
                if (isHardTriangle && z == bound1)
                {
                    z -= countTriangles * addBound;
                    isEnd = true;
                }
                triangles.AddRange(new int[] { index1, index2, z });
                index1++;
            }

            if (loop < count * countTriangles && (index2 < bound2 && !isEnd))
            {
                int z = index1 + 1;
                if (isHardTriangle && z == bound1)
                    z -= countTriangles * addBound;
                triangles.AddRange(new int[] { index1, index2, z });
                index1 = z;
            }
            else if (loop >= count * countTriangles && (index1 < bound1 && !isEnd))
            {
                int z = index2 + 1;
                if (isHardTriangle && z == bound2)
                    z -= countTriangles * (addBound - 1);
                triangles.AddRange(new int[] { index1, index2, z });
                index2 = z;
            }
            else
            {
                loop++;
                if ((loop + 1) % countTriangles == 0)
                    isHardTriangle = true;
                else
                    isHardTriangle = false;
                if (loop % countTriangles == 0)
                {
                    if (loop > count * countTriangles)
                        addBound--;
                    else
                        addBound++;
                    index1 = bound1;
                    index2 = bound2;
                }
                bound1 += addBound;
                bound2 += addBound + (loop >= count * countTriangles ? -1 : 1);
            }
        }
        mesh.triangles = triangles.ToArray();
        meshFilter.mesh = mesh;
    }
}