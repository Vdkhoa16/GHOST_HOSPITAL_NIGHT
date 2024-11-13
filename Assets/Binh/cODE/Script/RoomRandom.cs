using UnityEngine;
using System.Collections;

public class CubeRandomizer : MonoBehaviour
{
    public GameObject[] objects; // Mảng chứa các object
    private int currentIndex = 0; // Chỉ số hiện tại của object đang hiển thị
    private bool isReadyToChange = false; // Kiểm tra nếu đã sẵn sàng để chuyển đối tượng tiếp theo

    private void Start()
    {
        HideAllObjects(); // Ẩn tất cả object khi bắt đầu
        ShowCurrentObject(); // Hiển thị object đầu tiên
    }

    private void ShowCurrentObject()
    {
        HideAllObjects(); // Ẩn tất cả object trước khi hiển thị cái mới

        if (currentIndex < objects.Length) // Kiểm tra nếu còn object trong danh sách
        {
            objects[currentIndex].SetActive(true); // Hiển thị object hiện tại
        }
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
        if (other.CompareTag("Player"))
        {
            if (isReadyToChange) // Kiểm tra nếu đã sẵn sàng chuyển sang đối tượng tiếp theo
            {
                if (currentIndex < objects.Length - 1) // Nếu chưa phải object cuối cùng
                {
                    currentIndex++; // Tăng chỉ số để hiển thị object tiếp theo
                    ShowCurrentObject(); // Hiển thị object tiếp theo
                }
                isReadyToChange = false; // Reset trạng thái để chờ lần va chạm tiếp theo
            }
            else
            {
                // Nếu chưa sẵn sàng, bắt đầu đếm thời gian chờ 5 giây
                StartCoroutine(PrepareToChangeAfterDelay(5f));
            }
        }
    }

    // Coroutine để kích hoạt trạng thái sẵn sàng chuyển object sau một khoảng thời gian
    private IEnumerator PrepareToChangeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Đợi trong khoảng thời gian delay
        isReadyToChange = true; // Đặt trạng thái đã sẵn sàng để chuyển đối tượng khi va chạm tiếp theo
    }
}
