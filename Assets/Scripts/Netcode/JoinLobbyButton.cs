using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinLobbyButton : MonoBehaviour
{
    public bool needPassword;
    public string lobbyId;
    public AudioSource click;

    public void JoinLobbyButtonPressed()
    {
        LobbyManager.Instance.JoinLobby(lobbyId, needPassword);
        click.Play();
    }
}