using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
            interactUI.SetActive(inNPC);
            if (inNPC)
            {
                potentialNPC.RotateTowards(transform.GetChild(0));
                ConversationManager.SetPlayer(this);
                if (potentialNPC.GetHasDialogue())
                {
                    ConversationManager.SetConversationPartners(potentialNPC);
                }
                else
                {
                    interactUI.transform.GetChild(0).GetComponent<Text>().text = "Talk to " + potentialNPC.GetName();
                }
            }
            else
            {
                potentialNPC.PlayerLeft();
            }
        }
    }
}
