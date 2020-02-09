using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockLogic : MonoBehaviour
{
    #region Editable Attributes

    public static float fallTime = 1.5f; // Speed at which Blocks Fall
    public float speedIncrement = 0.1f; // Increment at which speed increases per time
    public float timeIncrement = 30f; // Time increment before speed increases
    public float maxFallTime = 0.1f; // Maximum speed at which blocks can fall
    public static float timePlaceholder;

    #endregion

    #region Class Attributes

    private float prevTime;
    public Vector3 rotationPoint;

    // Grid Dimensions and Instantiation
    public static int height = 20;
    public static int width = 10;
    private static Transform[,] grid = new Transform[width, height];

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(fallTime);
        // Increase fall speed over time
        if (Time.time - timePlaceholder > timeIncrement)
        {
            fallTime = (fallTime <= maxFallTime) ? fallTime : fallTime - speedIncrement;
            timePlaceholder = Time.time;
        }

        // If Left arrow is pressed perform a vector transformation to the left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(-1, 0, 0);
        }
        // If right arrow is pressed perform a vector transformation to the right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!ValidMove())
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
        }

        if (Time.time - prevTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckLines();

                this.enabled = false;
                FindObjectOfType<blockSpawn>().SpawnNewBlocks();
            }
            prevTime = Time.time;
        }
    }

    #region Class Methods

    void CheckLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if(CompletedLine(i))
            {
                RemoveLine(i);
                MoveRowDown(i);
            }
        }
    }

    bool CompletedLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }

    void RemoveLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    void MoveRowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int x = Mathf.RoundToInt(children.transform.position.x);
            int y = Mathf.RoundToInt(children.transform.position.y);

            grid[x, y] = children;
        }
    }

    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int x = Mathf.RoundToInt(children.transform.position.x);
            int y = Mathf.RoundToInt(children.transform.position.y);

            if(x < 0 || x >= width || y < 0 || y >= height)
            {
                return false;
            }

            if (grid[x, y] != null)
                return false;
        }
        return true;
    }

    #endregion
}
