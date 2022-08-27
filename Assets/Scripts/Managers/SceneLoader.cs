using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private int currentSceneIndex;
    private int previousSceneIndex;
    public enum Scenes
    {
        MainMenu, //0
        Level1,   //1
    }

    #region Singleton
    public static SceneLoader Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
    }
    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
    #endregion
    public void ChangeScene(Scenes nextScene)
    {
        // Opens given scene in scene index, change later to load current level. Index 0 = main menu
        SceneManager.LoadScene((int)nextScene);
    }
    private void OnSceneChange(Scene currentScene, Scene nextScene)
    {
        previousSceneIndex = currentScene.buildIndex;
        currentSceneIndex = nextScene.buildIndex;
        if (nextScene.buildIndex != 0)
        {
            EventManager.OnChangedScenes();
        }
        
        /*
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            EventManager.OnChangedScenes();
        }
        */
    }
    public bool InMainMenu() {
        
        if ((int)Scenes.MainMenu == currentSceneIndex)
        {
            
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ReturnToMain()
    {
        Debug.Log("Loading Main Menu..");
        SceneManager.LoadScene((int)Scenes.MainMenu);
        GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.mainMenuState);
    }
    
    public int GetCurrentScene()
    {
        return currentSceneIndex;
    }

}
