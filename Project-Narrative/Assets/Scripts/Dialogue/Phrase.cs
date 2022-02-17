using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phrase
{
    protected List<DialogueBubble> bubbles;
    private int bubbleIndex = -1;

    public Phrase(List<DialogueBubble> Bubbles)
    {
        bubbles = Bubbles;
    }

    public DialogueBubble IncrementBubble()
    {
        bubbleIndex++;
        if(bubbles.Count > 0)
            return bubbles[bubbleIndex];
        return null;
    }

    public int GetNumOfBubbles()
    {
        return bubbles.Count - (bubbleIndex + 1);
    }
    public void ResetIndex()
    {
        bubbleIndex = -1;
    }

}
