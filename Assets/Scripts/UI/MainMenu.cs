using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
    // Use Scene Loader to load current Level - integrate save and load system later
    // GEt scene index from save file to open current scene level
   public void StartGame()
    {
        // Scene loader - load current level
        SceneLoader sceneLoader = SceneLoader.Instance;
        sceneLoader.ChangeScene(SceneLoader.Scenes.Level1);  //// make level current level of save file;

    }
    public void QuitGame()
    {
        //CLoses app and quits game
        Debug.Log("Quit game");
        Application.Quit();
    }

}
