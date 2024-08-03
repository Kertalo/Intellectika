using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Solid
{
    public GameObject gameObject;
    public Mesh mesh;
    public string[] nameValues;

    public Solid(GameObject gameObject)
    {
        mesh = new Mesh();
        this.gameObject = gameObject;
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}

public class Parallelepiped: Solid
{
    public Parallelepiped(GameObject gameObject, float[] values)
        : base(gameObject)
    {
        nameValues = new string[] { "Ширина", "Высота", "Длина" };

        float x = values[0] / 2;
        float y = values[1];
        float z = values[2] / 2;
        
        Vector3[] vertices =
        {
            new(-x, 0, -z), new(-x, y, -z),
            new(x, y, -z), new(x, 0, -z),
            new(-x, 0, z), new(-x, y, z),
            new(x, y, z), new(x, 0, z),
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
    }
}

public class Prism : Solid
{
    public Prism(GameObject gameObject, float[] values)
        : base(gameObject)
    {
        nameValues = new string[] { "Радиус", "Высота", "Кол-во граней" };

        float r = values[0];
        float y = values[1];
        int count = (int)values[2];
        float addAngle = 2 * Mathf.PI / count;

        Vector3[] vertices = new Vector3[count * 2];
        float angle = 0;
        for (int i = 0; i < count; i++)
        {
            float x = Mathf.Cos(angle) * r;
            float z = Mathf.Sin(angle) * r;
            vertices[i] = new(x, 0, z);
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
    }
}

public class Sphere : Solid
{
    public Sphere(GameObject gameObject, float[] values)
        : base(gameObject)
    {
        nameValues = new string[] { "Радиус", "Кол-во граней" };

        int countTriangles = 5;
        float r = values[0];
        int count = (int)values[1];

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
        int loop = 1;

        while (loop < countTriangles * count * 2)
        {
            if (loop < count * countTriangles)
            {
                triangles.AddRange(new int[] { index1, index2, index2 + 1 });
                index2++;
            }
            else
            {
                triangles.AddRange(new int[] { index1, index2, index1 + 1 });
                index1++;
            }

            if (loop < count * countTriangles && index2 < bound2)
            {
                triangles.AddRange(new int[] { index1, index2, index1 + 1 });
                index1++;
            }
            else if (loop > count * countTriangles && index1 < bound1)
            {
                triangles.AddRange(new int[] { index1, index2, index2 + 1 });
                index2++;
            }
            else
            {
                loop++;
                if (loop % countTriangles == 0)
                {
                    int copyIndex2 = index2;
                    loop++;
                    if (loop - 1 > count * countTriangles)
                        addBound--;
                    else
                        addBound++;
                    index1 = copyIndex2 - (countTriangles - 1) * addBound;
                    index2 = index2 + addBound;
                    bound1 = index1;
                    bound2 = index2;
                }
                bound1 += addBound;
                bound2 += addBound + 1;
            }
        }
        mesh.triangles = triangles.ToArray();
    }
}