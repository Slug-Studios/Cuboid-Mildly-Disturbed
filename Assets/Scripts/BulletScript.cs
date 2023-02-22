using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float pierce;
    public float lifespanMax;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifespanMax);
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
