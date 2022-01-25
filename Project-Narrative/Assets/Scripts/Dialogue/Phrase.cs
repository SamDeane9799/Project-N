using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phrase
{
    protected Queue<DialogueBubble> bubbles;
    protected int backgroundTexture;
    protected Color backgroundColor;
    protected int textAnimation;
    protected int location;
    protected Vector3 scale;
    protected Vector3 rotation;
    protected float exitTime;

    public Phrase(Queue<DialogueBubble> Bubbles, int BackgroundTexture, Color BackgroundColor, int TextAnimation, int Location, Vector3 Scale, Vector3 Rotation, float ExitTime)
    {
        bubbles = Bubbles;
        backgroundTexture = BackgroundTexture;
        backgroundColor = BackgroundColor;
        textAnimation = TextAnimation;
        location = Location;
        scale = Scale;
        rotation = Rotation;
        exitTime = ExitTime;
    }


}
