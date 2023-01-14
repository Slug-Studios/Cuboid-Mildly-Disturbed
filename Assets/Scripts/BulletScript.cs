using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float pierce;
    public float lifespanMax;
    private float lifespan;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        lifespan = lifespan + Time.deltaTime;
        if (lifespan > lifespanMax)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        pierce--;
        if (pierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
