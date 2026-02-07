using Godot;
using System;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;

public partial class Cat : CharacterBody2D
{

	int gridSize = 48;
	[Export] private TileMapLayer tilemap;
	[Export] private RayCast2D rayCast2D;
	

    public override void _Input(InputEvent @event)
    {
		if (@event.IsActionPressed("MoveForward"))
		{
			move(Vector2.Up);
		}else if(@event.IsActionPressed("MoveBackward"))
		{
			move(Vector2.Down);
		}else if(@event.IsActionPressed("MoveLeft"))
		{
			move(Vector2.Left);
		}else if (@event.IsActionPressed("MoveRight"))
		{
			move(Vector2.Right);
		}
		
    }

	private void move(Vector2 direction)
	{
		Vector2I destination = (Vector2I) direction * gridSize;
		rayCast2D.TargetPosition = destination;
		rayCast2D.ForceRaycastUpdate();
		if (!rayCast2D.IsColliding())
		{
			Position += destination;
		}
	

	}

}
