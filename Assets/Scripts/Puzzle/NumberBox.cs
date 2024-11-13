using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;


public class NumberBox : MonoBehaviour
{
    // Start is called before the first frame update
    public int index = 0;
    int x = 0;
    int y = 0;
    private Action<int, int> swapFunc = null;
    public void Init(int i, int j, int index, Sprite sprite, Action<int, int> swapFunc)
    {
        this.index = index;
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        UpdatePos(i, j);
        this.swapFunc = swapFunc;
    }

    public void UpdatePos(int i, int j)
    {
        x = i;
        y = j;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float elapsedTime = 0;
        float duration = 0.1f;
        Vector2 start = this.gameObject.transform.localPosition;
        Vector2 end = new Vector2(x, y);
        while (elapsedTime < duration)
        {
            this.gameObject.transform.localPosition = Vector2.Lerp(start, end, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.gameObject.transform.localPosition = end;
    }
    public bool IsEmpty()
    {
        return index == 16;
    }
    public void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.Mouse0) && swapFunc != null)
            swapFunc(x, y);
        Debug.Log("Chạm");
    }
}
