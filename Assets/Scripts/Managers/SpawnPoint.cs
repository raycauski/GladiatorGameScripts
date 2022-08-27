using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private float radius;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        //Calls Reference to game manager to ensure it spawns in when needed, and sends reference to current spawn point
        gameManager = GameManager.Instance;
    }
  
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, radius);
        Gizmos.DrawRay(transform.position, transform.forward * 2);
    }
}
