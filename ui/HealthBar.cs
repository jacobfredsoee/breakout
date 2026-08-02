using Godot;
using System.Collections.Generic;

public partial class HealthBar : Node2D
{
	[Export]
	public int Lives = 3;

	[Signal]
	public delegate void GameLostEventHandler();

	private readonly List<Sprite2D> _livesSprites = [];
	private Texture2D _lifeTexture;
	private int _maxLives;

	public override void _Ready()
	{
		_lifeTexture = GD.Load<Texture2D>("uid://5uflq7q1ipb");
		_maxLives = Lives;
	}

	public void Reset()
	{
		foreach (Sprite2D life in _livesSprites)
		{
			life.QueueFree();
		}
		_livesSprites.Clear();
		Lives = _maxLives;

		for (int i = 0; i < Lives; i++)
		{
			var life = new Sprite2D
			{
				Texture = _lifeTexture,
				Position = new Vector2(i * (_lifeTexture.GetWidth() + 12), 0)
			};
			AddChild(life);
			_livesSprites.Add(life);
		}
	}

	public void RemoveLife()
	{
		if (Lives <= 0) return;

		Lives--;
		_livesSprites[Lives].Visible = false;
		if (Lives == 0) EmitSignal(SignalName.GameLost);
	}
}
