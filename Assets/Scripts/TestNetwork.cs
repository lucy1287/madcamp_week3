using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class TestNetwork : MonoBehaviourPunCallbacks
{
    public Text StatusText;
    public InputField NickNameInput;
    public GameObject GameObject;
    public string gameVersion = "1.0";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        ConnectToPhoton();
    }

    void ConnectToPhoton()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");

        // 닉네임 설정
        SetNickname();

        // 랜덤한 방에 입장 시도
        PhotonNetwork.JoinRandomRoom();
    }

    void SetNickname()
    {
       // if (!string.IsNullOrEmpty(NickNameInput.text))
       // {
       //     PhotonNetwork.NickName = NickNameInput.text;
      //  }
      //  else
      //  {
            PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);
       // }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        StatusText.text = "Joined Room: " + PhotonNetwork.CurrentRoom.Name;

        // 방에 입장 후 플레이어 생성
        Debug.Log("이름: " + GameObject.name);
        // 디버깅 로그 추가
        if (GameObject != null)
        {
            Debug.Log("PlayerPrefab name: " + GameObject.name);
        }
        else
        {
            Debug.LogError("PlayerPrefab is not assigned!");
        }

        Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
        PhotonNetwork.Instantiate(GameObject.name, spawnPosition, Quaternion.identity);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Join Random Room Failed: " + message);
        StatusText.text = "Join Random Failed: " + message;

        // 랜덤한 방에 입장 실패 시 방 생성
        CreateRoom();
    }

    void CreateRoom()
    {
        string roomName = "Room" + Random.Range(1000, 9999);
        StatusText.text = "Creating Room: " + roomName;
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created: " + PhotonNetwork.CurrentRoom.Name);
        StatusText.text = "Room Created: " + PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Create Room Failed: " + message);
        StatusText.text = "Create Room Failed: " + message;
    }

    void Update()
    {
        // 네트워크 연결 상태를 UI에 표시
        if (StatusText != null)
        {
            StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        }
    }
}