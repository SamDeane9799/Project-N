using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResponse
{
    private Queue<DialogueBubble> bubbles;
    private int priority;
    private Trait requiredTrait;
    private int[] isInterrupt;
    private ResponseType type;
    private short childID;

    public PlayerResponse(int Priority, Trait RequiredTrait, int[] IsInterrupt, ResponseType Type, int ChildID, Queue<DialogueBubble> Bubbles) 
    {
        priority = Priority;
        requiredTrait = RequiredTrait;
        if (isInterrupt.Length != 2)
            throw new Exception("IsInterrupt is incorrect length in PlayerResponse with childID: " + childID);
        isInterrupt = IsInterrupt;
        type = Type;
        bubbles = Bubbles;
    }

    public short GetChildID()
    {
        return childID;
    }
}
