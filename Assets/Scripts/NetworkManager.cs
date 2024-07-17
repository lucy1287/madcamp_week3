/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text stateText;
    [SerializeField] TMP_InputField playerNickNameInput;


    // Start is called before the first frame update
    void Start()
    {
        Connected();
        playerNickNameInput.onEndEdit.AddListener(OnInputFieldEndEdit); // 엔터 키 입력 이벤트 추가
    }

    // Update is called once per frame
    void Update()
    {
        stateText.text = "ServerState : " + PhotonNetwork.NetworkClientState.ToString();
    }


    //ServerConnect
    public void Connected() => PhotonNetwork.ConnectUsingSettings();
    public override void OnConnected() => Debug.Log("서버 연결 완료");
    public override void OnDisconnected(DisconnectCause cause) => Debug.Log("연결 종료");

    // InputField에서 엔터 키 입력을 감지
    private void OnInputFieldEndEdit(string input)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CreateRoom();
        }
    }


    //RoomCreate
    public void CreateRoom() => CheckCreateRoom();
    public override void OnCreatedRoom() => AfterCreateRoom();
    public override void OnCreateRoomFailed(short returnCode, string message) => Debug.Log("방 생성 실패");
    void AfterCreateRoom(){
        Debug.Log("방 생성완료");
        SceneManager.LoadScene("Scene2");
    }
    void CheckCreateRoom(){
        if(playerNickNameInput.text == ""){
            Debug.Log("닉네임을 입력해야합니다.");
        }
        else{
            Debug.Log("닉네임이 있음.");
            PhotonNetwork.LocalPlayer.NickName = playerNickNameInput.text;
            PhotonNetwork.CreateRoom("100",new RoomOptions { MaxPlayers = 2 });
        }
    }


    //JoinRoom
    public void JoinRoom() => CheckJoinRoom();
    public override void OnJoinedRoom() => AfterJoinRoom();
    public override void OnJoinRoomFailed(short returnCode, string message) => print("방 참가 실패");
    void AfterJoinRoom(){
        Debug.Log("방 참가완료" + PhotonNetwork.CurrentRoom.Name);
        SceneManager.LoadScene("Scene2");
    }
    void CheckJoinRoom(){
        if(playerNickNameInput.text == "" && roomCodeInput.text == ""){
            Debug.Log("InputField를 입력해야합니다.");
        }
        else{
            Debug.Log("닉네임이 있음.");
            PhotonNetwork.LocalPlayer.NickName = playerNickNameInput.text;
            PhotonNetwork.JoinRoom(roomCodeInput.text);
        }
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private static NetworkManager instance;
    public GameObject playerPrefab;
    private bool isSceneLoaded = false;

    // Awake()는 게임이 시작되기 전에 실행됨. 즉, 가장 먼저 실행 된다.
    private void Awake()
    {
        // 싱글톤 패턴 적용
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // 마스터 클라이언트가 PhotonNetwork.LoadLevel()을 호출할 수 있도록 하고,
        // 같은 룸에 있는 모든 클라이언트가 레벨을 동기화하게 함
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }

    public void Connect()
    {
        // 연결 되었는지를 체크해서, 룸에 참여할지 재연결을 시도할지 결정
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Already connected to server, joining lobby...");
            PhotonNetwork.JoinLobby();
        }
        else
        {
            // 서버 연결에 실패하면 서버에 연결 시도
            Debug.Log("Connecting to server...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 클라이언트가 마스터에 연결되면 호출됨
    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master server");
        // 마스터에 연결되면 로비에 입장
        PhotonNetwork.JoinLobby();
    }

    // 클라이언트가 로비에 입장하면 호출됨
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby, joining random room...");
        // 로비에 입장하면 방에 랜덤으로 입장
        PhotonNetwork.JoinRandomRoom();
    }

    // 클라이언트가 어떤 방식으로든 연결이 끊어지면 호출됨
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("disconnected to server. 사유 : {0}", cause);
    }

    // 랜덤 방 입장에 실패할 경우 호출됨
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("fail to join random room. create new room");
        // 랜덤 방 입장에 실패하면 서버 연결이 끊기지 않았다면, 방이 가득 찼거나 방이 없거나이므로 방을 새로 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = 2 });
    }

    // 방에 입장하면 호출됨 
    public override void OnJoinedRoom()
    {
        Debug.Log("join room");
        PhotonNetwork.LoadLevel("New Scene");
    }

}