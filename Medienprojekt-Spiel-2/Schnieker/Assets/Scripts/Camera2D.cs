using UnityEngine;
using System.Collections;

public class Camera2D : MonoBehaviour {
	public Transform target;
	public int zOffset = -10;
	public int yOffset = 3;
	Vector3 position;

	// Use this for initialization
	void Start () {
	
	}
	
	// LateUpdate is called after Update once per frame
	void LateUpdate () {
        if (target) //still alive there?
        {
            position.x = target.position.x;
            position.z = zOffset;
            position.y = yOffset;

            transform.position = position;
        }
	}
}
