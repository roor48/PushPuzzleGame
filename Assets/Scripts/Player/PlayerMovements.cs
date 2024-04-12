using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovements : MonoBehaviour
{
    public Text clearText;
    
    private MapGen mapGen;
    
    public MoveObject[] moveObjects;

    public float Speed => _gameManager.speed;

    public _2DArray[] mapInfo;

    public int curY;
    public int curX;

    public int mapHeight;
    public int mapWidth;
    
    public bool isMoving;

    private GameManager _gameManager;
    private void Start()
    {
        _gameManager = GameManager.Instance;
        
        mapGen = FindObjectOfType<MapGen>();
        clearText = GameObject.Find("ClearText").GetComponent<Text>();
        clearText.text = "";
        
        moveObjects = FindObjectsOfType<MoveObject>();

        transform.position = new Vector3(curX, curY, 0);
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

    public void VerticalMove(int val)
    {
        Move(val, 0);
    }

    public void HorizontalMove(int val)
    {
        Move(0, val);
    }
    
    private void Move(int Vertical, int Horizontal)
    {
        if (!CanMove(Vertical, Horizontal))
            return;
            
        int nextY = curY + Vertical;
        int nextX = curX + Horizontal;

        if (mapInfo[nextY].arr[nextX] is 2 or 4)
        {
            int nextObjectPosY = nextY + Vertical;
            int nextObjectPosX = nextX + Horizontal;

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
                
            StartCoroutine(FindPositionMoveObject(nextY, nextX).Move(Vertical, Horizontal, Speed));
        }

        StartCoroutine(MoveAnim(nextY, nextX));
    }

    private bool CanMove(int Vertical, int Horizontal)
    {
        if (isMoving)
            return false;
        
        int nextY = curY + Vertical;
        int nextX = curX + Horizontal;
        
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
            int nextObjectPosY = nextY + Vertical;
            int nextObjectPosX = nextX + Horizontal;
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

    private MoveObject FindPositionMoveObject(int yPos, int xPos)
    {
        foreach (var moveObject in moveObjects)
        {
            if (yPos == moveObject.curY && xPos == moveObject.curX)
                return moveObject;
        }

        return null;
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
}
