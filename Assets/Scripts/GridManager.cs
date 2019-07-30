using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour {

	
	
	public BackgroundTile tilePrefab;
	public DotTouch dotPrefab;
	public GameObject dotHolder;
	public BackgroundTile[,] allTiles;
	//public List<BackgroundTile>
	private Quaternion _rotation;
	private Vector3 _position;
	private Vector3 _positionRight;
	private Vector3 _positionUp;
	private int dotCounter = 0;
	
	[HideInInspector] 
	public List<DotTouch> allDots;
	
	


	
	void Start(){
		Debug.Log ("Starting...");
		allTiles = new BackgroundTile[GameManager.instance.rows, GameManager.instance.cols];
		CreateBoard();
		
	}
	private void CreateBoard()
	{
		
		_rotation = Quaternion.Euler(90,0,0);

		for (int row = 0; row < GameManager.instance.rows; row++) { //Create cell by selected row and height
			for (int col = 0; col < GameManager.instance.cols; col++)
			{
				
				//position.x = (z%2)*Hexagon.innerRadius+x * (Hexagon.innerRadius*2f); ////// If we rotate hexagon 60 degrees, we should use these calculations
				//position.y = z * (Hexagon.outerRadius*1.5f);/////////////////////////////// If we rotate hexagon 60 degrees, we should use these calculations
				_position.x = row*Hexagon.outerRadius *1.5f ; // row counter * outerRadius  
				_position.y = col*Hexagon.innerRadius*2f + Mathf.Cos(Mathf.PI*(Mathf.Pow(row,2)+1)/2)*Hexagon.innerRadius ; //column counter * innerRadius + (0 or -1 * innerRadius)
				_position.z = 0f;
				_positionUp.x = _position.x + Hexagon.outerRadius *0.5f;
				_positionUp.y = _position.y + Hexagon.innerRadius;
				_positionUp.z = 0f;
				_positionRight.x = _position.x + Hexagon.outerRadius;
				_positionRight.y = _position.y;
				_positionRight.z = 0f;
				BackgroundTile backgroundTile =Instantiate(tilePrefab, _position, _rotation);
				backgroundTile.transform.parent = GameManager.instance.grid.transform;
				backgroundTile.name = row + "," + col;
				backgroundTile.coordinateX = row;
				backgroundTile.coordinateY = col;
				allTiles[row, col] = backgroundTile;
				if (row != (GameManager.instance.rows - 1))
				{
					if (row % 2 == 0)
					{
						if (col != (GameManager.instance.cols - 1))
						{
							DotTouch dotright =
								Instantiate(dotPrefab, _positionRight, Quaternion.Euler(Vector3.zero));
							dotright.transform.parent = dotHolder.transform;
							dotright.index = dotCounter++;
							dotright.name = row + "," + col + "." + (row + 1) + "," +
							                (col + 1)+ "." + (row + 1) + "," + col  ;
							allDots.Add(dotright);
							DotTouch dotup = Instantiate(dotPrefab, _positionUp, Quaternion.Euler(Vector3.zero));
							dotup.transform.parent = dotHolder.transform;
							dotup.name = row + "," + col + "." + row + "," + (col + 1) + "." + (row + 1) + "," +
							             (col + 1);
							dotup.index= dotCounter++;
							allDots.Add(dotup);

						}
					}
					else
					{
						if (col != (GameManager.instance.cols - 1))
						{
							DotTouch dotup = Instantiate(dotPrefab, _positionUp, Quaternion.Euler(Vector3.zero));
							dotup.transform.parent = dotHolder.transform;
							dotup.name = row + "," + col + "." + row + "," + (col + 1) + "." + (row + 1) + "," + col;
							dotup.index= dotCounter++;
							allDots.Add(dotup);
						}

						if (col != 0)
						{
							DotTouch dotright =
								Instantiate(dotPrefab, _positionRight, Quaternion.Euler(Vector3.zero));
							dotright.transform.parent = dotHolder.transform;
							dotright.name = row + "," + col + "." + (row + 1) +
							                "," + col +"."+(row + 1) + "," + (col - 1) ;
							dotright.index= dotCounter++;
							allDots.Add(dotright);
						}
					}
				}



			}
		}
	}

	public void newHexagonGenerateFromTop()
	{
/*		_position.x = row*Hexagon.outerRadius *1.5f ; // row counter * outerRadius  
		_position.y = col*Hexagon.innerRadius*2f + Mathf.Cos(Mathf.PI*(Mathf.Pow(row,2)+1)/2)*Hexagon.innerRadius ; //column counter * innerRadius + (0 or -1 * innerRadius)
		_position.z = 0f;*/
	}
}
