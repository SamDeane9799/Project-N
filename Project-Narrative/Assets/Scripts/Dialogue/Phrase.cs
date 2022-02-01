using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phrase
{
    protected Queue<DialogueBubble> bubbles;

    public Phrase(Queue<DialogueBubble> Bubbles)
    {
        bubbles = Bubbles;
    }

    public DialogueBubble IncrementBubble()
    {
        if(bubbles.Count > 0)
            return bubbles.Dequeue();
        return null;
    }

    public int GetNumOfBubbles()
    {
        return bubbles.Count;
    }

}
