using Godot;
using System.Collections.Generic;

public enum BlockColor
{
    Red,
    Blue,
    Yellow,
}
public static class BlockFactory
{
    private const float BlockWidth = 64f;

    private static readonly PackedScene _blockScene = GD.Load<PackedScene>("uid://02uiqp1atn6p");
    public static Block CreateBlock(Vector2 position, BlockColor color, int health = 1)
    {
        Block block = _blockScene.Instantiate<Block>();
        block.Initialize(position, GetColor(color), health);
        return block;
    }

    // Creates a horizontal row of `count` blocks centered on `viewportWidth`, with `gap`
    // pixels between each block (and therefore equal margins on both sides). Blocks are
    // returned positioned but not added to the tree; the caller owns parenting.
    public static List<Block> CreateBlockLine(int count, BlockColor color, int health, float gap, float viewportWidth, float y)
    {
        List<Block> blocks = new(count);
        if (count <= 0)
        {
            return blocks;
        }

        float rowWidth = count * BlockWidth + (count - 1) * gap;
        float firstCenterX = (viewportWidth - rowWidth) / 2f + BlockWidth / 2f;
        float step = BlockWidth + gap;

        for (int i = 0; i < count; i++)
        {
            float x = firstCenterX + i * step;
            blocks.Add(CreateBlock(new Vector2(x, y), color, health));
        }

        return blocks;
    }

    private static Color GetColor(BlockColor color)
    {
        switch (color)
        {
            case BlockColor.Red:
                return new Color(1, 0, 0);
            case BlockColor.Blue:
                return new Color(0, 0, 1);
            case BlockColor.Yellow:
                return new Color(1, 1, 0);
            default:
                GD.PushError($"Unhandled block color: {color}");
                return new Color(1, 1, 1);
        }
    }
}
