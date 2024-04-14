using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnStack
{
    public Vector3 playerPos;
    public Vector3 moveObjectPos;
    public int moveObjectIndex;

    public ReturnStack(Vector3 _playerPos)
    {
        playerPos = _playerPos;
        moveObjectIndex = -1;
    }

    public ReturnStack(Vector3 _playerPos, Vector3 _moveObjectPos, int _moveObjectIndex)
    {
        playerPos = _playerPos;
        moveObjectPos = _moveObjectPos;
        moveObjectIndex = _moveObjectIndex;
    }
}

public class ReturnMove : MonoBehaviour
{
    private PlayerMovements playerMove;

    public Stack<ReturnStack> returnStack;

    private void Start()
    {
        playerMove = GetComponent<PlayerMovements>();

        returnStack = new Stack<ReturnStack>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DoReturn();
        }
    }

    private void DoReturn()
    {
        if (returnStack.Count == 0)
            return;
        ReturnStack lastPos = returnStack.Pop();

        transform.position = lastPos.playerPos;
        playerMove.curX = (int)lastPos.playerPos.x;
        playerMove.curY = (int)lastPos.playerPos.y;

        playerMove.ChangeMoveCount(-1);
        
        if (lastPos.moveObjectIndex == -1)
            return;

        playerMove.moveObjects[lastPos.moveObjectIndex].transform.position = lastPos.moveObjectPos;
        
        int nextObjectPosX = (int)lastPos.moveObjectPos.x;
        int nextObjectPosY = (int)lastPos.moveObjectPos.y;

        int beforePosX = playerMove.moveObjects[lastPos.moveObjectIndex].curX;
        int beforePosY = playerMove.moveObjects[lastPos.moveObjectIndex].curY;
        playerMove.moveObjects[lastPos.moveObjectIndex].curX = nextObjectPosX;
        playerMove.moveObjects[lastPos.moveObjectIndex].curY = nextObjectPosY;
        

        if (playerMove.mapInfo[nextObjectPosY].arr[nextObjectPosX] == 3)
        {
            playerMove.mapInfo[nextObjectPosY].arr[nextObjectPosX] = 4;
        }
        else
        {
            playerMove.mapInfo[nextObjectPosY].arr[nextObjectPosX] = 2;
        }
            
        if (playerMove.mapInfo[beforePosY].arr[beforePosX] == 2)
        {
            playerMove.mapInfo[beforePosY].arr[beforePosX] = 0;
        }
        else if (playerMove.mapInfo[beforePosY].arr[beforePosX] == 4)
        {
            playerMove.mapInfo[beforePosY].arr[beforePosX] = 3;
        }
    }
}
