using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Audio_dir : MonoBehaviour
{
    [SerializeField] private AudioSource zoneAudioSource; // Âm thanh phát trong vùng
    //private HashSet<ulong> playersInZone = new HashSet<ulong>(); // Danh sách người chơi trong vùng
    //[SerializeField] private bool isRepeatable = true; // Âm thanh lặp lại hay chỉ phát một lần
    //private HashSet<ulong> playersWhoTriggered = new HashSet<ulong>(); // Danh sách người chơi đã kích hoạt âm thanh một lần

    private void Start()
    {
        if (zoneAudioSource != null)
        {
            zoneAudioSource.Stop(); // Dừng âm thanh khi bắt đầu
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player") && other.TryGetComponent<NetworkObject>(out var networkObject))
        //{
        //    ulong clientId = networkObject.OwnerClientId;

        //    // Kiểm tra nếu là âm thanh không lặp lại và đã phát với người chơi này
        //    if (!isRepeatable && playersWhoTriggered.Contains(clientId))
        //    {
        //        return; // Không làm gì nếu âm thanh đã phát
        //    }

        //    if (!playersInZone.Contains(clientId))
        //    {
        //        playersInZone.Add(clientId);
        //        playersWhoTriggered.Add(clientId); // Đánh dấu người chơi đã kích hoạt âm thanh
        //        UpdateSoundStateServerRpc(clientId, true); // Bật âm thanh cho người chơi
        //    }
        //}
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<NetworkObject>().IsOwner)
            {
                zoneAudioSource.Play();
            }
            else
            {
                zoneAudioSource.Stop();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Player") && other.TryGetComponent<NetworkObject>(out var networkObject))
        //{
        //    ulong clientId = networkObject.OwnerClientId;
        //    if (playersInZone.Contains(clientId))
        //    {
        //        playersInZone.Remove(clientId);
        //        UpdateSoundStateServerRpc(clientId, false); // Tắt âm thanh cho người chơi
        //    }
        //}
        if (other.CompareTag("Player"))
        {
            zoneAudioSource.Stop();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateSoundStateServerRpc(ulong clientId, bool playSound)
    {
        UpdateSoundStateClientRpc(playSound, new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new List<ulong> { clientId }
            }
        });
    }

    [ClientRpc]
    private void UpdateSoundStateClientRpc(bool playSound, ClientRpcParams rpcParams = default)
    {
        if (zoneAudioSource != null)
        {
            if (playSound)
            {
                if (!zoneAudioSource.isPlaying) zoneAudioSource.Play();
            }
            else
            {
                if (zoneAudioSource.isPlaying) zoneAudioSource.Stop();
            }
        }
    }
}
