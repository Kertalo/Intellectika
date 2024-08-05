using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule: Solid
{
    private Sphere[] solids;
    
    public Capsule(GameObject gameObject, int[] values)
        : base(gameObject)
    {
        type = SolidType.Capsule;
        nameValues = new string[] { "Радиус", "Кол-во граней", "Высота" };
        this.values = values;
        solids = new Sphere[2];
        for (int i = 0; i < 2; i++)
        {
            GameObject solid = new GameObject("Solid");
            solid.transform.parent = this.gameObject.transform;
            solid.AddComponent<MeshFilter>();
            solid.AddComponent<MeshRenderer>();
            solid.GetComponent<MeshRenderer>().material = this.gameObject.GetComponent<MeshRenderer>().material;
            solids[i] = new Sphere(solid, new int[] { values[0], values[1] });
        }
        CreateSolid();
    }

    public new void CreateSolid()
    {
        new Prism(this.gameObject, new int[] { values[0], values[1], values[2] });
        for (int i = 0; i < 2; i++)
        {
            solids[i].values[0] = values[0];
            solids[i].values[1] = values[1];
            solids[i].gameObject.transform.position = new(0, (i * 2 - 1) * values[2] / 2.0f, 0);
            solids[i].CreateSolid();
        }
    }

    public new void ChangeColor(Color color)
    {
        gameObject.GetComponent<MeshRenderer>().materials[0].color = color;
        for (int i = 0; i< solids.Length; i++)
            solids[i].gameObject.GetComponent<MeshRenderer>().materials[0].color = color;
    }
}
