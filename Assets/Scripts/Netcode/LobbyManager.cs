using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.Playables;

public class LobbyManager : MonoBehaviour
{
    private CharacterSelectDisplay characterSelectDisplay;
    public static LobbyManager Instance { get; private set; }

    [SerializeField] private AudioSource SounLbby, SoundPlayGame;

    [SerializeField] private RelayManager relayManager;


    [Header("Lobby creation")]
    [SerializeField] private GameObject lobbyCreationParent;
    [SerializeField] private TMP_InputField createLobbyNameField;
    [SerializeField] private TMP_InputField createLobbyMaxPlayersField;
    [SerializeField] private TMP_InputField createLobbyPasswordField;
    [SerializeField] private Toggle createLobbyPrivateToggle;
    [SerializeField] private GameObject createLobbyButton;

    [Space(10)]
    [Header("Lobby list")]
    [SerializeField] private GameObject lobbyListParent;
    [SerializeField] private Transform lobbyContentParent;
    [SerializeField] private Transform lobbyItemPrefab;
    [SerializeField] private TMP_InputField searchLobbyNameInputField;

    [Space(10)]
    [Header("Profile setup")]
    [SerializeField] private GameObject profileSetupParent;
    [SerializeField] private TMP_InputField profileNameField;

    [Space(10)]
    [Header("Joined lobby")]
    [SerializeField] private GameObject joinedLobbyParent;
    [SerializeField] private Transform playerItemPrefab;
    [SerializeField] private Transform playerListParent;
    [SerializeField] private TextMeshProUGUI joinedLobbyNameText;
    [SerializeField] private GameObject joinedLobbyStartButton;

    [Space(10)]
    [Header("Password protection")]
    [SerializeField] private Button inputPasswordButton;
    [SerializeField] private TMP_InputField inputPasswordField;
    [SerializeField] private GameObject inputPasswordParent;


    [Space(10)]
    [Header("Charater seleceted")]
    [SerializeField] private GameObject playerCLon;
    [SerializeField] private GameObject BKround;

    [Space(10)]
    [Header("Loading game")]
    [SerializeField] private GameObject LoadingParent;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text textloading;
    [SerializeField] private GameObject CharactersHolder;

    [Space(10)]
    [Header("Inventory")]
    [SerializeField] private GameObject inventory;

    [Space(10)]
    [Header("Error")]
    [SerializeField] private GameObject error;
    [SerializeField] private TextMeshProUGUI errorText;

    [Space(10)]
    [Header("Timeline")]
    [SerializeField] private PlayableDirector timeLineStartLobby;

    private string playerName;
    private Player playerData;
    private string joinedLobbyId;

    // check lobby chạy xong
    private bool isDelayComplete = false;

    private async void Start()
    {
        SounLbby.Play();// nhạc chạy lobby

        characterSelectDisplay = FindObjectOfType<CharacterSelectDisplay>();
        Instance = this;

        createLobbyPrivateToggle.onValueChanged.AddListener(OnCreateLobbyPrivateToggle); // đăng ký sự kiện trạng thái

        await UnityServices.InitializeAsync(); // khởi tạo 
        await AuthenticationService.Instance.SignInAnonymouslyAsync(); // đăng nhập vào cho phép người dùng tham gia không cần tạo tài khoản


        // setting cài đặt ẩn hiện cho danh sách lobby
        profileSetupParent.SetActive(true);
        lobbyListParent.SetActive(false);
        joinedLobbyParent.SetActive(false);
        lobbyCreationParent.SetActive(false);
        inputPasswordParent.SetActive(false);
        error.SetActive(false);
        createLobbyButton.SetActive(false);
    }
    private void Update()
    {
        // kiểm tra form
        CheckInputCreateLobby();
    }

    public void OnCreateLobbyPrivateToggle(bool value)
    {
        // đăng kí sự kiện phòng có mật khẩu hay không
        createLobbyPasswordField.gameObject.SetActive(value);
    }

