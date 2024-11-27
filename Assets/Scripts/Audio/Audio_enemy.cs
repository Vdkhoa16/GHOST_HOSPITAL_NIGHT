using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Audio_enemy : MonoBehaviour
{
    [SerializeField] private AudioSource zoneAudioSource;

    private void Start()
    {
        if (zoneAudioSource != null)
        {
            zoneAudioSource.Stop(); // Dừng âm thanh khi bắt đầu
        }
    }


    public void OnTriggerEnter(Collider other)
    {

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

    public void OnTriggerExit(Collider other)
    {

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
