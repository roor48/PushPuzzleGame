using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovements : MonoBehaviour
{
    private GameManager _gameManager;
    private ReturnMove _returnMove;

    public Text clearText;
    public Text moveCountText;
    
    private MapGen mapGen;
    
    private PlayerInput input;

    public MoveObject[] moveObjects;

    public float Speed => _gameManager.speed;

    public _2DArray[] mapInfo;

    public int curY;
    public int curX;

    public int mapHeight;
    public int mapWidth;
    
    public bool isMoving;
    
    private void Start()
    {
        _gameManager = GameManager.Instance;
        _returnMove = GetComponent<ReturnMove>();
        
        mapGen = FindObjectOfType<MapGen>();
        clearText = GameObject.Find("ClearText").GetComponent<Text>();
        clearText.text = "";

        moveCountText = GameObject.Find("MoveCount").GetComponent<Text>();
        moveCountText.text = "이동횟수: 0";
        
        input = GetComponent<PlayerInput>();
        moveObjects = FindObjectsOfType<MoveObject>();

        transform.position = new Vector3(curX, curY, 0);
    }

    private void Update()
    {
        Move();
    }

    public void Init(int yPos, int xPos, _2DArray[] map)
    {
        isMoving = false;
        
        curY = yPos;
        curX = xPos;
        mapInfo = map;
        
        mapHeight = mapInfo.Length;
        mapWidth = mapInfo[0].arr.Length;

        transform.position = new Vector3(curX, curY, 0);
    }
    
    private void Move()
    {
        if (input.Vertical == 0 && input.Horizontal == 0) return;
        
        if (!CanMove())
            return;
        
        int nextY = curY + input.Vertical;
        int nextX = curX + input.Horizontal;

        if (mapInfo[nextY].arr[nextX] is 2 or 4)
        {
            int nextObjectPosY = nextY + input.Vertical;
            int nextObjectPosX = nextX + input.Horizontal;

            if (mapInfo[nextObjectPosY].arr[nextObjectPosX] == 3)
            {
                mapInfo[nextObjectPosY].arr[nextObjectPosX] = 4;
            }
            else
            {
                mapInfo[nextObjectPosY].arr[nextObjectPosX] = 2;
            }
                
            if (mapInfo[nextY].arr[nextX] == 2)
            {
                mapInfo[nextY].arr[nextX] = 0;
            }
            else if (mapInfo[nextY].arr[nextX] == 4)
            {
                mapInfo[nextY].arr[nextX] = 3;
            }
                
            StartCoroutine(moveObjects[FindPositionMoveObject(nextY, nextX)].Move(input.Vertical, input.Horizontal, Speed));
        }

        StartCoroutine(MoveAnim(nextY, nextX));
        
        ChangeMoveCount(1);
        
        ReturnStack temp = new ReturnStack(new Vector3(curX, curY, 0), new Vector3(nextX, nextY, 0), FindPositionMoveObject(nextY, nextX));
        _returnMove.returnStack.Push(temp);
    }

    private bool CanMove()
    {
        if (isMoving)
            return false;
        
        int nextY = curY + input.Vertical;
        int nextX = curX + input.Horizontal;
        
        if (nextX >= mapWidth || nextX < 0 ||
            nextY >= mapHeight || nextY < 0)
        {
            return false;
        }
        if (mapInfo[nextY].arr[nextX] == 1)
        {
            return false;
        }

        if (mapInfo[nextY].arr[nextX] == 2 || mapInfo[nextY].arr[nextX] == 4)
        {
            int nextObjectPosY = nextY + input.Vertical;
            int nextObjectPosX = nextX + input.Horizontal;
            if (nextObjectPosX >= mapWidth || nextObjectPosX < 0 ||
                nextObjectPosY >= mapHeight || nextObjectPosY < 0)
            {
                return false;
            }

            if (mapInfo[nextObjectPosY].arr[nextObjectPosX] == 1 || mapInfo[nextObjectPosY].arr[nextObjectPosX] == 2 || mapInfo[nextObjectPosY].arr[nextObjectPosX] == 4)
                return false;
        }
        
        return true;
    }

    private IEnumerator MoveAnim(int nextY, int nextX)
    {
        isMoving = true;

        Vector3 startPos = new Vector3(curX, curY, 0);
        Vector3 endPos = new Vector3(nextX, nextY, 0);

        float percent = 0f;
        while (percent <= 1f)
        {
            percent += Time.deltaTime * Speed;
            transform.position = Vector3.Lerp(startPos, endPos, percent);

            yield return null;
        }

        curY = nextY;
        curX = nextX;

        isMoving = false;
        
        CheckIsComplete();
    }

    private int FindPositionMoveObject(int yPos, int xPos)
    {
        for (int i = 0; i < moveObjects.Length; i++)
        {
            if (yPos == moveObjects[i].curY && xPos == moveObjects[i].curX)
                return i;
        }

        return -1;
    }
    private void CheckIsComplete()
    {
        if (moveObjects.Any(moveObject => mapInfo[moveObject.curY].arr[moveObject.curX] != 4))
        {
            clearText.text = "";
            return;
        }

        clearText.text = "성공";
    }

    public void ChangeMoveCount(int val)
    {
        _gameManager.moveCount += val;
        moveCountText.text = $"이동횟수: {_gameManager.moveCount:N0}";
    }
}
