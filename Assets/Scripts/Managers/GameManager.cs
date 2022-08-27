using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private GameObject player;
    [SerializeField] private Transform spawnPoint;

    private GameObject pauseMenuObject;
    private GameObject pauseMenu;
    private void Start()
    {
        LoadPlayer();
        LoadPauseMenu();
        if (!SceneLoader.Instance.InMainMenu())
        {
            FindSpawnPoint();
            SpawnPauseMenu();
            SpawnPlayer();
        }
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
    private void LoadPlayer()
    {
        playerObject = Resources.Load<GameObject>("PlayerFPS");
    }
    private void SpawnPlayer()
    {
        // null check if reference exists in scene first
        player = Instantiate(playerObject, spawnPoint.transform.position, spawnPoint.rotation).transform.GetChild(0).gameObject; // finds player object which spawned in
        GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.playerGameplayState);
    }
    private void LoadPauseMenu()
    {
        pauseMenuObject = Resources.Load<GameObject>("PauseMenu");
    }
    private void SpawnPauseMenu()
    {
        // null check if reference exists in scene first
        pauseMenu = Instantiate(pauseMenuObject, spawnPoint);
    }
    private void FindSpawnPoint()
    {
        if (spawnPoint == null)
        {
            spawnPoint = GameObject.FindWithTag("Respawn").transform;
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public GameObject GetPauseMenu()
    {
        return pauseMenu;
    }
}
