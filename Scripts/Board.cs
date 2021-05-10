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
    public static List<Tile> tileArray;

    public void InitializeTiles(int level)
    {   
        tileArray = new List<Tile>();
        for(int i=0; i < 81; i++)
        {
            if (BoardConstant.levelList[level][i] == 1)
                tilePrefab.isWall = false;
            else
                tilePrefab.isWall = true;
        
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
