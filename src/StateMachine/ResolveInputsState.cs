using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace Minoulation.StateMachine;

public class ResolveInputsState : IState
{
    private List<Cat> _characters;

    public ResolveInputsState(List<Cat> characters)
    {
        _characters = characters;
    }
    
    public void EnterState()
    {
        
        PlayAllActions();
    }

    private void ResetPositions()
    {
        foreach (Cat cat in _characters)
        {
            cat.ResetPosition();
        }
    }

    private void ResetLists()
    {
        foreach (Cat cat in _characters)
        {
            cat.ResetListActions();
        }
    }

    private async Task PlayAllActions()
    {
        ResetPositions();
        await Task.Delay(TimeSpan.FromSeconds(.5f));
        
        
        int maxActions = _characters.Max(c => c.GetActionsNumber());
        for (int i = 0; i < maxActions; i++)
        {
            foreach (Cat cat in _characters)
            {
                cat.PlayAction(i);
                cat.IsCollidingWithCat();
            }

            await Task.Delay(TimeSpan.FromSeconds(.5f));
        }
        
        ResetPositions();
        ResetLists();
        LeaveState();
    }

    public void LeaveState()
    {
        StateController.Instance.GoToNextState();
    }
}