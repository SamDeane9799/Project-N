using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private string name;
    [SerializeField]
    private float turnSpeed;

    private bool hasDialogue;
    private bool lookingAtPlayer;

    private Vector3 positionLookingAt;


    //Later replaced with dialogue object
    private string dialogue;

    // Start is called before the first frame update
    void Start()
    {
        SphereCollider trigger = gameObject.AddComponent<SphereCollider>();
        trigger.isTrigger = true;
        trigger.radius = 25;
    }

    // Update is called once per frame
    void Update()
    {
        if(lookingAtPlayer)
        {
            float step = Time.deltaTime * turnSpeed;
            positionLookingAt = GameManager.GetPlayerPosition() - transform.position;
            Vector3 newAngle = Vector3.RotateTowards(transform.forward, positionLookingAt, step, 0.0f);

            transform.rotation = Quaternion.LookRotation(new Vector3(newAngle.x, 0, newAngle.z));
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
        lookingAtPlayer = false;
    }

    public void RotateTowards(Transform toLookTowards)
    {
        lookingAtPlayer = true;
        positionLookingAt = toLookTowards.position - transform.position;
    }


    public string DisplayText()
    {
        return dialogue;
    }
}
