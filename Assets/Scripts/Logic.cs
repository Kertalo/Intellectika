using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Logic : MonoBehaviour
{
    public GameObject solidPrefab;
    public GameObject changeValue;
    public GameObject changeSolid;
    public Dropdown dropdown;
    public Dropdown dropdownColor;

    private int index;
    private int nextIndexDropdown = 1;

    public Color[] colors = new[] { Color.yellow, Color.red, Color.blue, Color.green, Color.black };

    public List<Solid> solids = new List<Solid>();

    public void AddSolid(int solidIndex)
    {
        switch (solidIndex)
        {
            case 0:
                solids.Add(new Parallelepiped(Instantiate(solidPrefab), new int[] { 4, 5, 3 }));
                break;
            case 1:
                solids.Add(new Sphere(Instantiate(solidPrefab), new int[] { 3, 5 }));
                break;
            case 2:
                solids.Add(new Prism(Instantiate(solidPrefab), new int[] { 2, 5, 7 }));
                break;
            case 3:
                solids.Add(new Capsule(Instantiate(solidPrefab), new int[] { 3, 5, 7 }));
                break;
        }
        index = solids.Count - 1;
        dropdown.options.Add(new("Тело " + nextIndexDropdown));
        nextIndexDropdown++;
        dropdown.value = index + 1;
        ShowValues();
    }

    public void ShowValues()
    {
        changeSolid.SetActive(true);
        while (changeSolid.transform.childCount > 2)
            DestroyImmediate(changeSolid.transform.GetChild(2).gameObject);

        for (int i = 0; i < solids[index].nameValues.Length; i++)
        {
            var valueObject = Instantiate(changeValue, changeSolid.transform);
            valueObject.transform.localPosition = new(0, -60 * i - 100, 0);
            valueObject.GetComponent<Value>().
                SetValues(this, solids[index].nameValues[i], i, solids[index].values[i]);
        }
    }

    public void ValueChanged(int valueIndex, int newValue)
    {
        if (newValue < 1 || newValue > 99)
            return;

        solids[index].values[valueIndex] = newValue;
        switch (solids[index].type) {
            case SolidType.Parallelepiped:
                ((Parallelepiped) solids[index]).CreateSolid();
                break;
            case SolidType.Sphere:
                ((Sphere)solids[index]).CreateSolid();
                break;
            case SolidType.Prism:
                ((Prism)solids[index]).CreateSolid();
                break;
            case SolidType.Capsule:
                ((Capsule)solids[index]).CreateSolid();
                break;
        }
        //solids[index].CreateSolid();
    }

    public void SolidChanged()
    {
        if (dropdown.value > 0)
        {
            index = dropdown.value - 1;
            ShowValues();
        }
        else
        {
            changeSolid.SetActive(false);
        }
    }

    public void ColorChange()
    {
        solids[index].ChangeColor(colors[dropdownColor.value]);
    }

    public void Destroy()
    {
        if (index >= solids.Count)
            return;

        solids[index].Destroy();
        solids.RemoveAt(index);
        dropdown.options.RemoveAt(index + 1);
        dropdown.value = 0;
        changeSolid.SetActive(false);
    }
}
