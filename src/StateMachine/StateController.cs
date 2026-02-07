using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Minoulation.StateMachine;

public partial class StateController : Node
{
	private static StateController _instance;

	public static StateController Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new StateController();
			}

			return _instance;
		}
	}
	
	public List<IState> States;
	private int _index;
		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void StartStateSequences()
	{
		if (States.Count > 0)
		{
			_index = 0;
			States[0].EnterState();
		}
	}

	public void GoToNextState()
	{
		if (ShouldEndState())
		{
			_index++;
			States[_index % States.Count].EnterState();
		}
	}

	private bool ShouldEndState()
	{
		return true;
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
