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
        GD.PrintErr("entering state controller");
        _playerController.EnablePlayerController(_characters);
        _playerController.OnLaunch += LeaveState;
    }

    public void LeaveState()
    {
        GD.PrintErr("leaving state controller");
        _playerController.DisablePlayerController();
        _playerController.OnLaunch -= LeaveState;
        StateController.Instance.GoToNextState();
    }
}