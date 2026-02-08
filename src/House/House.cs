using Godot;
using System;

public partial class House : Area2D
{
 
	[Export] private Cat _mycat;
	private bool _isGoodCat = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Monitoring = true;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_area_entered(Area2D area2D)
	{
		if(area2D.GetParent() == _mycat)
		{
			_isGoodCat = true;
			GD.Print("Its the good cat enter");
		}
		
	}

	private void _on_area_exited(Area2D area2D)
	{
		if(area2D.GetParent() == _mycat)
		{
			_isGoodCat = false;
			GD.Print("Its the good cat leave");

		}
	}

	public bool getIsGoodCat()
	{
		return _isGoodCat;
	}
}
