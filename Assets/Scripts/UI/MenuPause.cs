using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    public static bool partieEnPause = false;
    public GameObject menuPauseUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (partieEnPause)
            {
                Continuer();
            } else
            {
                Pauser();
            }
        }    
    }

    public void Continuer()
    {
        menuPauseUI.SetActive(false);
        Time.timeScale = 1f;
        partieEnPause = false;
    }

    public void Recommencer(){
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Continuer();
    }

    void Pauser()
    {
        menuPauseUI.SetActive(true);
        Time.timeScale = 0f;
        partieEnPause = true;
    }

    public void Quitter()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
