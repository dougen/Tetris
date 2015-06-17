using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour 
{
    public static int w = 10;   // 游戏宽度10
    public static int h = 20;   // 游戏高度20
    public static GameObject[,] grid = new GameObject[w, h];

    // 将二维坐标取整
	public static Vector2 RoundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    // 判断坐标是否在界内,在界内返回true,在界外返回false
    public static bool InsideBorder(Vector2 pos)
    {
        return (int)pos.x >= 0 && (int)pos.x < w && (int)pos.y >= 0;
    }

    // 删除某一行, y为删除的行数
    public static void DeleteRow(int y)
    {
        for (int x=0; x<w; ++x)
        {
            Destroy(grid[x, y]);
            grid[x, y] = null;
        }
    }

    
    public static void DecreaseRow(int y)
    {
        for (int x=0; x<w; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].transform.position += new Vector3(0, -1, 0);
            }
        }
    }

    public static void DecreaseRowsAbove(int y)
    {
        for (int i=y; i<h; ++i)
        {
            DecreaseRow(i);
        }
    }

    // 判断某一行是否已满，满了返回true，没满返回false
    public static bool IsRowFull(int y)
    {
        for (int x=0; x<w; ++x)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    public static void DeleteFullRows()
    {
        for (int y=0; y<h; ++y)
        {
            if (IsRowFull(y))
            {
                DeleteRow(y);
                GameManager.SCORE += 100;
                DecreaseRowsAbove(y + 1);
                --y;  // 继续检查当前这行
            }
        }
    }
}
