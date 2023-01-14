using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordImpale : MonoBehaviour
{
    public SliderJoint2D Impaler;
    public GameObject Blade;
    public bool Stick;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //create slider
        if (collision.GetComponent<Rigidbody2D>() != null && Blade.GetComponent<SliderJoint2D>() == null)
        {
            Impaler = Blade.AddComponent<SliderJoint2D>();
            Impaler.autoConfigureAngle = false;
            Impaler.angle = 90;
            Impaler.enableCollision = true;
            Blade.GetComponent<BoxCollider2D>().enabled = false;
            Impaler.connectedBody = collision.GetComponent<Rigidbody2D>();
            if (Stick)
            {
                StartCoroutine("StickIn");
            }
            
        }
        //deal damage if enemy, proportional to relative velocity
        if (collision.GetComponent<enemyCrtl>() != null)
        {
            collision.GetComponent<enemyCrtl>().Health = collision.GetComponent<enemyCrtl>().Health - Blade.transform.localScale.x * 20 * Mathf.Abs(Mathf.Sqrt(Mathf.Pow(collision.GetComponent<Rigidbody2D>().velocity.x, 2) + Mathf.Pow(collision.GetComponent<Rigidbody2D>().velocity.y, 2)) - Mathf.Sqrt(Mathf.Pow(GetComponent<Rigidbody2D>().velocity.x, 2) + Mathf.Pow(GetComponent<Rigidbody2D>().velocity.y, 2)));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            Blade.GetComponent<BoxCollider2D>().enabled = true;
            Destroy(Blade.GetComponent<SliderJoint2D>());
        }
    }
    IEnumerator StickIn()
    {
        yield return new WaitForSeconds(0.1f);
        var Motor = Impaler.motor;
        Motor.maxMotorTorque = 1000000;
        Impaler.motor = Motor;
    }
}
