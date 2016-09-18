using UnityEngine;
using System.Collections;

public class Position2D : MonoBehaviour {

	public float axisOffset = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;

		pos.z = 0 + axisOffset;

		transform.position = pos;
	}
}
