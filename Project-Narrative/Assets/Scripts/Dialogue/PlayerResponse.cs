using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResponse : DialogueBubble
{
    private int priority;
    private Trait requiredTrait;
    public KeyValuePair<bool, int> isInterrupt { get; }
    private ResponseType type;
    private short childID;

    public PlayerResponse(int Priority, Trait RequiredTrait, KeyValuePair<bool, int> IsInterrupt, ResponseType Type, short ChildID,
        string Text, int BackgroundTexture, int BackgroundAnimation, Vector3 BackgroundColor, Vector3 TextColor, int TextAnimation, int Location, Vector3 Scale, Vector3 Rotation, float EntryTime)
        : base(Text, BackgroundTexture, BackgroundAnimation, BackgroundColor, TextColor, TextAnimation, Location, Scale, Rotation, EntryTime)
    {
        priority = Priority;
        requiredTrait = RequiredTrait;
        isInterrupt = IsInterrupt;
        childID = ChildID;
        type = Type;
    }

    public short GetChildID()
    {
        return childID;
    }
}
