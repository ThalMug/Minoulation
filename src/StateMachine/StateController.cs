using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Minoulation.Player;
using Minoulation.StateMachine;

public partial class StateController : Node
{
	private static StateController _instance;
	[Export] private Node2D _sceneContainer;
	private List<CharacterBody2D> _characterBody2Ds;
	
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
	
	private List<IState> States = new();
	private int _index;
		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PlayerController playerController = GetNode<PlayerController>("PlayerController");
		if (playerController == null)
		{
			GD.PrintErr("PlayerController is null");
			return;
		}
		
		States.Add(new EnableControllerState(playerController, GetAllCharactersInScene()));
		States.Add(new ResolveInputsState());
	}

	public void StartStateSequences()
	{
		if (States?.Count > 0)
		{
			_index = 0;
			States[0].EnterState();
		}
		else
		{
			GD.PrintErr("States empty");
		}
	}

	public void GoToNextState()
	{
		if (!ShouldEndState())
		{
			_index++;
			States[_index % States.Count].EnterState();
		}
	}

	private bool ShouldEndState()
	{
		return false;
	}

	private List<Cat> GetAllCharactersInScene()
	{
		List<Cat> characters = new List<Cat>();
		foreach (Node child in _sceneContainer.GetChild(0).GetChildren())
		{
			if (child is Cat characterBody2D)
			{
				characters.Add(characterBody2D);
			}
		}
		
		return characters;
	}

	private bool istrue = false;
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (Input.IsActionPressed("MoveForward") && !istrue)
		{
			istrue = true;
			GD.PrintErr("Starting");
			StartStateSequences();			
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
