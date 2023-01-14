using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketControll : MonoBehaviour
{
    public GameObject Player;
    public ParticleSystem Trail;
    public Vector2 mousePos;
    private float MaxSpeed = 25;
    private float Power = 2500;
    

    // Start is called before the first frame update
    void Start()
    {
        Trail.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //Get the mouses position, then uses tan to get angle
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * -Mathf.Atan2(mousePos.x, mousePos.y) + 90);

        if (isActiveAndEnabled || Player.GetComponent<Rigidbody2D>().velocity.x <= MaxSpeed || Player.GetComponent<Rigidbody2D>().velocity.y <= MaxSpeed)
        {
            Player.GetComponent<Rigidbody2D>().AddForce(mousePos * Time.deltaTime * Power);
        }
    }
}