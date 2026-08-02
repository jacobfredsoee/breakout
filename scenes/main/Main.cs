using Godot;
using System.Collections.Generic;

public partial class Main : Node
{
	private Ball _ball;
	private Timer _serveTimer;
	private HUD _hud;
	private Arena _arena;
	private int _remainingBlocks = 0;
	private bool _gameOver = false;

	public override void _Ready()
	{
		_ball = GetNode<Ball>("Ball");
		_serveTimer = GetNode<Timer>("ServeTimer");
		_hud = GetNode<HUD>("HUD");
		_arena = GetNode<Arena>("Arena");
		_arena.BallLost += OnBallLost;
		_hud.GameLost += OnGameLost;
		_hud.RestartGame += RestartGame;
		SpawnBlocks();
		ResetBall();
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
			block.Destroyed += _ => CheckForWin();
			AddChild(block);
		}
		_remainingBlocks = blocks.Count;
	}

	private void RemoveAllBlocks()
	{
		foreach (Node node in GetChildren())
		{
			if (node is Block)
			{
				node.QueueFree();
			}
		}
	}

	private Vector2 BallStart => new(GetViewport().GetVisibleRect().Size.X / 2, 300f);

	private void OnGameLost()
	{
		_ball.Park(BallStart);
		_ball.Visible = false;
		_serveTimer.Stop();
		_gameOver = true;
	}

	private void CheckForWin()
	{
		_remainingBlocks--;
		if (_remainingBlocks > 0)
		{
			return;
		}
		_gameOver = true;
		_ball.Park(BallStart);
		_ball.Visible = false;
		_serveTimer.Stop();
		_hud.DisplayGameWon();
	}

	private void OnBallLost()
	{
		_hud.LoseLife();
		if (_gameOver) return;
		ResetBall();
	}

	private void ResetBall()
	{
		_ball.Visible = true;
		_ball.Park(BallStart);
		_serveTimer.Start();
	}

	public void OnServeTimerTimeout()
	{
		float straightUp = -Mathf.Pi / 2;
		float halfCone = Mathf.DegToRad(45f) / 2f;
		float direction = straightUp + (float)GD.RandRange(-halfCone, halfCone);
		_ball.Launch(Vector2.FromAngle(direction));
	}

	private void RestartGame()
	{
		RemoveAllBlocks();
		SpawnBlocks();
		ResetBall();
		_gameOver = false;
	}
}
