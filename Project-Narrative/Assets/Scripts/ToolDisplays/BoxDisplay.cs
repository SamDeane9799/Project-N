using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxDisplay : MonoBehaviour
{
    private DialogueBox myBox;

    [SerializeField]
    private Text desc;
    [SerializeField]
    private Text ID;
    [SerializeField]
    private Button openButton;
    [SerializeField]
    private Button deleteButton;
    [SerializeField]
    private Text childIDs;

    // Start is called before the first frame update
    void Start()
    {
        ChangeDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeDisplay()
    {
        desc.text += myBox.GetNumOfPhrases().ToString();
        ID.text = "ID: " + myBox.GetID().ToString();
        List<short> idsAdded = new List<short>();
        foreach(short id in myBox.childIDs)
        {
            if (!idsAdded.Contains(id))
            {
                idsAdded.Add(id);
                childIDs.text += id.ToString() + ", ";
            }
        }
    }

    public void SetDialogueBox(DialogueBox box)
    {
        myBox = box;
    }

    /*    public void SetOpenAndDeleteMethod(UnityEngine.Events.UnityAction openMethod, UnityEngine.Events.UnityAction deleteMethod)
        {
            openButton.onClick.AddListener(openMethod);
            deleteButton.onClick.AddListener(deleteMethod);
        }*/

    public void SetDeleteMethod(UnityEngine.Events.UnityAction deleteMethod)
    {
        deleteButton.onClick.AddListener(deleteMethod);
    }

    public short GetID()
    {
        return myBox.GetID();
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
