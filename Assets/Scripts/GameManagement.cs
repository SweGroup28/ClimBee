using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour, IDataPersistence
{
    private bool _gameHasEnded;

    private string _currentLevelName;

    public GameObject completeLevelUI;
    
    public GameObject failLevelUI;

    public GameObject levelNo;

    public void MainMenu()
    {
        _currentLevelName = SceneManager.GetActiveScene().name;
        SceneManager.LoadSceneAsync(sceneBuildIndex: 0);
    }

    public void LoadData(GameData data)
    {
        _currentLevelName = data.levelName;
    }

    public void SaveData(ref GameData data)
    {
        Debug.Log("Current Level: " + _currentLevelName);
        data.levelName = _currentLevelName;
        Debug.Log("Data: " + data.levelName);
    }
    
    public void Go()
    {
        SceneManager.LoadSceneAsync(_currentLevelName);
    }
    
    public void RestartScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    
    public void NextScene()
    {
        Debug.Log("Current Level Name: " + SceneManager.GetActiveScene().name);
        Debug.Log("Current Build Index: " + SceneManager.GetActiveScene().buildIndex);
        _currentLevelName = SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).name;
        Debug.Log("Current Level Name After: " + _currentLevelName);
        SceneManager.LoadSceneAsync(_currentLevelName);
    }
    
    public void CompleteLevel()
    {
        if (_gameHasEnded == false)
        {
            _gameHasEnded = true;
            completeLevelUI.SetActive(true);
        }
    }
    public void FailLevel()
    {
        if (_gameHasEnded == false)
        {
            _gameHasEnded = true;
            failLevelUI.SetActive(true);
        }
    }
}