using Godot;

public partial class Ball : RigidBody2D
{
	[Export]
	public float Speed = 200f;

	private bool _parked;
	private Vector2 _parkPosition;

	public override void _Ready()
	{
		CanSleep = false;
	}

	// Runs inside the physics solver — the ONLY place a RigidBody's transform and
	// velocity can be set authoritatively. Setting Position from outside gets
	// overwritten by the physics server on the next tick (teleports one frame, snaps back).
	public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		if (_parked)
		{
			// Pin the body still at the park position.
			state.Transform = new Transform2D(0f, _parkPosition);
			state.LinearVelocity = Vector2.Zero;
			state.AngularVelocity = 0f;
			return;
		}

		// Pin the speed so bounces never change the magnitude.
		state.LinearVelocity = state.LinearVelocity.Normalized() * Speed;
	}

	// Hold the ball motionless at a position (reset / between serves / game over).
	public void Park(Vector2 position)
	{
		_parkPosition = position;
		_parked = true;
	}

	// Release the ball and send it on its way (call when the serve fires).
	public void Launch(Vector2 direction)
	{
		_parked = false;
		LinearVelocity = direction * Speed;
	}
}
