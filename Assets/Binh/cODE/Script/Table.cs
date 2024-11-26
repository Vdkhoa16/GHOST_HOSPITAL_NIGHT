using UnityEngine;
using System.Collections;
using Unity.Netcode;
using System.Collections.Generic;

public class GhostAppear : NetworkBehaviour
{
    public GameObject ghost; // Tham chiếu tới đối tượng con ma
                             // public GameObject panel; // Tham chiếu tới đối tượng panel
    public AudioSource audio;
    private bool hasShownPanel = false; // Biến flag kiểm tra xem panel đã hiển thị chưa
    private void Start()
    {
      //  ghost.SetActive(false); // Ẩn con ma lúc ban đầu
       // panel.SetActive(false); // Ẩn panel lúc ban đầu
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasShownPanel) // Kiểm tra nếu người chơi tiến vào và panel chưa hiển thị
        {
            hasShownPanel = true; // Đánh dấu rằng panel đã hiển thị

            //panel.SetActive(true); // Hiện panel khi người chơi lại gần
            OnOpendServerRpc(true);

        }
    }

    // Coroutine để ẩn panel và hiển thị ghost sau 3 giây
    private IEnumerator HidePanelAndShowGhost(float delay,bool visible)
    {
        yield return new WaitForSeconds(delay); // Đợi trong 3 giây
        audio.Play();
       // panel.SetActive(false); // Ẩn panel
        ghost.SetActive(visible); // Hiện con ma
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnOpendServerRpc(bool visible)
    {
        OnOpendClientRpc(visible);
    }

    [ClientRpc]
    private void OnOpendClientRpc(bool visible, ClientRpcParams rpcParams = default)
    {
        StartCoroutine(HidePanelAndShowGhost(3f,visible));
    }

}
