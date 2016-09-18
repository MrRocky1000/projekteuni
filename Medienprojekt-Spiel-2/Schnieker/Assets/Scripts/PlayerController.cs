using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
    //base variables for player movement
	public float gravity = 10;
	public float speed = 300.0f;
	public float jumpPower = 500.0f;
    public float maxFallSpeed = 25.0f;
    private float downForce = 0;
    private float distToGround;
    //all of this is jump related
    bool grounded = false;
	bool inputJump = false;
    bool jumpAvaliable = false;
    bool jumpActive = false;
    bool jumpInAir = false;
    bool jumpStop = false;
	float velocity = 0;
	Rigidbody rigi;
    public Renderer rend;
    private float meleeCooldown = 0;
    private float rangeCooldown = 0;
    private float hitTimer = 0, flipTimer = 0;
    public int HP = 100;
    //UI-items->--
    public Slider healthBar;
    public Image damageImage;
    public Image element1ImageN;
    public Image element1ImageP;
    public Image element1ImageSc;
    public Image element1ImageSt;
    public Image element2ImageN;
    public Image element2ImageP;
    public Image element2ImageSc;
    public Image element2ImageSt;
    public Image actualImage1;
    public Image actualImage2;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f,0f,0f,0.1f);
    private Color shootColor = new Color(1f, 1f, 0f);
    //--<-
    bool damaged;
    public Enemy inDeadEnemy = null;
    public Color baseColor, curColor;
    private int faceDir = 1;
    ObjElement secondElm = new ObjElement();
    private GameObject healObj = null;
    //sounds
    AudioSource audio;
    public AudioClip jump;
    public AudioClip getElm;
    public AudioClip heal;
    private Animator animator;
    private Vector3 normScale;
    private Vector3 saveVec = Vector3.zero;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        secondElm.myElement = ObjElement.elms.NORMAL;
        rend = transform.GetChild(0).GetComponent<Renderer>();
        if (rend == null) Debug.LogError("Missing renderer");
        curColor = baseColor = rend.material.color;
        actualImage1 = element1ImageN;
        actualImage2 = element2ImageN;
		rigi = GetComponent<Rigidbody>();
        Mesh m = GetComponent<MeshFilter>().sharedMesh;
        distToGround = (transform.localScale.y * m.bounds.size.y) / 2;
        normScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        PauseMenu.doUpdate();
        if (PauseMenu.getPaused()) return;
		InputCheck();
		move();
        checkDamage();
        //if player falls below ground
        if (transform.position.y < -2)
        {
            if (saveVec != Vector3.zero)
                transform.position = saveVec;
            else
                Destroy(this.gameObject);
        }
	}
    /*
        Check if Player is damaged:
        true - flashing UI to show
    */
    void checkDamage()
    {
        if (damaged)
        {
            damageImage.color = flashColor;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }
    void doAttack(float dist)
    {
        Vector3 chk; chk.x = faceDir; chk.y = 0; chk.z = 0;
        RaycastHit h;
        if (Physics.Raycast(transform.position, chk, out h, dist))
        {
            Enemy e = h.transform.gameObject.GetComponent<Enemy>();
            ObjElement myElement = GetComponent<ObjElement>();
            if (e != null && myElement != null)
            {
                bool ret = e.gotHit(myElement.myElement, transform.position);
                if (ret)
                {
                    //leave the object around but just not bother rendering physics
                    Physics.IgnoreCollision(e.GetComponent<Collider>(), GetComponent<Collider>());
                }
            }
        }
    }
    /*
        Fallunterschiedung der Elemente
     */
    void setPlayerElement(ObjElement.elms eElm)
    {
        //copy first element over
        secondElm.myElement = GetComponent<ObjElement>().myElement;
        if (secondElm.myElement == ObjElement.elms.STONE)
            changeElementUI(element2ImageSt, false);
        else if (secondElm.myElement == ObjElement.elms.SCISSOR)
            changeElementUI(element2ImageSc, false);
        else if (secondElm.myElement == ObjElement.elms.PAPER)
            changeElementUI(element2ImageP, false);
        else
            changeElementUI(element2ImageN, false);
        //set new first element
        GetComponent<ObjElement>().myElement = eElm;
        if (eElm == ObjElement.elms.STONE)
        {
            changeElementUI(element1ImageSt, true);
            curColor = new Color(0.3f, 0.3f, 0.3f);
        }
        else if (eElm == ObjElement.elms.SCISSOR)
        {
            changeElementUI(element1ImageSc, true);
            curColor = new Color(0.6f, 0.6f, 0.6f);
        }
        else if (eElm == ObjElement.elms.PAPER)
        {
            changeElementUI(element1ImageP, true);
            curColor = new Color(1f, 1f, 1f);
        }
        else
        {
            changeElementUI(element1ImageN, true);
            curColor = baseColor;
        }
        if (rangeCooldown == 0)
            rend.material.color = curColor;
    }
    
    /*
        change Image in GUI
        @image - new Image to replace in the GUI
        @b - true = Image1 , false = Image2
    */
    void changeElementUI(Image image, bool b)
    {
        if(b == true)
        {
            actualImage1.color = new Color(1, 1, 1, 0);
            image.color = new Color(1, 1, 1, 0.5f);
            actualImage1 = image;

        }
        else
        {
            actualImage2.color = new Color(1, 1, 1, 0);
            image.color = new Color(1, 1, 1, 0.5f);
            actualImage2 = image;
        }
    }
	void InputCheck()
    {
		velocity = Input.GetAxis("Horizontal") * speed;
        if (velocity > 0)
        {
            if (Mathf.Round(transform.rotation.eulerAngles.y) != 90)
                transform.Rotate(Vector3.up, 180, Space.World);
            faceDir = 1;
        }
        else if (velocity < 0)
        {
            if (Mathf.Round(transform.rotation.eulerAngles.y) != 270)
                transform.Rotate(Vector3.up, 180, Space.World);
            faceDir = -1;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (healObj)
            {
                HP = Mathf.Min(100,HP+25);
                //UI
                healthBar.value = HP;
                Destroy(healObj.gameObject);
                audio.PlayOneShot(heal, 1f);
            }
            else if (inDeadEnemy)
            {
                ObjElement.elms eElm = inDeadEnemy.GetComponent<ObjElement>().myElement;
                setPlayerElement(eElm);
                Destroy(inDeadEnemy.gameObject);
                inDeadEnemy = null;
                audio.PlayOneShot(getElm, 1f);
                animator.SetTrigger("eat");
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            setPlayerElement(ObjElement.elms.NORMAL);
            setPlayerElement(ObjElement.elms.NORMAL);
        }

        if(meleeCooldown > 0 && Time.time >= meleeCooldown)
            meleeCooldown = 0;

        if(Input.GetKeyDown(KeyCode.Q) && meleeCooldown == 0) //melee
        {
            doAttack(1f);
            meleeCooldown = Time.time + 0.5f; //half a second
            animator.SetTrigger("attack");
        }

        if (rangeCooldown > 0)
        {
            if (Time.time >= rangeCooldown)
            {
                rend.material.color = curColor;
                rangeCooldown = 0;
            }
            else
            {
                float calcDiff = rangeCooldown - Time.time;
                rend.material.color = Color.Lerp(curColor, shootColor, calcDiff);
            }
        }
        if (Input.GetKey(KeyCode.E) && rangeCooldown == 0) //ranged
        {
            doAttack(100f);
            rend.material.color = shootColor;
            rangeCooldown = Time.time + 1f; //one second, powerful
            animator.SetTrigger("attack");
        }

		if(Input.GetKey(KeyCode.Space)){
			inputJump = true;
		}else{
			inputJump = false;
            if (jumpActive)
                jumpStop = true;
            else
                jumpStop = false;
		}
	}

    void move()
    {
        Vector3 s = rigi.velocity;
        if (Physics.Raycast(transform.position, -Vector3.up, distToGround))
        {
            grounded = true;
            if (!jumpActive && !jumpInAir)
            {
                jumpAvaliable = true;
                animator.SetBool("jumping", false);
            }
        }
        else
        {
            grounded = false;
            if (jumpActive)
                jumpInAir = true;
            else //prevent jump mid-falling
                jumpAvaliable = false;
        }
        if (inputJump && !jumpStop && (jumpAvaliable || jumpActive))
        {
            jumpActive = true;
            if (jumpAvaliable)
            {
                audio.PlayOneShot(jump, 1f);
                animator.SetBool("jumping", true);
            }
            jumpAvaliable = false;
            if (downForce < jumpPower)
                downForce += jumpPower * 0.02f * Time.smoothDeltaTime;
            s.y = (jumpPower * Time.smoothDeltaTime) - downForce;
        }
        if (jumpInAir && grounded)
        {
            jumpStop = true;
            jumpActive = false;
            jumpInAir = false;
            downForce = 0;
        }
        if(jumpActive && jumpInAir && jumpStop && !grounded)
        {
            if (s.y > -maxFallSpeed)
                s.y -= jumpPower * 0.06f * Time.smoothDeltaTime;
        }
        if(Mathf.Abs(s.x) < Mathf.Abs(velocity))
            s.x = velocity * Time.smoothDeltaTime;
        animator.SetBool("idle", (velocity == 0));
        if (hitTimer == 0) //normal movement
        {
            rigi.velocity = s;
        }
        else
        {
            //flickering player every 0.1 seconds
            if (Time.time - flipTimer > 0.1f)
            {
                transform.localScale = (transform.localScale == Vector3.zero ? normScale : Vector3.zero);
                flipTimer = Time.time;
            }
            if (hitTimer - Time.time <= 0) //stop iframes
            {
                transform.localScale = normScale;
                setEnemyIgnore(false);
                hitTimer = 0;
            }
            else if (hitTimer - Time.time <= 0.7f) //iframes with movement
            {
                rigi.velocity = s;
            }
        }
    }

    public void gotHit(ObjElement.elms attacker, Vector3 tPos)
    {
        if (hitTimer > 0) return;
        damaged = true;

        Vector3 hitPos = this.gameObject.transform.position - tPos;
        ObjElement.elms attacked = GetComponent<ObjElement>().myElement;
        HP -= ObjElement.calcDmg(attacker, attacked);
        //UI
        healthBar.value = HP;

        if (HP <= 0)
        {   //kill player movement
            damageImage.color = new Color(0f, 0f, 0f, 0.5f);
            Destroy(this.gameObject);
        }
        flipTimer = Time.time;
        hitTimer = Time.time + 1.0f;
        setEnemyIgnore(true);
        Vector3 impact = Vector3.zero; impact.x = (hitPos.x > 0 ? 5.0f : -5.0f); impact.y = 2.0f;
        rigi.AddForce(impact, ForceMode.Impulse);
    }

    void setEnemyIgnore(bool stat)
    {
        Collider thisCol = GetComponent<CapsuleCollider>();
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.GetComponent<Enemy>() != null)
                Physics.IgnoreCollision(thisCol, go.GetComponent<Collider>(), stat);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Heal")
        {
            healObj = collision.gameObject;
        }
        else if(collision.tag == "Respawn")
        {
            saveVec.x = collision.transform.position.x;
            saveVec.y = 3;
            saveVec.z = transform.position.z;
        }
        else
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            if (e != null && e.dead)
                inDeadEnemy = e;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Heal")
        {
            healObj = null;
        }
        else
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            if (e != null && e.dead)
                inDeadEnemy = null;
        }
    }
}
