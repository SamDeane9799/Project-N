using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class AssetLoader
{
    private static Dictionary<string, Sprite> bubbles = new Dictionary<string, Sprite>();

    public static void LoadSprites()
    {
        string path = Application.streamingAssetsPath + "/" + "Resources" + "/" + "DialogueBubbles";
        string[] files = Directory.GetFiles(path);
        foreach(string file in files)
        {
            if (Path.GetExtension(file) == ".png")
            {
                LoadSprite(Path.GetFileNameWithoutExtension(file));
            }
        }
    }

    private static Sprite LoadSprite(string fileName)
    {
        string newPath = "DialogueBubbles/" + fileName;
        Sprite spriteToAdd = Resources.Load<Sprite>(newPath);
        bubbles.Add(fileName, spriteToAdd);
        return spriteToAdd;
    }

    public static Sprite GetBubble(string fileName)
    {
        if (!bubbles.ContainsKey(fileName))
            return LoadSprite(fileName);
        return bubbles[fileName];
    }
}
