using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlagGame : MonoBehaviour
{
    public int gridLength = 9;
    public GameObject gridPrefab;
    int[,] gameBoard;
    GameObject[,] terrain;
    int flagX, flagY;

    // Start is called before the first frame update
    void Start()
    {
        // 0 (start), 1 (path), 2 (mountain), 3 (lava), 4 (water), 5 (flag)
        gameBoard = new int[gridLength, gridLength];
        terrain = new GameObject[gridLength, gridLength];
        flagX = Random.Range(0, gridLength);
        flagY = Random.Range(2, gridLength);

        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridLength; j++)
            {
                if (i == flagX && j == flagY) gameBoard[i, j] = 5;
                else if (i == 0 && j == 0) gameBoard[i, j] = 0;
                else gameBoard[i, j] = Random.Range(2, 5);
            }
        }

        createPath(gridLength * 2);

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
                else if (gameBoard[i, j] == 2)
                {
                    v = Random.Range(4f, 20f);
                    b = Color.red;
                    m = Color.green;
                    t = Color.grey;
                }
                else if (gameBoard[i, j] == 3)
                {
                    v = Random.Range(-4f, -10f);
                    b = Color.black;
                    m = Color.red;
                    t = Color.red;
                }
                else if (gameBoard[i, j] == 4)
                {
                    v = Random.Range(-4f, -10f);
                    b = Color.blue;
                    m = Color.black;
                    t = Color.blue;
                }
                else
                {
                    v = Random.Range(2f, 5f);
                    b = Color.yellow;
                    m = Color.cyan;
                    t = Color.magenta;
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

    void createPath(int n)
    {
        int sX = 0;
        int sY = 0;
        HashSet<string> path = new HashSet<string>();
        path.Add("0 0");
        path.Add("" + flagX + " " + flagY);

        while (sX != flagX || sY != flagY)
        {
            if (flagX != sX) sX++;
            else if (flagY != sY) sY++;

            if (!(sX == flagX && sY == flagY))
            {
                path.Add("" + sX + " " + sY);
                gameBoard[sX, sY] = 1;
                n--;
            }
        }

        List<string> t;
        HashSet<int> visitedIndices = new HashSet<int>();
        int c = 0;
        while (c < n)
        {
            int r = Random.Range(2, path.Count);
            c++;
            if (visitedIndices.Contains(r)) continue;

            visitedIndices.Add(r);
            t = path.ToList();

            string bump = t[r];
            string[] pt = bump.Split(" ");
            int x = int.Parse(pt[0]);
            int y = int.Parse(pt[1]);

            List<string> check = new List<string>();

            if (x + 1 < gridLength && !path.Contains("" + (x + 1) + " " + y))
            {
                check.Add("" + (x + 1) + " " + y);
            }
            else if (x - 1 >= 0 && !path.Contains("" + (x - 1) + " " + y))
            {
                check.Add("" + (x - 1) + " " + y);
            }
            else if (y + 1 < gridLength && !path.Contains("" + x + " " + (y + 1)))
            {
                check.Add("" + x + " " + (y + 1));
            }
            else if (y - 1 >= 0 && !path.Contains("" + x + " " + (y - 1)))
            {
                check.Add("" + x + " " + (y - 1));
            }

            if (check.Count == 0) continue;

            int dir = Random.Range(0, check.Count);
            string[] tPt = check[dir].Split(" ");
            int tX = int.Parse(tPt[0]);
            int tY = int.Parse(tPt[1]);
            bool p1 = false, p2 = false;

            if (tX == x)
            {
                if (tX + 1 < gridLength && !path.Contains("" + (tX + 1) + " " + tY))
                {
                    gameBoard[tX + 1, tY] = 1;
                    path.Add("" + (tX + 1) + " " + tY);
                    n--;
                    p1 = true;
                }

                if (tX - 1 >= 0 && !path.Contains("" + (tX - 1) + " " + tY))
                {
                    gameBoard[tX - 1, tY] = 1;
                    path.Add("" + (tX - 1) + " " + tY);
                    n--;
                    p2 = true;
                }
            }
            else if (tY == y)
            {
                if (tY + 1 < gridLength && !path.Contains("" + tX + " " + (tY + 1)))
                {
                    gameBoard[tX, tY + 1] = 1;
                    path.Add("" + tX + " " + (tY + 1));
                    n--;
                    p1 = true;
                }

                if (tY - 1 >= 0 && !path.Contains("" + tX + " " + (tY - 1)))
                {
                    gameBoard[tX, tY - 1] = 1;
                    path.Add("" + tX + " " + (tY - 1));
                    n--;
                    p2 = true;
                }
            }
            if (p1 || p2)
            {
                gameBoard[x, y] = Random.Range(2, 5);
                gameBoard[tX, tY] = 1;
                path.Remove(bump);
                path.Add(check[dir]);
                visitedIndices.Remove(r);
                c = 0;
            }
        }
        return;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
