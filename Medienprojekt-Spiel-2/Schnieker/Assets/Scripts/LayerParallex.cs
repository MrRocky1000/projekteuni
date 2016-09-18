using UnityEngine;
using System.Collections;

public class LayerParallex : MonoBehaviour {
	public Rigidbody rigi;
	public float speed = 1;
    private float curOffset = 0;
    private float jumpAmount, jumpBounds;
	Vector3 oldTrans = Vector3.zero;
	// Use this for initialization
	void Start () {
        jumpAmount = transform.localScale.x * 14; //14=position difference
        jumpBounds = jumpAmount / 2; //do a jump after half a position move
	}
	
	// Update is called once per frame
	void Update () {
        if (rigi) //still alive there?
        {
            Vector3 ourPos = transform.position;
            Vector3 playerPos = rigi.transform.position;
            //add offset difference if the player moved by a good amount
            if (Mathf.Abs(oldTrans.x - playerPos.x) > 0.01f)
                curOffset += (rigi.velocity.x * speed * Time.smoothDeltaTime);
            //this automatically moves the layers if the player gets out of their display
            if (ourPos.x - playerPos.x > jumpBounds) curOffset -= jumpAmount;
            else if (ourPos.x - playerPos.x < -jumpBounds) curOffset += jumpAmount;
            ourPos.x = playerPos.x + curOffset;
            transform.position = ourPos;
            oldTrans = playerPos;
        }
	}
}