    public void CreateProfile()
    {
        // kiểm tra form
        if (profileNameField.text == "")
        {
            error.SetActive(true); // thông báo erro bật
            errorText.text = "Vui lòng điền đầy đủ thông tin";
        }
        else
        {
            playerName = profileNameField.text; // lưu tên người chơi vào biến playerName 
            profileSetupParent.SetActive(false); 
            lobbyListParent.SetActive(true); // ẩn profile và hiển list room

            PlayerDataObject playerDataObjectName = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName); // tạo đối tượng PlayerDataObject , VisibilityOptions công khai

            playerData = new Player(id: AuthenticationService.Instance.PlayerId, data:
                new Dictionary<string, PlayerDataObject> { { "Name", playerDataObjectName } }); // Tạo một đối tượng Player mới, sử dụng ID người chơi từ dịch vụ xác thực và lưu trữ tên trong một từ điển. Điều này cho phép lưu trữ thông tin người chơi một cách có cấu trúc.

            ShowLobbies(); // gọi hàm showLobbies hiển thị danh sách các phòng 
            error.SetActive(false);
        }

    }


    public async void JoinLobby(string lobbyID, bool needPassword)
    {
        if (needPassword) //kiểm tra xem phòng có yêu cầu mật khẩu hay không
        {
            try
            {
                await LobbyService.Instance.JoinLobbyByIdAsync(lobbyID, new JoinLobbyByIdOptions
                { Password = await InputPassword(), Player = playerData }); // tiến hành bật form điền mật khẩu

                joinedLobbyId = lobbyID; 
                lobbyListParent.SetActive(false);
                joinedLobbyParent.SetActive(true);
                UpdateLobbyInfo();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
        else
        {
            try
            {
                await LobbyService.Instance.JoinLobbyByIdAsync(lobbyID, new JoinLobbyByIdOptions
                { Player = playerData });
                lobbyListParent.SetActive(false);
                joinedLobbyParent.SetActive(true);

                joinedLobbyId = lobbyID;
                UpdateLobbyInfo();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
    private async Task<string> InputPassword() // task<string> trả về một chuỗi 
    {
        bool waiting = true; // điều kiện kiểm tra
        inputPasswordParent.SetActive(true); // bật UI

        while (waiting)
        {
            inputPasswordButton.onClick.AddListener(() => waiting = false); // thay đổi điều kiện kết thúc vòng lặp
            await Task.Yield(); //Chờ một khung hình (frame) trước khi tiếp tục vòng lặp. Điều này cho phép Unity xử lý các sự kiện khác (như nhấn nút) trong khi vòng lặp đang chờ.
        }

        inputPasswordParent.SetActive(false);
        return inputPasswordField.text; // trả về một chuỗi mật khẩu
    }


    private async void ShowLobbies()
    { // vòng lặp chạy với đk đang chạy và đang hiển thị
        while (Application.isPlaying && lobbyListParent.activeInHierarchy)
        {
            // tìm kiếm room
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions();
            queryLobbiesOptions.Filters = new List<QueryFilter>();

            if (searchLobbyNameInputField.text != string.Empty)
            {
                queryLobbiesOptions.Filters.Add(new QueryFilter(QueryFilter.FieldOptions.Name, searchLobbyNameInputField.text, QueryFilter.OpOptions.CONTAINS));
            }

            // hiển thị lobby
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            // xóa danh sách lobby
            foreach (Transform t in lobbyContentParent)
            {
                Destroy(t.gameObject);
            }

            // hiển thị thông tin các phòng 
            foreach (Lobby lobby in queryResponse.Results)
            {
                Transform newLobbyItem = Instantiate(lobbyItemPrefab, lobbyContentParent);
                newLobbyItem.GetComponent<JoinLobbyButton>().lobbyId = lobby.Id; // id phòng
                newLobbyItem.GetComponent<JoinLobbyButton>().needPassword = lobby.HasPassword;
                newLobbyItem.GetChild(0).GetComponent<TextMeshProUGUI>().text = lobby.Name;
                newLobbyItem.GetChild(1).GetComponent<TextMeshProUGUI>().text = lobby.Players.Count + "/" + lobby.MaxPlayers;
            }

            await Task.Delay(5000); // tạm dừng bất đồng bộ trong 5s
        }
    }

    public async void LobbyStart()
    {
        textloading.text = "Loading..."; // khi người chơi bấm bắt đầu
        CharactersHolder.SetActive(false); // tắt giao diện chọn nhân vật
        Lobby lobby = await Lobbies.Instance.GetLobbyAsync(joinedLobbyId); // tắt dịch vụ bất đồng bộ 
        // gọi phương thức GetLobbyAsync
        string JoinCode = await relayManager.StartHostWithRelay(lobby.MaxPlayers);
        // khởi tạo máy chủ 
        // Show loading 
        LoadingParent.SetActive(true);
        loadingBar.value = 0.1f;
        //
        isJoined = true;
        // xác nhận người chơi đã tham gia
        await Lobbies.Instance.UpdateLobbyAsync(joinedLobbyId, new UpdateLobbyOptions // cập nhật thông tin
        {
            Data = new Dictionary<string, DataObject> { { "JoinCode", new DataObject(DataObject.VisibilityOptions.Public, JoinCode) } } //Tạo một DataObject mới, đặt chế độ hiển thị là Public và gán JoinCode (có thể là mã tham gia) làm giá trị của dữ liệu này.
        });
        loadingBar.value = 0.3f;
        lobbyListParent.SetActive(false);
        joinedLobbyParent.SetActive(false);
        loadingBar.value = 0.7f;
        BKround.SetActive(false);
        playerCLon.SetActive(false);
        loadingBar.value = 0.9f;
        StartCoroutine(DelayGame());

    }


    private bool isJoined = false;
    private async void UpdateLobbyInfo()
    {
        while (Application.isPlaying)
        {
            if (string.IsNullOrEmpty(joinedLobbyId))
            {
                return;
            }

            Lobby lobby = await Lobbies.Instance.GetLobbyAsync(joinedLobbyId);
            if (!isJoined && lobby.Data["JoinCode"].Value != string.Empty)
            {
                textloading.text = "Loading...";
                await relayManager.StartClientWithRelay(lobby.Data["JoinCode"].Value);
                isJoined = true;
                LoadingParent.SetActive(true);
                loadingBar.value = 0.3f;
                CharactersHolder.SetActive(false);
                joinedLobbyParent.SetActive(false);
                loadingBar.value = 0.7f;
                BKround.SetActive(false);
                playerCLon.SetActive(false);
                loadingBar.value = 0.9f;
                // chạy hàm delay
                StartCoroutine(DelayGame());
                return;
            }

            if (AuthenticationService.Instance.PlayerId == lobby.HostId)
            {
                joinedLobbyStartButton.SetActive(true);
            }
            else
            {
                joinedLobbyStartButton.SetActive(false);
            }

            joinedLobbyNameText.text = lobby.Name;

            foreach (Transform t in playerListParent)
            {
                Destroy(t.gameObject);
            }

            foreach (Player player in lobby.Players)
            {
                Transform newPlayerItem = Instantiate(playerItemPrefab, playerListParent);
                newPlayerItem.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.Data["Name"].Value;
                newPlayerItem.GetChild(1).GetComponent<TextMeshProUGUI>().text = (lobby.HostId == player.Id) ? "Chủ Phòng" : "Người Chơi";
            }

            await Task.Delay(1000);
        }
    }


    public void ExitLobbyCreationButton()
    {
        lobbyCreationParent.SetActive(false);
        lobbyListParent.SetActive(true);
        ShowLobbies();
    }

    public void CreateNewLobbyButton()
    {
        lobbyCreationParent.SetActive(true);
        lobbyListParent.SetActive(false);
    }


    public async void CreateLobby()
    {
        int pass = createLobbyPasswordField.text.Length;
        if (createLobbyPrivateToggle.isOn && pass < 8)
        {
            error.SetActive(true);
            errorText.text = "Mật khẩu ít nhất 8 kí tự";
        }
        else
        {
            if (!int.TryParse(createLobbyMaxPlayersField.text, out int maxPlayers))
            {
                Debug.Log("số lượng người chơi không đúng");
                error.SetActive(true);
                errorText.text = "abc";
                return;
            }

            Lobby createdLobby = null;

            CreateLobbyOptions options = new CreateLobbyOptions();
            options.IsPrivate = false;
            options.Player = playerData;

            if (createLobbyPrivateToggle.isOn)
            {
                options.Password = createLobbyPasswordField.text;
            }

            DataObject dataObjectJoinCode = new DataObject(DataObject.VisibilityOptions.Public, string.Empty);

            options.Data = new Dictionary<string, DataObject> { { "JoinCode", dataObjectJoinCode } };

            try
            {
                createdLobby = await LobbyService.Instance.CreateLobbyAsync(createLobbyNameField.text, maxPlayers, options);
                lobbyCreationParent.SetActive(false);
                joinedLobbyParent.SetActive(true);
                joinedLobbyId = createdLobby.Id;
                UpdateLobbyInfo();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }

            LobbyHeartbeat(createdLobby);
            error.SetActive(false);
        }
    }

    private async void LobbyHeartbeat(Lobby lobby)
    {
        while (true)
        {
            //kiểm tra có người trong lobby k
            if (lobby == null)
            {
                return;
            }

            await LobbyService.Instance.SendHeartbeatPingAsync(lobby.Id);

            await Task.Delay(15 * 1000);
            // sau 15s tạo lại
        }
    }
    private IEnumerator DelayGame()
    {
        SounLbby.Stop(); // dừng âm thanh
        inventory.SetActive(true);
        yield return new WaitForSeconds(5); // Đợi 5 giây
        loadingBar.value = 1.0f;
        LoadingParent.SetActive(false);
        Debug.Log("Đã qua 15s");
        SoundPlayGame.Play();

        characterSelectDisplay.Pick(); // chọn nhân vật vào game
        timeLineStartLobby.Play();
        // Đánh dấu là đã hoàn tất
        isDelayComplete = true;
    }
    public bool IsDelayComplete()
    {
        return isDelayComplete;
    }

    private void CheckInputCreateLobby()
    {

        // kiểm tra form 
        if (createLobbyNameField.text == "" || createLobbyMaxPlayersField.text == ""
            || (createLobbyPrivateToggle.isOn && createLobbyPasswordField.text == ""))
        {
            createLobbyButton.SetActive(false);

        }
        else if (int.TryParse(createLobbyMaxPlayersField.text, out int maxplayer)) // kiểm tra phải số hay không
        {
            if (maxplayer >= 5)
            {                
                // kiểm tra số lượng người chơi
                error.SetActive(true);
                errorText.text = "Tối đa 4 người chơi";
                createLobbyButton.SetActive(false);

            }
            else
            {
                createLobbyButton.SetActive(true);
                // nếu đúng thì button sẽ hiển thị
            }
        }
        else
        {
            createLobbyButton.SetActive(true);
            // nếu đúng thì button sẽ hiển thị
        }

    }
}