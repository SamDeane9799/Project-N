using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Trait
{
    ABRASIVE,
    STUPID
}

public enum ResponseType
{
    PROFESSIONAL,
    DEFENSIVE
}
public static class ConversationManager
{
    private static NPC partner;
    private static Player player;
    private static GameObject dialogueBubblePrefab;

    private static bool inConversation;

    private static Vector3[] positions = { 
    new Vector3(0, 1, 0), new Vector3(.75f, .5f, 0), new Vector3(1, 0, 0),
    new Vector3(.75f, -.5f, 0),new Vector3(0, -1, 0), new Vector3(-.75f, -.5f, 0),
    new Vector3(-1, 0, 0), new Vector3(-.75f, .5f, 0) };

    public static void SetConversationPartners(NPC Partner)
    {
        if (inConversation)
        {
            //partner.Ignored();
        }
        partner = Partner;
    }

    //May have to remove ref later on, I just want a constantly update reference to player during conversation :/
    //Would also be way easier in c++ :)
    public static void SetPlayer(ref Player Player)
    {
        player = Player;
    }

    //MIGHT HAVE TO USE THIS IF CREATEPRIMITIVE DOESNT WORK!
/*    public static void SetBubblePrefab(GameObject prefab)
    {

    }*/

    public static void DisplayBubble(DialogueBubble bubbleInfo)
    {
        GameObject currentBubble = GameObject.CreatePrimitive(PrimitiveType.Plane);
        currentBubble.transform.parent = partner.GetHeadTransform();
        currentBubble.transform.localPosition = positions[bubbleInfo.location];
        
    }

}
