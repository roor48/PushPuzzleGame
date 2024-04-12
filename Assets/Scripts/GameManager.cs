using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public float speed;

    protected override void Awake()
    {
        base.Awake();
        
        FindObjectOfType<MapGen>().DeleteMap();
        FindObjectOfType<MapGen>().BuildMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    public void Btn_Exit()
    {
        Application.Quit();
    }
}
