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
}
