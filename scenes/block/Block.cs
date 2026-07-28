using Godot;

public partial class Block : StaticBody2D
{
	[Export]
	public int Health { get; set; } = 1;

	public void Initialize(Vector2 position, Color color, int health = 1)
	{
		Position = position;
		Modulate = color;
		Health = health;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void TakeHit()
	{
		Health--;
		if (Health <= 0)
		{
			Die();
		}
	}

	public void Die()
	{
		QueueFree();
	}
}
