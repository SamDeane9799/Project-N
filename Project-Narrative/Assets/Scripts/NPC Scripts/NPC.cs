using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private string name;

    private bool hasDialogue;

    //Later replaced with dialogue object
    private string dialogue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetName()
    {
        return name;
    }
    public bool GetHasDialogue()
    {
        return hasDialogue;
    }

    public void RotateTowards(Transform toLookTowards)
    {
        Vector3 targetToMe = transform.position - toLookTowards.position;
        Quaternion rotation = Quaternion.LookRotation(toLookTowards.position, new Vector3(0, 1, 0));
        transform.rotation = rotation;

    }


    public string DisplayText()
    {
        return dialogue;
    }
}
