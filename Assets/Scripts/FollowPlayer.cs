using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour
{
    public Canvas DeathScreen;
    private bool dead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        
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
