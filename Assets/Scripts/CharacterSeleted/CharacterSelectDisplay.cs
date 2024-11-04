﻿using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelectDisplay : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private Transform charactersHolder;
    [SerializeField] private CharacterSelectButton selectButtonPrefab;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private Transform introSpawnPoint;
    [SerializeField] private Transform spawnPoint;

    private GameObject introInstance;
    private List<CharacterSelectButton> characterButtons = new List<CharacterSelectButton>();
    private Character selectedCharacter;

    private void Start()
    {

        Character[] allCharacters = characterDatabase.GetAllCharacters(); // lấy danh sách nhân vật

        foreach (var character in allCharacters)
        {
            var selectButtonInstance = Instantiate(selectButtonPrefab, charactersHolder);
            selectButtonInstance.SetCharacter(this, character);
            characterButtons.Add(selectButtonInstance); // thêm bustton cho các nhân vật
        }
    }

    public void Select(Character character)
    {
        // chọn nhân vật hiển thị thông tin
        characterNameText.text = character.DisplayName;

        if (introInstance != null)
        {
            Destroy(introInstance);
        }

        introInstance = Instantiate(character.IntroPrefab, introSpawnPoint); // tạo ra nhân vật intro
        selectedCharacter = character;
    }

    public void Pick()
    {
        // chọn nhân vật 
        if (selectedCharacter == null)
        {
            SubmitCharacterSelectionServerRpc(1);
        }

        if (selectedCharacter != null)
        {
            SubmitCharacterSelectionServerRpc(selectedCharacter.Id);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitCharacterSelectionServerRpc(int characterId, ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;

        if (playerObject != null)
        {
            var character = characterDatabase.GetCharacterById(characterId);
            if (character != null)
            {
                var spawnPos = spawnPoint.position;
                var characterInstance = Instantiate(character.GameplayPrefab, spawnPos, Quaternion.identity);
                NetworkObject networkObject = characterInstance.GetComponent<NetworkObject>();
                networkObject.SpawnWithOwnership(clientId);
            }
        }
    }
}
