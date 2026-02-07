using Godot;

namespace Minoulation.Player;

public partial class PlayerController : Node
{
    private Vector2 _targetPosition;
    private Camera2D _playerCamera;

    public override void _Ready()
    {
        _targetPosition = _playerCamera.GlobalPosition;
    }

    public override void _Process(double delta)
    {
        if (Mathf.Abs(_playerCamera.GlobalPosition.DistanceTo(_targetPosition)) > 0.5)
        {
            _playerCamera.GlobalPosition.MoveToward(_targetPosition, (float)delta);
        }
    }
}