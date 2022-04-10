using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class AssetLoader
{
    private static Dictionary<string, Texture> bubbles = new Dictionary<string, Texture>();
    private static Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public static void LoadSprites()
    {
        Texture[] textures = Resources.LoadAll<Texture>("DialogueBubbles");

        foreach(Texture t in textures)
        {
            bubbles.Add(t.name.ToLower(), t);
        }
    }/*
    public static void LoadAudioClips()
    {
        string[] files = Directory.GetFiles(Application.streamingAssetsPath + "\\" + "Sounds");
        foreach (string f in files)
        {
            audioClips.Add(c.name.ToLower(), AudioClip.);
        }
    }*/
    public static Texture GetBubble(string fileName)
    {
        if(bubbles.ContainsKey(fileName))
            return bubbles[fileName];
        throw new System.Exception("Unable to find texture " + fileName);
    }/*
    public static AudioClip GetClip(string fileName)
    {
        if (audioClips.ContainsKey(fileName.ToLower()))
            return audioClips[fileName.ToLower()];
        throw new System.Exception("Unable to find Audio Clip " + fileName);
    }*/

    public static string[] GetBubbleNames()
    {
        string[] names = new string[bubbles.Count];
        bubbles.Keys.CopyTo(names, 0);
        return names;
    }
}
