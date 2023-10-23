using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public bool exitDungeon = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (exitDungeon)
        {
            SceneManager.LoadScene("overworld");
        }
        else
        {
            GAMEINITIALIZER.globalGameLevel++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GAMEINITIALIZER.init();
        }
    }
}
