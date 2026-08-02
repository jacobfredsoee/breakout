using Godot;

public partial class Arena : Node2D
{
	[Signal]
	public delegate void BallLostEventHandler();

	public void OnBottomAreaBodyEntered(Node2D body)
	{
		if (body is Ball)
		{
			EmitSignal(SignalName.BallLost);
		}
	}
}
