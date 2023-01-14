using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_Script : MonoBehaviour
{
    public GameObject UpgradeSprite;
    public GameObject Player;
    public int UpgradeIndexInt;

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
        Player.GetComponent<Movement>().Upgrades[UpgradeIndexInt] = true;
        GetComponent<ParticleSystem>().Play();
        Destroy(UpgradeSprite);
        Destroy(gameObject, 5);
    }
}