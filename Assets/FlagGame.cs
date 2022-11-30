using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagGame : MonoBehaviour
{
    public int gridLength = 9;
    public GameObject gridPrefab;
    int[,] gameBoard;
    GameObject[,] terrain;

    // Start is called before the first frame update
    void Start()
    {
        // 0 (start), 1 (path), 2 (mountain), 3 (lava), 4 (water), 5 (flag)
        gameBoard = new int[gridLength, gridLength];
        terrain = new GameObject[gridLength, gridLength];
        int flagX = Random.Range(2, gridLength + 1);
        int flagY = Random.Range(0, gridLength + 1);

        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridLength; j++)
            {
                if (i == flagX && j == flagY) gameBoard[i, j] = 5;
                else if (i == 0 && j == 0) gameBoard[i, j] = 0;
                else gameBoard[i, j] = Random.Range(1, 5);
            }
        }

        float v;
        Color b;
        Color m;
        Color t;

        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridLength; j++)
            {
                if (gameBoard[i, j] == 0 || gameBoard[i, j] == 5)
                {
                    v = 0f;
                    b = Color.green;
                    m = Color.green;
                    t = Color.green;
                }
                else if (gameBoard[i, j] != 2)
                { 
                    v = Random.Range(-4f, -10f);
                    if (gameBoard[i, j] == 4)
                    {
                        b = Color.green;
                        m = Color.green;
                        t = Color.blue;
                    }
                    else
                    {
                        b = Color.black;
                        m = Color.red;
                        t = Color.red;
                    }
                }
                else
                {
                    v = Random.Range(4f, 20f);
                    b = Color.red;
                    m = Color.green;
                    t = Color.grey;
                }
                GameObject location = Instantiate(gridPrefab, new Vector3(i * 10, 0, j * 10), Quaternion.identity) as GameObject;
                location.name = "Location " + i + ", " + j;

                MeshGenerator curr = location.GetComponent(typeof(MeshGenerator)) as MeshGenerator;
                curr.xSize = 10;
                curr.zSize = 10;
                curr.variance = v;
                curr.bottom = b;
                curr.middle = m;
                curr.top = t;
                terrain[i, j] = location;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
