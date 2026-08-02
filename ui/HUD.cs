using Godot;

public partial class HUD : CanvasLayer
{
	[Signal]
	public delegate void GameLostEventHandler();
	[Signal]
	public delegate void RestartGameEventHandler();
	private Label _scoreLabel;
	private HealthBar _healthBar;
	private Label _gameLostLabel;
	private Label _gameWonLabel;
	private Button _restartGameButton;
	private int _score;

	public override void _Ready()
	{
		_scoreLabel = GetNode<Label>("ScoreLabel");
		_healthBar = GetNode<HealthBar>("HealthBar");
		_healthBar.GameLost += OnGameLost;
		_gameLostLabel = GetNode<Label>("GameLostLabel");
		_gameWonLabel = GetNode<Label>("GameWonLabel");
		_restartGameButton = GetNode<Button>("RestartGameButton");

		Reset();
	}
	public void OnGameLost()
	{
		EmitSignal(SignalName.GameLost);
		_gameLostLabel.Visible = true;
		_restartGameButton.Visible = true;
	}

	public void DisplayGameWon()
	{
		_gameWonLabel.Visible = true;
		_restartGameButton.Visible = true;
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

	public void OnRestartGameButtonPressed()
	{
		Reset();
		EmitSignal(SignalName.RestartGame);
	}

	public void Reset()
	{
		_healthBar.Reset();
		_score = 0;
		_scoreLabel.Text = _score.ToString();
		_gameLostLabel.Visible = false;
		_gameWonLabel.Visible = false;
		_restartGameButton.Visible = false;
	}
}
