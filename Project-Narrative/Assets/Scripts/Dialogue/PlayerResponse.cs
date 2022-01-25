using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResponse : Phrase
{
    private int priority;
    private Trait requiredTrait;
    private int[] isInterrupt;
    private ResponseType type;
    private int childID;

    public PlayerResponse(int Priority, Trait RequiredTrait, int[] IsInterrupt, ResponseType Type, int ChildID, Queue<DialogueBubble> Bubbles,
        int BackgroundTexture, Color BackgroundColor, int TextAnimation, int Location, Vector3 Scale, Vector3 Rotation, float ExitTime) 
        : base(Bubbles, BackgroundTexture, BackgroundColor, TextAnimation, Location, Scale, Rotation, ExitTime)
    {
        priority = Priority;
        requiredTrait = RequiredTrait;
        if (isInterrupt.Length != 2)
            throw new Exception("IsInterrupt is incorrect length in PlayerResponse with childID: " + childID);
        isInterrupt = IsInterrupt;
        type = Type;
    }
}
