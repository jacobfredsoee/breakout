using Godot;
using System;
using System.Collections.Generic;

public partial class HealthBar : Node2D
{
	[Export]
	public int Lives = 3;

	[Signal]
	public delegate void GameLostEventHandler();

	public List<Sprite2D> LivesSprites = new List<Sprite2D>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var lifeTexture = GD.Load<Texture2D>("uid://5uflq7q1ipb");
		for (int i = 0; i < Lives; i++)
		{
			var life = new Sprite2D
			{
				Texture = lifeTexture,
				Position = new Vector2(i * (lifeTexture.GetWidth() + 12), 0)
			};
			AddChild(life);
			LivesSprites.Add(life);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void RemoveLife()
	{
		if (Lives > 0)
		{
			Lives--;
			LivesSprites[Lives].Visible = false;
		}
		if (Lives == 0)
		{
			EmitSignal(SignalName.GameLost);
		}
	}
}
