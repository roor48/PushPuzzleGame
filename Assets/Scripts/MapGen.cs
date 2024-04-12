using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class _2DArray
{
    public int[] arr;

    public _2DArray(int size)
    {
        arr = new int[size];
    }
}

public class MapGen : MonoBehaviour
{
    public Transform cameraTrans;
    public Texture2D mapImage;

    public Color wallColor = new Color(255, 255, 255);
    public GameObject wallPrefab;
    
    public Color moveObjectColor = new Color(185, 122, 87);
    public GameObject moveObjPrefab;

    public Color destinationColor = new Color(63, 72, 204);
    public GameObject destinationPrefab;

    public Color spawnColor = new Color(181, 230, 29);
    public GameObject playerPrefab;

    public _2DArray[] mapInfo;

    public PlayerMovements player;
    public int playerPosY, playerPosX;

    private void SetCameraPos(int height, int width)
    {
        Camera.main.orthographicSize = height / 2f;
        cameraTrans.position = new Vector3(height / 2f - 0.5f, width / 2f - 0.5f, -10);
    }
    
    public void BuildMap() => GenerateMap();
    private void GenerateMap()
    {
        int height = mapImage.height;
        int width = mapImage.width;
        
        SetCameraPos(height, width);

        mapInfo = new _2DArray[height];
        for (int i = 0; i < height; i++)
        {
            mapInfo[i] = new _2DArray(width);
        }
        
        Debug.Log($"height: {height}, width: {width}");

        Color[] pixels = mapImage.GetPixels();

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (pixels[i * height + j].Equals(wallColor))
                {
                    Debug.Log("Wall");
                    GameObject temp = Instantiate(wallPrefab, transform);
                    temp.transform.position = new Vector2(j, i);

                    mapInfo[i].arr[j] = 1;
                }
                
                else if (pixels[i * height + j].Equals(moveObjectColor))
                {
                    Debug.Log("move");
                    GameObject temp = Instantiate(moveObjPrefab, transform);
                    temp.transform.position = new Vector2(j, i);

                    MoveObject moveObj = temp.GetComponent<MoveObject>();
                    moveObj.Init(i, j);

                    mapInfo[i].arr[j] = 2;
                }
                
                else if (pixels[i * height + j].Equals(destinationColor))
                {
                    Debug.Log("des");
                    GameObject temp = Instantiate(destinationPrefab, transform);
                    temp.transform.position = new Vector2(j, i);

                    mapInfo[i].arr[j] = 3;
                }
                    
                else if (pixels[i * height + j].Equals(spawnColor))
                {
                    Debug.Log("spawn");
                    GameObject temp = Instantiate(playerPrefab);
                    temp.transform.position = new Vector2(j, i);

                    playerPosY = i; playerPosX = j;
                    player = temp.GetComponent<PlayerMovements>();
                }
            }
        }
        
        player.Init(playerPosY, playerPosX, mapInfo);
    }

    public void DeleteMap()
    {
        PlayerMovements playerObj = FindObjectOfType<PlayerMovements>();
        if (playerObj)
            DestroyImmediate(playerObj.gameObject);

        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
