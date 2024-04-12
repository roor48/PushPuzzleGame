using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public int curX;
    public int curY;

    private void Start()
    {
        transform.position = new Vector3(curX, curY, 0);
    }

    public void Init(int yPos, int xPos)
    {
        curY = yPos;
        curX = xPos;

        transform.position = new Vector3(curX, curY, 0);
    }

    public IEnumerator Move(int yVal, int xVal, float speed)
    {
        Vector3 startPos = new Vector3(curX, curY, 0);
        Vector3 endPos = new Vector3(curX + xVal, curY + yVal, 0);

        float percent = 0f;
        while (percent <= 1f)
        {
            percent += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(startPos, endPos, percent);

            yield return null;
        }

        curY += yVal;
        curX += xVal;
    }
}
