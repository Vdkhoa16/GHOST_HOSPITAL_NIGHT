using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [HideInInspector]public bool isSelected;
    [HideInInspector]public GameObject selected_obj;

    public GameObject[] Tubes;

    public bool[] level_tubes_completed;

    bool isGameCompleted;

    [SerializeField] GameObject GameComplete_Canvas;
    private void Start()
    {
        isSelected = false;
    }

    private void OnDestroy()
    {
        Destroy(instance);
    }
    public void Reset_Selected()
    {
        for (int i = 0; i < Tubes.Length; i++)
        {
            Tubes[i].gameObject.transform.GetChild(3).gameObject.transform.GetComponent<tube_scipts>().isSelectedthis = false;
        }

    }
    public void CheckGameComplete()
    {
        for (int i = 0; i < level_tubes_completed.Length; i++)
        {
            if (level_tubes_completed[i] == false)
            {
                isGameCompleted = false;
            }
            else if (level_tubes_completed[i] == true)
            {
                isGameCompleted = true;
            }
        }
        if (isGameCompleted)
        {
            GameComplete_Canvas.SetActive(true);
            Debug.Log("Game is completed");
            // Gọi phương thức hiển thị text từ TextManager
            TextManager.instance.ShowText();

            // Gọi coroutine để thực hiện unload scene sau 2 giây
            StartCoroutine(UnloadSceneAfterDelay(2f));
        }
    }
    private IEnumerator UnloadSceneAfterDelay(float delay)
    {
        // Chờ trong khoảng thời gian đã chỉ định
        yield return new WaitForSeconds(delay);

        // Unload scene nếu scene đã được load
        string sceneToUnload = "BallSorting3DPuzzle"; // Thay thế bằng tên scene bạn đã load
        SceneManager.UnloadSceneAsync(sceneToUnload);
        Debug.Log("Scene unloaded successfully");

        // Khóa và ẩn con trỏ chuột
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
