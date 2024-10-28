using UnityEngine;

public class Cubeeeee : MonoBehaviour
{
    public AudioSource audioSource; // Âm thanh sẽ được phát khi lại gần đối tượng

    private void Start()
    {
        audioSource.Stop(); // Đảm bảo âm thanh không phát khi bắt đầu
    }

    // Khi người chơi vào vùng của đối tượng
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu người chơi (Player) vào vùng
        {
            if (!audioSource.isPlaying) // Chỉ phát nếu âm thanh chưa phát
            {
                audioSource.Play(); // Phát âm thanh khi lại gần
            }
        }
    }

    // Khi người chơi rời khỏi vùng của đối tượng
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu người chơi (Player) rời khỏi vùng
        {
            if (audioSource.isPlaying) // Chỉ tắt nếu âm thanh đang phát
            {
                audioSource.Stop(); // Tắt âm thanh khi rời xa
            }
        }
    }
}
