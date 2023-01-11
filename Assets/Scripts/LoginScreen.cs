using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class LoginScreen : MonoBehaviour
{
    public TextMeshProUGUI username;
    public GameManagement gameManagement;

    public void SubmitOnClick()
    {
        StartCoroutine(HandleSubmitRequest());
    }

    private IEnumerator HandleSubmitRequest()
    {
        byte[] auth = System.Text.Encoding.UTF8.GetBytes(username.text);
        const string url = "https://europe-west1-thermal-origin-372310.cloudfunctions.net/GetRequest";
        UnityWebRequest www = UnityWebRequest.Put(url, auth);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("cloud: " + www.downloadHandler.text);
            string check = "Hello " + username.text + '!';
            if (www.downloadHandler.text == check)
            {
                gameManagement.MainMenu();
            }
            else
            {
                gameManagement.ViewLeaderBoard();
            }
        }
    }

    private void GetAnswer() 
    {
        UnityWebRequest wwwAnswer = UnityWebRequest.Get("https://europe-west1-thermal-origin-372310.cloudfunctions.net/GetRequest");
    }
}