using UnityEngine;
using System.Collections;

public class MoveAI : MonoBehaviour {
    public float moveSpeed = 0.025f;
    private float moveDir = 1.0f;
    private float distToCorner = 0.0f;
    private float distToGround = 0.0f;
    private Enemy eObj;
	// Use this for initialization
	void Start () {
        eObj = GetComponent<Enemy>();
        if (eObj == null) Debug.LogError("Missing enemy script");
        Mesh m = GetComponent<MeshFilter>().sharedMesh;
        distToCorner = (transform.localScale.x * m.bounds.size.x) / 2;
        distToGround = (transform.localScale.y * m.bounds.size.y) / 2;
	}
	
	// Update is called once per frame
	void Update () {
        if (PauseMenu.getPaused()) return;
        if (eObj.dead) return;
        Vector3 leftCorner = transform.position;
        leftCorner.x -= distToCorner;
        Vector3 rightCorner = transform.position;
        rightCorner.x += distToCorner;
        if (Physics.Raycast(leftCorner, -Vector3.up, distToGround + 0.1f) == false)
            moveDir = 1.0f;
        else if (Physics.Raycast(rightCorner, -Vector3.up, distToGround + 0.1f) == false)
            moveDir = -1.0f;
        Vector3 pos = transform.position;
        pos.x += (moveSpeed * moveDir);
        if (eObj.hitTimer == 0)
            transform.position = pos;
        else if (eObj.hitTimer - Time.time <= 0)
            eObj.hitTimer = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((GetComponent<Enemy>() != null && collision.gameObject.GetComponent<Enemy>() != null) 
            || collision.gameObject.tag == "Player")
        {
            float myPos = transform.position.x;
            float colPos = collision.gameObject.transform.position.x;
            if ((myPos < colPos && moveDir > 0) || (myPos > colPos && moveDir < 0))
                moveDir *= (-1);
        }
    }
}
