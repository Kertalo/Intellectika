using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic : MonoBehaviour
{
    public enum SolidType { Parallelepiped, Sphere, Prism, Capsule }

    public GameObject solidPrefab;

    private int index;

    public List<Solid> solids = new List<Solid>();

    public void AddSolid(int solidIndex)
    {
        switch (solidIndex)
        {
            case 0:
                solids.Add(new Parallelepiped(Instantiate(solidPrefab), new float[] { 4, 5, 1 }));
                break;
            case 1:
                solids.Add(new Sphere(Instantiate(solidPrefab), new float[] { 3, 10 }));
                break;
            case 2:
                solids.Add(new Prism(Instantiate(solidPrefab), new float[] { 2, 5, 7 }));
                break;
        }

        index = solids.Count - 1;
        solids[index].gameObject.GetComponent<MeshFilter>().mesh = solids[index].mesh;
    }

    public void Destroy()
    {
        solids[index].Destroy();
        solids.RemoveAt(index);
    }
}
