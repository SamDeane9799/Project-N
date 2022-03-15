using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class TreeDisplay : MonoBehaviour
{
    private string[] textToSet = new string[3];

    public Text[] textBoxes;
    [SerializeField]
    private Button deleteButton;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < textBoxes.Length; i++)
        {
            textBoxes[i].text = textToSet[i];
        }
        transform.localScale = new Vector3(1, 1, 1);

        deleteButton.onClick.AddListener(DeleteButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(string id, string desc, string levels)
    {
        textToSet[0] = id;
        textToSet[1] = desc;
        textToSet[2] = levels;
    }

    private void DeleteButton()
    {
        File.Delete(Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + "DialogueTrees" + Path.AltDirectorySeparatorChar + textToSet[0] + ".dlt");
        DialogueFileLoader.RemoveTree(short.Parse(textToSet[0]));
        Destroy(gameObject);
    }
}
