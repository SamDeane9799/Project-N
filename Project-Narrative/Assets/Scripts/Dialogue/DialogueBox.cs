using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox
{
    private Queue<Phrase> phrases;
    private PlayerResponse[] responses;
    private short[] parentIDs;
    private List<short>[] childrenIDs;
    private float entryTime;
    private short animationID;

    public DialogueBox(Queue<Phrase> Phrases, PlayerResponse[] Responses, short[] ParentIDs, float EntryTime, short AnimationID)
    {
        phrases = Phrases;
        responses = Responses;
        parentIDs = ParentIDs;
        entryTime = EntryTime;
        animationID = AnimationID;
        GetChildrenIDs();
    }

    //Go through all our responses and add their child dialogue box to the list
    private void GetChildrenIDs()
    {

    }
}
