using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBubbleDisplay : MonoBehaviour
{
    private GameObject textPrefab;
    private TextMeshPro myText;
    private DialogueBubble myInfo;
    private Vector3 nextPosition;
    private AudioSource audioPlayer;

    private float timeActive;
    private float currentTime;

    private bool inView = false;
    // Start is called before the first frame update
    void Start()
    {
        myText = Instantiate<GameObject>(textPrefab, transform).GetComponent<TextMeshPro>();
        myText.transform.localPosition = new Vector3(0, .5f, 0);
        myText.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
        myText.rectTransform.sizeDelta = new Vector2(10, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inView && myInfo != null)
        {
            currentTime += Time.deltaTime;
            float lerpValue = currentTime / myInfo.entryTime;
            if(!(myInfo is PlayerResponse))
            {
                this.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), nextPosition, lerpValue);
                this.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), myInfo.scale, lerpValue);
                int stringLength = (int)(lerpValue * (float)myInfo.text.Length);
                myText.text = myInfo.text.Substring(0, stringLength);

            }
            if (currentTime >= myInfo.entryTime)
            {
                this.transform.localScale = myInfo.scale;
                if (myInfo is PlayerResponse)
                {
                    this.transform.localPosition = nextPosition;
                    this.transform.localRotation = Quaternion.Euler(myInfo.rotation.x - 90, myInfo.rotation.y + 270, myInfo.rotation.z + 90);
                    this.transform.localScale = new Vector3(transform.localScale.x / 5f, transform.localScale.y / 5f, transform.localScale.z / 5f);
                    myText.text = myInfo.GetLocation() + 1 + ". " + myInfo.text;
                }
                else
                {
                    this.transform.localRotation = Quaternion.Euler(myInfo.rotation.x - 90, myInfo.rotation.y, myInfo.rotation.z + 180);
                }
                audioPlayer.Play();
                inView = true;
            }
            return;
        }

        timeActive += Time.deltaTime;
    }

    public void SetTextPrefab(GameObject prefab)
    {
        textPrefab = prefab;
    }

    public void SetInfo(DialogueBubble info, Vector3 posToAdd, AudioClip clip)
    {
        myInfo = info;
        nextPosition = posToAdd;
        this.transform.localScale = new Vector3(0, 0, 0);
        this.transform.localPosition = new Vector3(0, 0, 0);
        if (!gameObject.TryGetComponent<AudioSource>(out audioPlayer))
            audioPlayer = gameObject.AddComponent<AudioSource>();
        audioPlayer.clip = clip;
        audioPlayer.volume = .5f;

        if(info is PlayerResponse)
        {
            this.transform.localRotation = Quaternion.Euler(myInfo.rotation.x - 90, myInfo.rotation.y + 270, myInfo.rotation.z + 90);
            myText.text = myInfo.text;
        }
        else
        {
            this.transform.localRotation = Quaternion.Euler(myInfo.rotation.x - 90, myInfo.rotation.y, myInfo.rotation.z + 180);
            myText.text = "";
        }

        myText.color = new Color(myInfo.textColor.x, myInfo.textColor.y, myInfo.textColor.z);
        Material mat = gameObject.GetComponent<MeshRenderer>().material;
        mat.color = new Color(myInfo.backgroundColor.x, myInfo.backgroundColor.y, myInfo.backgroundColor.z);
        mat.mainTexture = AssetLoader.GetBubble(myInfo.backgroundTexture);
    }

    public bool IsInView()
    {
        return inView;
    }

    public float GetEntryTime()
    {
        return myInfo.entryTime;
    }

    public void ResetBubble()
    {
        myText.text = "";
        currentTime = 0;
        timeActive = 0;
        myInfo = null;
        inView = false;
        transform.parent = null;
        transform.localPosition = new Vector3(0, -1000, 0);
    }
}
