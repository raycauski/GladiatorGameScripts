using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateMachine : MonoBehaviour
{
    [SerializeField] public GameBaseState currentState { get; private set; }
    public GameBaseState previousState { get; private set; }

// Create all Game States
    public GameBaseState mainMenuState = new MainMenuState();
    public GameBaseState playerGameplayState = new PlayerGameplayState();
    public GameBaseState pauseMenuState = new PauseMenuState();
    public GameBaseState inventoryState = new InventoryState();
    public GameBaseState dialogState = new DialogState();

    // Create all Input Managers for each State
    public MenuInputManager menuInputManager { get; private set; }
    public PlayerInputManager playerInputManager { get; private set; }
    public PauseInputManager pauseInputManager { get; private set; }

    #region Singleton
    public static GameStateMachine Instance { get; private set; }
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
    }
    #endregion
    private void Start()
    {
        // Creates input managers for each state
        CreateInputManagers();
        //Choose starting state
        if (SceneLoader.Instance.InMainMenu())
        {
            currentState = mainMenuState;
            mainMenuState.EnterState(this);
        }
        else
        {
            currentState = playerGameplayState;
            playerGameplayState.EnterState(this);
        }
    }

    private void Update()
    {
        //Update logic for current state
        currentState.LogicUpdate(this);
    }

    public void ChangeState(GameBaseState newState)
    {

        //transition between game states
        if (newState == currentState || currentState == null)
        {
            return;
        }
        previousState = currentState;
        currentState.ExitState(this);
        currentState = newState;
        Debug.Log(previousState + " -> " + currentState);
        newState.EnterState(this);
    }

    private void CreateInputManagers()
    {
        //Creates all disabled input managers to be activated one at a time
        menuInputManager = gameObject.AddComponent<MenuInputManager>();
        menuInputManager.enabled = false;
        playerInputManager = gameObject.AddComponent<PlayerInputManager>();
        playerInputManager.enabled = false;
        pauseInputManager = gameObject.AddComponent<PauseInputManager>();
        pauseInputManager.enabled = false;
    }
}
