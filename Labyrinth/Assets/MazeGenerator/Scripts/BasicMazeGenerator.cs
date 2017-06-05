using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//<summary>
//Basic class for maze generation logic
//</summary>
public abstract class BasicMazeGenerator {
	public int RowCount{ get{ return mMazeRows; } }
	public int ColumnCount { get { return mMazeColumns; } }

	private int mMazeRows;
	private int mMazeColumns;
	private MazeCell[,] mMaze;
	public string s;

	public BasicMazeGenerator(int rows, int columns){
		mMazeRows = Mathf.Abs(rows);
		mMazeColumns = Mathf.Abs(columns);
		if (mMazeRows == 0) {
			mMazeRows = 1;
		}
		if (mMazeColumns == 0) {
			mMazeColumns = 1;
		}
		mMaze = new MazeCell[rows,columns];
		for (int row = 0; row < rows; row++) {
			for(int column = 0; column < columns; column++){
				MazeCell tmp = new MazeCell();
				if(row==0 && column==0)
					tmp.IsStart=true;
				tmp.x=column;
				tmp.y=row;
				mMaze[row,column]=tmp;
			}
		}
	}

	public ArrayList GetGoals(){
		ArrayList goals=new ArrayList();
		for (int row = 0; row < mMazeRows; row++) {
			for(int column = 0; column < mMazeColumns; column++){
				MazeCell tmp = this.GetMazeCell(row,column);
				if(tmp.IsGoal){
					goals.Add(tmp);
				}
			}
		}
		return goals;
	}

	public void SetEnd(){
		int maxValue=0;
		MazeCell end=null;
		ArrayList goals=this.GetGoals();
		foreach(MazeCell m in goals){
			if(m.distance>maxValue){
				maxValue=m.distance;
				end=m;
			}
		}
		if(end!=null){
			end.IsEnd=true;
		}
	}


	public abstract void GenerateMaze();

	public MazeCell GetMazeCell(int row, int column){
		if (row >= 0 && column >= 0 && row < mMazeRows && column < mMazeColumns) {
			return mMaze[row,column];
		}else{
			Debug.Log(row+" "+column);
			throw new System.ArgumentOutOfRangeException();
		}
	}

	protected void SetMazeCell(int row, int column, MazeCell cell){
		if (row >= 0 && column >= 0 && row < mMazeRows && column < mMazeColumns) {
			mMaze[row,column] = cell;
		}else{
			throw new System.ArgumentOutOfRangeException();
		}
	}
}
