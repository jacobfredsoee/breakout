using Godot;

public partial class Paddle : AnimatableBody2D
{
	private Vector2 _screenSize;
	private int _borderInset = 5;
	public float Speed = 400f;
	private Sprite2D _sprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_screenSize = GetViewportRect().Size;
		_sprite = GetNode<Sprite2D>("Sprite2D");
		GD.Print(_sprite.Texture.GetWidth());
		GD.Print(_sprite.Texture.GetHeight());
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		var direction = Input.GetAxis("move_left", "move_right");
		var velocity = Vector2.Right * direction * Speed;

		Position += velocity * (float)delta;
		Position = new Vector2(Mathf.Clamp(Position.X, 0 + _borderInset + _sprite.Texture.GetWidth() / 2, _screenSize.X - _sprite.Texture.GetWidth() / 2 - _borderInset), Position.Y);
	}
}
