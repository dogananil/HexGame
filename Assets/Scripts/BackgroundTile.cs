using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundTile : MonoBehaviour
{
	List<Vector3> _changedVertices;
	
	public HexCell cellPrefab;
	private Quaternion _rotation;
	public int coordinateX, coordinateY;
	[HideInInspector] 
	public List<Color> colors =new List<Color>();
    // Start is called before the first frame update
    void Start()
    {
	    colors.Add (Color.red);
	    colors.Add (Color.green);
	    colors.Add (Color.blue);
	    colors.Add (Color.yellow);
	    colors.Add (Color.white);
	    CreateHexagons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateHexagon(params Hexagon.Node[] points){
	    AssignVertices (points);

    }
    public void AssignVertices(Hexagon.Node[] points){

        for (int i = 0; i < points.Length; i++) {
            if (points [i].vertexIndex == -1) {
                points [i].vertexIndex = _changedVertices.Count;
                _changedVertices.Add (points [i].position);
            }	
        }
    }
    
    
    void CreateHexagons () {
	    Hexagon.Node[] hexPoints;
		_changedVertices = new List<Vector3> ();
		Vector3 scale = new Vector3(0.5f,0.5f,0.5f);
		_rotation = Quaternion.Euler(0,30,0);
		
		HexCell cell  =Instantiate(cellPrefab,transform.position,_rotation);

		cell.transform.parent = this.transform;
		cell.transform.name = this.transform.name;
		
		string[] firstparse = this.transform.name.Split(',');
		int i = Convert.ToInt32(firstparse[0]);
		int j = Convert.ToInt32(firstparse[1]);
		
		GameManager.instance.allHexagons[i, j] = cell;
		
		cell.transform.localScale=scale;
		cell.transform.localRotation = _rotation;
		
		hexPoints = new Hexagon.Node[Hexagon.corners.Length];
		for (int r = 0; r < Hexagon.corners.Length; r++) {
			Hexagon.Node point;
			point =new Hexagon.Node(Hexagon.corners[r]);
			hexPoints [r] = point;
		}

		CreateHexagon (hexPoints);
		
		Mesh cellMesh = new Mesh ();
		cell.GetComponent<MeshFilter> ().mesh=cellMesh;
		Renderer rend = cell.GetComponent<Renderer> ();
		Vector3[] hexVertices = cellMesh.vertices;




		cellMesh.vertices = _changedVertices.ToArray();
		cellMesh.triangles = Hexagon.triangles;
		int random =   Mathf.RoundToInt (Random.Range (0.0f, 4.0f));
		rend.material.color = colors[random];
		
		

		if (i > 0 )
		{
			while (GameManager.instance.checkColor(rend,i,j)!="NoSameColor")
				{
					random =   Mathf.RoundToInt (Random.Range (0.0f, 4.0f));
					rend.material.color = colors[random];
				}
		}

		if (i == GameManager.instance.rows - 1 && j == GameManager.instance.cols - 1)
		{
			GameManager.instance.RotationOrNot = true;
		}

		cellMesh.RecalculateNormals ();
		/*Text label = Instantiate<Text> (cellLabelPrefab);
		label.rectTransform.SetParent (gridCanvas.transform, false);
		label.rectTransform.anchoredPosition = new Vector2 (position.x, position.z);
		label.text = x.ToString() + " , " + z.ToString();*/
	}

    
}
