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
    private Button createBoxButton;
    [SerializeField]
    private InputField boxIDInput;
    [SerializeField]
    private Text errorOutput;
    [SerializeField]
    private Text boxErrorOutput;

    private JArray treeJson;
    private short treeID;
    private JToken boxJson;
    private short boxID;

    // Start is called before the first frame update
    void Start()
    {
        DialogueFileLoader.LoadDialogueTrees();
        trees = DialogueFileLoader.GetDialogueTrees();
        treePanel.SetActive(true);
        boxPanel.SetActive(true);
        foreach (KeyValuePair<short, DialogueTree> kvp in trees)
        {
            DisplayNewDialogueTree(kvp.Value, kvp.Key);
        }
        createTreeButton.onClick.AddListener(CreateNewDialogueTree);
        createBoxButton.onClick.AddListener(CreateDialogueBox);

        GameObject[] backButtons = GameObject.FindGameObjectsWithTag("BackButton");
        foreach(GameObject btn in backButtons)
        {
            btn.GetComponent<Button>().onClick.AddListener(BackButton);
        }
        treePanel.SetActive(false);
        boxPanel.SetActive(false);
        panelTracker.Push(treePanel);
        panelTracker.Peek().SetActive(true);

        //AssetLoader.LoadSprites();
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
        jsonWriter.WriteEndArray();
        jsonWriter.Close();

        trees.Add(id, newTree);
        DisplayNewDialogueTree(newTree, id);
    }

    private void DisplayNewDialogueTree(DialogueTree dtree, short id)
    {
        TreeDisplay treeDisplay = Instantiate<TreeDisplay>(treeDispPrefab);
        treeDisplay.transform.SetParent(treePanel.transform);
        treeDisplay.Init(id.ToString(), dtree.GetDescription(), "0", OpenDialogueTree);
        treeDisplay.transform.localPosition = new Vector3(-315, -(id * 30) + 125, 0);
    }

    private void OpenDialogueTree()
    {
        short id = EventSystem.current.currentSelectedGameObject.transform.parent.GetComponent<TreeDisplay>().GetID();
        treeID = id;

        string path = Application.streamingAssetsPath + Path.DirectorySeparatorChar + "DialogueTrees" + Path.DirectorySeparatorChar + id + ".dlt";

        StreamReader reader = new StreamReader(path);
        string jsonAsString = reader.ReadToEnd();
        reader.Close();
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

        newDisplay.transform.localPosition = new Vector3(-285 + (box.GetID() * 130), 125 + ((int)(box.GetID() / 5) * 200), 0);

        newDisplay.SetDeleteMethod(DeleteDialogueBox);
    }

    private void DeleteDialogueBox()
    {
        short id = EventSystem.current.currentSelectedGameObject.transform.parent.GetComponent<BoxDisplay>().GetID();
        for(int i = 1; i < treeJson.Count; i++)
        {
            JToken currentObj = treeJson[i];
            if(currentObj.Value<short>("id") == id)
            {
                treeJson.RemoveAt(i);
                EventSystem.current.currentSelectedGameObject.transform.parent.GetComponent<BoxDisplay>().DestroyMe();
                SerializeTree();
                DialogueFileLoader.GetDialogueTree(treeID).RemoveDialogueBox(id);
                return;
            }
        }
    }

    private void CreateDialogueBox()
    {
        short id = -1;

        if (!short.TryParse(boxIDInput.text, out id) || TreeContainsID(id))
        {
            boxErrorOutput.text = "Invalid Dialogue Box ID.";
            return;
        }

        DialogueBox newBox = new DialogueBox(id);

        JsonWriter writer = treeJson.CreateWriter();
        writer.WriteStartObject();
        writer.WritePropertyName("id");
        writer.WriteValue(id);
        writer.WritePropertyName("entry_time");
        writer.WriteValue(2);
        writer.WritePropertyName("animation_id");
        writer.WriteValue(-1);
        writer.WritePropertyName("responses");
        writer.WriteStartArray();
        writer.WriteEndArray();
        writer.WritePropertyName("phrases");
        writer.WriteStartArray();
        writer.WriteEndArray();
        writer.WriteEndObject();

        SerializeTree();

        DialogueFileLoader.GetDialogueTree(treeID).AddDialogueBox(newBox);

        DisplayDialogueBox(newBox);

    }

    private bool TreeContainsID(short id)
    {
        for(int i = 1; i < treeJson.Count; i++)
        {
            if (treeJson[i].Value<short>("id") == id)
                return true;
        }
        return false;
    }



    private void BackButton()
    {
        string pageNum = panelTracker.Peek().name;

        if(pageNum == "Page2")
        {
            treeJson = null;
            treeID = -1;
            BoxDisplay[] boxes = GameObject.FindObjectsOfType<BoxDisplay>();
            for(int i = 0; i < boxes.Length; i++)
            {
                boxes[i].DestroyMe();
            }

            panelTracker.Pop().SetActive(false);
            panelTracker.Peek().SetActive(true);
        }
    }

    private void OpenDialogueBoxe()
    {
        short id = EventSystem.current.currentSelectedGameObject.transform.parent.GetComponent<BoxDisplay>().GetID();
        boxID = id;

        for(int i = 1; i < treeJson.Count; i++)
        {
            if(treeJson[i].Value<short>("id") == id)
            {
                boxJson = treeJson[i];
            }
        }
        if (boxJson == null)
            return;

        
    }

    private void SerializeTree()
    {
        StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + Path.DirectorySeparatorChar + "DialogueTrees" + Path.DirectorySeparatorChar + treeID + ".dlt", false);
        writer.Write(treeJson.ToString());
        writer.Close();
    }
}
