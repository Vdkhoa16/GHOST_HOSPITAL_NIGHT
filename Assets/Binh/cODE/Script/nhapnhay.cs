using UnityEngine;

public class BlinkWithAlpha : MonoBehaviour
{
    public Renderer targetRenderer; // Renderer của object
    public float blinkSpeed = 2f; // Tốc độ nhấp nháy

    private Material material;
    private Color originalColor;

    private void Start()
    {
        material = targetRenderer.material;
        originalColor = material.color;
    }

    private void Update()
    {
        float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed)); // Tạo hiệu ứng nhấp nháy
        material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }
}
