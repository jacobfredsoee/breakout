using Godot;
using System;

public partial class HUD : CanvasLayer
{
	[Signal]
	public delegate void GameLostEventHandler();
	private Label _scoreLabel;
	private HealthBar _healthBar;
	private int _score;

	public override void _Ready()
	{
		_scoreLabel = GetNode<Label>("ScoreLabel");
		_healthBar = GetNode<HealthBar>("HealthBar");
		_healthBar.GameLost += OnGameLost;
		GetNode<Label>("GameLostLabel").Visible = false;
		GetNode<Label>("GameWonLabel").Visible = false;
	}
	public void OnGameLost()
	{
		EmitSignal(SignalName.GameLost);
		GetNode<Label>("GameLostLabel").Visible = true;
	}

	public void DisplayGameWon()
	{
		GetNode<Label>("GameWonLabel").Visible = true;
	}

	public void LoseLife()
	{
		_healthBar.RemoveLife();
	}

	public void AddScore(int points)
	{
		_score += points;
		_scoreLabel.Text = _score.ToString();
	}
}
