using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState: MonoBehaviour
{
    protected static PlayerStateMachine StateMachine { get; private set; }
    protected static PlayerInputManager PlayerInput { get; private set; }
    protected static PlayerSharedMovement PlayerMovement { get; private set; }

    private void Awake()
    {
        StateMachine = GetComponent<PlayerStateMachine>();
        PlayerInput = StateMachine.playerInput;
        PlayerMovement = StateMachine.playerMovement;

        //Debug.Log(StateMachine + " " + PlayerInput + " " + PlayerMovement);
    }
    public abstract void EnterState();
    public abstract void LogicUpdate();
    public abstract void ExitState();
}
