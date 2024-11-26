using UnityEngine;

public class hihi : MonoBehaviour
{
    public GameObject letterUI; // UI của lá thư

    void Start()
    {
        if (letterUI != null)
        {
            letterUI.SetActive(false); // Ẩn UI lá thư khi bắt đầu
        }
    }

    void Update()
    {
        // Nếu người chơi ở gần object và nhấn F thì bật/tắt UI lá thư
        //if (isNearLetter && Input.GetKeyDown(KeyCode.F))
        //{
        //    letterUI.SetActive(!letterUI.activeSelf); // Toggle trạng thái UI lá thư
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu là người chơi
        {
            letterUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            letterUI.SetActive(false); // Đóng UI lá thư khi đi ra xa
        }
    }
}
