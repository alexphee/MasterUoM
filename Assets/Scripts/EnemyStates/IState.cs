using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter(Enemy parent); //prepares the state

    void Update();

    void Exit();
}
