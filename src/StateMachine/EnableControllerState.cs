using System;
using System.Collections.Generic;
using Godot;
using Minoulation.Player;

namespace Minoulation.StateMachine;

public class EnableControllerState : IState
{
    private PlayerController _playerController;
    private List<Cat> _characters;
    public EnableControllerState(PlayerController playerController, List<Cat> catCharacters)
    {
        _playerController = playerController;
        _characters = catCharacters;
    }
    
    public void EnterState()
    {
        _playerController.EnablePlayerController(_characters);
    }

    public void LeaveState()
    {
        StateController.Instance.GoToNextState();
    }
}