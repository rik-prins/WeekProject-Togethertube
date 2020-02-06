using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoronaOverbodigScript : MonoBehaviour
{

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Pime");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
