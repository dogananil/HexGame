using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour {

	public const float outerRadius=1f;
	public const float innerRadius= outerRadius * 0.866025404f;

	public static Vector3[] corners = {
		new Vector3(0f, 0f, outerRadius),
		new Vector3(innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(0f, 0f, -outerRadius),
		new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(-innerRadius, 0f, 0.5f * outerRadius)
	};
	public static int[] triangles = {
		0,1,2,
		0,2,5,
		5,2,3,
		5,3,4
	};
	public class Node {
		public Vector3 position;
		public int vertexIndex=-1;

		public Node(Vector3 _pos){
			position=_pos;
		}
	}
}
