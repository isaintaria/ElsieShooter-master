using UnityEngine;
using System.Collections;

[System.Serializable]
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;

	public Done_Boundary boundary;
	public GameObject shot;
	public Transform shotSpawn;
    public GameObject bombEffect;
    public Transform bombSpawn;

    public SimpleTouchPad touchPad;
    public AttackButton attackButton;
    public BombButton bombButton;

    public float fireRate;
    public float bombCoolTime;
	 
	private float nextFire;
    private float nextBomb;

    private bool isMoveable;

    EffectManager effectManager;
    public bool IsMoveable
    {
        get
        {
            return isMoveable;
        }

        set
        {
            isMoveable = value;
        }
    }
    private Done_GameController gameController;
    private Quaternion calibrationQuaternion;


    void Awake()
    {
        Debug.Log("비행기 Awke");
    }

    public void doFire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            effectManager.FirePlayerBeamEffect();
        }
    }
    public void doBomb()
    {
        if( Time.time > nextBomb )
        {
            if (gameController.BombCount <= 0)
                return;
            gameController.BombCount--;
            nextBomb = Time.time + bombCoolTime;
            Instantiate(bombEffect, bombSpawn.position, bombSpawn.rotation); 
            Bomb_Action();
        }
    }
    void Update ()
	{

	}



    void Start()
    {

        Debug.Log("비행기 새로 생성됨");
        var obj = GameObject.Find("Game Controller/Canvas/Movement Zone");
        touchPad = obj.GetComponent<SimpleTouchPad>();
        touchPad.GetDirection();
        attackButton = GameObject.Find("Game Controller/Canvas/Fire Zone").GetComponent<AttackButton>();
        bombButton = GameObject.Find("Game Controller/Canvas/Bomb Zone").GetComponent<BombButton>();


        attackButton.OnButtonDown += AttackButton_OnButtonDown;
        bombButton.OnButtonDown += BombButton_OnButtonDown;

        effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<Done_GameController>();
        IsMoveable = true;
        CalibrateAccelerometer();

    }

    private void BombButton_OnButtonDown()
    {
        doBomb();
    }

    private void AttackButton_OnButtonDown()
    {
        doFire();
    }

    void Bomb_Action()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
       
        foreach( GameObject obj in gos )
        {
            var test = obj.GetComponent<Done_DestroyByContact>();
            if( test != null )
            {
                gameController.AddScore(test.scoreValue);
                Destroy(obj);
            }               
            
        }
        effectManager.FirePlayerBombExplosionEffect();

    }  
    public void MoveUp()
    {
        Vector3 up = new Vector3(0, 0, 1.0f);
        GetComponent<Rigidbody>().velocity = up * 5.0f;
    }

    void CalibrateAccelerometer()
    {
        Vector3 accelerationSnapshot = Input.acceleration;
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
        calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);
    }

    //Get the 'calibrated' value from the Input
    Vector3 FixAcceleration(Vector3 acceleration)
    {
        Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
        return fixedAcceleration;
    }

    void OnEnable()
    {

  
    }

    void OnDestroy()
    {
        attackButton.OnButtonDown -= AttackButton_OnButtonDown;
        bombButton.OnButtonDown -= BombButton_OnButtonDown;
    }

    void FixedUpdate ()
	{
        if( IsMoveable )
        {

            //float moveHorizontal = Input.GetAxis("Horizontal");
            //float moveVertical = Input.GetAxis("Vertical");
            //  Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            Vector3 accelerationRaw = Input.acceleration;
            Vector3 acceleration = FixAcceleration(accelerationRaw);
            //   Vector3 movement = new Vector3(acceleration.x, 0.0f, acceleration.y);

            Vector2 direction = touchPad.GetDirection();
            Vector3 movement = new Vector3(direction.x, 0.0f, direction.y);

            GetComponent<Rigidbody>().velocity = movement * speed;
            GetComponent<Rigidbody>().position = new Vector3
            (
                Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
            );  
            GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
        }
		
	}
}
