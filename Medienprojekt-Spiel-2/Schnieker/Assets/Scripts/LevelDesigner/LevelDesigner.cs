using UnityEngine;
using System.Collections;

public class LevelDesigner : MonoBehaviour {
	public Vector2 gizmoPosition;
	public Color color = Color.grey;
	public float tiefe = 0;
	public GameObject prefab;

	void OnDrawGizmos(){
		Gizmos.color = color;
		//zeichnet ein Quardrat an der Mausposition mit der Größe 1,1,1
		Gizmos.DrawWireCube(new Vector3(gizmoPosition.x, gizmoPosition.y, tiefe), new Vector3(1,1,1));
	}
}
