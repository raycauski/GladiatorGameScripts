using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _Instance;
    public static GameManager Instance 
    {
        get

        {
            if (!_Instance)
            {
                // NOTE: read docs to see directory requirements for Resources.Load!
                var prefab = Resources.Load<GameObject>("GameManager");
                // create the prefab in your scene
                var inScene = Instantiate<GameObject>(prefab);
                // try find the instance inside the prefab
                _Instance = inScene.GetComponentInChildren<GameManager>();
                // guess there isn't one, add one
                if (!_Instance) _Instance = inScene.AddComponent<GameManager>();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.transform.root.gameObject);
            }
            return _Instance;
        }
    }
    #endregion
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject pauseMenuObject;
    //[SerializeField] private GameObject playerInventoryObject;
    public GameObject PauseMenu { get; private set; }
    public GameObject Player { get; private set; }
    private Transform spawnPoint;
    public PlayerInventory PlayerInventory { get; private set; }

    
    
    private void Start()
    {
        if (!SceneLoader.Instance.InMainMenu())
        {
            FindSpawnPoint();
            SpawnPauseMenu();
            SpawnPlayer();
        }
        //PlayerInventory = playerInventoryObject.GetComponent<PlayerInventory>();
        PlayerInventory = GetComponent<PlayerInventory>();
    }
    private void OnEnable()
    {
        EventManager.OnChangedScenes += FindSpawnPoint;
        EventManager.OnChangedScenes += SpawnPlayer;
        EventManager.OnChangedScenes += SpawnPauseMenu;
    }
    private void OnDisable()
    {
        EventManager.OnChangedScenes += FindSpawnPoint;
        EventManager.OnChangedScenes -= SpawnPlayer;
        EventManager.OnChangedScenes -= SpawnPauseMenu;
    }



    private void SpawnPlayer()
    {
        // null check if reference exists in scene first
        Player = Instantiate(playerObject, spawnPoint.transform.position, spawnPoint.rotation).transform.GetChild(0).gameObject; // finds player object which spawned in
        GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.playerGameplayState);
    }
   
    private void SpawnPauseMenu()
    {
        // null check if reference exists in scene first
        PauseMenu = Instantiate(pauseMenuObject, spawnPoint);
    }
    private void FindSpawnPoint()
    {
        if (spawnPoint == null)
        {
            spawnPoint = GameObject.FindWithTag("Respawn").transform;
        }
    }

}
