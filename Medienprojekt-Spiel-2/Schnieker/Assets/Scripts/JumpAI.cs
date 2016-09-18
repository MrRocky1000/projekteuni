using UnityEngine;
using System.Collections;

public class JumpAI : MonoBehaviour {
    public float moveSpeed = 0.05f;
    private float moveDir = 1.0f;
    private Enemy eObj;
    // Use this for initialization
    void Start()
    {
        eObj = GetComponent<Enemy>();
        if (eObj == null) Debug.LogError("Missing enemy script");
    }

    // Update is called once per frame
    void Update()
    {
        if (eObj.dead) return;
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
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().AddForce(Vector2.up * 500f, ForceMode.Force);
        }
    }
}
