using Godot;
using System.Collections.Generic;

public partial class Main : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SpawnBlocks();
	}

	private void SpawnBlocks()
	{
		float viewportWidth = GetViewport().GetVisibleRect().Size.X;
		float rowSpacing = Settings.BlockGapVertical + Settings.BlockHeight;
		(BlockColor color, int rows)[] layout = [(BlockColor.Red, 4), (BlockColor.Blue, 4), (BlockColor.Yellow, 4)];

		List<Block> blocks = [];
		float y = Settings.BlockRowY;
		foreach ((BlockColor color, int rows) in layout)
		{
			for (int row = 0; row < rows; row++)
			{
				blocks.AddRange(BlockFactory.CreateBlockLine(Settings.BlockCount, color, Settings.BlockGapHorizontal, viewportWidth, y));
				y += rowSpacing;
			}
		}

		foreach (Block block in blocks)
		{
			AddChild(block);
		}
	}
}
