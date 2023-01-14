using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyCrtl : MonoBehaviour
{
    public float Health;
    public float MaxHealth;
    public float MaxFall;
    public float damageRes;
    public GameObject Player;
    public List<bool> movementType;
    public float followRange;
    public float Speed;
    public float maxSpeed;
    public GameObject Canvas;
    public Slider healthBar;
    public float ContactDamage;

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

        
        
        //Int 0 is roll movement
        if (movementType[0])
        {
            if (Mathf.Abs(Player.transform.position.x - transform.position.x) <= followRange && Mathf.Abs(gameObject.GetComponent<Rigidbody2D>().angularVelocity) <= maxSpeed)
            {
                gameObject.GetComponent<Rigidbody2D>().AddTorque(-Mathf.Clamp(Player.transform.position.x - transform.position.x, -1, 1) * Time.deltaTime * Speed);
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Take damage if hit, proportional to half of kinetic energy and the damage resistance, also take damage if hit wall too fast
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            if (Mathf.Sqrt(Mathf.Pow(gameObject.GetComponent<Rigidbody2D>().velocity.x, 2) + Mathf.Pow(gameObject.GetComponent<Rigidbody2D>().velocity.y, 2)) >= MaxFall)
            {
                Health = Health - (Mathf.Pow(gameObject.GetComponent<Rigidbody2D>().velocity.x, 2) + Mathf.Pow(gameObject.GetComponent<Rigidbody2D>().velocity.y, 2)) * gameObject.GetComponent<Rigidbody2D>().mass / 4 * 1 - damageRes;
            }
            Health = Health - (Mathf.Pow(collision.GetComponent<Rigidbody2D>().velocity.x, 2) + Mathf.Pow(collision.GetComponent<Rigidbody2D>().velocity.y, 2)) * collision.GetComponent<Rigidbody2D>().mass / 4 * 1 - damageRes;

            //Update health bar
            healthBar.value = Health;

            //if health is 0 or less, play particles, then die
            if (Health <= 0)
            {
                GetComponent<ParticleSystem>().Play();
                Destroy(gameObject.GetComponent<SpriteRenderer>());
                gameObject.GetComponent<Rigidbody2D>().mass = 0;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(Canvas);
                Destroy(gameObject, 5);
                Health = 1;
            }
        }
        //If contact with player, deal contact damage
        if (collision.GetComponent<Movement>() != null)
        {
            collision.GetComponent<Movement>().Health = collision.GetComponent<Movement>().Health - ContactDamage;
        }
    }
    
}
