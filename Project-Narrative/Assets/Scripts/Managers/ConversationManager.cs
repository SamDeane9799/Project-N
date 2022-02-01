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
    //private static GameObject dialogueBubblePrefab;
    private static float currentEntryTime;
    private static float maxEntryTime;

    private static bool inConversation;
    private static DialogueBox currentBox;
    private static Phrase currentPhrase;
    private static GameObject currentBubble;
    private static List<GameObject> bubblesOnScreen;
    private static int currentPhraseIndex;

    private static int poolIndex;
    private const int MAX_NUM_OF_POOL = 12;
    private static GameObject[] objectPool;

    private static List<KeyValuePair<int, int>> interruptsOn;

    private static Vector3[] positions = { 
    new Vector3(0, 1, 0), new Vector3(.75f, .5f, 0), new Vector3(1, 0, 0),
    new Vector3(.75f, -.5f, 0),new Vector3(0, -1, 0), new Vector3(-.75f, -.5f, 0),
    new Vector3(-1, 0, 0), new Vector3(-.75f, .5f, 0) };
    private static Vector3[] responsePositions = {
    new Vector3(-1, -.5f, 0), new Vector3(-1, .5f, 0),
    new Vector3(1, .5f, 0), new Vector3(1, .5f, 0)};

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

    public static void IncrementTime(float deltaTime)
    {
        currentEntryTime += deltaTime;
        if(currentEntryTime >= maxEntryTime && !currentBubble.activeInHierarchy)
        {
            currentBubble.SetActive(true);
            if (currentPhrase.GetNumOfBubbles() > 0)
                DisplayBubble(currentPhrase.IncrementBubble());
            else if (currentBox.GetNumOfPhrases() > 0)
                SetPhrase(currentBox.IncrementPhrase());
            else
                DisplayPlayerResponses();
        }
    }




    //MIGHT HAVE TO USE THIS IF CREATEPRIMITIVE DOESNT WORK!
/*    public static void SetBubblePrefab(GameObject prefab)
    {

    }*/

    public static void SetBox(DialogueBox box)
    {
        currentBox = box;
        for (int i = 0; i < box.responses.Length; i++)
        {
            if (box.responses[i].isInterrupt.Key)
                interruptsOn.Add(new KeyValuePair<int, int>(i, box.responses[i].isInterrupt.Value));
        }
        currentPhraseIndex = 0;
        SetPhrase(box.IncrementPhrase());
    }


    private static void SetPhrase(Phrase phrase)
    {
        currentPhraseIndex++;
        interruptsOn.ForEach((e) =>
        {
            //Detect if we've hit a phrase with an interrupt on it
            if (e.Value == currentPhraseIndex)
                DisplayBubble(currentBox.responses[e.Key]);
        });
        currentPhrase = phrase;
        ClearScreen();
        DisplayBubble(currentPhrase.IncrementBubble());
    }


    public static void DisplayBubble(DialogueBubble bubbleInfo)
    {
        GameObject newBubble = objectPool[poolIndex];
        poolIndex++;
        if (poolIndex >= MAX_NUM_OF_POOL)
            poolIndex = 0;
        newBubble.transform.parent = partner.GetHeadTransform();
        if (bubbleInfo is PlayerResponse)
            newBubble.transform.localPosition = responsePositions[bubbleInfo.location];
        else
            newBubble.transform.localPosition = positions[bubbleInfo.location];
        newBubble.transform.localScale = bubbleInfo.scale;
        newBubble.transform.Rotate(bubbleInfo.rotation);
        currentEntryTime = 0;
        maxEntryTime = bubbleInfo.entryTime;
        currentBubble = newBubble;
        bubblesOnScreen.Add(currentBubble);
    }

    private static void DisplayPlayerResponses()
    {
        foreach(PlayerResponse pr in currentBox.responses)
        {
            if(!pr.isInterrupt.Key)
                DisplayBubble(pr);
        }
    }
    private static void ClearScreen()
    {
        bubblesOnScreen.ForEach((e) =>
        {
            e.SetActive(false);
            e.transform.position = new Vector3(0, -1000, 0);
        });
        poolIndex = 0;
    }

    public static void CreatePool()
    {
        poolIndex = 0;
        objectPool = new GameObject[MAX_NUM_OF_POOL];
        for (int i = 0; i < MAX_NUM_OF_POOL; i++)
        {
            objectPool[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
            objectPool[i].transform.position = new Vector3(0, -1000, 0);
            objectPool[i].SetActive(false);
        }
    }
}
