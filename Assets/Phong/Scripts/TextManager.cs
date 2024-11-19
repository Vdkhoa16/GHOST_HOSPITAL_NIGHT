using UnityEngine;

public class TextManager : MonoBehaviour
{
    public static TextManager instance; // Thể hiện duy nhất

    [SerializeField] private GameObject textObject; // Đối tượng chứa text

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Đảm bảo đối tượng không bị xóa khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // Hủy nếu đã có một instance
        }
    }

    public void ShowText()
    {
        textObject.SetActive(true); // Hiện text
        Debug.Log("Text is displayed");
    }
}
