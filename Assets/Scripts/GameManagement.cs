using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour, IDataPersistence
{
    public bool gameHasEnded;

    public string currentLevelName;

    private int _max;

    private int _buildIndex;

    public GameObject completeLevelUI;
    
    public GameObject failLevelUI;

    public GameObject gameScreenUI;

    public Transform topLevel;

    private void Start()
    {
        gameHasEnded = false;
        _max = 2;
        currentLevelName = "LEVEL 01";
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(sceneBuildIndex: 1);
    }

    public void LoadData(GameData data)
    {
        currentLevelName = data.levelName;
        _max = SceneManager.GetSceneByName(currentLevelName).buildIndex;
    }

    public void SaveData(ref GameData data)
    {
        currentLevelName = SceneManager.GetSceneByBuildIndex(_max).name;
        data.levelName = currentLevelName;
    }
    
    public void Go()
    {
        SceneManager.LoadSceneAsync(currentLevelName);
    }
    
    public void RestartScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    
    public void NextScene()
    {
        SceneManager.LoadSceneAsync(sceneBuildIndex: (SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ViewLeaderBoard()
    {
        SceneManager.LoadSceneAsync("LeaderBoard");
    }
    
    public void CompleteLevel()
    {
        gameHasEnded = true;
        gameScreenUI.SetActive(false);
        completeLevelUI.SetActive(true);
    }
    public void FailLevel()
    {
        gameHasEnded = true;
        gameScreenUI.SetActive(false);
        failLevelUI.SetActive(true);
    }
}