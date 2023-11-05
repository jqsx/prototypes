using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public bool exitDungeon = false;

    public bool goTo = false;
    public string to = "";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (goTo)
        {
            SceneManager.LoadScene(to);
            return;
        }

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
