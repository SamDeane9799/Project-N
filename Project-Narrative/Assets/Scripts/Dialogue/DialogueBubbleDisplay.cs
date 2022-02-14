using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBubbleDisplay : MonoBehaviour
{
    private GameObject textPrefab;
    private TextMeshPro myText;
    private string textToSet;
    private Color textColor;
    // Start is called before the first frame update
    void Start()
    {
        myText = Instantiate<GameObject>(textPrefab, transform).GetComponent<TextMeshPro>();
        myText.transform.localPosition = new Vector3(0, .5f, 0);
        myText.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
        myText.rectTransform.sizeDelta = new Vector2(10, 10);
        myText.text = textToSet;
        myText.color = textColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTextPrefab(GameObject prefab)
    {
        textPrefab = prefab;
    }

    public void SetText(string text)
    {
        textToSet = text;
        if (myText != null)
            myText.text = textToSet;
    }
    public void SetTextColor(Vector3 color)
    {
        textColor = new Color(color.x, color.y, color.z);
        if (myText != null)
            myText.color = textColor;

    }
}
