using Godot;
using System;

public partial class Ball : RigidBody2D
{
	[Export]
	public float Speed { get; set; } = 200f;

	// Runs inside the physics solver. Bounces set the direction; we pin the
	// magnitude here so the ball can never gain or lose speed over time.
	public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		state.LinearVelocity = state.LinearVelocity.Normalized() * Speed;
	}

}
