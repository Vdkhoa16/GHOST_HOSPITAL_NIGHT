using System.Collections.Generic;
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
        // lấy tất cả các charac ter có trong dataa
        Character[] allCharacters = characterDatabase.GetAllCharacters();

        foreach (var character in allCharacters)
        {
        
            var selectButtonInstance = Instantiate(selectButtonPrefab, charactersHolder);
            selectButtonInstance.SetCharacter(this, character);
            characterButtons.Add(selectButtonInstance); // tạo list button để chọn nhân vật
            if (character.Look)
            {
                selectButtonInstance.SetDisabled();
            }
        }
        
    }

    public void Select(Character character)
    {
        // hiển thị tên nv
        characterNameText.text = character.DisplayName;

        if (introInstance != null)
        {
            Destroy(introInstance);
            
        }
        // tạo ra nhân vật nhảy
        introInstance = Instantiate(character.IntroPrefab, introSpawnPoint);
        selectedCharacter = character;
    }

    public void Pick()
    {
        // sự kiển khi chọn vào nhân vật
        if (selectedCharacter == null)
        {
            // nếu k chọn mặc định là nhân vaatj 1
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
        // đồng bộ nhầm thông báo cho các player khác
        var clientId = serverRpcParams.Receive.SenderClientId;
        var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;

        if (playerObject != null)
        {
            var character = characterDatabase.GetCharacterById(characterId);
            if (character != null)
            {
                var spawnPos = spawnPoint.position; // tạo ra nhân vật ở vi trí
                var characterInstance = Instantiate(character.GameplayPrefab, spawnPos, Quaternion.identity);
                NetworkObject networkObject = characterInstance.GetComponent<NetworkObject>();
                networkObject.SpawnWithOwnership(clientId);
            }
        }
    }
}
