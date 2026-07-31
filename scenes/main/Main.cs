using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node
{
	[Signal]
	public delegate void GameLostEventHandler();
	private Ball _ball;
	private Timer _serveTimer;
	private int _score = 0;
	private HUD _hud;
	private Arena _arena;
	private int remainingBlocks = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_ball = GetNode<Ball>("Ball");
		_serveTimer = GetNode<Timer>("ServeTimer");
		_hud = GetNode<HUD>("HUD");
		_arena = GetNode<Arena>("Arena");
		_arena.BallLost += OnBallLost;
		_hud.GameLost += OnGameLost;
		SpawnBlocks();
		SpawnBall();
		ResetBall();
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
			block.Destroyed += _hud.AddScore;
			block.Destroyed += CheckForWin;
			AddChild(block);
		}
		remainingBlocks = blocks.Count;
	}

	public void OnGameLost()
	{
		_ball.QueueFree();
		_serveTimer.Stop();
	}

	public void CheckForWin(int points)
	{
		remainingBlocks--;
		if (remainingBlocks > 0)
		{
			return;
		}
		_ball.QueueFree();
		_serveTimer.Stop();
		_hud.DisplayGameWon();
	}

	public void OnBallLost()
	{
		_hud.LoseLife();
		ResetBall();
	}

	public void ResetBall()
	{
		_ball.Position = new Vector2(GetViewport().GetVisibleRect().Size.X / 2, 300);
		_ball.LinearVelocity = Vector2.Zero;
		_serveTimer.Start();
	}

	public void OnServeTimerTimeout()
	{
		float straightUp = -Mathf.Pi / 2;
		float halfCone = Mathf.DegToRad(45f) / 2f;
		float direction = straightUp + (float)GD.RandRange(-halfCone, halfCone);
		_ball.LinearVelocity = Vector2.FromAngle(direction) * _ball.Speed;
	}
}
