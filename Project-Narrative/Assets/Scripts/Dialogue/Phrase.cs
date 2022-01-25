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


}
