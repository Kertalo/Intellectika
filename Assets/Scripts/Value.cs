using UnityEngine;
using UnityEngine.UI;

public class Value : MonoBehaviour
{
    public Text text;
    public InputField input;

    private int index;
    private Logic logic;

    public void SetValues(Logic logic, string title, int index, int value)
    {
        this.logic = logic;
        this.index = index;

        text.text = title;
        input.text = value.ToString();
    }

    public void ChangeValue(string valueString)
    {
        if (int.TryParse(valueString, out int value))
            logic.ValueChanged(index, value);
    }
}
