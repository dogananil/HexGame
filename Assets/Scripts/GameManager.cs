using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

	public static GameManager instance  = null; // It is for that you can use every puplic variable and function of GameManager in another scripts
	public int rows;
	public int cols ;
	public bool right = false;
	public GridManager grid;
	private Renderer[] _rend;
	private int rotateCounter;
	private List<SpriteRenderer> _selected ;
	private int _hex1X, _hex1Y, _hex2X, _hex2Y, _hex3X, _hex3Y;
	private string[] firstHex;
	private string[] secondHex;
	private string[] thirdHex;
	
	private BackgroundTile tempTile;
	public HexCell[,] allHexagons;
	public bool RotationOrNot;
	private bool popOneMoveBefore;
	private List<HexCell> deletedHex;
	private List<BackgroundTile> deletedTile;
	private int indexDeleted;





	private int previndex=0;
	// Use this for initialization
	void Awake(){
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}
	void Start () {
		allHexagons = new HexCell[rows,cols];
		_selected = new List<SpriteRenderer>();
		deletedHex = new List<HexCell>();
		deletedTile =new List<BackgroundTile>();
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		//popSameColor();
		//StartCoroutine(Example());
		//replaceAllHex();
		

	}

	private IEnumerator Example()
	{
		yield return new WaitForSeconds(0.5f);
	}
	public void SelectHexagons(string[]split,int dot_index)
	{
		deletedHex.Clear();
		deletedTile.Clear();
		indexDeleted = 0;
		firstHex = split[0].Split(',');
		secondHex = split[1].Split(',');
		thirdHex = split[2].Split(',');
		if (_hex1X == Convert.ToInt32(firstHex[0]) && _hex1Y == Convert.ToInt32(firstHex[1]) &&
		    _hex2X == Convert.ToInt32(secondHex[0]) && _hex2Y == Convert.ToInt32(secondHex[1]))
		{
			turnHexagons(right,dot_index);
		}
		else
		{
			right = true && !right;
			
				
			
			//wait until setPatents
			if (!popOneMoveBefore)
			{
				StartCoroutine(WaitUntilPrevHexReset(right));
			}
			
			

				previndex = dot_index;
			if (_selected.Count!=0)
			{
				_selected[0].color=Color.black;
				_selected[1].color = Color.black;
				_selected[2].color=Color.black;
				_selected.Clear();
			}
			
			_hex1X=Convert.ToInt32(firstHex[0]);
			_hex1Y=Convert.ToInt32(firstHex[1]);
			_hex2X=Convert.ToInt32(secondHex[0]);
			_hex2Y=Convert.ToInt32(secondHex[1]);
			_hex3X=Convert.ToInt32(thirdHex[0]);
			_hex3Y=Convert.ToInt32(thirdHex[1]);

			SpriteRenderer rendhex1 =grid.allTiles[_hex1X, _hex1Y].transform.GetChild(0).GetComponent<SpriteRenderer>();
			rendhex1.color = Color.white;
			SpriteRenderer rendhex2 =grid.allTiles[_hex2X, _hex2Y].transform.GetChild(0).GetComponent<SpriteRenderer>();
			rendhex2.color = Color.white;
			SpriteRenderer rendhex3 =grid.allTiles[_hex3X, _hex3Y].transform.GetChild(0).GetComponent<SpriteRenderer>();
			rendhex3.color = Color.white;
			_selected.Add(rendhex1);
			_selected.Add(rendhex2);
			_selected.Add(rendhex3);
		
		}
		
	}

	private void turnHexagons( bool rightOrLeft,int dotIndex)
	{

		HexCell tempHexagon;
		StartCoroutine(WaitUntilCurrentHexSetParent(dotIndex));
		if (right == true)
		{
			
			tempTile = grid.allTiles[_hex3X, _hex3Y];
			tempHexagon = allHexagons[_hex3X, _hex3Y];
			
			allHexagons[_hex3X, _hex3Y] = allHexagons[_hex2X, _hex2Y];
			grid.allTiles[_hex3X, _hex3Y] = grid.allTiles[_hex2X, _hex2Y];
			
			grid.allTiles[_hex3X, _hex3Y].coordinateX = _hex3X;
			grid.allTiles[_hex3X, _hex3Y].coordinateY = _hex3Y;
			
			allHexagons[_hex2X, _hex2Y] = allHexagons[_hex1X, _hex1Y];
			grid.allTiles[_hex2X, _hex2Y] = grid.allTiles[_hex1X, _hex1Y];
			
			grid.allTiles[_hex2X, _hex2Y].coordinateX = _hex2X;
			grid.allTiles[_hex2X, _hex2Y].coordinateY = _hex2Y;

			allHexagons[_hex1X, _hex1Y] = tempHexagon;
			grid.allTiles[_hex1X, _hex1Y] = tempTile;
			
			grid.allTiles[_hex1X, _hex1Y].coordinateX = _hex1X;
			grid.allTiles[_hex1X, _hex1Y].coordinateY = _hex1Y;
			//Debug.Log("Coordinates are Hex "+ _hex1X + "," + _hex1Y + " ->>>>>>>>>>>>>>>>>  x = " +grid.allTiles[_hex1X, _hex1Y].coordinateX + "   y = " + grid.allTiles[_hex1X, _hex1Y].coordinateY);
			//Debug.Log("Coordinates are Hex "+ _hex2X + "," + _hex2Y + " ->>>>>>>>>>>>>>>>>  x = " +grid.allTiles[_hex2X, _hex2Y].coordinateX + "   y = " + grid.allTiles[_hex2X, _hex2Y].coordinateY);
			//Debug.Log("Coordinates are Hex "+ _hex3X + "," + _hex3Y + " ->>>>>>>>>>>>>>>>>  x = " +grid.allTiles[_hex3X, _hex3Y].coordinateX + "   y = " + grid.allTiles[_hex3X, _hex3Y].coordinateY);
			
			
			StartCoroutine(rotateHexs(right,dotIndex));
			
			rotateCounter++;
		}
		else
		{
			tempTile = grid.allTiles[_hex2X, _hex2Y];
			grid.allTiles[_hex2X, _hex2Y] = grid.allTiles[_hex3X, _hex3Y];
			grid.allTiles[_hex2X, _hex2Y].coordinateX = _hex2X;
			grid.allTiles[_hex2X, _hex2Y].coordinateY = _hex2Y;
			grid.allTiles[_hex3X, _hex3Y] = grid.allTiles[_hex1X, _hex1Y];
			grid.allTiles[_hex3X, _hex3Y].coordinateX = _hex3X;
			grid.allTiles[_hex3X, _hex3Y].coordinateY = _hex3Y;
			grid.allTiles[_hex1X, _hex1Y] = tempTile;
			grid.allTiles[_hex1X, _hex1Y].coordinateX = _hex1X;
			grid.allTiles[_hex1X, _hex1Y].coordinateY = _hex1Y;
			
			tempHexagon = allHexagons[_hex2X, _hex2Y];
			allHexagons[_hex2X, _hex2Y] = allHexagons[_hex3X, _hex3Y];
			allHexagons[_hex3X, _hex3Y] = allHexagons[_hex1X, _hex1Y];
			allHexagons[_hex1X, _hex1Y] = tempHexagon;
			
			
			StartCoroutine(rotateHexs(right,dotIndex));
			
			rotateCounter++;

		}
	}
	
	private bool popSameColor(Renderer rend,int x,int y)
	{
		
		switch (checkColor(rend, x, y))
		{
			case "W0":
				
				grid.allTiles[x,y].transform.gameObject.SetActive(false);
				grid.allTiles[x-1,y].transform.gameObject.SetActive(false);
				grid.allTiles[x-1,y+1].transform.gameObject.SetActive(false);
				Debug.Log("W0");
				if (!deletedHex.Contains(allHexagons[x, y]))
				{
					deletedHex.Add(allHexagons[x,y]);
					deletedTile.Add(grid.allTiles[x,y]);
					allHexagons[x, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x-1, y]))
				{
					deletedHex.Add(allHexagons[x-1,y]);
					deletedTile.Add(grid.allTiles[x-1,y]);
					allHexagons[x-1,y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x-1, y+1]))
				{
					deletedHex.Add(allHexagons[x-1,y+1]);
					deletedTile.Add(grid.allTiles[x-1,y+1]);
					allHexagons[x-1, y+1].deleted= true;
				}
				return true;
			
			case "SW0":
			
				grid.allTiles[x,y].transform.gameObject.SetActive(false);
				grid.allTiles[x,y-1].transform.gameObject.SetActive(false);
				grid.allTiles[x-1,y].transform.gameObject.SetActive(false);
				
				
				if (!deletedHex.Contains(allHexagons[x, y]))
				{
					deletedHex.Add(allHexagons[x,y]);
					deletedTile.Add(grid.allTiles[x,y]);
					allHexagons[x, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x, y-1]))
				{
					deletedHex.Add(allHexagons[x,y-1]);
					deletedTile.Add(grid.allTiles[x,y-1]);
					allHexagons[x, y-1].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x-1, y]))
				{
					deletedHex.Add(allHexagons[x-1,y]);
					deletedTile.Add(grid.allTiles[x-1,y]);
					allHexagons[x-1, y].deleted= true;
				}
				return true;
			
			
			case "E0":
				
				grid.allTiles[x,y].transform.gameObject.SetActive(false);
				grid.allTiles[x+1,y].transform.gameObject.SetActive(false);
				grid.allTiles[x+1,y+1].transform.gameObject.SetActive(false);
				
				if (!deletedHex.Contains(allHexagons[x, y]))
				{
					deletedHex.Add(allHexagons[x,y]);
					deletedTile.Add(grid.allTiles[x,y]);
					allHexagons[x, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x+1, y]))
				{
					deletedHex.Add(allHexagons[x+1,y]);
					deletedTile.Add(grid.allTiles[x+1,y]);
					allHexagons[x+1, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x+1, y+1]))
				{
					deletedHex.Add(allHexagons[x+1,y+1]);
					deletedTile.Add(grid.allTiles[x+1,y+1]);
					allHexagons[x+1, y+1].deleted= true;
				}
				return true;
			
			
			case "NE0":
				
				grid.allTiles[x,y].transform.gameObject.SetActive(false);
				grid.allTiles[x+1,y+1].transform.gameObject.SetActive(false);
				grid.allTiles[x,y+1].transform.gameObject.SetActive(false);
				
				if (!deletedHex.Contains(allHexagons[x, y]))
				{
					deletedHex.Add(allHexagons[x, y]);
					deletedTile.Add(grid.allTiles[x,y]);
					allHexagons[x, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x, y+1]))
				{
					deletedHex.Add(allHexagons[x, y+1]);
					deletedTile.Add(grid.allTiles[x,y+1]);
					allHexagons[x, y+1].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x+1, y+1]))
				{
					deletedHex.Add(allHexagons[x+1, y+1]);
					deletedTile.Add(grid.allTiles[x+1,y+1]);
					allHexagons[x+1, y+1].deleted= true;
				}
				
				return true;
			
			case "SE0":
				
				grid.allTiles[x,y].transform.gameObject.SetActive(false);
				grid.allTiles[x,y-1].transform.gameObject.SetActive(false);
				grid.allTiles[x+1,y].transform.gameObject.SetActive(false);
				
				if (!deletedHex.Contains(allHexagons[x, y]))
				{
					deletedHex.Add(allHexagons[x, y]);
					deletedTile.Add(grid.allTiles[x,y]);
					allHexagons[x, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x+1, y]))
				{
					deletedHex.Add(allHexagons[x+1, y]);
					deletedTile.Add(grid.allTiles[x+1,y]);
					allHexagons[x+1, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x, y-1]))
				{
					deletedHex.Add(allHexagons[x, y-1]);
					deletedTile.Add(grid.allTiles[x,y-1]);
					allHexagons[x, y-1].deleted= true;
				}
				return true;
			
			case "NW0":
				
				grid.allTiles[x,y].transform.gameObject.SetActive(false);
				grid.allTiles[x,y+1].transform.gameObject.SetActive(false);
				grid.allTiles[x-1,y+1].transform.gameObject.SetActive(false);
				
				if (!deletedHex.Contains(allHexagons[x, y]))
				{
					deletedHex.Add(allHexagons[x, y]);
					deletedTile.Add(grid.allTiles[x,y]);
					allHexagons[x, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x, y+1]))
				{
					deletedHex.Add(allHexagons[x, y+1]);
					deletedTile.Add(grid.allTiles[x,y+1]);
					allHexagons[x, y+1].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x-1, y+1]))
				{
					deletedHex.Add(allHexagons[x-1, y+1]);
					deletedTile.Add(grid.allTiles[x-1,y+1]);
					allHexagons[x-1, y+1].deleted= true;
				}
				
				return true;
			
			case "W1":
				
				grid.allTiles[x,y].transform.gameObject.SetActive(false);
				grid.allTiles[x-1,y-1].transform.gameObject.SetActive(false);
				grid.allTiles[x-1,y].transform.gameObject.SetActive(false);
				
				if (!deletedHex.Contains(allHexagons[x, y]))
				{
					deletedHex.Add(allHexagons[x, y]);
					deletedTile.Add(grid.allTiles[x,y]);
					allHexagons[x, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x-1, y-1]))
				{
					deletedHex.Add(allHexagons[x-1, y-1]);
					deletedTile.Add(grid.allTiles[x-1,y-1]);
					allHexagons[x-1, y-1].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x-1, y]))
				{
					deletedHex.Add(allHexagons[x-1, y]);
					deletedTile.Add(grid.allTiles[x-1,y]);
					allHexagons[x-1, y].deleted= true;
				}

				return true;
			case "SW1":
				
				grid.allTiles[x,y].transform.gameObject.SetActive(false);
				grid.allTiles[x,y-1].transform.gameObject.SetActive(false);
				grid.allTiles[x-1,y-1].transform.gameObject.SetActive(false);
				
				if (!deletedHex.Contains(allHexagons[x, y]))
				{
					deletedHex.Add(allHexagons[x, y]);
					deletedTile.Add(grid.allTiles[x,y]);
					allHexagons[x, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x, y-1]))
				{
					deletedHex.Add(allHexagons[x, y-1]);
					deletedTile.Add(grid.allTiles[x,y-1]);
					allHexagons[x, y-1].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x-1, y-1]))
				{
					deletedHex.Add(allHexagons[x-1, y-1]);
					deletedTile.Add(grid.allTiles[x-1,y-1]);
					allHexagons[x-1, y-1].deleted= true;
				}
				return true;
			
			
			case "E1":
			
				grid.allTiles[x,y].transform.gameObject.SetActive(false);
				grid.allTiles[x+1,y].transform.gameObject.SetActive(false);
				grid.allTiles[x+1,y-1].transform.gameObject.SetActive(false);
				
				if (!deletedHex.Contains(allHexagons[x, y]))
				{
					deletedHex.Add(allHexagons[x, y]);
					deletedTile.Add(grid.allTiles[x,y]);
					allHexagons[x, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x+1, y]))
				{
					deletedHex.Add(allHexagons[x+1, y]);
					deletedTile.Add(grid.allTiles[x+1,y]);
					allHexagons[x+1, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x+1, y-1]))
				{
					deletedHex.Add(allHexagons[x+1, y-1]);
					deletedTile.Add(grid.allTiles[x+1,y-1]);
					allHexagons[x+1, y-1].deleted= true;
				}
				return true;
			
			
			case "NE1":
			
				grid.allTiles[x,y].transform.gameObject.SetActive(false);
				grid.allTiles[x+1,y].transform.gameObject.SetActive(false);
				grid.allTiles[x,y+1].transform.gameObject.SetActive(false);
				
				if (!deletedHex.Contains(allHexagons[x, y]))
				{
					deletedHex.Add(allHexagons[x, y]);
					deletedTile.Add(grid.allTiles[x,y]);
					allHexagons[x, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x+1, y]))
				{
					deletedHex.Add(allHexagons[x+1, y]);
					deletedTile.Add(grid.allTiles[x+1,y]);
					allHexagons[x+1, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x, y+1]))
				{
					deletedHex.Add(allHexagons[x, y+1]);
					deletedTile.Add(grid.allTiles[x,y+1]);
					allHexagons[x, y+1].deleted= true;
				}
				return true;
			
			case "SE1":
				
				grid.allTiles[x,y].transform.gameObject.SetActive(false);
				grid.allTiles[x,y-1].transform.gameObject.SetActive(false);
				grid.allTiles[x+1,y-1].transform.gameObject.SetActive(false);
				
				if (!deletedHex.Contains(allHexagons[x, y]))
				{
					deletedHex.Add(allHexagons[x, y]);
					deletedTile.Add(grid.allTiles[x,y]);
					allHexagons[x, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x, y-1]))
				{
					deletedHex.Add(allHexagons[x, y-1]);
					deletedTile.Add(grid.allTiles[x,y-1]);
					allHexagons[x, y-1].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x+1, y-1]))
				{
					deletedHex.Add(allHexagons[x+1, y-1]);
					deletedTile.Add(grid.allTiles[x+1,y-1]);
					allHexagons[x+1, y-1].deleted= true;
				}
				return true;
			
			case "NW1":
				
				grid.allTiles[x,y].transform.gameObject.SetActive(false);
				grid.allTiles[x-1,y].transform.gameObject.SetActive(false);
				grid.allTiles[x,y+1].transform.gameObject.SetActive(false);
				
				if (!deletedHex.Contains(allHexagons[x, y]))
				{
					deletedHex.Add(allHexagons[x, y]);
					deletedTile.Add(grid.allTiles[x,y]);
					allHexagons[x, y].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x, y+1]))
				{
					deletedHex.Add(allHexagons[x, y+1]);
					deletedTile.Add(grid.allTiles[x,y+1]);
					allHexagons[x, y+1].deleted= true;
				}
				if (!deletedHex.Contains(allHexagons[x-1, y]))
				{
					deletedHex.Add(allHexagons[x-1, y]);
					deletedTile.Add(grid.allTiles[x-1,y]);
					allHexagons[x-1, y].deleted= true;
				}
				return true;
			case "NoSameColor":
				return false;
		}

		return false;
	}
	private IEnumerator rotateHexs(bool right,int dotIndex)
	{
		
		float startRotation = grid.dotHolder.transform.GetChild(dotIndex).transform.eulerAngles.z;
		float endRotation=0;
		if (right)
		{
			endRotation = startRotation - 120.0f;
		}
		else
		{
			endRotation = startRotation + 120.0f;
		}
		float t = 0.0f;
		float zRotation = 0.0f;
		while (t<1)
			{
				t += Time.deltaTime;
				zRotation = Mathf.Lerp(startRotation, endRotation, t ) % 360.0f;
				 if (right == true)
				 {
					 
					 grid.dotHolder.transform.GetChild(dotIndex).transform.eulerAngles = new Vector3(grid.dotHolder.transform.GetChild(dotIndex).transform.eulerAngles.x,grid.dotHolder.transform.GetChild(dotIndex).transform.eulerAngles.y, zRotation);
				 }
				 else
				 {
					
					 grid.dotHolder.transform.GetChild(dotIndex).transform.eulerAngles = new Vector3(grid.dotHolder.transform.GetChild(dotIndex).transform.eulerAngles.x,grid.dotHolder.transform.GetChild(dotIndex).transform.eulerAngles.y,zRotation);
				 }
				//grid.dotHolder.transform.GetChild(dotIndex).transform.Rotate(Vector3.back,1,Space.World);
				yield return null;

			}

		if (right)
		{
			popOneMoveBefore = false;
			Renderer rend1 = allHexagons[_hex1X, _hex1Y].GetComponent<Renderer>();
			while (popSameColor(rend1, _hex1X, _hex1Y))
			{
				popOneMoveBefore = true;
			}
			Renderer rend2 = allHexagons[_hex2X, _hex2Y].GetComponent<Renderer>();
			while (popSameColor(rend2, _hex2X, _hex2Y))
			{
				popOneMoveBefore = true;
			}


			Renderer rend3 = allHexagons[_hex3X, _hex3Y].GetComponent<Renderer>();
			while (popSameColor(rend3, _hex3X, _hex3Y))
			{
				popOneMoveBefore = true;
			}
			
		}
		else
		{
			popOneMoveBefore = false;
			Renderer rend1 = allHexagons[_hex1X, _hex1Y].GetComponent<Renderer>();
			
			while (popSameColor(rend1, _hex1X, _hex1Y))
			{
				popOneMoveBefore = true;
			}
			
			Renderer rend2 = allHexagons[_hex2X, _hex2Y].GetComponent<Renderer>();
			while (popSameColor(rend2, _hex2X, _hex2Y))
			{
				popOneMoveBefore = true;
			}
			
			Renderer rend3 = allHexagons[_hex3X, _hex3Y].GetComponent<Renderer>();
			while (popSameColor(rend3, _hex3X, _hex3Y))
			{
				popOneMoveBefore = true;
			}
		}
		replaceAllHex();
		
		
	}
	private void replaceAllHex( )//replace columns after pop same colors
	{
		int positionDown;
		//Vector3 newposition;
		
		bool foundDeleted = false;
		StartCoroutine(WaitUntilCurrentHexParentReset());
		for (int i = 0; i < rows; i++)
		{
			foundDeleted = false;
			positionDown = 0;
			for (int j = 0; j < cols; j++)
			{
				if (deletedHex.Contains(allHexagons[i, j]))
				{
					grid.allTiles[i, j] = null;
					positionDown++;
					foundDeleted = true;
					continue;
				}
				else
				{
					if (grid.allTiles[i, j] == null)
					{
						break;
					}
					if (foundDeleted)
					{
						//Debug.Log(i+","+j);
						allHexagons[i, j - positionDown] = allHexagons[i, j];
						allHexagons[i, j - positionDown].deleted = false;
						allHexagons[i, j].deleted = true;
						grid.allTiles[i, j - positionDown] = grid.allTiles[i, j];
						//Debug.Log("i ve j " + grid.allTiles[i, j].coordinateY);
						//Debug.Log(" i ve j-positionDown" + grid.allTiles[i, j - positionDown]);
						grid.allTiles[i, j - positionDown].coordinateY = j - positionDown;
						grid.allTiles[i, j - positionDown].coordinateX = i;
						grid.allTiles[i, j] = null;
						//Debug.Log("i	= " +i + "j-positionDown "+(j-positionDown));
						
						//newposition= new Vector3(grid.allTiles[i,j-positionDown].transform.localPosition.x,(j-positionDown)*Hexagon.innerRadius*2f + Mathf.Cos(Mathf.PI*(Mathf.Pow(i,2)+1)/2)*Hexagon.innerRadius ,grid.allTiles[i,j-positionDown].transform.localPosition.z);

						StartCoroutine(positionDownAnim(i, j, positionDown,false,j*Hexagon.innerRadius*2f + Mathf.Cos(Mathf.PI*(Mathf.Pow(i,2)+1)/2)*Hexagon.innerRadius));
						//grid.allTiles[i, j - positionDown].transform.localPosition = newposition;
						
					}
				}
			}
			refillAllBoard(i);
		}
		
		//deletedHex.Clear();
		//Debug.Log("Length" + deletedHex.Count);

		
	}

	private IEnumerator positionDownAnim(int i, int j, int positionDown,bool refillOrNot,float startPositiony)
	{
		float t = 0.0f;
		Vector3 startPosition;
		if (refillOrNot)
		{
			 startPosition = new Vector3( i*Hexagon.outerRadius *1.5f,10*Hexagon.innerRadius*2f + Mathf.Cos(Mathf.PI*(Mathf.Pow(i,2)+1)/2)*Hexagon.innerRadius,0);

		}
		else
		{
			 startPosition = new Vector3( i*Hexagon.outerRadius *1.5f,startPositiony,0);

		}
		Vector3 targetPosition = new Vector3(i*Hexagon.outerRadius *1.5f,(j-positionDown)*Hexagon.innerRadius*2f + Mathf.Cos(Mathf.PI*(Mathf.Pow(i,2)+1)/2)*Hexagon.innerRadius);
		while(t<1)
		{
			t += Time.deltaTime;
			grid.allTiles[i, j - positionDown].transform.localPosition =Vector3.Lerp(startPosition, targetPosition, t);

			yield return null;
		}

	}
	private void refillAllBoard(int i)
	{
		Renderer rend;
		
		for (int j = 0; j < cols; j++)
		{
			
			if (grid.allTiles[i, j] == null)
			{
				//Debug.Log("j = " + j);
				//Debug.Log("deleted tile  " + deletedTile[indexDeleted]);
				//Debug.Log("deleted tile Length " + deletedTile.Count);
				//Debug.Log("İndex Deleted= : " +indexDeleted);
				grid.allTiles[i, j] = deletedTile[indexDeleted];
				grid.allTiles[i, j].coordinateY = j;
				grid.allTiles[i, j].coordinateX = i;
				allHexagons[i, j] = deletedHex[indexDeleted];
				allHexagons[i, j].deleted = false;
				grid.allTiles[i, j].transform.gameObject.SetActive(true);
				rend = allHexagons[i,j].GetComponent<Renderer>();
				
				
				int random =   Mathf.RoundToInt (Random.Range (0.0f, 4.0f));
				Debug.Log("random  " + random);
				rend.material.color = grid.allTiles[i,j].colors[random];
			//	if (i > 0 )
			//	{
					while (GameManager.instance.checkColor(rend,i,j)!="NoSameColor")
					{
						random =   Mathf.RoundToInt (Random.Range (0.0f, 4.0f));
						rend.material.color = grid.allTiles[i,j].colors[random];
						Debug.Log(" Random 2   " + random + " j = " + j);
						
					}
				//}

				
				indexDeleted++;
				StartCoroutine(positionDownAnim(i, j, 0, true, 0));
				
			}

			
		}
	}
	
	private IEnumerator WaitUntilPrevHexReset(bool right)
	{
		
		tempTile = grid.allTiles[_hex1X, _hex1Y];
		if (rotateCounter % 3 == 1 && right==false || rotateCounter%3==2 && right==true)//if rotate clockwise once or counterclocwise twice
		{
			
			
			grid.allTiles[_hex1X, _hex1Y] = grid.allTiles[_hex2X, _hex2Y];
			grid.allTiles[_hex1X, _hex1Y].coordinateX = _hex1X;
			grid.allTiles[_hex1X, _hex1Y].coordinateY = _hex1Y;
			
			grid.allTiles[_hex2X, _hex2Y] = grid.allTiles[_hex3X, _hex3Y];
			grid.allTiles[_hex2X, _hex2Y].coordinateX = _hex2X;
			grid.allTiles[_hex2X, _hex2Y].coordinateY = _hex2Y;

			
			grid.allTiles[_hex3X, _hex3Y] = tempTile;
			grid.allTiles[_hex3X, _hex3Y].coordinateX = _hex3X;
			grid.allTiles[_hex3X, _hex3Y].coordinateY =_hex3Y;

			
		}
		else if (rotateCounter % 3 == 2 && right==false || rotateCounter%3==1 && right==true)//if rotate clockwise twice or counterclockwise once
		{
			
			grid.allTiles[_hex1X, _hex1Y] = grid.allTiles[_hex3X, _hex3Y];
			grid.allTiles[_hex1X, _hex1Y].coordinateX = _hex1X;
			grid.allTiles[_hex1X, _hex1Y].coordinateY = _hex1Y;

			grid.allTiles[_hex3X, _hex3Y] = grid.allTiles[_hex2X, _hex2Y];
			grid.allTiles[_hex3X, _hex3Y].coordinateX = _hex3X;
			grid.allTiles[_hex3X, _hex3Y].coordinateY = _hex3Y;

			grid.allTiles[_hex2X, _hex2Y]=tempTile;
			grid.allTiles[_hex2X, _hex2Y].coordinateX=_hex2X;
			grid.allTiles[_hex2X, _hex2Y].coordinateY=_hex2Y;

		}
			
			
		rotateCounter = 0;
		grid.dotHolder.transform.GetChild(previndex).transform.rotation=Quaternion.Euler(Vector3.zero);
		//Debug.Log("Coordinates of hexagons previous " + _hex1X +","+_hex1Y+"      " +_hex2X +"," +_hex2Y +"       " +_hex3X +","+ _hex3Y);
		yield return StartCoroutine(WaitUntilCurrentHexParentReset());
	}

	private IEnumerator WaitUntilCurrentHexParentReset()
	{
		grid.allTiles[_hex1X, _hex1Y].transform.SetParent(grid.transform);
		grid.allTiles[_hex2X,_hex2Y].transform.SetParent(grid.transform);
		grid.allTiles[_hex3X,_hex3Y].transform.SetParent(grid.transform);
		yield return new WaitForSeconds(2f);
	}
	private IEnumerator WaitUntilCurrentHexSetParent(int dotIndex)
	{
		grid.allTiles[_hex1X, _hex1Y].transform.SetParent(grid.dotHolder.transform.GetChild(dotIndex).transform);
		grid.allTiles[_hex2X,_hex2Y].transform.SetParent(grid.dotHolder.transform.GetChild(dotIndex).transform);
		grid.allTiles[_hex3X,_hex3Y].transform.SetParent(grid.dotHolder.transform.GetChild(dotIndex).transform);
		yield return new WaitForSeconds(2f);
	}
	public string checkColor(Renderer rend,int i,int j)
	{
		Renderer rend1, rend2;
		if (i % 2 == 0 )
		{
			if (i > 0)
			{
				if (j < cols - 1)
				{
					if (allHexagons[i - 1, j].deleted&& allHexagons[i - 1, j + 1].deleted && allHexagons[i,j].deleted)
					{
						
					}
					else
					{
						rend1 = GameManager.instance.allHexagons[i - 1, j].GetComponent<Renderer>();
						rend2 = GameManager.instance.allHexagons[i - 1, j + 1].GetComponent<Renderer>();
						if (rend.material.color == rend1.material.color && rend.material.color == rend2.material.color)
						{
							return "W0"; // West for even col
						}
					}
					
				}
				if ( j > 0)
				{
					if (allHexagons[i , j-1].deleted&& allHexagons[i - 1, j ].deleted && allHexagons[i,j].deleted)
					{
						
						
					}
					else
					{
						rend1 = GameManager.instance.allHexagons[i, j - 1].GetComponent<Renderer>();
						rend2 = GameManager.instance.allHexagons[i - 1, j].GetComponent<Renderer>();
						if (rend.material.color == rend1.material.color && rend.material.color == rend2.material.color)
						{
							return "SW0"; // South West for even col
						}
					}
				}
			}

			if (RotationOrNot) // Check color for after rotate hexagons to pop them
			{
				if (i < (rows - 1) && j < (cols - 1))
				{
					if (allHexagons[i + 1, j].deleted&& allHexagons[i + 1, j + 1].deleted && allHexagons[i,j].deleted)
					{
						
						
					}
					else
					{
						rend1 = GameManager.instance.allHexagons[i + 1, j + 1].GetComponent<Renderer>();
						rend2 = GameManager.instance.allHexagons[i + 1, j].GetComponent<Renderer>();
						if (rend.material.color == rend1.material.color && rend.material.color == rend2.material.color)
						{
							return "E0"; // East for even col
						}
					}

					if (allHexagons[i , j+1].deleted&& allHexagons[i + 1, j + 1].deleted && allHexagons[i,j].deleted)
					{
						
						
					}
					else
					{
						rend1 = GameManager.instance.allHexagons[i , j+1].GetComponent<Renderer>();
						rend2 = GameManager.instance.allHexagons[i + 1, j+1].GetComponent<Renderer>();
						if (rend.material.color==rend1.material.color && rend.material.color== rend2.material.color)
						{
							return "NE0";//North East for even col
						} 
					}

					if (j > 0)
					{
						if (allHexagons[i , j-1].deleted&& allHexagons[i + 1, j ].deleted && allHexagons[i,j].deleted)
						{
						
						
						}
						else
						{
							rend1 = GameManager.instance.allHexagons[i, j - 1].GetComponent<Renderer>();
							rend2 = GameManager.instance.allHexagons[i + 1, j].GetComponent<Renderer>();
							if (rend.material.color == rend1.material.color &&
							    rend.material.color == rend2.material.color)
							{
								return "SE0"; // South West for even col
							}
						}
					}
					if (i > 0 )
					{
						if (allHexagons[i -1, j+1].deleted&& allHexagons[i , j + 1].deleted && allHexagons[i,j].deleted)
						{
						
						
						}
						else
						{
							rend1 = GameManager.instance.allHexagons[i - 1, j + 1].GetComponent<Renderer>();
							rend2 = GameManager.instance.allHexagons[i, j + 1].GetComponent<Renderer>();
							if (rend.material.color == rend1.material.color &&
							    rend.material.color == rend2.material.color)
							{
								return "NW0"; // North West for even col
							}
						}
					}
				}
			}

		}
		else
		{
			if (j > 0) // Check color for creating part 
			{
				if (allHexagons[i-1 , j-1].deleted&& allHexagons[i - 1, j ].deleted && allHexagons[i,j].deleted)
				{
						
						
				}
				else
				{
					rend1 = GameManager.instance.allHexagons[i - 1, j - 1].GetComponent<Renderer>();
					rend2 = GameManager.instance.allHexagons[i - 1, j].GetComponent<Renderer>();
					if (rend.material.color == rend1.material.color && rend.material.color == rend2.material.color)
					{
						return "W1"; // West for odd column
					}
				}
				if (allHexagons[i , j-1].deleted&& allHexagons[i - 1, j - 1].deleted && allHexagons[i,j].deleted)
				{
						
						
				}
				else
				{
					rend1 = GameManager.instance.allHexagons[i, j - 1].GetComponent<Renderer>();
					rend2 = GameManager.instance.allHexagons[i - 1, j - 1].GetComponent<Renderer>();
					if (rend.material.color == rend1.material.color && rend.material.color == rend2.material.color)
					{
						return "SW1"; // South West for odd column
					}
				}
			}
			
			if (RotationOrNot)// Check color for after rotate hexagons to pop them
			{
				
				if (j != 0 && i<(rows-1))
				{
					if (allHexagons[i +1, j-1].deleted&& allHexagons[i + 1, j].deleted && allHexagons[i,j].deleted)
					{
						
						
					}
					else
					{
						rend1 = GameManager.instance.allHexagons[i + 1, j - 1].GetComponent<Renderer>();
						rend2 = GameManager.instance.allHexagons[i + 1, j].GetComponent<Renderer>();
						if (rend.material.color == rend1.material.color && rend.material.color == rend2.material.color)
						{
							return "E1"; // West for odd column
						}
					}
					if (allHexagons[i +1, j-1].deleted&& allHexagons[i , j - 1].deleted && allHexagons[i,j].deleted)
					{
						
						
					}
					else
					{
						rend1 = GameManager.instance.allHexagons[i + 1, j - 1].GetComponent<Renderer>();
						rend2 = GameManager.instance.allHexagons[i, j - 1].GetComponent<Renderer>();
						if (rend.material.color == rend1.material.color && rend.material.color == rend2.material.color)
						{
							return "SE1"; // South West for odd column
						}
					}
				}

				if (j < (cols - 1)  )
				{
					
					if (i < (rows - 1))
					{
						if (allHexagons[i+1 , j].deleted&& allHexagons[i , j + 1].deleted && allHexagons[i,j].deleted)
						{
						
						
						}
						else
						{
							rend1 = GameManager.instance.allHexagons[i + 1, j].GetComponent<Renderer>();
							rend2 = GameManager.instance.allHexagons[i, j + 1].GetComponent<Renderer>();
							if (rend.material.color == rend1.material.color &&
							    rend.material.color == rend2.material.color)
							{
								return "NE1"; // South West for odd column
							}
						}
					}
					
					
					if (allHexagons[i -1, j].deleted&& allHexagons[i , j + 1].deleted && allHexagons[i,j].deleted)
					{
						
						
					}
					else
					{
						rend1 = GameManager.instance.allHexagons[i - 1, j].GetComponent<Renderer>();
						rend2 = GameManager.instance.allHexagons[i, j + 1].GetComponent<Renderer>();
						if (rend.material.color == rend1.material.color && rend.material.color == rend2.material.color)
						{
							return "NW1"; // South West for odd column
						}
					}
				}
			}
			
		    
		   
		}
	    
		return "NoSameColor";
	}
	
}
