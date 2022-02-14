using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBubble
{
    public int backgroundTexture { get; }
    public int backgroundAnimation { get; }
    public Vector3 backgroundColor { get; }
    public Vector3 textColor { get; }
    public int textAnimation { get; }
    public int location { get; }
    public Vector3 scale { get; }
    public Vector3 rotation { get; }
    public float entryTime { get; }
    public string text { get; }

    private DialogueBubble interruptBubble;

    public DialogueBubble(string Text, int BackgroundTexture, int BackgroundAnimation, Vector3 BackgroundColor, Vector3 TextColor, int TextAnimation, int Location, Vector3 Scale, Vector3 Rotation, float EntryTime)
    {
        backgroundTexture = BackgroundTexture;
        backgroundColor = BackgroundColor;
        backgroundAnimation = BackgroundAnimation;
        textAnimation = TextAnimation;
        location = Location;
        scale = Scale;
        rotation = Rotation;
        entryTime = EntryTime;
        text = Text;
        textColor = TextColor;
    }

    public void SetInterrupt(DialogueBubble bubble)
    {
        interruptBubble = bubble;
    }

    public DialogueBubble GetInterrupt()
    {
        return interruptBubble;
    }
}
