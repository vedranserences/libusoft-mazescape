using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
	public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public bool FullRandom = false;
	public int RandomSeed = 12345;
	public GameObject Floor = null;
	public GameObject Wall = null;
	public GameObject Pillar = null;
	public GameObject Teleporter=null;

	public GameObject[] traperinos;
	public int Rows = 5;
	public int Columns = 5;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;
	public GameObject GoalPrefab = null;
	[Range(0,0.5f)]
	public float TrapDensity=0.1f;

	private BasicMazeGenerator mMazeGenerator = null;

	private Vector2 GenerateCoordsForTrap(){
		int x=Random.Range(0,Columns);
		int y=Random.Range(0,Rows);
		while(mMazeGenerator.GetMazeCell(y,x).IsGoal || mMazeGenerator.GetMazeCell(y,x).IsStart || mMazeGenerator.GetMazeCell(y,x).IsEnd){
				x=Random.Range(0,Columns);
				y=Random.Range(0,Rows);
			}
		return new Vector2(x,y);

	}

	private GameObject GetEasyTrap(){
		GameObject trap=null;
		int i=Random.Range(0,2);
			foreach(GameObject go in traperinos){
				if(go.tag=="NeedleTrap" && i%2==0){
					trap=go;
				}
				else if(go.tag=="TrapCutter" && i%2==1){
					trap=go;
				}
			}
		return trap;
	}

	void Start () {
		if (!FullRandom) {
			// Random.seed = RandomSeed;
			Random.InitState(RandomSeed);
		}
		switch (Algorithm) {
		case MazeGenerationAlgorithm.PureRecursive:
			mMazeGenerator = new RecursiveMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RecursiveTree:
			mMazeGenerator = new RecursiveTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RandomTree:
			mMazeGenerator = new RandomTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.OldestTree:
			mMazeGenerator = new OldestTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RecursiveDivision:
			mMazeGenerator = new DivisionMazeGenerator (Rows, Columns);
			break;
		}
		mMazeGenerator.GenerateMaze ();
		HashSet <Vector2> traps=new HashSet<Vector2>();
		int NumberOfTraps=(int)(Rows*Columns*TrapDensity);
		for(int i=0;i<NumberOfTraps;i++){
			while(true){
				if(traps.Add(GenerateCoordsForTrap()))
					break;
			}
		}

		for (int row = 0; row < Rows; row++) {
			for(int column = 0; column < Columns; column++){
				float x = column*(CellWidth+(AddGaps?0.2f:0));
				float z = row*(CellHeight+(AddGaps?0.2f:0));
				MazeCell cell = mMazeGenerator.GetMazeCell(row,column);
				GameObject tmp;
				tmp = Instantiate(Floor,new Vector3(x,0,z), Quaternion.Euler(0,0,0)) as GameObject;
				tmp.transform.localScale=new Vector3(CellHeight,0.1f,CellWidth);
				tmp.transform.parent = transform;

				if(cell.IsStart){
					tmp.GetComponent<Renderer>().material.color=Color.green;
				}
				if(cell.IsEnd){
					GameObject tmp2=Instantiate(Teleporter,new Vector3(x,0,z),Quaternion.Euler(-90,0,0));
					tmp2.transform.localScale=new Vector3(1,1,1);
				}

				foreach(Vector2 v in traps){
					if((int)v.x==column && (int)v.y==row){
						GameObject tempTrap = traperinos[Random.Range(0,traperinos.Length)];
						GameObject tmp2=Instantiate(tempTrap, new Vector3(x, 0, z), Quaternion.Euler(0,0,0));
						if(tmp2.tag=="NeedleTrap"){
							tmp2.transform.position=new Vector3(tmp2.transform.position.x,-0.37f,tmp2.transform.position.z);
						}
						else if(tmp2.tag=="TrapCutter"){
							tmp2.transform.position=new Vector3(tmp2.transform.position.x,-0.70f,tmp2.transform.position.z);
						}
						else if(tmp2.tag=="SpearTrap"){
							if(cell.WallLeft){
								tmp2.transform.position=new Vector3(tmp2.transform.position.x-4.5f,tmp2.transform.position.y+5,tmp2.transform.position.z);
								tmp2.transform.rotation= Quaternion.Euler(90,90,0);
								
							} else if(cell.WallRight){
								tmp2.transform.position=new Vector3(tmp2.transform.position.x+4.5f,tmp2.transform.position.y+5,tmp2.transform.position.z);
								tmp2.transform.rotation= Quaternion.Euler(-90,90,0);
								
							}else if(cell.WallFront){
								tmp2.transform.position=new Vector3(tmp2.transform.position.x,tmp2.transform.position.y+5,tmp2.transform.position.z+4.5f);
								tmp2.transform.rotation= Quaternion.Euler(270,0,0);
								
							}else if(cell.WallBack){
								tmp2.transform.position=new Vector3(tmp2.transform.position.x,tmp2.transform.position.y+5,tmp2.transform.position.z-4.5f);
								tmp2.transform.rotation= Quaternion.Euler(90,90,90);
								
							}else{
								GameObject.Destroy(tmp2);
								tmp2 = Instantiate(GetEasyTrap(), new Vector3(x, 0, z), Quaternion.Euler(0,0,0));
								if(tmp2.tag=="NeedleTrap"){
									tmp2.transform.position=new Vector3(tmp2.transform.position.x,-0.37f,tmp2.transform.position.z);
								}
								else if(tmp2.tag=="TrapCutter"){
									tmp2.transform.position=new Vector3(tmp2.transform.position.x,-0.70f,tmp2.transform.position.z);
								}
							}
							
						}else if(tmp2.tag=="GreatAxeTrap"){
							if(cell.WallFront && cell.WallBack){
								GameObject tmp3 = Instantiate(Pillar, new Vector3(x,CellWidth+1f,z),Quaternion.Euler(90,0,0));
								tmp3.transform.localScale = new Vector3(CellWidth/5,CellWidth,CellWidth/5);

								tmp2.transform.position = new Vector3(tmp3.transform.position.x,tmp3.transform.position.y-1f,tmp3.transform.position.z);
								tmp2.transform.localScale = new Vector3(1,2,1);
								tmp2.transform.rotation =  Quaternion.Euler(0,90,0);
								

							}else if(cell.WallLeft && cell.WallRight){
								GameObject tmp3 = Instantiate(Pillar, new Vector3(x,CellWidth+1f,z),Quaternion.Euler(0,0,90));
								tmp3.transform.localScale = new Vector3(CellWidth/5,CellWidth,CellWidth/5);

								tmp2.transform.position = tmp3.transform.position;
								tmp2.transform.localScale = new Vector3(1,2,1);
								tmp2.transform.rotation =  Quaternion.Euler(0,0,0);
							}else{
								GameObject.Destroy(tmp2);
								tmp2 = Instantiate(GetEasyTrap(), new Vector3(x, 0, z), Quaternion.Euler(0,0,0));
								if(tmp2.tag=="NeedleTrap"){
									tmp2.transform.position=new Vector3(tmp2.transform.position.x,-0.37f,tmp2.transform.position.z);
								}
								else if(tmp2.tag=="TrapCutter"){
									tmp2.transform.position=new Vector3(tmp2.transform.position.x,-0.70f,tmp2.transform.position.z);
								}
							}
							
						}else if(tmp2.tag=="SawTrap02"){
							if(cell.WallFront && cell.WallBack){
								GameObject tmp3 = Instantiate(tmp2, new Vector3(x+2.5f,0,z+2.5f),Quaternion.Euler(0,0,0));
								tmp3.transform.localScale = new Vector3(1,1.5f,1.25f);

								tmp2.transform.position = new Vector3(tmp2.transform.position.x-2.5f,tmp2.transform.position.y,tmp2.transform.position.z-2.5f);
								tmp2.transform.localScale = new Vector3(1,1.5f,1.25f);
								tmp2.transform.rotation =  Quaternion.Euler(0,180,0);
								

							}else if(cell.WallLeft && cell.WallRight){
								GameObject tmp3 = Instantiate(tmp2, new Vector3(x-2.5f,0,z-2.5f),Quaternion.Euler(0,-90,0));
								tmp3.transform.localScale = new Vector3(1,1.5f,1.25f);

								tmp2.transform.position = new Vector3(tmp2.transform.position.x+2.5f,tmp2.transform.position.y,tmp2.transform.position.z+2.5f);
								tmp2.transform.localScale = new Vector3(1,1.5f,1.25f);
								tmp2.transform.rotation =  Quaternion.Euler(0,90,0);
							}else{
								GameObject.Destroy(tmp2);
								tmp2 = Instantiate(GetEasyTrap(), new Vector3(x, 0, z), Quaternion.Euler(0,0,0));
								if(tmp2.tag=="NeedleTrap"){
									tmp2.transform.position=new Vector3(tmp2.transform.position.x,-0.37f,tmp2.transform.position.z);
								}
								else if(tmp2.tag=="TrapCutter"){
									tmp2.transform.position=new Vector3(tmp2.transform.position.x,-0.70f,tmp2.transform.position.z);
								}
							}
						}
					}
				}
				if(cell.WallRight){
					tmp = Instantiate(Wall,new Vector3(x+CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,90,0)) as GameObject;// right
					tmp.transform.localScale = new Vector3(CellWidth/1.1f, CellHeight*2, 1.001f);
					tmp.transform.parent = transform;
				}
				if(cell.WallFront){
					tmp = Instantiate(Wall,new Vector3(x,0,z+CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,0,0)) as GameObject;// front
					tmp.transform.localScale = new Vector3(CellWidth/1.1f, CellHeight*2, 1.001f);
					tmp.transform.parent = transform;
				}
				if(cell.WallLeft){
					tmp = Instantiate(Wall,new Vector3(x-CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,270,0)) as GameObject;// left
					tmp.transform.localScale = new Vector3(CellWidth/1.1f, CellHeight*2, 1.001f);
					tmp.transform.parent = transform;
				}
				if(cell.WallBack){
					tmp = Instantiate(Wall,new Vector3(x,0,z-CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,180,0)) as GameObject;// back
					tmp.transform.localScale = new Vector3(CellWidth/1.1f, CellHeight*2, 1.001f);
					tmp.transform.parent = transform;
				}
				if(cell.IsGoal && GoalPrefab != null){
					// tmp = Instantiate(GoalPrefab,new Vector3(x,1,z), Quaternion.Euler(0,0,0)) as GameObject;
					// tmp.transform.parent = transform;
					// Debug.Log("("+cell.x+","+cell.y+")="+cell.distance);
				}
				
			}
		}
		if(Pillar != null){
			for (int row = 0; row < Rows+1; row++) {
				for (int column = 0; column < Columns+1; column++) {
					float x = column*(CellWidth+(AddGaps?0.2f:0));
					float z = row*(CellHeight+(AddGaps?0.2f:0));
					GameObject tmp = Instantiate(Pillar,new Vector3(x-CellWidth/2,0,z-CellHeight/2),Quaternion.identity) as GameObject;
					tmp.transform.localScale = new Vector3(1, CellHeight*2, 1);
					tmp.transform.parent = transform;
				}
			}
		}
	}
}
