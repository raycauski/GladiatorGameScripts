using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Melee : MonoBehaviour
{
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected int defense;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int recharge;
    [SerializeField]
    protected int energy;

    public PlayerHandsController playerHands;

    void Start()
    {
        playerHands = GameManager.Instance.Player.GetComponent<PlayerHandsController>();
    }

    public void Attack()
    {

    }

}
