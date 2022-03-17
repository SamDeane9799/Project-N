using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ToolManager : MonoBehaviour
{
    private Dictionary<short, DialogueTree> trees = new Dictionary<short, DialogueTree>();
    [SerializeField]
    private GameObject treePanel;
    [SerializeField]
    private GameObject boxPanel;
    private Stack<GameObject> panelTracker = new Stack<GameObject>();

    [SerializeField]
    private TreeDisplay treeDispPrefab;
    [SerializeField]
    private BoxDisplay boxDispPrefab;
    [SerializeField]
    private Button createTreeButton;
    [SerializeField]
    private InputField idInput;
    [SerializeField]
    private InputField descInput;
    [SerializeField]
    private Text errorOutput;

    private JArray treeJson;
    private JObject boxJson;

    // Start is called before the first frame update
    void Start()
    {
        DialogueFileLoader.LoadDialogueTrees();
        trees = DialogueFileLoader.GetDialogueTrees();
        panelTracker.Push(treePanel);
        panelTracker.Peek().SetActive(true);
        foreach(KeyValuePair<short, DialogueTree> kvp in trees)
        {
            DisplayNewDialogueTree(kvp.Value, kvp.Key);
        }
        GameObject[] openButtons = GameObject.FindGameObjectsWithTag("OpenButton");
        foreach(GameObject btn in openButtons)
        {
            btn.GetComponent<Button>().onClick.AddListener(OpenDialogueTree);
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

        trees.Add(id, newTree);
        DisplayNewDialogueTree(newTree, id);
    }

    private void DisplayNewDialogueTree(DialogueTree dtree, short id)
    {
        TreeDisplay treeDisplay = Instantiate<TreeDisplay>(treeDispPrefab);
        treeDisplay.transform.SetParent(treePanel.transform);
        treeDisplay.Init(id.ToString(), dtree.GetDescription(), "0");
        treeDisplay.transform.localPosition = new Vector3(-315, -(id * 30) + 125, 0);
    }


    private void DeleteDialogueBox()
    {
        short id = EventSystem.current.gameObject.GetComponent<BoxDisplay>().GetID();


    }

    private void OpenDialogueTree()
    {
        short id = EventSystem.current.currentSelectedGameObject.transform.parent.GetComponent<TreeDisplay>().GetID();

        string path = Application.streamingAssetsPath + Path.DirectorySeparatorChar + "DialogueTrees" + Path.DirectorySeparatorChar + id + ".dlt";

        StreamReader reader = new StreamReader(path);
        string jsonAsString = reader.ReadToEnd();
        treeJson = JArray.Parse(jsonAsString);

        panelTracker.Peek().SetActive(false);
        panelTracker.Push(boxPanel);
        panelTracker.Peek().SetActive(true);
        
        DialogueBox[] boxes = DialogueFileLoader.GetDialogueTree(id).GetDialogueBoxes();
        foreach(DialogueBox db in boxes)
        {
            DisplayDialogueBox(db);
        }

    }

    private void DisplayDialogueBox(DialogueBox box)
    {
        BoxDisplay newDisplay = Instantiate<BoxDisplay>(boxDispPrefab);
        newDisplay.SetDialogueBox(box);
        newDisplay.transform.SetParent(boxPanel.transform);

        newDisplay.transform.localPosition = new Vector3(-285 + (box.GetID() * 130), 165 + ((int)(box.GetID() / 5) * 200), 0); 
    }
}
