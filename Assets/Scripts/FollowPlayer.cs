using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour
{
    public GameObject Player;
    public Canvas DeathScreen;
    private bool dead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z);
        
    }
    public void death()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
        DeathScreen.enabled = true;
    }
    public void Restart()
    {
        SceneManager.LoadScene("DemoMain");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
