// using UnityEngine;
// using Photon.Pun;
// using Photon.Realtime;
// using UnityEngine.UI;

// public class TestNetwork : MonoBehaviourPunCallbacks
// {
//     public Text StatusText;
//     public InputField NickNameInput;
//     public GameObject GameObject;
//     public string gameVersion = "1.0";

//     void Awake()
//     {
//         PhotonNetwork.AutomaticallySyncScene = true;
//     }

//     void Start()
//     {
//         ConnectToPhoton();
//     }

//     void ConnectToPhoton()
//     {
//         PhotonNetwork.GameVersion = gameVersion;
//          PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "a7a07b8b-7f92-42a0-a320-3ded50984f49";
//         PhotonNetwork.ConnectUsingSettings();
//     }

//     public override void OnConnectedToMaster()
//     {
//         Debug.Log("Connected to Photon Master Server");

//         // 닉네임 설정
//         SetNickname();

//         // 랜덤한 방에 입장 시도
//         PhotonNetwork.JoinRandomRoom();
//     }

//     void SetNickname()
//     {
//        // if (!string.IsNullOrEmpty(NickNameInput.text))
//        // {
//        //     PhotonNetwork.NickName = NickNameInput.text;
//       //  }
//       //  else
//       //  {
//             PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);
//        // }
//     }

//     public override void OnJoinedRoom()
//     {
//         Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
//         StatusText.text = "Joined Room: " + PhotonNetwork.CurrentRoom.Name;

//         // 방에 입장 후 플레이어 생성
//         Debug.Log("이름: " + GameObject.name);
//         // 디버깅 로그 추가
//         if (GameObject != null)
//         {
//             Debug.Log("PlayerPrefab name: " + GameObject.name);
//         }
//         else
//         {
//             Debug.LogError("PlayerPrefab is not assigned!");
//         }

//         Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
//         PhotonNetwork.Instantiate(GameObject.name, spawnPosition, Quaternion.identity);
//     }

//     public override void OnJoinRandomFailed(short returnCode, string message)
//     {
//         Debug.LogWarning("Join Random Room Failed: " + message);
//         StatusText.text = "Join Random Failed: " + message;

//         // 랜덤한 방에 입장 실패 시 방 생성
//         CreateRoom();
//     }

//     void CreateRoom()
//     {
//         string roomName = "Room" + Random.Range(1000, 9999);
//         StatusText.text = "Creating Room: " + roomName;
//         PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2 });
//     }

//     public override void OnCreatedRoom()
//     {
//         Debug.Log("Room Created: " + PhotonNetwork.CurrentRoom.Name);
//         StatusText.text = "Room Created: " + PhotonNetwork.CurrentRoom.Name;
//     }

//     public override void OnCreateRoomFailed(short returnCode, string message)
//     {
//         Debug.LogWarning("Create Room Failed: " + message);
//         StatusText.text = "Create Room Failed: " + message;
//     }

//     void Update()
//     {
//         // 네트워크 연결 상태를 UI에 표시
//         if (StatusText != null)
//         {
//             StatusText.text = PhotonNetwork.NetworkClientState.ToString();
//         }
//     }
// }


using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class TestNetwork : MonoBehaviourPunCallbacks
{
    public Text StatusText;
    public InputField NickNameInput;
    public GameObject PlayerPrefab;
    public string gameVersion = "1.0";

    private GameObject playerInstance; // 현재 플레이어 인스턴스
    private bool isGameStarted = false; // 게임이 시작되었는지 여부를 나타내는 플래그

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        ConnectToPhoton();

         // 게임 시작 전에는 Move와 Astronaut 스크립트를 비활성화
        Move moveScript = PlayerPrefab.GetComponent<Move>();
        if (isGameStarted == false)
            moveScript.enabled = false;

        Astronaut astronautScript = PlayerPrefab.GetComponent<Astronaut>();
        if (isGameStarted == false)
            astronautScript.enabled = false;
    }

    void ConnectToPhoton()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        StatusText.text = "마스터 서버에 연결 중...";
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon 마스터 서버에 연결됨");

        SetNickname();
        JoinOrCreateRoom();
    }

    void SetNickname()
    {
        PhotonNetwork.NickName = "플레이어" + Random.Range(1000, 9999);
    }

    void JoinOrCreateRoom()
    {
        StatusText.text = "랜덤 방에 입장 시도 중...";
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방에 입장함: " + PhotonNetwork.CurrentRoom.Name);
        StatusText.text = "방에 입장함: " + PhotonNetwork.CurrentRoom.Name;

      if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
         {
            // 첫 번째 플레이어는 왼쪽 초기 위치에서 생성
            Vector3 spawnPosition = new Vector3(-7.6f, -16.22f, 0);
            GameObject playerInstance = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPosition, Quaternion.identity);
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            // 두 번째 플레이어는 오른쪽 초기 위치에서 생성
            Vector3 spawnPosition = new Vector3(7.6f, -16.22f, 0);
            GameObject playerInstance = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPosition, Quaternion.identity);
            isGameStarted = true;
            StartGame();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning("랜덤 방 입장 실패: " + message);
        StatusText.text = "랜덤 입장 실패: " + message;

        CreateRoom();
    }

    void CreateRoom()
    {
        string roomName = "방" + Random.Range(1000, 9999);
        StatusText.text = "방 생성 중: " + roomName;

        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성됨: " + PhotonNetwork.CurrentRoom.Name);
        StatusText.text = "방 생성됨: " + PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("방 생성 실패: " + message);
        StatusText.text = "방 생성 실패: " + message;
    }

    void StartGame()
    {
        Debug.Log("게임 시작");

        // 게임이 시작되면 캐릭터를 생성하고 제어할 수 있도록 설정
        Vector3 spawnPosition = new Vector3(7.6f, -16.22f, 0);
        playerInstance = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPosition, Quaternion.identity);

        isGameStarted = true;
    }

    void Update()
    {
        if (isGameStarted && playerInstance != null)
        {
            // 게임이 시작되었고, 플레이어 인스턴스가 있으면 플레이어의 움직임을 처리
            HandlePlayerMovement();
        }
    }

    void HandlePlayerMovement()
    {
        // 예시로 플레이어의 움직임을 제어하는 코드를 작성
        // 여기에 플레이어의 움직임 제어 로직을 추가합니다.
        // 예: 키 입력을 받아 움직임 처리 등
    }
}