using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class LoginScreen : MonoBehaviour
{
    public TextMeshProUGUI username;
    public GameManagement gameManagement;
    public GameObject panel;
    public string highScore;

    public void SubmitOnClick()
    {
        StartCoroutine(HandleSubmitRequest());
    }

    private IEnumerator HandleSubmitRequest()
    {
        var toSend = username.text;
        byte[] auth = System.Text.Encoding.UTF8.GetBytes(toSend);
        Debug.Log(toSend);
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
            var user = www.downloadHandler.text.Split();
            if (user[2] != "True")
            {
                highScore = "Level 0" + user[1];
                gameManagement.currentLevelName = highScore;
                gameManagement.MainMenu();
            }
            else
            {
                panel.SetActive(true);
                Invoke(nameof(ClosePanel),2f);
            }
        }
    }

    private void ClosePanel()
    {
        panel.SetActive(false);
    }
}