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
    public DialogueTree(string Description)
    {
        dialogueBoxes = new Dictionary<short, DialogueBox>();
        description = Description;
        currentID = -1;
        CalcTiers();
        CalcParents();
    }

    public DialogueBox[] GetDialogueBoxes()
    {
        DialogueBox[] boxes = new DialogueBox[dialogueBoxes.Count];
        int i = 0;
        foreach (KeyValuePair<short, DialogueBox> kvp in dialogueBoxes)
        {
            boxes[i] = kvp.Value;
            i++;
        }
        return boxes;   
    }

    public int GetTiers()
    {
        return tiers;
    }

    public string GetDescription()
    {
        return description;
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
                if (s != -1)
                {
                    if (dialogueBoxes.ContainsKey(s))
                        dialogueBoxes[s].AddParentID(db.Key);
                    else
                        Debug.LogWarning("Missing dialogue box " + s + " in Dialogue tree " + description);
                }
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

    public void AddDialogueBox(DialogueBox box)
    {
        dialogueBoxes.Add(box.GetID(), box);
    }
    public void RemoveDialogueBox(short boxID)
    {
        dialogueBoxes.Remove(boxID);
    }

    //TODO: Given the tree's current dialogue boxes go through and find out how many tiers there are
    private int CalcTiers()
    {
        return -1;
    }
}
