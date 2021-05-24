using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{

    public Tile tilePrefab;
    public Transform parentPrefab;
    public Vector2Int BallLocation { get => BallLocation; set => BallLocation = value; }
    public Direction MoveDirection { get => MoveDirection; set => MoveDirection = value; }
    public List<Tile> tileArray;

    public void InitializeTiles(int level)
    {   
        tileArray = new List<Tile>();
        for(int i=0; i < 81; i++)
        {
            if (BoardConstant.levelList[level][i] == 1)
                tilePrefab.isWall = true;
            else
                tilePrefab.isWall = false;
        
            Debug.Log("Tile index " + i + ", isWall: " + tilePrefab.isWall);
            if (tilePrefab.isWall)
            {
                tilePrefab.GetComponent<Image>().color = Color.white;
                tilePrefab.tag = "Wall";
            }
            else
            {
                tilePrefab.GetComponent<Image>().color = Color.gray;
                tilePrefab.tag = "Tile";

            } 
            
            tileArray.Add(Instantiate(tilePrefab, transform));
        }
        for (int i = 0; i < 81; i++)
        {
            AssignTileDirections(i);
        }
    }

    public void AssignTileDirections(int index)
    {
        if (index >= 0 && index < 9)// upper side of the board
        {
            tileArray[index].is_Up_DirectionAvailable = false;
            if (index == 0)
            {
                tileArray[index].is_Right_DirectionAvailable = tileArray[index + 1].isWall ? false : true;
                tileArray[index].is_Down_DirectionAvailable = tileArray[index + 9].isWall ? false : true;
            }
            else if (index == 8)
            {
                tileArray[index].is_Left_DirectionAvailable = tileArray[index - 1].isWall ? false : true;
                tileArray[index].is_Down_DirectionAvailable = tileArray[index + 9].isWall ? false : true;
            }
            else
            {
                tileArray[index].is_Right_DirectionAvailable = tileArray[index + 1].isWall ? false : true;
                tileArray[index].is_Left_DirectionAvailable = tileArray[index - 1].isWall ? false : true;
                tileArray[index].is_Down_DirectionAvailable = tileArray[index + 9].isWall ? false : true;
            }
        }
        else if (index % 9 == 0) // left side of the board
        {
            tileArray[index].is_Left_DirectionAvailable = false;
            if (index == 0)
            {
                tileArray[index].is_Right_DirectionAvailable = tileArray[index + 1].isWall ? false : true;
                tileArray[index].is_Down_DirectionAvailable = tileArray[index + 9].isWall ? false : true;
            }
            else if (index == 72)
            {
                tileArray[index].is_Right_DirectionAvailable = tileArray[index + 1].isWall ? false : true;
                tileArray[index].is_Up_DirectionAvailable = tileArray[index - 9].isWall ? false : true;
            }
            else
            {
                tileArray[index].is_Up_DirectionAvailable = tileArray[index - 9].isWall ? false : true;
                tileArray[index].is_Right_DirectionAvailable = tileArray[index + 1].isWall ? false : true;
                tileArray[index].is_Down_DirectionAvailable = tileArray[index + 9].isWall ? false : true;
            }
        }
        else if (index + 1 % 9 == 0) // right side of the board
        {
            tileArray[index].is_Right_DirectionAvailable = false;
            if (index == 8)
            {
                tileArray[index].is_Left_DirectionAvailable = tileArray[index - 1].isWall ? false : true;
                tileArray[index].is_Down_DirectionAvailable = tileArray[index + 9].isWall ? false : true;
            }
            else if (index == 80)
            {
                tileArray[index].is_Left_DirectionAvailable = tileArray[index - 1].isWall ? false : true;
                tileArray[index].is_Up_DirectionAvailable = tileArray[index - 9].isWall ? false : true;
            }
            else
            {
                tileArray[index].is_Up_DirectionAvailable = tileArray[index - 9].isWall ? false : true;
                tileArray[index].is_Left_DirectionAvailable = tileArray[index - 1].isWall ? false : true;
                tileArray[index].is_Down_DirectionAvailable = tileArray[index + 9].isWall ? false : true;
            }
        }
        else if (index >= 72 && index < 81) // down side of the board
        {
            tileArray[index].is_Down_DirectionAvailable = false;
            if (index == 72)
            {
                tileArray[index].is_Right_DirectionAvailable = tileArray[index + 1].isWall ? false : true;
                tileArray[index].is_Up_DirectionAvailable = tileArray[index - 9].isWall ? false : true;
            }
            else if (index == 80)
            {
                tileArray[index].is_Left_DirectionAvailable = tileArray[index - 1].isWall ? false : true;
                tileArray[index].is_Up_DirectionAvailable = tileArray[index - 9].isWall ? false : true;
            }
            else
            {
                tileArray[index].is_Up_DirectionAvailable = tileArray[index - 9].isWall ? false : true;
                tileArray[index].is_Left_DirectionAvailable = tileArray[index - 1].isWall ? false : true;
                tileArray[index].is_Right_DirectionAvailable = tileArray[index + 1].isWall ? false : true;
            }
        }
        else
        {
            tileArray[index].is_Right_DirectionAvailable = tileArray[index + 1].isWall ? false : true;
            tileArray[index].is_Left_DirectionAvailable = tileArray[index - 1].isWall ? false : true;
            tileArray[index].is_Down_DirectionAvailable = tileArray[index + 9].isWall ? false : true;
            tileArray[index].is_Up_DirectionAvailable = tileArray[index - 9].isWall ? false : true;
        }
        Debug.Log("Tile" + index + " : Available routes; Up: " + tileArray[index].is_Up_DirectionAvailable +
            " Down: " + tileArray[index].is_Down_DirectionAvailable + " Left: " + tileArray[index].is_Left_DirectionAvailable +
            "Right: " + tileArray[index].is_Right_DirectionAvailable);

    }

    public void ClearTileList()
    {
        for (int i = 0; i < 81; i++)
        {
            Destroy(tileArray[i].gameObject);
        }
    }
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
