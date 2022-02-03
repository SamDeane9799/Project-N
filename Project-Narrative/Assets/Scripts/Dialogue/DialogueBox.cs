using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox
{
    public Queue<Phrase> phrases { get; }
    public PlayerResponse[] responses { get; }
    private short ID;
    private short[] parentIDs;
    private List<short> childrenIDs;
    private int animationID;

    public DialogueBox(short boxID, Queue<Phrase> Phrases, PlayerResponse[] Responses, int AnimationID)
    {
        ID = boxID;
        phrases = Phrases;
        responses = Responses;
        animationID = AnimationID;
        GetChildrenIDs();
    }

    //Go through all our responses and add their child dialogue box to the list
    private short[] GetChildrenIDs()
    {
        foreach(PlayerResponse pr in responses)
        {
            childrenIDs.Add(pr.GetChildID());
        }
        short[] IDs = new short[childrenIDs.Count];
        childrenIDs.CopyTo(IDs);
        return IDs;
    }

    public Phrase IncrementPhrase()
    {
        if (phrases.Count > 0)
            return phrases.Dequeue();
        return null;
    }

    public int GetNumOfPhrases()
    {
        return phrases.Count;
    }
}
