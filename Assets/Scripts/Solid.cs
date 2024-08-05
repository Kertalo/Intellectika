using System;
using UnityEngine;

public enum SolidType { Parallelepiped, Sphere, Prism, Capsule }

public class Solid
{
    public GameObject gameObject;
    public SolidType type;
    public MeshFilter meshFilter;
    public string[] nameValues;
    public int[] values;

    public Solid(GameObject gameObject)
    {
        this.gameObject = gameObject;
        meshFilter = this.gameObject.GetComponent<MeshFilter>();
    }

    public void CreateSolid()
    {

    }

    public void ChangeColor(Color color)
    {
        gameObject.GetComponent<MeshRenderer>().materials[0].color = color;
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}