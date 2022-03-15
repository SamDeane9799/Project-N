using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Newtonsoft.Json;

public class ToolManager : MonoBehaviour
{
    private Dictionary<short, DialogueTree> trees = new Dictionary<short, DialogueTree>();
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TreeDisplay treeDispPrefab;
    [SerializeField]
    private Button createTreeButton;
    [SerializeField]
    private InputField idInput;
    [SerializeField]
    private InputField descInput;
    [SerializeField]
    private Text errorOutput;

    private DialogueBox currentBox;

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

        createTreeButton.onClick.AddListener(CreateNewDialogueTree);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateNewDialogueTree()
    {
        short id = -1;
        
        if (!short.TryParse(idInput.text, out id) || DialogueFileLoader.GetDialogueTrees().ContainsKey(id))
        {
            errorOutput.text = "Invalid Dialogue Tree ID.";
            return;
        }

        DialogueTree newTree = new DialogueTree(descInput.text);
        string path = Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + "DialogueTrees" + Path.AltDirectorySeparatorChar + id.ToString() + ".dlt";
        
        TextWriter writer = File.CreateText(path);
        JsonTextWriter jsonWriter = new JsonTextWriter(writer);

        jsonWriter.WriteStartArray();
        jsonWriter.WriteStartObject();
        jsonWriter.WritePropertyName("description");
        jsonWriter.WriteValue(descInput.text);
        jsonWriter.WritePropertyName("id");
        jsonWriter.WriteValue(id);
        jsonWriter.WriteEndObject();
        jsonWriter.WriteStartObject();
        jsonWriter.WriteEndObject();
        jsonWriter.WriteEndArray();
        jsonWriter.Close();
    }


    private void DeleteDialogueBubble()
    {
        short id = EventSystem.current.gameObject.GetComponent<BoxDisplay>().GetID();


    }

}
