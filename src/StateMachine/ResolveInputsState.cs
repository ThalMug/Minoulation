using Godot;

namespace Minoulation.StateMachine;

public class ResolveInputsState : IState
{
    public void EnterState()
    {
        // Get all characters in the world
        // Dezoom camera to see all players 
        // Play all states
        LeaveState();
    }

    public void LeaveState()
    {
        StateController.Instance.GoToNextState();
    }
}