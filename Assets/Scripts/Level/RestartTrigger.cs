using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out PlayerMovement player))
            return;
        Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>();

        player.Death();
        LevelManager.Instance.LockKeyInput();
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(1);
        LevelManager.Instance.LoadSceneByName(SceneManager.GetActiveScene().name);
    }
}
