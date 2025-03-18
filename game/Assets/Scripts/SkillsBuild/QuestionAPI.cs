using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class QuestionAPIResponseAnswer
{
    public string correct;
    public string[] incorrect;
}

[Serializable]
public class QuestionAPIResponse
{
    public string question;
    public QuestionAPIResponseAnswer answer;
}

public class QuestionAPIRequest
{
    [SerializeField]
    private string requestUrl = "http://127.0.0.1:5000/ask";

    public string question;

    public QuestionAPIRequest(string question)
    {
        this.question = question;
    }

    public IEnumerator FakeSendRequest(Action<QuestionAPIResponse> onComplete, Action<string> onError)
    {
        Debug.Log("Simulating request...");

        yield return new WaitForSeconds(3.0f);

        QuestionAPIResponse dummyResponse = new QuestionAPIResponse
        {
            question = "This is a dummy response for the question: " + this.question,
            answer = new QuestionAPIResponseAnswer
            {
                correct = "This is the correct answer",
                incorrect = new string[] {
                            "First incorrect answer",
                        "Second incorrect answer",
                        "Third incorrect answer"
                }
            }
        };

        onComplete?.Invoke(dummyResponse);
    }

    public IEnumerator SendRequest(Action<QuestionAPIResponse> onComplete, Action<string> onError)
    {
        Debug.Log("Running Thing");

        // DEBUG: WAIT 5 SECONDS
        yield return new WaitForSeconds(5.0f);

        WWWForm form = new WWWForm();
        form.AddField("question", this.question);

        using (UnityWebRequest request = UnityWebRequest.Post(requestUrl, form))
        {
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke($"Error: {request.error}\nResponse: {request.downloadHandler.text}");
            }
            else
            {
                string response = request.downloadHandler.text;
                Debug.Log($"Response: {response}");
                var obj = JsonUtility.FromJson<QuestionAPIResponse>(response);
                onComplete?.Invoke(obj);
            }
        }
    }
}
