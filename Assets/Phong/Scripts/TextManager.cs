using UnityEngine;

public class TextManager : MonoBehaviour
{
    public static TextManager instance; // Thể hiện duy nhất

    [SerializeField] private GameObject textObject; // Đối tượng chứa text

    private void Awake()
    {
        // Nếu chưa có instance, gán nó và giữ lại đối tượng khi chuyển scene
        if (instance == null)
        {
            instance = this;
            // Kiểm tra xem gameObject này có phải là root GameObject không
            if (transform.parent != null)
            {
                // Nếu không phải root, chuyển nó về root
                transform.SetParent(null);
            }
            DontDestroyOnLoad(gameObject); // Đảm bảo đối tượng không bị xóa khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // Hủy nếu đã có một instance
        }
    }

    public void ShowText()
    {
        if (textObject != null)
        {
            textObject.SetActive(true); // Hiện text
            Debug.Log("Text is displayed");
        }
        else
        {
            Debug.LogError("TextObject is null, cannot show text!");
        }
    }

    public void HideText()
    {
        if (textObject != null)
        {
            textObject.SetActive(false); // Ẩn text
            Debug.Log("Text is hidden");
        }
    }
}
