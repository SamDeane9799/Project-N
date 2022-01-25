using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTree
{
    private Dictionary<short, DialogueBox> dialogueBoxes;
    private int tiers;

    public DialogueTree(Dictionary<short, DialogueBox> DialogueBoxes)
    {
        dialogueBoxes = DialogueBoxes;
        CalcTiers();
    }

    public void SetTiers(int num)
    {
        tiers = num;
    }
    public int GetTiers()
    {
        return tiers;
    }

    //TODO: Given the tree's current dialogue boxes go through and find out how many tiers there are
    private int CalcTiers()
    {
        return -1;
    }
}
