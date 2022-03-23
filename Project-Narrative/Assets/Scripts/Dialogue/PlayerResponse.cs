using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResponse : DialogueBubble
{
    public int priority { get; }
    public Trait requiredTrait { get; }
    public KeyValuePair<bool, int> isInterrupt { get; }
    public ResponseType type { get; }
    public short childID { get; }

    public PlayerResponse(int Priority, Trait RequiredTrait, KeyValuePair<bool, int> IsInterrupt, ResponseType Type, short ChildID,
        string Text, string BackgroundTexture, int BackgroundAnimation, Vector3 BackgroundColor, Vector3 TextColor, int TextAnimation, Vector3 Scale, Vector3 Rotation, float EntryTime)
        : base(Text, BackgroundTexture, BackgroundAnimation, BackgroundColor, TextColor, TextAnimation, -1, Scale, Rotation, EntryTime)
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

    public void SetLocation(int newLocation)
    {
        location = newLocation;
    }
}
