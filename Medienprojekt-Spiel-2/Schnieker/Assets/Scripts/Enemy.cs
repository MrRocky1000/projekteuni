using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public float hitTimer = 0;
    public int HP = 100;
    public int faceDir = 1;
    public bool dead = false;
	// Use this for initialization
	void Start () {
		/**
        Renderer rend = GetComponent<Renderer>();
        ObjElement.elms e = GetComponent<ObjElement>().myElement;
        if (e == ObjElement.elms.STONE)
            rend.material.color = new Color(0.3f, 0.3f, 0.3f);
        else if (e == ObjElement.elms.SCISSOR)
            rend.material.color = new Color(0.6f, 0.6f, 0.6f);
        else if (e == ObjElement.elms.PAPER)
            rend.material.color = new Color(1f, 1f, 1f);
		*/
		}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision collision)
    {
        ObjElement e = GetComponent<ObjElement>();
        PlayerController p = collision.gameObject.GetComponent<PlayerController>();
        if (e != null && p != null)
            p.gotHit(e.myElement, transform.position);
    }

    private void destroyEnemy()
    {
        if (this.gameObject)
        {
            Vector3 pos = this.gameObject.transform.position;
            pos.y -= 500; //far far below ground
            this.gameObject.transform.position = pos;
            //this will let the triggerExit event happen
            Destroy(this.gameObject,1);
        }
    }

    public bool gotHit(ObjElement.elms attacker, Vector3 tPos)
    {
        Vector3 hitPos = this.gameObject.transform.position - tPos;
        ObjElement.elms attacked = GetComponent<ObjElement>().myElement;
        HP -= ObjElement.calcDmg(attacker, attacked);
        if (HP <= 0)
        {
            Rigidbody b = this.gameObject.GetComponent<Rigidbody>();
            b.useGravity = false; //disables gravity
            b.velocity = Vector3.zero;
            b.angularVelocity = Vector3.zero;
            this.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //kill player movement
            dead = true;
            //setup auto-kill after 5 seconds
            Invoke("destroyEnemy", 5);
            return dead;
        }
        hitTimer = Time.time + 0.3f;
        Vector3 impact = Vector3.zero; impact.x = (hitPos.x > 0 ? 5.0f : -5.0f); impact.y = 2.0f;
        Rigidbody mine = GetComponent<Rigidbody>();
        mine.AddForce(impact, ForceMode.Impulse);
        return dead;
    }
}
