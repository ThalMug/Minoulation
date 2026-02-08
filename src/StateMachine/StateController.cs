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
				GD.PrintErr("instanceee");
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
		_instance = this;
		
		PlayerController playerController = GetNode<PlayerController>("PlayerController");
		if (playerController == null)
		{
			GD.PrintErr("PlayerController is null");
			return;
		}

		var characters = GetAllCharactersInScene();
		var houses = GetAllHousesInScene();
		States.Add(new EnableControllerState(playerController, characters, houses));
		States.Add(new ResolveInputsState(characters, houses));
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

	private List<House> GetAllHousesInScene()
	{
		List<House> houses = new List<House>();
		foreach (Node child in _sceneContainer.GetChild(0).GetChildren())
		{
			if (child is House area2D)
			{
				houses.Add(area2D);
			}
		}
		
		return houses;
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
