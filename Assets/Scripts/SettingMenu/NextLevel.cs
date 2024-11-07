using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;  // Để sử dụng UI như Slider

public class NextLevel : MonoBehaviour
{
    public Slider loadingBar;  // Thanh trượt UI để hiển thị tiến trình tải
    public GameObject load;
    public string sceneName = "Scene1";  // Tên scene bạn muốn tải

    private AsyncOperation asyncLoad;  // Lưu AsyncOperation để kiểm soát việc chuyển scene
    private bool isSceneReady = false;  // Kiểm tra xem scene đã tải xong chưa


    void Start()
    {
        // Bắt đầu tải scene bất đồng bộ ngay khi game bắt đầu
        StartCoroutine(PreloadScene());
    }

    // Tải scene bất đồng bộ ngay từ đầu
    private IEnumerator PreloadScene()
    {
        // Bắt đầu tải scene bất đồng bộ
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;  // Không cho phép scene kích hoạt ngay lập tức

        // Cập nhật thanh trượt trong khi tải
        while (!asyncLoad.isDone)
        {
            loadingBar.value = asyncLoad.progress;

            // Khi tiến trình đạt 0.9 (khoảng 90% tải xong), cho phép scene được kích hoạt
            if (asyncLoad.progress >= 0.9f)
            {
                loadingBar.value = 1f;  // Đảm bảo thanh trượt đầy khi hoàn thành
                isSceneReady = true;  // Đánh dấu rằng scene đã tải xong và sẵn sàng
                load.SetActive(false);
  
            }

            yield return null;
        }
    }

    // Hàm chuyển scene khi nhấn nút Next
    public void Next()
    {
        if (isSceneReady)
        {
            asyncLoad.allowSceneActivation = true;  // Kích hoạt scene khi người dùng nhấn Next
        }
    }

    // Hàm thoát game
    public void ExitGame()
    {
#if UNITY_EDITOR
        // Dừng chế độ Play trong Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // Thoát game khi build
            Application.Quit();
#endif
    }
}
