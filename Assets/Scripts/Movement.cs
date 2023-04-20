using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public Rigidbody2D PlayerBody;
    private float MaxRotSpeed = 360;
    public List<bool> Upgrades;
    private float jumpForce = 25000;
    private int jumpsLeft;
    public int jumpsMax;
    public GameObject rocket;
    public ParticleSystem rocketExp;
    private float rocketTime;
    public Text rocketCool;
    public GameObject Legs;
    public GameObject Sword;
    public float swordRot;
    public GameObject SwordControl;
    public float RotForce =  100000000000000000;
    public float Health;
    public float MaxHealth = 500;
    public GameObject Springs;
    public Slider healthBar;
    public float damageRes;
    public float swordScale = 1;
    public GameObject swordBlade;
    public SliderJoint2D swordStab;
    public Slider swordSizeSlider;
    public Text swordSizeText;
    public GameObject grapplingHook;
    public Slider hookTypeSlider;
    public Text hookTypeText;
    public GameObject Menu;
    public List<GameObject> Guns;
    public int gunSelected;
    private HingeJoint2D gunHinge;
    private GameObject gunOut;
    public bool IsGunOut;
    public Slider gunSelectSlider;
    public Text gunSelectText;
    public Text gunInfo;
    private float focusTimeMax = 10;
    private float focusTime;
    private float telekinesesWeakForce = 200;
    private float telekinesesStrongForce = 500;
    private float focusTimeSlowFac = 0.5f;
    public GameObject focusCircle;
    public FocusCameraShading focusShader;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
        healthBar.maxValue = MaxHealth;
        healthBar.value = Health;

    }

    // Update is called once per frame
    void Update()
    {
        //Quick meme that changes 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.SetActive(!Menu.activeInHierarchy);
        }
        //Rotate cube, if too fast no rotate, Movement, Index 0
        if (Mathf.Abs(PlayerBody.angularVelocity) <= MaxRotSpeed && Upgrades[0])
        {
            PlayerBody.AddTorque(-Input.GetAxis("Horizontal") * Time.deltaTime * 250 * RotForce);
        }
        //Fast rototate, Index 1
        if (Upgrades[1] && Input.GetKey(KeyCode.LeftShift))
        {
            MaxRotSpeed = 1800;
        } else
        {
            MaxRotSpeed = 360;
        }
        //Basic Jump, Index 2, Dash is 3
        if (Upgrades[2] && Input.GetKeyDown(KeyCode.Space) && jumpsLeft>0)
        {
            SquareDash();
        }
        //Basic Rocket, Index 4
        if (Upgrades[4]&& Input.GetKeyDown(KeyCode.LeftControl) && rocketTime == 0)
        {
            StartCoroutine("BasicRocket");
            rocketTime = 15;
        }
        //If the rocket is on cooldown, lower the cooldown, if it's less than 0, make it 0, also set the text
        if (rocketTime > 0)
        {
            rocketTime = rocketTime - Time.deltaTime;
        }
        if (rocketTime < 0)
        {
            rocketTime = 0;
        }
        rocketCool.text = "Rocket cooldown:" + (int)rocketTime;

        // Sword swing Script, also thing that changes types
        if (Upgrades[6])
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            swordRot = -Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg - Sword.GetComponent<Rigidbody2D>().rotation;
            //if the rotation angle is too high, reverse it
            if (swordRot < -180)
            {
                swordRot = swordRot + 360;
            } else if (swordRot > 180)
            {
                swordRot = swordRot - 360;
            }
            Sword.GetComponent<Rigidbody2D>().AddTorque(Mathf.Clamp(swordRot, -40, 40) * 50 * Time.deltaTime * swordBlade.transform.localScale.x * swordBlade.transform.localScale.y / 10);
            if (swordScale < 1)
            {
                var Motor = swordStab.motor;
                Motor.motorSpeed = (Input.GetAxis("Fire1") * 2 - 1) * 1000;

                swordStab.motor = Motor;
            }
        }
        //Update Gun stuff if a gun is out
        if (IsGunOut)
        {
            if (gunOut.GetComponent<GunScript>().reloading)
            {
                gunInfo.text = ("Reloading...");
            } else
            {
                gunInfo.text = ("Ammo: " + gunOut.GetComponent<GunScript>().ammo + "/" + gunOut.GetComponent<GunScript>().ammoMax);
            }
        }
        //telekenesis
        if (Upgrades[10])
        {
            if (Input.GetKey(KeyCode.F))
            {
                //if(focusTime < focusTimeMax)
                focusTime+= Time.deltaTime;
                Time.timeScale = focusTimeSlowFac;
                //focusCircle.SetActive(true); I'll be raplacing this with shader magic soon
                if(focusShader.intensity < 1)
                {
                    focusShader.intensity = Mathf.Clamp01(focusShader.intensity + (2f * (Time.deltaTime/focusTimeSlowFac)));
                    Debug.Log(focusShader.intensity);
                }
                
                focusShader.enabled = true;
            } else
            {
                if(Time.timeScale == focusTimeSlowFac) { Time.timeScale = 1; }
                if (focusTime > 0)
                {
                    focusTime -= Time.deltaTime / 2;
                }
                //focusCircle.SetActive(false);
                if (focusShader.intensity > 0)
                {
                    focusShader.intensity = Mathf.Clamp01(focusShader.intensity - (4f*Time.deltaTime));
                    Debug.Log(focusShader.intensity);
                }
                else
                {
                    focusShader.enabled = false;
                }
            }
        }
    }

    //Dirrectional Dash/Jump Function
    void SquareDash()
    {
        if (Upgrades[3])
        {
            
            PlayerBody.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position) * jumpForce/3);
        }
        else
        {
            PlayerBody.AddForce(new Vector2(0, 1) * jumpForce);
        }
        jumpsLeft--;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        jumpsLeft = jumpsMax;
        //Calculate fall damage based on 1/40 of kinetic energy
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            if (Mathf.Sqrt(Mathf.Pow(gameObject.GetComponent<Rigidbody2D>().velocity.x, 2) + Mathf.Pow(gameObject.GetComponent<Rigidbody2D>().velocity.y, 2)) >= 20)
            {
                Health = Health - (Mathf.Pow(gameObject.GetComponent<Rigidbody2D>().velocity.x, 2) + Mathf.Pow(gameObject.GetComponent<Rigidbody2D>().velocity.y, 2)) * gameObject.GetComponent<Rigidbody2D>().mass / 80 * (1 - damageRes);
            }
            //Calculate damage taken based on 1/40 of kinetic energy
            Health = Health - (Mathf.Pow(collision.GetComponent<Rigidbody2D>().velocity.x, 2) + Mathf.Pow(collision.GetComponent<Rigidbody2D>().velocity.y, 2)) * collision.GetComponent<Rigidbody2D>().mass / 80 * (1 - damageRes);
        }
        //update health bar
        healthBar.value = Health;
        //if health low, run death Function
        if (Health <= 0)
        {
            Camera.main.GetComponent<FollowPlayer>().death();
            Destroy(gameObject);
        }
    }
    IEnumerator BasicRocket()
    {
        rocket.SetActive(true);
        yield return new WaitForSeconds(5);
        rocket.SetActive(false);
        rocketExp.Play();
    }
    public void swordSizeCustom()
    {
        swordScale = swordSizeSlider.value/100;
        swordSizeText.text = ("Sword Size: " + swordScale);
    }
    public void HookType()
    {
        switch (hookTypeSlider.value)
        {
            case 0:
                hookTypeText.text = ("Type: Normal");
                break;
            case 1:
                hookTypeText.text = ("Type: Barbed");
                break;
        }
        gameObject.GetComponent<grapplingScript>().hookType = (int)hookTypeSlider.value;
    }
    public void gunType()
    {
        gunSelected = (int)gunSelectSlider.value;
        switch (gunSelected)
        {
            default:
                gunSelectText.text = ("Selected: null");
                break;
            case 0:
                gunSelectText.text = ("Selected: pistol");
                break;
            case 1:
                gunSelectText.text = ("Selected: uzi");
                break;
            case 2:
                gunSelectText.text = ("Selected: AK-47");
                break;
        }
    }
    //Menu shit
    public void Togg00()//Movement, 0
    {
        Upgrades[0] = !Upgrades[0];
    }
    public void Togg01()//fast rotate, 1
    {
        Upgrades[1] = !Upgrades[1];
    }
    public void Togg02()//Jump, 2
    {
        Upgrades[2] = !Upgrades[2];
    }
    public void Togg03()//Dash, 3
    {
        Upgrades[3] = !Upgrades[3];
    }
    public void Togg04()//rocket, 4
    {
        Upgrades[4] = !Upgrades[4];
    }
    public void Togg05()//Legs, index 5
    {
        GetComponent<LegController>().enabled = !GetComponent<LegController>().enabled;
        Legs.SetActive(!Legs.activeInHierarchy);
        transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
    }
    public void Togg06()//Sword, index 6
    {
        Upgrades[6] = !Upgrades[6];
        SwordControl.SetActive(!SwordControl.activeInHierarchy);
        //Scale Sword
        swordBlade.transform.localPosition = new Vector2(0, swordScale * 5 + .5f);
        swordBlade.transform.localScale = new Vector2(Mathf.Clamp(swordScale, 0.75f, 1.8f), swordScale * 10);
        swordBlade.GetComponent<Rigidbody2D>().mass = swordBlade.transform.localScale.x * swordBlade.transform.localScale.y/10;
    }
    public void Togg07()//Springs, index 7
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Springs.SetActive(!Springs.activeInHierarchy);
    }
    public void Togg08()//Grappling Hook, Index 8 
    {
        gameObject.GetComponent<grapplingScript>().DestroyHook();
        grapplingHook.SetActive(!grapplingHook.activeInHierarchy);
        gameObject.GetComponent<grapplingScript>().enabled = !gameObject.GetComponent<grapplingScript>().isActiveAndEnabled;
    }
    public void Togg09()//Guns, Index 9
    {
        switch (IsGunOut)
        {
            case true:
                Destroy(gunHinge);
                Destroy(gunOut);
                IsGunOut = false;
                gunInfo.enabled = false;
                break;
            case false:
                gunHinge = gameObject.AddComponent<HingeJoint2D>();
                gunHinge.autoConfigureConnectedAnchor = false;
                gunOut = Instantiate(Guns[gunSelected], transform.position, transform.rotation);
                gunHinge.connectedBody = gunOut.GetComponent<Rigidbody2D>();
                gunHinge.connectedAnchor = new Vector2(0,0);
                IsGunOut = true;
                gunInfo.enabled = true;
                break;
        }
    }
    public void Togg10()//basic telekinesis
    {
        Upgrades[10] = !Upgrades[10];
    }

}