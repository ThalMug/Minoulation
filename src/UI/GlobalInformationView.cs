using Godot;
using System;
using Minoulation.Player;

public partial class GlobalInformationView : Node
{
	[Export] private PlayerController _playerController;
	[Export] private Label _catActionCountLabel;
	[Export] private Label _minCountLabel;
	[Export] private CanvasLayer _canvasLayer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playerController.OnCatSelected += OnCatSelected;
		_playerController.OnLaunch += HideCanvas;
	}

	private void OnCatSelected(Cat cat)
	{
		ShowCanvas();
		
		cat.OnActionMade -= ChangeCatActionCountLabel;
		cat.OnActionMade += ChangeCatActionCountLabel;
		
		int currentCatActionCount = cat.GetActionsNumber();
		ChangeCatActionCountLabel(currentCatActionCount);
	}

	private void ChangeCatActionCountLabel(int number)
	{
		_catActionCountLabel.Text = number.ToString();
	}

	private void HideCanvas()
	{
		_canvasLayer.Hide();
	}

	public void ShowCanvas()
	{
		_canvasLayer.Show();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
