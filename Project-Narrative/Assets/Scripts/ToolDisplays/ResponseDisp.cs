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
    private Dropdown traitInputField;
    void Start()
    {
        
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
        //
    }
}
