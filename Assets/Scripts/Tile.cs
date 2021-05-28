using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Tile : MonoBehaviour
{
    public int index;
    public bool isWall;
    public bool isBomb;
    public Vector2Int location;
    public Color color; // white -> wall, Red -> colored tile, Gray -> non-colored tile
    public bool is_Up_DirectionAvailable;
    public bool is_Left_DirectionAvailable;
    public bool is_Down_DirectionAvailable;
    public bool is_Right_DirectionAvailable;

}
