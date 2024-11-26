using Unity.Netcode;
using UnityEngine;

public class SoundOnProximity : MonoBehaviour
{
    public AudioSource audioSource; // Tham chiếu đến AudioSource

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.loop = true; // Nếu bạn muốn âm thanh phát liên tục
            audioSource.Stop(); // Đảm bảo âm thanh không phát khi bắt đầu
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && audioSource != null && !audioSource.isPlaying)
        {
            // Phát âm thanh khi người chơi vào vùng gần

            if (other.GetComponent<NetworkObject>().IsOwner)
            {
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && audioSource != null && audioSource.isPlaying)
        {
            // Dừng âm thanh khi người chơi rời xa
            audioSource.Stop();
        }
    }
}
