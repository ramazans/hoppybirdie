using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour {

    public GameObject up1;
    public GameObject up2;
    public GameObject up3;
    public GameObject up4;
    public GameObject up5;
    public GameObject up6;
    public GameObject up7;
    public GameObject up8;
    public GameObject up9;

    public GameObject down9;
    public GameObject down8;
    public GameObject down7;
    public GameObject down6;
    public GameObject down5;
    public GameObject down4;
    public GameObject down3;
    public GameObject down2;
    public GameObject down1;

    int[] upRange = { 1, 9 };
    int up, upIndex, down, downIndex = 0;

    List<GameObject> upList = new List<GameObject>();
    List<GameObject> downList = new List<GameObject>();

    void Start()
    {
        upList.Add(up1);
        upList.Add(up2);
        upList.Add(up3);
        upList.Add(up4);
        upList.Add(up5);
        upList.Add(up6);
        upList.Add(up7);
        upList.Add(up8);
        upList.Add(up9);

        downList.Add(down1);
        downList.Add(down2);
        downList.Add(down3);
        downList.Add(down4);
        downList.Add(down5);
        downList.Add(down6);
        downList.Add(down7);
        downList.Add(down8);
        downList.Add(down9);
    }

    public void RandomBlock()
    {
        up = Random.Range(upRange[0], upRange[1]);
        down = GetDownValue(up);

        foreach (var item in upList)
        {
            upIndex++;
            item.SetActive(upIndex <= up);
        }
        upIndex = 0;

        foreach (var item in downList)
        {
            downIndex++;
            item.SetActive(downIndex <= down);
        }
        downIndex = 0;

        //Debug.Log("up: " + up + " down: " + down);
    }

    int GetDownValue(int up)
    {
        switch (up)
        {
            case 1:
                return Random.Range(7, 9);
            case 2:
                return Random.Range(6, 8);
            case 3:
                return Random.Range(5, 7);
            case 4:
                return Random.Range(4, 7);
            case 5:
                return Random.Range(3, 5);
            case 6:
                return Random.Range(2, 4);
            case 7:
                return Random.Range(2, 4);
            case 8:
                return Random.Range(2, 3);
            case 9:
                return Random.Range(1, 3);
            default:
                break;
        }
        return 0;
    }
}
