using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTree
{
    private Dictionary<short, DialogueBox> dialogueBoxes;
    private int tiers;
    private string description;
    private short currentID;

    public DialogueTree(Dictionary<short, DialogueBox> DialogueBoxes, string Description)
    {
        dialogueBoxes = DialogueBoxes;
        description = Description;
        currentID = -1;
        CalcTiers();
        CalcParents();
    }

    public int GetTiers()
    {
        return tiers;
    }

    public DialogueBox GetDialogueBox(short id)
    {
        currentID = id;
        return dialogueBoxes[id];
    }

    public DialogueBox IncrementBox()
    {
        currentID++;
        if(dialogueBoxes.ContainsKey(currentID))
            return dialogueBoxes[currentID];
        return null;
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

    public void Reset()
    {
        currentID = -1;
        foreach(KeyValuePair<short, DialogueBox> kvp in dialogueBoxes)
        {
            dialogueBoxes[kvp.Key].ResetBox();
        }
    }

    public bool HasMoreBoxes()
    {
        return dialogueBoxes.ContainsKey((short)(currentID + 1));
    }

    //TODO: Given the tree's current dialogue boxes go through and find out how many tiers there are
    private int CalcTiers()
    {
        return -1;
    }
}
