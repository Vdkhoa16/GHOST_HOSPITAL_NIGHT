using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Puzzle : MonoBehaviour
{
    // Start is called before the first frame update
    public NumberBox boxPrefab;
    public NumberBox[,] boxes = new NumberBox[4, 4];
    public Sprite[] sprites;
    public GameObject puzzlePanel;
    // Start is called before the first frame update
    void Start()
    {
        Init();
/*        for (int i = 0; i < 999; i++)*/
            Shuffle();
    }

    // Update is called once per frame
    void Init()
    {
        int n = 0;

        for (int y = 3; y >= 0; y--)
            for (int x = 0; x < 4; x++)
            {
                /*                NumberBox box = Instantiate(boxPrefab, new Vector2(x,y), Quaternion.identity,transform);*/
                // Tạo GameObject NumberBox dưới PuzzlePanel (UI)
                NumberBox box = Instantiate(boxPrefab, puzzlePanel.transform);

                box.Init(x, y, n + 1,sprites[n], ClickToSwap);
                boxes[x, y] = box;
                n++;
            }
    }
    void ClickToSwap(int x, int y)
    {
        int dx = getDx(x, y);
        int dy = getDy(x, y);
        Swap(x, y, dx, dy);
    }
    void Swap(int x, int y, int dx, int dy)
    {


        var from = boxes[x, y];
        var target = boxes[x + dx, y + dy];

        // swap this 2 boxes
        boxes[x, y] = target;
        boxes[x + dx, y + dy] = from;

        //update pos 2 boxes
        from.UpdatePos(x + dx, y + dy);
        target.UpdatePos(x, y);

        // After each swap, check if the puzzle is solved
        CheckWinCondition();
    }
    int getDx(int x, int y)
    {
        // is right empty
        if (x < 3 && boxes[x + 1, y].IsEmpty())
            return 1;

        // is left empty
        if (x > 0 && boxes[x - 1, y].IsEmpty())
            return -1;

        return 0;
    }
    int getDy(int x, int y)
    {
        // is top empty
        if (y < 3 && boxes[x, y + 1].IsEmpty())
            return 1;

        // is bottom empty
        if (y > 0 && boxes[x, y - 1].IsEmpty())
            return -1;

        return 0;
    }
    void Shuffle()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (boxes[i, j].IsEmpty())
                {
                    Vector2 pos = getValidMove(i, j);
                    Swap(i, j, (int)pos.x, (int)pos.y);
                }
            }
        }
    }
    private Vector2 lastMove;
    void CheckWinCondition()
    {
        int correctIndex = 1; // The correct sequence should be 1, 2, ..., 15

        // Loop through all boxes except the empty one
        for (int y = 3; y >= 0; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                // Skip the empty box (index 16)
                if (boxes[x, y].IsEmpty())
                    continue;

                // Check if the current box has the correct number
                if (boxes[x, y].index != correctIndex)
                {
                    return; // Puzzle is not solved yet
                }

                correctIndex++;

                // If we reach 15, we have successfully arranged the puzzle
                if (correctIndex == 16)
                {
                    Debug.Log("Puzzle solved!");
                    TriggerCombat();
                    return;
                }
            }
        }
    }
    void TriggerCombat()
    {
        Debug.Log("Combat mode triggered!");

    }
    Vector2 getValidMove(int x, int y)
    {
        Vector2 pos = new Vector2();
        do
        {
            int n = Random.Range(0, 4);
            if (n == 0)
                pos = Vector2.left;
            else if (n == 1)
                pos = Vector2.right;
            else if (n == 2)
                pos = Vector2.up;
            else
                pos = Vector2.down;
        } while (!(isValidRange(x + (int)pos.x) && isValidRange(y + (int)pos.y)) || isRepeatMove(pos));
        lastMove = pos;
        return pos;
    }
    bool isValidRange(int n)
    {
        return n >= 0 && n <= 3;
    }
    bool isRepeatMove(Vector2 pos)
    {
        return pos * -1 == lastMove;
    }
}
