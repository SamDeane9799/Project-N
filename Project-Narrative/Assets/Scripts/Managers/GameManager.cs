using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private static Player playerRef;

    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static Vector3 GetPlayerPosition()
    {
        return playerRef.transform.position;
    }

    private static void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "SampleScene")
        {
            playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            DialogueFileLoader.LoadDialogueTrees(scene.name);
        }
    }
}
