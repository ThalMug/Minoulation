using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minoulation.Player;
using Minoulation.StateMachine;

public partial class StateController : Node
{
	private static StateController _instance;
	[Export] private Node2D _sceneContainer;
	[Export] private Godot.Collections.Array<PackedScene> _levelScenes;
	private int _currentLevelIndex = 0;
	private List<CharacterBody2D> _characterBody2Ds;
	public Action OnLevelChanged;
	
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
		SetStatesAndScene();
		StartStateSequences();
	}

	private async void SetStatesAndScene()
	{
		if (!await GoToNextLevelOrQuit())
		{
			return;
		}
		
		States.Clear();
		
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
		
		StartStateSequences();
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

	public void GoToNextState(bool shouldLoop = true)
	{
		GD.Print(shouldLoop.ToString());
		if (shouldLoop)
		{
			_index++;
			States[_index % States.Count].EnterState();
		}
		else
		{
			SetStatesAndScene();
		}

	}

	private async Task<bool> GoToNextLevelOrQuit() // Change return type to Task<bool>
	{
		OnLevelChanged?.Invoke();
	
		if (_currentLevelIndex > _levelScenes.Count - 1)
		{
			return false;
		}

		var children = _sceneContainer.GetChildren();
		var exitTasks = new List<SignalAwaiter>();

		foreach (Node child in children)
		{
			exitTasks.Add(ToSignal(child, Node.SignalName.TreeExited));
			child.QueueFree();
		}

		foreach (var task in exitTasks)
		{
			await task;
		}

		Node newLevel = _levelScenes[_currentLevelIndex].Instantiate();
		_sceneContainer.AddChild(newLevel);
		++_currentLevelIndex;

		return true;
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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
