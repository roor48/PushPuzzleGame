using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerMovements playerMove;

    private void Start()
    {
        playerMove = FindObjectOfType<PlayerMovements>();
    }

    public void Btn_Vertical(int val)
    {
        playerMove.VerticalMove(val);
    }

    public void Btn_Horizontal(int val)
    {
        playerMove.HorizontalMove(val);
    }
}
