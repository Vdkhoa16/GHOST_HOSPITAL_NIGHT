using UnityEngine;
using System.Collections;

public class GhostAppear : MonoBehaviour
{
    public GameObject ghost; // Tham chiếu tới đối tượng con ma
    public GameObject panel; // Tham chiếu tới đối tượng panel

    private bool hasShownPanel = false; // Biến flag kiểm tra xem panel đã hiển thị chưa

    private void Start()
    {
        ghost.SetActive(false); // Ẩn con ma lúc ban đầu
        panel.SetActive(false); // Ẩn panel lúc ban đầu
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasShownPanel) // Kiểm tra nếu người chơi tiến vào và panel chưa hiển thị
        {
            hasShownPanel = true; // Đánh dấu rằng panel đã hiển thị

            panel.SetActive(true); // Hiện panel khi người chơi lại gần

            StartCoroutine(HidePanelAndShowGhost(3f)); // Bắt đầu Coroutine để ẩn panel và hiển thị ghost sau 3 giây
        }
    }

    // Coroutine để ẩn panel và hiển thị ghost sau 3 giây
    private IEnumerator HidePanelAndShowGhost(float delay)
    {
        yield return new WaitForSeconds(delay); // Đợi trong 3 giây

        panel.SetActive(false); // Ẩn panel
        ghost.SetActive(true); // Hiện con ma
    }
}
