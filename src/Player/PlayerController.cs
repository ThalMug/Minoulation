using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;

namespace Minoulation.Player;

public partial class PlayerController : Node
{
	public Action OnLaunch;
	
	[Export] 
	private Camera2D PlayerCamera;
	private Vector2 _targetPosition => _selectedCharacter?.Position ?? PlayerCamera.Position;
	private Vector2 _targetZoom = new(0.5f, 0.5f);
	
	private bool _controllerActive;
	private List<Cat> _characters = new();
	private Cat _selectedCharacter;

	public void EnablePlayerController(List<Cat> characterBody2Ds)
	{
		_targetZoom = new Vector2(2f, 2f);
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

	public void DisablePlayerController()
	{
		_targetZoom = new Vector2(0.5f, 0.5f);
		_controllerActive = false;
	}

	public override void _Input(InputEvent @event)
	{
		if (!_controllerActive)
		{
			return;	
		}
		
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
		}
		else if (@event.IsActionPressed("Wait"))
		{
			_selectedCharacter.Wait();	
		}
		else if (@event.IsActionPressed("LaunchGame"))
		{
			OnLaunch?.Invoke();
		}
		else if (@event.IsActionPressed("ZoomIn"))
		{
			float newX = _targetZoom.X += 0.5f;
			float newY = _targetZoom.Y += 0.5f;
			_targetZoom = new Vector2(Mathf.Clamp(newX, 0.25f, 3f), Mathf.Clamp(newY, 0.25f, 3f));
		}
		else if (@event.IsActionPressed("ZoomOut"))
		{
			float newX = _targetZoom.X -= 0.5f;
			float newY = _targetZoom.Y -= 0.5f;
			_targetZoom = new Vector2(Mathf.Clamp(newX, 0.25f, 3f), Mathf.Clamp(newY, 0.25f, 3f));
		}
	}

	private void SetLeftCharacter()
	{
		var orderedLeftCharacters = _characters.OrderBy(c => c.Position.X).ToList();
		int selectedIndex = orderedLeftCharacters.IndexOf(_selectedCharacter);

		if (selectedIndex != 0)
		{
			_selectedCharacter = orderedLeftCharacters[selectedIndex - 1];
		}
	}

	private void SetRightCharacter()
	{
		var orderedLeftCharacters = _characters.OrderBy(c => c.Position.X).ToList();
		int selectedIndex = orderedLeftCharacters.IndexOf(_selectedCharacter);

		if (selectedIndex != orderedLeftCharacters.Count - 1)
		{
			_selectedCharacter = orderedLeftCharacters[selectedIndex + 1];
		}
	}
	
	
	public override void _Process(double delta)
	{
		PlayerCamera.Zoom = PlayerCamera.Zoom.MoveToward(_targetZoom, (float)delta * 3f);
		
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
