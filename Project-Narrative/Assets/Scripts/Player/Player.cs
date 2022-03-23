using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    private bool inNPC;
    [SerializeField]
    private GameObject interactUI;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard.digit1Key.wasPressedThisFrame)
            ConversationManager.ChooseResponse(1);
        if (keyboard.digit2Key.wasPressedThisFrame)
            ConversationManager.ChooseResponse(2);
        if (keyboard.digit3Key.wasPressedThisFrame)
            ConversationManager.ChooseResponse(3);
        if (keyboard.digit4Key.wasPressedThisFrame)
            ConversationManager.ChooseResponse(4);
    }

    private void OnTriggerEnter(Collider other)
    {
        NPCEnterExit(other);
    }

    private void OnTriggerExit(Collider other)
    {
        NPCEnterExit(other);
    }

    private void NPCEnterExit(Collider other)
    {
        NPC potentialNPC;
        if (other.TryGetComponent<NPC>(out potentialNPC))
        {
            inNPC = !inNPC;
            if (inNPC)
            {
                potentialNPC.RotateTowards(transform.GetChild(0));
                ConversationManager.SetPlayer(this);
                if (potentialNPC.GetHasDialogue())
                {
                    ConversationManager.SetConversationPartners(potentialNPC);
                }/*
                else
                {
                    interactUI.transform.GetChild(0).GetComponent<Text>().text = "Talk to " + potentialNPC.GetName();
                }*/
            }
            else
            {
                ConversationManager.Reset();
                potentialNPC.PlayerLeft();
            }
        }
    }
}
