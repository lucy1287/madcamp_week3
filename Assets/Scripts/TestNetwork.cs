using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class TestNetwork : MonoBehaviourPunCallbacks
{
    public Text StatusText;
    public InputField NickNameInput;
    public GameObject Cube;
    public string gameVersion = "1.0";


    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player3", Cube.transform.position, Quaternion.identity);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        this.CreateRoom();
    }

    void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }
}
