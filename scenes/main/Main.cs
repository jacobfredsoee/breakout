using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node
{
	private Ball _ball;
	private Timer _serveTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_ball = GetNode<Ball>("Ball");
		_serveTimer = GetNode<Timer>("ServeTimer");
		SpawnBlocks();
		SpawnBall();
	}

	private void SpawnBall()
	{
		_ball.Position = new Vector2(GetViewport().GetVisibleRect().Size.X / 2, 300);
		_serveTimer.Start();
	}

	private void SpawnBlocks()
	{
		float viewportWidth = GetViewport().GetVisibleRect().Size.X;
		float rowSpacing = Settings.BlockGapVertical + Settings.BlockHeight;
		(BlockColor color, int rows, int health)[] layout = [(BlockColor.Red, 4, 2), (BlockColor.Blue, 4, 1), (BlockColor.Yellow, 4, 1)];

		List<Block> blocks = [];
		float y = Settings.BlockRowY;
		foreach ((BlockColor color, int rows, int health) in layout)
		{
			for (int row = 0; row < rows; row++)
			{
				blocks.AddRange(BlockFactory.CreateBlockLine(Settings.BlockCount, color, health, Settings.BlockGapHorizontal, viewportWidth, y));
				y += rowSpacing;
			}
		}

		foreach (Block block in blocks)
		{
			AddChild(block);
		}
	}

	public void OnServeTimerTimeout()
	{
		GD.Print("Serve");
		float straightUp = -Mathf.Pi / 2;
		float halfCone = Mathf.DegToRad(45f) / 2f;
		float direction = straightUp + (float)GD.RandRange(-halfCone, halfCone);
		_ball.LinearVelocity = Vector2.FromAngle(direction) * _ball.Speed;
	}
}
