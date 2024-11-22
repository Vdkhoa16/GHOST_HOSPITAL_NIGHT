using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;  // Để sử dụng UI như Slider

public class NextLevel : MonoBehaviour
{
    public Slider loadingBar; 
    public GameObject load;
    public string sceneName = "GamePlay";  // Tên scene bạn muốn tải

    private AsyncOperation asyncLoad;  // Lưu AsyncOperation để kiểm soát việc chuyển scene
    private bool isSceneReady = false;  // Kiểm tra xem scene đã tải xong chưa


    void Start()
    {
        // khi chạy gọi hàm bất đồng bộ để tải sceen trước giúp chuyển sceen nhanh
        StartCoroutine(PreloadScene());
    }

    // Tải scene bất đồng bộ 
    private IEnumerator PreloadScene() // IEnumerator tạm dừng
    {
        // Bắt đầu tải scene bất đồng bộ
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;  // Không cho phép kích hoạt ngay lập tức

        // Cập nhật thanh trượt trong khi tải
        while (!asyncLoad.isDone)
        {
            loadingBar.value = asyncLoad.progress;

            if (asyncLoad.progress >= 0.9f)
            {
                loadingBar.value = 1f;  // Đảm bảo thanh trượt đầy khi hoàn thành
                isSceneReady = true;  // báo sceen tải xong
                load.SetActive(false); // tải xong thì ẩn thanh load
  
            }

            yield return null;
        }
    }

    // Hàm chuyển scene khi nhấn nút Next
    public void Next()
    {
        // nếu sceen tải xong thì cho phép chuyển sceen
        if (isSceneReady)
        {
            asyncLoad.allowSceneActivation = true;  // Kích hoạt sceen
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
