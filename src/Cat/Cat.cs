using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;

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
	
	int gridSize = 48;
	[Export] private TileMapLayer tilemap;
	[Export] private RayCast2D rayCast2D;

	Vector2 intialPosition;

	List<Actions> listActions = new List<Actions>();


    public override void _Ready()
    {
        base._Ready();
		intialPosition = Position;
    }


    public override void _Input(InputEvent @event)
    {
		if (@event.IsActionPressed("MoveForward"))
		{
			GD.Print("for");
			listActions.Add(Actions.Forward);
			move(Actions.Forward);
		}else if(@event.IsActionPressed("MoveBackward"))
		{
			GD.Print("back");
			listActions.Add(Actions.Backward);
			move(Actions.Backward);
		}else if(@event.IsActionPressed("MoveLeft"))
		{
			GD.Print("left");
			listActions.Add(Actions.Left);
			move(Actions.Left);
		}else if (@event.IsActionPressed("MoveRight"))
		{
			GD.Print("right");
			listActions.Add(Actions.Right);
			move(Actions.Right);
		}else if (@event.IsActionPressed("Wait"))
		{
			GD.Print("Wait");
			listActions.Add(Actions.NoMove);
			move(Actions.NoMove);
		}else if (@event.IsActionPressed("LaunchGame"))
		{
			GD.Print("Launch");
			playActions();
		}
		
    }

	public async void playActions()
	{
		GD.Print("play actions");
		Position = intialPosition;
		foreach (Actions action in listActions)
		{
			move(action);
			await ToSignal(GetTree().CreateTimer(.5), "timeout");
		}

		listActions.Clear();
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
			Position += destination;
		}
	}


}
