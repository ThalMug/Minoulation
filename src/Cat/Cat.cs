using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Resolvers;

public enum Actions
	{
		Forward,
		Backward,
		Left,
		Right,
		NoMove
	}

public partial class Cat : CharacterBody2D
{
	
	private int gridSize = 48;
	[Export] private RayCast2D rayCast2D;
	private PackedScene _fadeSprite = GD.Load<PackedScene>("src\\Cat\\CatFade.tscn");

	private Vector2 _initialPosition;

	private bool resolution = false;

	List<Actions> _listActions = new List<Actions>();
	List<AnimatedSprite2D> _fadeSpritList = new List<AnimatedSprite2D>();

	private AnimatedSprite2D _animatedSprite;



	//initialition
	public override void _Ready()
	{
		base._Ready();
		_initialPosition = Position;
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		_animatedSprite.Play("idle");

		foreach (AnimatedSprite2D _sprite2D in _fadeSpritList)
		{
		
			_sprite2D.Play("idle");
		} 

	}


	public void Forward()
	{
		_listActions.Add(Actions.Forward);
		move(Actions.Forward);
	}
	public void Backward()
	{
		_listActions.Add(Actions.Backward);
		move(Actions.Backward);
	}
	public void Left()
	{
		_listActions.Add(Actions.Left);
		move(Actions.Left);
	}
	public void Right()
	{
		_listActions.Add(Actions.Right);
		move(Actions.Right);
	}
	public void Wait()
	{
		_listActions.Add(Actions.NoMove);
		move(Actions.NoMove);
	}
	public void Launch()
	{
		int i = 0;
		foreach (AnimatedSprite2D fadeSprite in _fadeSpritList)
		{
			GD.Print(i++);
			fadeSprite.Hide();
			fadeSprite.QueueFree();
			
		}
		GD.Print("Launch");
		playActions();
	}

	public int GetActionsNumber()
	{
		return _listActions.Count;
	}

	public void PlayAction(int index)
	{
		if (index > _listActions.Count - 1)
		{
			return;
		}
		
		move(_listActions[index]);
	}

	public void ResetPosition()
	{
		Position = _initialPosition;
	}

	public void ResetListActions()
	{
		_listActions.Clear();
	}
	
	public async void playActions()
	{
		resolution = true;
		GD.Print("play actions");
		Position = _initialPosition;
		foreach (Actions action in _listActions)
		{
			move(action);
			await ToSignal(GetTree().CreateTimer(.5), "timeout");
		}

		_listActions.Clear();
	}

	private void move(Actions action)
	{
		Vector2 direction = Vector2.Zero;
		switch (action)
			{
				case Actions.Forward:
					direction = Vector2.Up;
					break;
				case Actions.Backward:
					direction = Vector2.Down;
					break;
				case Actions.Left:
					direction = Vector2.Left;
					break;
				case Actions.Right:
					direction = Vector2.Right;
					break;
				case Actions.NoMove:
					break;
				default:
					GD.Print("No Action Setup error");
					break;
			}
		Vector2I destination = (Vector2I) direction * gridSize;
		rayCast2D.TargetPosition = destination;
		rayCast2D.ForceRaycastUpdate();
		if (!rayCast2D.IsColliding())
		{
			if (!resolution)
			{
				AnimatedSprite2D tempSprite = (AnimatedSprite2D) _fadeSprite.Instantiate();
				AddChild(tempSprite);
				tempSprite.Position = Position;
				_fadeSpritList.Add(tempSprite);
				GD.Print(_fadeSpritList.Count);
			}
			Position += destination;
		}
	}


}
