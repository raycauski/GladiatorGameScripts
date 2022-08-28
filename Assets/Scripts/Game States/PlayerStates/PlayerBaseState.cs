using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateMachine playerStateMachine);
    public abstract void LogicUpdate(PlayerStateMachine playerStateMachine);
    public abstract void ExitState(PlayerStateMachine playerStateMachine);
}
