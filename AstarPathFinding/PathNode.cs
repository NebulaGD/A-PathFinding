using System;

public class PathNode
{
    private Grid<PathNode> grid;
    public int x;
    public int y;
    public int gCost;
    public int hCost;
    public int fCost;
    public PathNode parent;
    public bool isWalkable;

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
