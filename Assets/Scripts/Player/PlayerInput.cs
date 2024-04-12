using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public int Vertical;
    public int Horizontal;
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            Vertical = (int)Input.GetAxisRaw("Vertical");
        }
        else
        {
            Vertical = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        {
            Horizontal = (int)Input.GetAxisRaw("Horizontal");
        }
        else
        {
            Horizontal = 0;
        }

        if (Vertical != 0 && Horizontal != 0)
        {
            Horizontal = 0;
        }
    }
}
