using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;

public static class DialogueFileLoader
{
    private static Dictionary<short, DialogueBox> dialogueBoxes;

    public static DialogueBox GetDialogueBox(short ID)
    {
        return dialogueBoxes[ID];
    }


    public static void LoadDialogueTree(string path)
    {
        StreamReader streamReader = new StreamReader(path);
        string jsonString = streamReader.ReadToEnd();

        JArray treeObject = JArray.Parse(jsonString);
        Dictionary<short, DialogueBox> dialogueBoxes = new Dictionary<short, DialogueBox>();
        for(int i = 1; i < treeObject.Count; i++)
        {
            JObject boxObject = (JObject)treeObject[i];

            JArray phrases = boxObject.Value<JArray>("phrases");
            Queue<Phrase> boxPhrases = new Queue<Phrase>();
            JArray responses = boxObject.Value<JArray>("responses");
            PlayerResponse[] playerResponses = new PlayerResponse[responses.Count];

            for(int j = 0; j < phrases.Count; j++)
            {
                JArray phraseObject = phrases.Value<JArray>(j);
                Queue<DialogueBubble> dialogueBubbles = new Queue<DialogueBubble>();
                for(int b = 0; b < phraseObject.Count; b++)
                {
                    JObject bubbleObject = (JObject)phraseObject[b];
                    dialogueBubbles.Enqueue(new DialogueBubble(bubbleObject.Value<string>("text"), bubbleObject.Value<int>("background_texture"), bubbleObject.Value<int>("background_animation"), bubbleObject.Value<Color>("background_color"), bubbleObject.Value<int>("text_animation"),
                        bubbleObject.Value<int>("location"), bubbleObject.Value<Vector3>("scale"), bubbleObject.Value<Vector3>("rotation"), bubbleObject.Value<float>("entry_time")));
                }
                boxPhrases.Enqueue(new Phrase(dialogueBubbles));
            }

            for(int pr = 0; pr < playerResponses.Length; pr++)
            {
                JObject responseObj = (JObject)responses[pr];
                playerResponses[pr] = new PlayerResponse(responseObj.Value<int>("priority"), responseObj.Value<Trait>("trait"), responseObj.Value<KeyValuePair<bool, int>>("is_interrupt"),
                    responseObj.Value<ResponseType>("type"), responseObj.Value<int>("child"), responseObj.Value<string>("text"), responseObj.Value<int>("background_texture"),
                    responseObj.Value<int>("background_animation"), responseObj.Value<Color>("background_color"), responseObj.Value<int>("text_animation"), responseObj.Value<int>("location"), responseObj.Value<Vector3>("scale"),
                    responseObj.Value<Vector3>("rotation"), responseObj.Value<float>("entry_time"));
            }


            dialogueBoxes.Add(boxObject.Value<short>("id"), new DialogueBox(boxObject.Value<short>("id"), boxPhrases, playerResponses, boxObject.Value<int>("animation")));
        }
        DialogueTree newTree = new DialogueTree(dialogueBoxes);
    }
}
