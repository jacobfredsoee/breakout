using Godot;

public partial class Block : StaticBody2D
{
	[Export]
	public int Health { get; set; } = 1;
	public AnimationPlayer _animationPlayer;
	private bool _dying = false;

	public void Initialize(Vector2 position, Color color, int health = 1)
	{
		Position = position;
		Modulate = color;
		Health = health;
		_animationPlayer = GetNode<AnimationPlayer>("HitFlashAnimation");
	}

	public void TakeHit()
	{
		if (_dying) return;

		Health--;
		_animationPlayer.Play("hit");
		if (Health <= 0)
		{
			Die();
		}
	}

	public async void Die()
	{
		_dying = true;
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		await ToSignal(_animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
		QueueFree();
	}

	public void OnBodyEntered(Node2D body)
	{
		if (body is not Ball) return;
		TakeHit();
	}
}
