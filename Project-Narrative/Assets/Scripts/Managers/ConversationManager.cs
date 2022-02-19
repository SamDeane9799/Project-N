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

    private static bool inConversation;
    private static DialogueTree currentTree;
    private static DialogueBox currentBox;
    private static Phrase currentPhrase;
    private static DialogueBubbleDisplay currentBubble;
    private static List<DialogueBubbleDisplay>[] bubblesOnScreen;
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


    public static void Init(GameObject TextPrefab)
    {
        textPrefab = TextPrefab;
        CreatePool();
    }

    public static void SetConversationPartners(NPC Partner)
    {
        if (inConversation)
        {
            //partner.Ignored();
        }
        partner = Partner;
        inConversation = true;
        currentTree = DialogueFileLoader.GetDialogueTree(partner.GetTreeID());
        SetBox(currentTree.IncrementBox());
    }

    public static void Reset()
    {
        partner = null;
        inConversation = false;
        HardClearScreen();
        currentTree.Reset();
        currentTree = null;
        currentBox.ResetBox();
        currentBox = null;
        currentPhrase = null;
        currentPhraseIndex = 0;
        phraseChanged = false;
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
        if(currentBubble.IsInView())
        {
            if (phraseChanged)
            {
                SoftClearScreen();
                phraseChanged = false;
            }
            if (currentPhrase.GetNumOfBubbles() > 0)
                DisplayBubble(currentPhrase.IncrementBubble());
            else if (currentBox.GetNumOfPhrases() > 0)
                SetPhrase(currentBox.IncrementPhrase());
            else if (currentTree.HasMoreBoxes())
            {
                SetBox(currentTree.IncrementBox());
            }
            //else
                //DisplayPlayerResponses();
        }
    }

    public static void ChooseResponse(int num)
    {
        if (num > responsesDisplayed.Count)
            return;
        HardClearScreen();
        SetBox(DialogueFileLoader.GetDialogueTree(partner.GetTreeID()).GetDialogueBox(responsesDisplayed[num].GetChildID()));
    }

    public static void SetBox(DialogueBox box)
    {
        HardClearScreen();
        currentBox = box;
        bubblesOnScreen = new List<DialogueBubbleDisplay>[currentBox.GetNumOfPhrases()];
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
        bubblesOnScreen[currentPhraseIndex] = new List<DialogueBubbleDisplay>();
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
        poolIndex++;
        if (poolIndex >= MAX_NUM_OF_POOL)
            poolIndex = 0;

        GameObject newBubble = objectPool[poolIndex];

        if (bubbleInfo.GetInterrupt() != null)
        {
            DisplayBubble(bubbleInfo.GetInterrupt());
        }

        if (bubbleInfo is PlayerResponse)
        {
            newBubble.transform.parent = player.transform;
            responsesDisplayed.Add((PlayerResponse)bubbleInfo);
        }
        else
        {            
            newBubble.transform.parent = partner.GetHeadTransform();
            newBubble.transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y + 5, player.transform.position.z));
            newBubble.transform.Rotate(new Vector3(-90, 90, 90));
        }

        DialogueBubbleDisplay bubbleDisp = newBubble.GetComponent<DialogueBubbleDisplay>();
        bubbleDisp.SetInfo(bubbleInfo, positions[bubbleInfo.location]);
        currentBubble = bubbleDisp;
        bubblesOnScreen[currentPhraseIndex].Add(bubbleDisp);
    }

    private static void DisplayPlayerResponses()
    {
        foreach(PlayerResponse pr in currentBox.responses)
        {
            if(!pr.isInterrupt.Key)
                DisplayBubble(pr);
        }
    }

    private static void SoftClearScreen()
    {
        if (currentPhraseIndex == 0)
            return;
        for (int i = 0; i < bubblesOnScreen[currentPhraseIndex - 1].Count; i++)
        {
              bubblesOnScreen[currentPhraseIndex - 1][i].GetComponent<DialogueBubbleDisplay>().ResetBubble();
        }
        bubblesOnScreen[currentPhraseIndex - 1].Clear();
        responsesDisplayed.Clear();
    }
    private static void HardClearScreen()
    {
        if (bubblesOnScreen == null)
            return;
        for(int i = 0; i < bubblesOnScreen.Length; i++)
        {
            if (bubblesOnScreen[i] == null)
                break;
            for (int j = 0; j < bubblesOnScreen[i].Count; j++)
            {
                bubblesOnScreen[i][j].ResetBubble();
            }
            bubblesOnScreen[i].Clear();
        }
        responsesDisplayed.Clear();
    }

    private static void CreatePool()
    {
        poolIndex = -1;
        objectPool = new GameObject[MAX_NUM_OF_POOL];
        textObjects = new GameObject[MAX_NUM_OF_POOL];
        for (int i = 0; i < MAX_NUM_OF_POOL; i++)
        {
            objectPool[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
            objectPool[i].transform.position = new Vector3(0, -1000, 0);
            objectPool[i].AddComponent<DialogueBubbleDisplay>().SetTextPrefab(textPrefab);
        }
    }
}
