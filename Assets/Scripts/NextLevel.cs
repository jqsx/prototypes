using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GAMEINITIALIZER.globalGameLevel++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GAMEINITIALIZER.init();
    }
}
