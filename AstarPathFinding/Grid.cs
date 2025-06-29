using System;
using Microsoft.Xna.Framework;

public class Grid<TGridObject>
{
    private int width;
    private int height;
    private TGridObject[,] gridArray;
    private float cellSize;
    private Vector2 originPosition;

    public Grid(int width, int height, float cellSize, Vector2 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> creatGridObejct)
    {
        this.width = width;
        this.height = height;
        gridArray = new TGridObject[width, height];
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = creatGridObejct(this, x, y);
            }
        }
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(x, y) * cellSize + originPosition;
    }

    public void GetXY(int worldPsotionX, int worldPsotionY, out int x, out int y)
    {
        x = (int)((worldPsotionX - originPosition.X) / cellSize);
        y = (int)((worldPsotionY - originPosition.Y) / cellSize);
    }

    public void GetXY(Vector2 worldPosition, out int x, out int y)
    {
        x = (int)((worldPosition - originPosition).X / cellSize);
        y = (int)((worldPosition - originPosition).Y / cellSize);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector2 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }
}
