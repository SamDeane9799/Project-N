using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponseDisp : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerResponse responseToDisp;

    [SerializeField]
    private InputField priorityInput;
    [SerializeField]
    private InputField childIDInput;
    [SerializeField]
    private InputField textAnimationInput;
    [SerializeField]
    private InputField backgroundAnimationInput;
    [SerializeField]
    private InputField entryTimeInput;
    [SerializeField]
    private InputField textInputField;
    [SerializeField]
    private InputField isInterruptInputField;
    [SerializeField]
    private Dropdown traitInputField;
    [SerializeField]
    private Dropdown typeInputField;
    [SerializeField]
    private Dropdown isInterruptDropdown;
    [SerializeField]
    private Dropdown backgroundOptions;
    public V3Display backgroundColor;
    public V3Display scale;
    public V3Display textColor;
    public V3Display rotation;

    private bool dirty;

    void Start()
    {
        InputField[] fields = GameObject.FindObjectsOfType<InputField>();
        for(int i = 0; i < fields.Length; i++)
        {
            fields[i].onValueChanged.AddListener(OnChangeInputField);
        }

        Dropdown[] dropdowns = GameObject.FindObjectsOfType<Dropdown>();
        for(int i = 0; i < dropdowns.Length; i++)
        {
            dropdowns[i].onValueChanged.AddListener(OnChangeDropdownField);
        }

        V3Display[] v3Displays = GameObject.FindObjectsOfType<V3Display>();
        for(int i = 0; i < v3Displays.Length; i++)
        {
            InputField[] inputFields = v3Displays[i].GetInputFields();
            for(int j = 0; j < inputFields.Length; j++)
            {
                inputFields[j].onValueChanged.AddListener(OnChangeInputField);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(PlayerResponse response)
    {
        responseToDisp = response;
        Display();
    }

    private void Display()
    {
        priorityInput.text = responseToDisp.priority.ToString();
        childIDInput.text = responseToDisp.childID.ToString();
        textAnimationInput.text = responseToDisp.textAnimation.ToString();
        backgroundAnimationInput.text = responseToDisp.backgroundAnimation.ToString();
        entryTimeInput.text = responseToDisp.entryTime.ToString();
        textInputField.text = responseToDisp.text;
        traitInputField.value = (int)responseToDisp.requiredTrait;
        typeInputField.value = (int)responseToDisp.type;
        if (responseToDisp.isInterrupt.Key)
            isInterruptDropdown.value = 1;
        else
            isInterruptDropdown.value = 0;
        isInterruptInputField.text = responseToDisp.isInterrupt.Value.ToString();
        
        List<UnityEngine.UI.Dropdown.OptionData> options = new List<UnityEngine.UI.Dropdown.OptionData>();
        string[] stringOptions = AssetLoader.GetBubbleNames();
        foreach(string name in stringOptions)
        {
            options.Add(new Dropdown.OptionData(name));
        }

        backgroundOptions.value = IndexOfArray(stringOptions, responseToDisp.backgroundTexture);

        SetV3ToVector(backgroundColor, responseToDisp.backgroundColor);
        SetV3ToVector(scale, responseToDisp.scale);
        SetV3ToVector(textColor, responseToDisp.textColor);
        SetV3ToVector(rotation, responseToDisp.rotation);

        dirty = false;
    }

    public PlayerResponse GetAsResponse()
    {
        PlayerResponse response = new PlayerResponse(int.Parse(priorityInput.text), (Trait)traitInputField.value, new KeyValuePair<bool, int>(bool.Parse(isInterruptDropdown.options[isInterruptDropdown.value].text), int.Parse(isInterruptInputField.text)),
            (ResponseType)typeInputField.value, short.Parse(childIDInput.text), textInputField.text, backgroundOptions.options[backgroundOptions.value].text, int.Parse(backgroundAnimationInput.text),
            backgroundColor.GetVector(), textColor.GetVector(), int.Parse(textAnimationInput.text), scale.GetVector(), rotation.GetVector(), float.Parse(entryTimeInput.text));

        return response;
    }

    private int IndexOfArray(string[] array, string name)
    {
        for(int i = 0; i < array.Length; i++)
        {
            if (array[i] == name)
                return i;
        }
        return -1;
    }

    private void SetV3ToVector(V3Display disp, Vector3 vector)
    {
        disp[0] = vector.x.ToString();
        disp[1] = vector.y.ToString();
        disp[2] = vector.z.ToString();
    }

    private void OnChangeInputField(string newVal)
    {
        dirty = true;
    }

    private void OnChangeDropdownField(int val)
    {
        dirty = true;
    }

}
