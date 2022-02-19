using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;

public static class DialogueFileLoader
{
    private static Dictionary<short, DialogueTree> dialogueTrees = new Dictionary<short, DialogueTree>();


    public static DialogueTree GetDialogueTree(short ID)
    {
        return dialogueTrees[ID];
    }

    public static void LoadDialogueTrees(string sceneName)
    {
        string resourcePath = Application.streamingAssetsPath + Path.DirectorySeparatorChar + sceneName;
        string[] resourceFiles = Directory.GetFiles(resourcePath);
        foreach (string path in resourceFiles)
        {
            if (Path.GetExtension(path) != ".dlt")
                continue;
            StreamReader streamReader = new StreamReader(path);
            string jsonString = streamReader.ReadToEnd();

            JArray treeObject = JArray.Parse(jsonString);
            Dictionary<short, DialogueBox> dialogueBoxes = new Dictionary<short, DialogueBox>();
            for (int i = 1; i < treeObject.Count; i++)
            {
                JObject boxObject = (JObject)treeObject[i];

                JArray phrases = boxObject.Value<JArray>("phrases");
                List<Phrase> boxPhrases = new List<Phrase>();
                JArray responses = boxObject.Value<JArray>("responses");
                PlayerResponse[] playerResponses = new PlayerResponse[responses.Count];

                for (int j = 0; j < phrases.Count; j++)
                {
                    JArray phraseObject = phrases.Value<JArray>(j);
                    List<DialogueBubble> dialogueBubbles = new List<DialogueBubble>();
                    for (int b = 0; b < phraseObject.Count; b++)
                    {
                        JObject bubbleObject = (JObject)phraseObject[b];
                        dialogueBubbles.Add(new DialogueBubble(bubbleObject.Value<string>("text"), bubbleObject.Value<string>("background_texture"), bubbleObject.Value<int>("background_animation"), ParseString(bubbleObject.Value<string>("background_color")), ParseString(bubbleObject.Value<string>("text_color")), bubbleObject.Value<int>("text_animation"),
                            bubbleObject.Value<int>("location"), ParseString(bubbleObject.Value<string>("scale")), ParseString(bubbleObject.Value<string>("rotation")), bubbleObject.Value<float>("entry_time")));
                    }
                    boxPhrases.Add(new Phrase(dialogueBubbles));
                }

                for (int pr = 0; pr < playerResponses.Length; pr++)
                {
                    JObject responseObj = (JObject)responses[pr];
                    short childID = -1;
                    if(responseObj.ContainsKey("child"))
                        childID = responseObj.Value<short>("child");
                    playerResponses[pr] = new PlayerResponse(responseObj.Value<int>("priority"), (Trait)Enum.Parse(typeof(Trait), responseObj.Value<string>("trait").ToUpper()), StringToPair(responseObj.Value<string>("is_interrupt")),
                        (ResponseType)Enum.Parse(typeof(ResponseType), responseObj.Value<string>("type").ToUpper()), childID, responseObj.Value<string>("text"), responseObj.Value<string>("background_texture"),
                        responseObj.Value<int>("background_animation"), ParseString(responseObj.Value<string>("background_color")), ParseString(responseObj.Value<string>("text_color")), responseObj.Value<int>("text_animation"), responseObj.Value<int>("location"), ParseString(responseObj.Value<string>("scale")),
                        ParseString(responseObj.Value<string>("rotation")), responseObj.Value<float>("entry_time"));
                }


                dialogueBoxes.Add(boxObject.Value<short>("id"), new DialogueBox(boxObject.Value<short>("id"), boxPhrases, playerResponses, boxObject.Value<int>("animation")));
            }
            DialogueTree newTree = new DialogueTree(dialogueBoxes, treeObject[0].Value<string>("description"));
            dialogueTrees[treeObject[0].Value<short>("id")] = newTree;
        }
    }


    private static KeyValuePair<bool, int> StringToPair(string toParse)
    {
        string[] parts = toParse.Split(',');
        return new KeyValuePair<bool, int>(bool.Parse(parts[0]), int.Parse(parts[1]));
    }
    private static Vector3 ParseString(string toVector)
    {
        string[] parts = toVector.Split(',');
        return new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
    }
}
