using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace Minoulation.StateMachine;

public class ResolveInputsState : IState
{
    private List<Cat> _characters;
    private List<House> _houses;

    public ResolveInputsState(List<Cat> characters, List<House> houses)
    {
        _characters = characters;
        _houses = houses;
    }
    
    public void EnterState()
    {
        
        PlayAllActions();
    }

    public bool IsWin()
    {
        foreach (var house in _houses)
        {
            if (!house.getIsGoodCat())
            {
                return false;
            }
        }
        
        return true;
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
                CheckCatsPositions(i);
            }

            await Task.Delay(TimeSpan.FromSeconds(.5f));
        }
        
        if (IsWin())
        {
            GD.Print("Allons y les ami.es c'est GAGNÉ");
        }
        else
        {
             GD.Print("Chat pleure OUIN OUIN");
        }
        ResetPositions();
        ResetLists();
        LeaveState();
    }

    private void CheckCatsPositions(int actionIndex)
    {
        // Check all positions to see if cats collided
        var catPositionsGroup = _characters
            .GroupBy(cat => cat.Position)
            .Where(gb => gb.Count() > 1);

        foreach (var cats in catPositionsGroup)
        {
            foreach (var cat in cats)
            {
                cat.EnableFight();
            }
        }
        
        // Check if two cats are facing each other
        for (int i = 0; i < _characters.Count; i++)
        {
            for (int j = i + 1; j < _characters.Count; j++)
            {
                Cat catA = _characters[i];
                Cat catB = _characters[j];

                Vector2 nextPosA = PredictNextPosition(catA, actionIndex);
                Vector2 nextPosB = PredictNextPosition(catB, actionIndex);

                if (nextPosA == catB.Position && nextPosB == catA.Position)
                {
                    catA.ResetListActions();
                }
            }
        }
    }

    private Vector2 PredictNextPosition(Cat cat, int actionIndex)
    {
        return cat.Position + cat.GetDirectionOfAction(actionIndex) * 48;
    }
    
    public void LeaveState()
    {
        StateController.Instance.GoToNextState(!IsWin());
    }
}