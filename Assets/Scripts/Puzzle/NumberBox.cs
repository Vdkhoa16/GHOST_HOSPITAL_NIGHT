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
    private Image imageComponent;
    public void Init(int i, int j, int index,Sprite sprite, Action<int, int> swapFunc)
    {
        this.index = index;
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        if (!IsEmpty())
        {
            imageComponent.sprite = sprite;
            imageComponent.enabled = true; // Ensure the image is visible
        }
        else
        {
            imageComponent.enabled = false; // Hide the image for the empty box
        }
        UpdatePos(i, j);
        this.swapFunc = swapFunc;
    }
    void Awake()
    {
        // Get the Image component from the GameObject
        imageComponent = GetComponent<Image>();
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
    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && swapFunc != null)
            swapFunc(x, y);
    }
}
