using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState: MonoBehaviour
{
    public abstract void EnterState(PlayerStateMachine playerStateMachine);
    public abstract void LogicUpdate();
    public abstract void ExitState();
}
