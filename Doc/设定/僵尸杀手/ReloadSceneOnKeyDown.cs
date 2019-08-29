using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneOnKeyDown : MonoBehaviour
{
    public KeyCode reloadKey = KeyCode.R;

    private void Update()
    {
        if (Input.GetKeyDown(this.reloadKey))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }
}

