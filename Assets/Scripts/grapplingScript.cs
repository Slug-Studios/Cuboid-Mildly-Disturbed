using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapplingScript : MonoBehaviour
{
    public GameObject hookLauncher;
    public List<GameObject> Hooks;
    public int hookType;
    public float shootingForce;
    private bool isHookOut;
    private GameObject hook;
    public GameObject rope;
    private JointMotor2D ropePull;
    public float ropeForce;
    public GameObject ropePrefab;
    public HingeJoint2D ropeBase;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = new Vector2();
        //Get the mouses position, then uses tan to get angle
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        hookLauncher.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * -Mathf.Atan2(mousePos.x, mousePos.y) + 90);

        //if you tap leftmouse, fire hook, make a slider and hinge, attach everything
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isHookOut)
        {
            isHookOut = true;
            ropeBase = gameObject.AddComponent<HingeJoint2D>();
            rope = Instantiate(ropePrefab, transform.position, hookLauncher.transform.rotation);
            ropeBase.autoConfigureConnectedAnchor = false;
            ropeBase.connectedBody = rope.GetComponent<Rigidbody2D>();
            ropeBase.connectedAnchor = new Vector2(0, 0);
            hook = Instantiate(Hooks[hookType], rope.transform.position, rope.transform.rotation);
            hook.transform.rotation = rope.transform.rotation;

            rope.GetComponent<SliderJoint2D>().connectedBody = hook.GetComponent<Rigidbody2D>();
            rope.GetComponent<SliderJoint2D>().enabled = true;
            ropePull = rope.GetComponent<SliderJoint2D>().motor;
            ropePull.maxMotorTorque = 0;
            hook.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(shootingForce, 0));

        }
        else if (Input.GetKey(KeyCode.Mouse1) && isHookOut)
        {
            ropePull.motorSpeed = -ropeForce;
            ropePull.maxMotorTorque = 10000;
            if (transform.position.x - hook.transform.position.x < 0.5f && transform.position.x - hook.transform.position.x > -0.5f && transform.position.y - hook.transform.position.y < 0.5f && transform.position.y - hook.transform.position.y > -0.5f)
            {
                DestroyHook();
            }
        } else if (isHookOut)
        {
            ropePull.motorSpeed = 0;
            ropePull.maxMotorTorque = 0;
        }
        if (isHookOut)
        {
            rope.GetComponent<SliderJoint2D>().motor = ropePull;
        }
    }
    public void DestroyHook()
    {
        if (isHookOut)
        {
            rope.GetComponent<SliderJoint2D>().enabled = false;
            Destroy(rope);
            Destroy(hook);
            Destroy(ropeBase);
            isHookOut = false;
        }
    }
}
