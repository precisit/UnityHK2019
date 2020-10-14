using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject visuals;
    public float GameOverTime;

    void OnEnable()
    {
        Player.OnPlayerDeath += OnPlayerDeath;
        visuals.SetActive(false);
    }

    void OnDisable()
    {
        Player.OnPlayerDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        visuals.SetActive(true);
        StartCoroutine(WaitToRestart(GameOverTime));
    }

    private IEnumerator WaitToRestart(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
