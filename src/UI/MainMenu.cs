using Godot;
using System;
using GodotPlugins.Game;

public partial class MainMenu : Control
{
	[Export] private PackedScene MainSceneToLoad;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (Input.IsActionJustPressed("LaunchGame"))
		{
			GetTree().ChangeSceneToPacked(MainSceneToLoad);
		}
	}
}
