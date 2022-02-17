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

    private static GameObject textPrefab;
    private static GameObject[] textObjects;

    private static float currentEntryTime;
    private static float maxEntryTime;

    private static bool inConversation;
    private static DialogueBox currentBox;
    private static Phrase currentPhrase;
    private static GameObject currentBubble;
    private static List<GameObject> bubblesOnScreen = new List<GameObject>();
    private static List<PlayerResponse> responsesDisplayed = new List<PlayerResponse>();
    private static int currentPhraseIndex = -1;
    private static bool phraseChanged;

    private static int poolIndex;
    private const int MAX_NUM_OF_POOL = 12;
    private static GameObject[] objectPool;

    private static List<KeyValuePair<int, int>> interruptsOn = new List<KeyValuePair<int, int>>();

    private static Vector3[] positions = { 
    new Vector3(0, 15, 0), new Vector3(7.5f, 12.5f, 0), new Vector3(15, 0, 0),
    new Vector3(10f, -7.5f, 0),new Vector3(0, -15, 0), new Vector3(-10f, -7.5f, 0),
    new Vector3(-15, 0, 0), new Vector3(-10f, 7.5f, 0) };
    private static Vector3[] responsePositions = {
    new Vector3(-2, -1f, 0), new Vector3(-2, 1f, 0),
    new Vector3(2, 1f, 0), new Vector3(2, 1f, 0)};

    public static void SetConversationPartners(NPC Partner)
    {
        if (inConversation)
        {
            //partner.Ignored();
        }
        partner = Partner;
        inConversation = true;
        SetBox(partner.GetMyDialogueBox());
    }

    public static void Reset()
    {
        partner = null;
        inConversation = false;
        ClearScreen();
        currentBox.ResetBox();
        currentBox = null;
        currentPhrase = null;
        currentPhraseIndex = 0;
        currentEntryTime = 0;
        phraseChanged = false;
        maxEntryTime = float.MaxValue;
    }

    //May have to remove ref later on, I just want a constantly update reference to player during conversation :/
    //Would also be way easier in c++ :)
    public static void SetPlayer(Player Player)
    {
        player = Player;
    }

    public static void IncrementTime(float deltaTime)
    {
        if (!inConversation)
            return;
        currentEntryTime += deltaTime;
        if(currentEntryTime >= maxEntryTime && !currentBubble.activeInHierarchy)
        {
            if (phraseChanged)
            {
                ClearScreen();
                phraseChanged = false;
            }
            currentBubble.SetActive(true);
            if (currentPhrase.GetNumOfBubbles() > 0)
                DisplayBubble(currentPhrase.IncrementBubble());
            else if(currentBox.GetNumOfPhrases() > 0)
                SetPhrase(currentBox.IncrementPhrase());
        }
    }

    public static void ChooseResponse(int num)
    {
        if (num > responsesDisplayed.Count)
            return;
        ClearScreen();
        SetBox(DialogueFileLoader.GetDialogueTree(partner.GetTreeID()).GetDialogueBox(responsesDisplayed[num].GetChildID()));
    }


    //MIGHT HAVE TO USE THIS IF CREATEPRIMITIVE DOESNT WORK!
    /*    public static void SetBubblePrefab(GameObject prefab)
        {

        }*/

    public static void SetTextPrefab(GameObject prefab)
    {
        textPrefab = prefab;
    }

    public static void SetBox(DialogueBox box)
    {
        currentBox = box;
        for (int i = 0; i < box.responses.Length; i++)
        {
            if (box.responses[i].isInterrupt.Key)
                interruptsOn.Add(new KeyValuePair<int, int>(i, box.responses[i].isInterrupt.Value));
        }
        currentPhraseIndex = -1;
        SetPhrase(box.IncrementPhrase());
    }


    private static void SetPhrase(Phrase phrase)
    {
        currentPhrase = phrase;
        currentPhraseIndex++;
        phraseChanged = true;
        for (int i = 0; i < interruptsOn.Count; i++)
        {
            //Detect if we've hit a phrase with an interrupt on it
            if (interruptsOn[i].Value == currentPhraseIndex)
            {
                DialogueBubble newBubble = currentPhrase.IncrementBubble();
                newBubble.SetInterrupt(currentBox.responses[interruptsOn[i].Key]);
                DisplayBubble(newBubble);
                return;
            }
        }
        currentPhrase = phrase;
        DisplayBubble(currentPhrase.IncrementBubble());
    }


    public static void DisplayBubble(DialogueBubble bubbleInfo)
    {
        if (bubbleInfo.GetInterrupt() != null)
            DisplayBubble(bubbleInfo.GetInterrupt());
        GameObject newBubble = objectPool[poolIndex];
        newBubble.transform.localRotation = Quaternion.Euler(bubbleInfo.rotation.x - 90, bubbleInfo.rotation.y + 90, bubbleInfo.rotation.z + 90);
        poolIndex++;
        if (poolIndex >= MAX_NUM_OF_POOL)
            poolIndex = 0;
        if (bubbleInfo is PlayerResponse)
        {
            newBubble.transform.localPosition = responsePositions[bubbleInfo.location];
            responsesDisplayed.Add((PlayerResponse)bubbleInfo);
        }
        else
        {            
            newBubble.transform.parent = partner.GetHeadTransform();
            newBubble.transform.localPosition = positions[bubbleInfo.location];
            newBubble.transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y + 5, player.transform.position.z));
            newBubble.transform.Rotate(new Vector3(-90, 90, 90));
        }
        newBubble.transform.localScale = bubbleInfo.scale;
        DialogueBubbleDisplay bubbleDisp = newBubble.GetComponent<DialogueBubbleDisplay>();
        bubbleDisp.SetText(bubbleInfo.text);
        bubbleDisp.SetTextColor(bubbleInfo.textColor);
        currentEntryTime = 0;
        maxEntryTime = bubbleInfo.entryTime;
        currentBubble = newBubble;
        bubblesOnScreen.Add(currentBubble);

        if (currentPhrase.GetNumOfBubbles() <= 0 && currentBox.GetNumOfPhrases() <= 0)
            DisplayPlayerResponses();
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
        for(int i = 0; i < bubblesOnScreen.Count; i++)
        {
            if (bubblesOnScreen[i].activeInHierarchy)
            {
                bubblesOnScreen[i].SetActive(false);
                bubblesOnScreen[i].transform.position = new Vector3(0, -1000, 0);
                bubblesOnScreen.Remove(bubblesOnScreen[i]);
                i--;
            }
        }
        poolIndex = bubblesOnScreen.Count;
        responsesDisplayed.Clear();
    }

    public static void CreatePool()
    {
        poolIndex = 0;
        objectPool = new GameObject[MAX_NUM_OF_POOL];
        textObjects = new GameObject[MAX_NUM_OF_POOL];
        for (int i = 0; i < MAX_NUM_OF_POOL; i++)
        {
            objectPool[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
            objectPool[i].transform.position = new Vector3(0, -1000, 0);
            objectPool[i].AddComponent<DialogueBubbleDisplay>().SetTextPrefab(textPrefab);
            objectPool[i].SetActive(false);
        }
    }
}
