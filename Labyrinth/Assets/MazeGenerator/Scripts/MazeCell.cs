using UnityEngine;
using System.Collections;

public enum Direction{
	Start,
	Right,
	Front,
	Left,
	Back,
};
//<summary>
//Class for representing concrete maze cell.
//</summary>
public class MazeCell {
	public bool IsVisited = false;
	public bool WallRight = false;
	public bool WallFront = false;
	public bool WallLeft = false;
	public bool WallBack = false;
	public bool IsGoal = false;
	public bool IsStart=false;
	public bool IsEnd=false;

	public int x;
	public int y;
	public int distance=0;

	public int CountWalls(){
		int i=0;
		if(WallBack)
			i++;
		if(WallFront)
			i++;
		if(WallRight)
			i++;
		if(WallLeft)
			i++;
		return i;
	}
}
