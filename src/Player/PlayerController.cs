using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;

namespace Minoulation.Player;

public partial class PlayerController : Node
{
	[Export] 
	private Camera2D PlayerCamera;
	private Vector2 _targetPosition => _selectedCharacter?.Position ?? PlayerCamera.Position;
	private bool _controllerActive;
	private List<Cat> _characters = new();
	private Cat _selectedCharacter;

	public void EnablePlayerController(List<Cat> characterBody2Ds)
	{
		_characters = characterBody2Ds;
		_controllerActive = true;
		if (_characters.Count > 0)
		{
			_selectedCharacter = _characters[0];
		}
		else
		{
			GD.PrintErr("No characters");
		}
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event.IsActionPressed("NextCharacter"))
		{
			SetRightCharacter();
		}
		else if (@event.IsActionPressed("PreviousCharacter"))
		{
			SetLeftCharacter();
		}

		else if (@event.IsActionPressed("MoveForward"))
		{
			_selectedCharacter.Forward();
		}
		else if (@event.IsActionPressed("MoveBackward"))
		{
			_selectedCharacter.Backward();
		}
		else if (@event.IsActionPressed("MoveLeft"))
		{
			_selectedCharacter.Left();
		}
		else if (@event.IsActionPressed("MoveRight"))
		{
			_selectedCharacter.Right();
		}else if (@event.IsActionPressed("Wait"))
		{
			_selectedCharacter.Wait();	
		}else if (@event.IsActionPressed("LaunchGame"))
		{
			_selectedCharacter.Launch();
		}
	}

	private void SetLeftCharacter()
	{
		var orderedLeftCharacters = _characters.OrderBy(c => c.Position.X).ToList();
		int selectedIndex = orderedLeftCharacters.IndexOf(_selectedCharacter);

		if (selectedIndex == 0 && orderedLeftCharacters.Last() != null)
		{
			_selectedCharacter = orderedLeftCharacters.Last();
		}
		else
		{
			_selectedCharacter = orderedLeftCharacters[selectedIndex - 1];
		}
	}

	private void SetRightCharacter()
	{
		var orderedLeftCharacters = _characters.OrderBy(c => c.Position.X).ToList();
		int selectedIndex = orderedLeftCharacters.IndexOf(_selectedCharacter);

		if (selectedIndex == orderedLeftCharacters.Count - 1 && orderedLeftCharacters.First() != null)
		{
			_selectedCharacter = orderedLeftCharacters.First();
		}
		else
		{
			_selectedCharacter = orderedLeftCharacters[selectedIndex + 1];
		}
	}
	
	
	public override void _Process(double delta)
	{
		if (!_controllerActive)
		{
			return;
		}
		
		float distance = Mathf.Abs(PlayerCamera.GlobalPosition.DistanceTo(_targetPosition));
		float speed = distance * 10;
		if (Mathf.Abs(PlayerCamera.GlobalPosition.DistanceTo(_targetPosition)) > 0.5)
		{
			PlayerCamera.GlobalPosition = PlayerCamera.GlobalPosition.MoveToward(_targetPosition, (float)delta * speed);
		}
	}
}
