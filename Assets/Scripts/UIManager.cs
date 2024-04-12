using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        SetPlayerSpeed();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public GameObject pauseMenu;
    private void PauseGame()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }
    
    public Slider speedSlider;
    public Text speedText;
    public void SetPlayerSpeed()
    {
        _gameManager.speed = speedSlider.value * 10;
        speedSlider.value = _gameManager.speed / 10f;

        speedText.text = $"{_gameManager.speed:F2}";
    }
}
