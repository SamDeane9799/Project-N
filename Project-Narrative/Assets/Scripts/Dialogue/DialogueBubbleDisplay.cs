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
    private Vector3 initialForward;
    private AudioSource audioPlayer;

    private float timeActive;
    private float animationTimer;
    private float currentTime;
    private float sizeScale;
    private float textTimer;

    private int textIndex;

    private bool inView = false;
    private bool isResponse;
    private bool rotatingClockwise;

    private float[] sizes = { 1.0f, .7f, 0.4f };
    // Start is called before the first frame update
    void Start()
    {
        myText = Instantiate<GameObject>(textPrefab, transform).GetComponent<TextMeshPro>();
        myText.transform.localPosition = new Vector3(0, .5f, 0);
        myText.transform.localRotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (isResponse && inView)
            ResponseAnimate();

        textTimer += Time.deltaTime;
        if (myInfo != null)
        {
            if (textIndex < myInfo.text.Length && textTimer >= 0.05f && !isResponse)
            {
                textIndex++;
                textTimer = 0;
                myText.text = myInfo.text.Substring(0, textIndex);
            }
            if (!inView)
            {
                currentTime += Time.deltaTime;
                float lerpValue = currentTime / myInfo.entryTime;

                if (!isResponse)
                {
                    this.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), nextPosition, lerpValue * 2);
                    this.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), myInfo.scale * sizeScale, lerpValue * 2);
                    myText.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), myInfo.scale * (1 - sizeScale + 1), lerpValue * 2);

                }
                if (currentTime >= myInfo.entryTime)
                {
                    this.transform.localScale = myInfo.scale * sizeScale;
                    myText.transform.localScale = myInfo.scale * (1 - sizeScale + 1);
                    if (myInfo is PlayerResponse)
                    {
                        this.transform.localPosition = nextPosition;
                        this.transform.localRotation = Quaternion.Euler(myInfo.rotation.x - 90, myInfo.rotation.y + 270, myInfo.rotation.z + 90);
                        this.transform.localScale = new Vector3(transform.localScale.x / 5f, transform.localScale.y / 5f, transform.localScale.z / 5f);
                        myText.text = myInfo.GetLocation() + 1 + ". " + myInfo.text;
                        audioPlayer.Play();
                    }
                    else
                    {
                        this.transform.localRotation = Quaternion.Euler(myInfo.rotation.x - 90, myInfo.rotation.y, myInfo.rotation.z + 180);
                    }

                    initialForward = -transform.forward;
                    inView = true;
                }
                return;
            }
        }
    }

    public void SetTextPrefab(GameObject prefab)
    {
        textPrefab = prefab;
    }

    public void SetInfo(DialogueBubble info, Vector3 posToAdd, AudioClip clip)
    {
        isResponse = info is PlayerResponse;
        textIndex = 0;
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
            myText.fontStyle = FontStyles.Italic;
        }
        else
        {
            this.transform.localRotation = Quaternion.Euler(myInfo.rotation.x - 90, myInfo.rotation.y, myInfo.rotation.z + 180);
            myText.text = "";
            audioPlayer.Play();
        }

        myText.color = new Color(myInfo.textColor.x, myInfo.textColor.y, myInfo.textColor.z);
        Material mat = gameObject.GetComponent<MeshRenderer>().material;
        mat.color = new Color(myInfo.backgroundColor.x, myInfo.backgroundColor.y, myInfo.backgroundColor.z, 1);
        mat.mainTexture = AssetLoader.GetBubble(myInfo.backgroundTexture);

        int sizeIndex = 0;/*
        if (myInfo.text.Length <= 20)
            sizeIndex = 1;
        else if (myInfo.text.Length <= 10)
            sizeIndex = 2;*/

        sizeScale = sizes[sizeIndex];

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
        myInfo = null;
        inView = false;
        transform.parent = null;
        transform.localPosition = new Vector3(0, -1000, 0);
    }

    void ResponseAnimate()
    {
        animationTimer += Time.deltaTime;
        if(animationTimer >= .75f)
        {
            animationTimer = 0;
            rotatingClockwise = !rotatingClockwise;
        }
        if (rotatingClockwise)
            transform.Rotate(initialForward, Time.deltaTime * 5.0f);
        else
            transform.Rotate(initialForward, -Time.deltaTime * 5.0f);
    }
}
