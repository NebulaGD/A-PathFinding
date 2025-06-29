using System;
using Microsoft.Xna.Framework;

/// <summary>
/// 计算路径.
/// </summary>
public class PathFinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closeList;

    public PathFinding(int width, int height)
    {
        grid = new Grid<PathNode>(width, height, 32, Vector2.Zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);
        openList = new List<PathNode> { startNode };
        closeList = new List<PathNode>();
        for (int x = 0; x <= 20; x++)
        {
            for (int y = 0; y <= 20; y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.parent = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closeList.Contains(neighbourNode))
                {
                    continue;
                }
                if (!neighbourNode.isWalkable)
                {
                    closeList.Add(neighbourNode);
                    continue;
                }
                int tentativeGcost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGcost < neighbourNode.gCost)
                {
                    neighbourNode.parent = currentNode;
                    neighbourNode.gCost = tentativeGcost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }


        return CalculatePath(endNode);
    }

    public PathNode GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    public List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        if (currentNode.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            //if (currentNode.y - 1 >= 0)
            //{
            //    neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            //}
            //if (currentNode.y + 1 < height)
            //{
            //    neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
            //}
        }

        if (currentNode.x + 1 < width)
        {
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            //if (currentNode.y - 1 >= 0)
            //{
            //    neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            //}
            //if (currentNode.y + 1 < width)
            //{
            //    neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
            //}
        }

        if (currentNode.y - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        }
        if (currentNode.y + 1 < height)
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
        }
        return neighbourList;
    }

    public List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.parent != null)
        {
            path.Add(currentNode.parent);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    public int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Math.Abs(a.x - b.x);
        int yDistance = Math.Abs(a.y - b.y);
        int remaining = Math.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Math.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    public PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }
}
