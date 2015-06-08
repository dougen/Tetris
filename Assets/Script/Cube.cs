using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour 
{
    private float lastFall = 0;
    private int rotationType = 0;

    private void Start()
    {
        if (!IsValidGridPos())
        {
            Debug.Log("Game Over");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);

            if (IsValidGridPos())
            {
                UpdateGrid();
            }
            else
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);

            if (IsValidGridPos())
            {
                UpdateGrid();
            }
            else
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotationCube(++rotationType);

            if (IsValidGridPos())
            {
                UpdateGrid();
            }
            else
            {
                RotationCube(--rotationType);
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Time.time - lastFall >= 1)
        {
            transform.position += new Vector3(0, -1, 0);

            if (IsValidGridPos())
            {
                UpdateGrid();
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);
                Grid.DeleteFullRows();
                FindObjectOfType<Spawner>().SpawnNext();
                enabled = false;  // 触底之后停止该脚本
            }
            lastFall = Time.time;
        }
    }

    private bool IsValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.RoundVec2(child.position);
            
            // 当子方块出界时，返回false
            if (!Grid.InsideBorder(v))
            {
                return false;
            }

            // 当子方块位置已经有其他方块，或者不是同一块Cube时，返回false
            if (Grid.grid[(int)v.x, (int)v.y] != null &&
                Grid.grid[(int)v.x, (int)v.y].transform.parent != transform)
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateGrid()
    {
        for (int y=0; y<Grid.h; ++y)
        {
            for (int x=0; x<Grid.w; ++x)
            {
                if (Grid.grid[x, y] != null)
                {
                    if (Grid.grid[x, y].transform.parent == transform)
                    {
                        Grid.grid[x, y] = null;
                    }
                }
            }
        }

        foreach (Transform child in transform)
        {
            Vector2 v = Grid.RoundVec2(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child.gameObject;
        }
    }

    // 根据方块类型决定旋转方式
    private void RotationCube(int rotType)
    {
        switch (Spawner.cubeType)
        {
            case 0:             // I方块
                if (rotType % 2 == 0)
                {
                    TransformPos(new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(2, 1, 0), new Vector3(3, 1, 0));
                }
                else if (rotType % 2 == 1)
                {
                    TransformPos(new Vector3(2, 2, 0), new Vector3(2, 1, 0), new Vector3(2, 0, 0), new Vector3(2, -1, 0));
                }
                break;

            case 1:             // J方块
                if (rotType % 4 == 0)
                {
                    TransformPos(new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(2, 1, 0), new Vector3(2, 0, 0));
                }
                else if (rotType % 4 == 1)
                {
                    TransformPos(new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 2, 0), new Vector3(2, 2, 0));
                }
                else if (rotType % 4 == 2)
                {
                    TransformPos(new Vector3(0, 1, 0), new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(2, 0, 0));
                }
                else if (rotType % 4 == 3)
                {
                    TransformPos(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 2, 0));
                }
                break;

            case 2:             // L方块
                if (rotType % 4 == 0)
                {
                    TransformPos(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(2, 1, 0));
                }
                else if (rotType % 4 == 1)
                {
                    TransformPos(new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 2, 0), new Vector3(2, 0, 0));
                }
                else if (rotType % 4 == 2)
                {
                    TransformPos(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(2, 0, 0), new Vector3(2, 1, 0));
                }
                else if (rotType % 4 == 3)
                {
                    TransformPos(new Vector3(0, 2, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 2, 0));
                }
                break;
            case 3:             // O方块
                // 不做任何变化
                rotType = 0;
                break;
            case 4:             // S方块
                if (rotType % 2 == 0)
                {
                    TransformPos(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(2, 1, 0));
                }
                else if (rotType % 2 == 1)
                {
                    TransformPos(new Vector3(0, 1, 0), new Vector3(0, 2, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0));
                }
                break;
            case 5:             // T方块
                if (rotType % 4 == 0)
                {
                    TransformPos(new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0), new Vector3(2, 1, 0));
                }
                else if (rotType % 4 == 1)
                {
                    TransformPos(new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 2, 0), new Vector3(2, 1, 0));
                }
                else if (rotType % 4 == 2)
                {
                    TransformPos(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(2, 0, 0), new Vector3(1, 1, 0));
                }
                else if (rotType % 4 == 3)
                {
                    TransformPos(new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 2, 0));
                }
                break;
            case 6:             // Z方块
                if (rotType % 2 == 0)
                {
                    TransformPos(new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0), new Vector3(2, 0, 0));
                }
                else if (rotType % 2 == 1)
                {
                    TransformPos(new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(2, 1, 0), new Vector3(2, 2, 0));
                }
                break;
        }
    }

    private void TransformPos(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        transform.GetChild(0).position = transform.position + v1;
        transform.GetChild(1).position = transform.position + v2;
        transform.GetChild(2).position = transform.position + v3;
        transform.GetChild(3).position = transform.position + v4;
    }
}
