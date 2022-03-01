using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolManager : MonoBehaviour
{
    private Dictionary<short, DialogueTree> trees = new Dictionary<short, DialogueTree>();
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TreeDisplay treeDispPrefab;

    // Start is called before the first frame update
    void Start()
    {
        DialogueFileLoader.LoadDialogueTrees();
        trees = DialogueFileLoader.GetDialogueTrees();
        int i = 0;
        foreach(KeyValuePair<short, DialogueTree> kvp in trees)
        {
            TreeDisplay treeDisplay = Instantiate<TreeDisplay>(treeDispPrefab);
            treeDisplay.transform.SetParent(panel.transform);
            treeDisplay.Init(kvp.Key.ToString(), trees[kvp.Key].GetDescription(), "0");
            treeDisplay.transform.localPosition = new Vector3(-315, -(i * 30) + 125, 0);
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
