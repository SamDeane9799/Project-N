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

    private float timeActive;
    private float currentTime;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        myText = Instantiate<GameObject>(textPrefab, transform).GetComponent<TextMeshPro>();
        myText.transform.localPosition = new Vector3(0, .5f, 0);
        myText.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
        myText.rectTransform.sizeDelta = new Vector2(10, 10);
        myText.color = textColor;
    }

    // Update is called once per frame
    void Update()
    {
        timeActive += Time.deltaTime;
        if (myText.text != textToSet && timeActive % .5f == 0)
        {
            myText.text = textToSet.Substring(0, (int)(timeActive * 2));
        }
    }

    public void SetTextPrefab(GameObject prefab)
    {
        textPrefab = prefab;
    }

    public void SetText(string text)
    {
        textToSet = text;
    }
    public void SetTextColor(Vector3 color)
    {
        textColor = new Color(color.x, color.y, color.z);
        if (myText != null)
            myText.color = textColor;

    }

    public void SetTimer(float time)
    {
        timer = time;
    }

    public void ResetBubble()
    {
        textToSet = "";
        myText.text = "";
        timer = 0;
        currentTime = 0;
        timeActive = 0;
    }
}
