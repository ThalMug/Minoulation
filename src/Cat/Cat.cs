using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
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
	[Export] private Area2D _area2D;
	[Export] private AnimatedSprite2D CatIdle;
	[Export] private AnimatedSprite2D FightCloud;
	
	[Export] private PackedScene _fadeSprite;
	[Export] private AnimatedSprite2D _animatedSprite;

	private Vector2 _initialPosition;


	List<Actions> _listActions = new List<Actions>();
	List<AnimatedSprite2D> _fadeSpritList = new List<AnimatedSprite2D>();

	



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
		Move(Actions.Forward);
	}
	public void Backward()
	{
		_listActions.Add(Actions.Backward);
		Move(Actions.Backward);
	}
	public void Left()
	{
		_listActions.Add(Actions.Left);
		Move(Actions.Left);
	}
	public void Right()
	{
		_listActions.Add(Actions.Right);
		Move(Actions.Right);
	}
	public void Wait()
	{
		_listActions.Add(Actions.NoMove);
		Move(Actions.NoMove);
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
		
		Move(_listActions[index], true);
	}

	public void ResetPosition()
	{
		DisableFight();
		//resetGhost
		foreach (AnimatedSprite2D fadeSprite in _fadeSpritList)
		{
			fadeSprite.Hide();
			fadeSprite.QueueFree();
		}
		_fadeSpritList.Clear();
		//resetPosition
		Position = _initialPosition;
	}

	public void ResetListActions()
	{
		_listActions.Clear();
	}

	public void EnableFight()
	{
		ResetListActions();
		
		CatIdle.Visible = false;
		FightCloud.Visible = true;
		FightCloud.Play();
	}

	public void DisableFight()
	{
		CatIdle.Visible = true;
		FightCloud.Visible = false;
		CatIdle.Play();
	}

	
	public async void playActions()
	{
		GD.Print("play actions");
		Position = _initialPosition;
		foreach (Actions action in _listActions)
		{
			Move(action);
			await ToSignal(GetTree().CreateTimer(.5), "timeout");
		}

		_listActions.Clear();
	}
	
	private void Move(Actions action, bool isResolution = false)
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
			if (!isResolution)
			{
				AnimatedSprite2D tempSprite = (AnimatedSprite2D) _fadeSprite.Instantiate();
				tempSprite.Modulate = tempSprite.Modulate with { A = 0.5f };
				AddChild(tempSprite);
				tempSprite.Position = Position;
				_fadeSpritList.Add(tempSprite);
				GD.Print(_fadeSpritList.Count);
			}
			Position += destination;
		}
	}
	
	public Vector2 GetDirectionOfAction(int index)
	{
		if (index >= _listActions.Count) return Vector2.Zero;
	
		return _listActions[index] switch
		{
			Actions.Forward => Vector2.Up,
			Actions.Backward => Vector2.Down,
			Actions.Left => Vector2.Left,
			Actions.Right => Vector2.Right,
			_ => Vector2.Zero
		};
	}


}
