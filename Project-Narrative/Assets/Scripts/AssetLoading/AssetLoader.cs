using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class AssetLoader
{
    private static Dictionary<string, Texture> bubbles = new Dictionary<string, Texture>();

    public static void LoadSprites()
    {
        Texture[] textures = Resources.LoadAll<Texture>("DialogueBubbles");

        foreach(Texture t in textures)
        {
            bubbles.Add(t.name.ToLower(), t);
        }
    }
    public static Texture GetBubble(string fileName)
    {
        if(bubbles.ContainsKey(fileName))
            return bubbles[fileName];
        throw new System.Exception("Unable to find texture " + fileName);
    }
}
