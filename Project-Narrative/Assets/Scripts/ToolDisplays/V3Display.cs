using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class V3Display : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private InputField[] inputs;

    public string this[int index]
    {
        get => inputs[index].text;
        set => inputs[index].text = value;
    }

    public Vector3 GetVector()
    {
        Vector3 v3 = new Vector3(float.Parse(inputs[0].text), float.Parse(inputs[1].text), float.Parse(inputs[2].text));
        return v3;
    }

    public InputField[] GetInputFields()
    {
        return inputs;
    }
}
