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

    public DialogueBubble incrementBubble()
    {
        if(bubbles.Count > 0)
            return bubbles.Dequeue();
        return null;
    }

}
