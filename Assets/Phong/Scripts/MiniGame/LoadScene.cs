using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObject : MonoBehaviour
{
    private bool isPlayerNearby = false;
    public string sceneToLoad = "BallSorting3DPuzzle"; // Tên scene bạn muốn load
    private bool isSceneLoaded = false;
    public GameObject pressEUI; // Tạo một biến công khai để lưu trữ GameObject hiển thị chữ "E"


    void Update()
    {
        // Kiểm tra nếu player đang gần và bấm phím E
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!isSceneLoaded)
            {
                // Load scene additively nếu chưa load
                SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
                isSceneLoaded = true;
                // Mở khóa và hiển thị con trỏ chuột
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                // Unload scene nếu scene đã được load
                SceneManager.UnloadSceneAsync(sceneToLoad);
                isSceneLoaded = false;
                // Khóa và ẩn con trỏ chuột lại
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    public void ActivateInputFields()
    {
        pressEUI.gameObject.SetActive(false);       
    }

    // Khi player vào vùng kích hoạt
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Đảm bảo player có tag "Player"
        {
            isPlayerNearby = true;        
            showPressEUI(true);  // Hiển thị chữ "E"
        }
    }

    // Khi player rời khỏi vùng kích hoạt
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;          
            showPressEUI(false);  // Ẩn chữ "E"
        }
    }
    void showPressEUI(bool show) // Hàm để hiển thị/ẩn chữ "E"
    {
        pressEUI.SetActive(show); // Chuyển đổi trạng thái hiển thị của pressEUI
    }
}
