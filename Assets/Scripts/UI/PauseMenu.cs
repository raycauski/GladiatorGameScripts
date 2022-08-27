using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseMenu : MonoBehaviour
{
    // Use Scene Loader to load current Level - integrate save and load system later
    // GEt scene index from save file to open current scene level
   public void ExitMenu()
    {
        GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.previousState);
    }

    public void ReturnToMainMenu()
    {
        SceneLoader.Instance.ReturnToMain();
    }
    

}
