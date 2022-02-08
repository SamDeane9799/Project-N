using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTree
{
    private Dictionary<short, DialogueBox> dialogueBoxes;
    private int tiers;
    private string description;

    public DialogueTree(Dictionary<short, DialogueBox> DialogueBoxes, string Description)
    {
        dialogueBoxes = DialogueBoxes;
        description = Description;
        CalcTiers();
        CalcParents();
    }

    public int GetTiers()
    {
        return tiers;
    }

    public DialogueBox GetDialogueBox(short id)
    {
        return dialogueBoxes[id];
    }

    private void CalcParents()
    {
        foreach(KeyValuePair<short, DialogueBox> db in dialogueBoxes)
        {
            short[] childIDs = dialogueBoxes[db.Key].childIDs;
            foreach(short s in childIDs)
            {
                if(s != -1)
                    dialogueBoxes[s].AddParentID(db.Key);
            }
        }
    }

    //TODO: Given the tree's current dialogue boxes go through and find out how many tiers there are
    private int CalcTiers()
    {
        return -1;
    }
}
