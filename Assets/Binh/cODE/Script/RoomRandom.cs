using UnityEngine;

public class CubeRandomizer : MonoBehaviour
{
    public GameObject[] objects; // Mảng chứa 4 object
    private GameObject currentActiveObject = null; // Object hiện tại đang hiển thị

    private void Start()
    {
        HideAllObjects(); // Ẩn tất cả object khi bắt đầu
    }

    private void RandomizeObjects()
    {
        HideAllObjects(); // Ẩn tất cả object trước khi random

        int randomIndex = Random.Range(0, objects.Length); // Random chỉ số của object
        currentActiveObject = objects[randomIndex];
        currentActiveObject.SetActive(true); // Hiển thị object được chọn
    }

    private void HideAllObjects()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false); // Ẩn tất cả object
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu là người chơi
        {
            HideAllObjects(); // Ẩn object hiện tại
            RandomizeObjects(); // Random object mới để hiển thị
        }
    }
}
