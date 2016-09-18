using UnityEngine;
using System.Collections;

public class LockRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().freezeRotation = true;
	}

}
