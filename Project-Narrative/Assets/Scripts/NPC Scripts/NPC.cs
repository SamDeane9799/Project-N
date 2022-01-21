using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private string name;

    private bool hasDialogue;
    private bool lookingAtSomething;

    private Vector3 positionLookingAt;

    //Later replaced with dialogue object
    private string dialogue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lookingAtSomething)
        {
            transform.LookAt(new Vector3(positionLookingAt.x, transform.position.y, positionLookingAt.z));
            transform.GetChild(0).transform.LookAt(positionLookingAt);
        }
    }

    public string GetName()
    {
        return name;
    }
    public bool GetHasDialogue()
    {
        return hasDialogue;
    }

    public void PlayerLeft()
    {
        lookingAtSomething = false;
    }

    public void RotateTowards(Transform toLookTowards)
    {
        lookingAtSomething = true;
        positionLookingAt = toLookTowards.position;
        transform.LookAt(new Vector3(positionLookingAt.x, transform.position.y, positionLookingAt.z));
        transform.GetChild(0).transform.LookAt(positionLookingAt);
    }


    public string DisplayText()
    {
        return dialogue;
    }
}
