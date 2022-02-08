using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private string npcName;
    [SerializeField]
    private float turnSpeed;
    private float baseTurnSpeed;
    [SerializeField]
    private float interactionBoxRadius;
    [SerializeField]
    private Transform headTransform;

    private bool hasDialogue;
    private bool lookingAtPlayer;

    public short treeID { get; }

    private Vector3 positionLookingAt;


    //Later replaced with dialogue object
    private DialogueTree dialogue;

    // Start is called before the first frame update
    void Start()
    {
        SphereCollider trigger = gameObject.AddComponent<SphereCollider>();
        trigger.isTrigger = true;
        if (interactionBoxRadius <= 0)
            interactionBoxRadius = 25;
        trigger.radius = interactionBoxRadius;

        if (turnSpeed <= 0)
            turnSpeed = 5;
        baseTurnSpeed = turnSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(lookingAtPlayer)
        {
            float step = Time.deltaTime * turnSpeed;
            positionLookingAt = GameManager.GetPlayerPosition() - transform.position;
            turnSpeed = (((interactionBoxRadius - positionLookingAt.magnitude) / interactionBoxRadius) * baseTurnSpeed) * 1.2f;
            Vector3 newAngle = Vector3.RotateTowards(transform.forward, positionLookingAt, step, 0.0f);

            transform.rotation = Quaternion.LookRotation(new Vector3(newAngle.x, 0, newAngle.z));
        }
    }

    public string GetName()
    {
        return npcName;
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

    public Transform GetHeadTransform()
    {
        return headTransform;
    }
}
