using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private string npcName;
    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private float interactionBoxRadius;
    [SerializeField]
    private Transform headTransform;
    [SerializeField]
    private short treeID;
    [SerializeField]
    private bool hasDialogue;

    private float baseTurnSpeed;
    private bool lookingAtPlayer;

    private DialogueBox myBox;
    private Vector3 positionLookingAt;

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

    public void SetTreeID(short id)
    {
        treeID = id;
    }
    public short GetTreeID()
    {
        return treeID;
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

    public DialogueBox GetMyDialogueBox()
    {
        myBox = DialogueFileLoader.GetDialogueTree(treeID).GetDialogueBox(1);
        return myBox;
    }
}
