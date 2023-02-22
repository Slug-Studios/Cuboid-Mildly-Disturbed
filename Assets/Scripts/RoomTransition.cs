using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTransition : MonoBehaviour
{
    public Vector2 targetPos;
    public string connectingScene;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Movement>() != null)
        {
            collision.transform.position = targetPos;
            SceneManager.LoadScene(connectingScene);
        } else
        {
            Destroy(collision.gameObject);
        }
    }
}
