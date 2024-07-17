using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public ButtonEvent buttonEvent;

    void Start()
    {
        Debug.Log("PlayerManager Start method called");
        
        if (playerPrefab == null)
        {
            Debug.LogError("playerPrefab이 설정되지 않았습니다. Inspector에서 설정해주세요.");
            return;
        }

        // 플레이어를 네트워크 상에서 생성
        if (PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer != null && PhotonNetwork.CurrentRoom != null)
        {
            Vector3 spawnPosition = GetSpawnPosition();
            Debug.Log("Spawning player at position: " + spawnPosition);
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);

            if (player != null)
            {
                Debug.Log("Player instantiated successfully");

                // ButtonEvent 스크립트에 Player 객체를 설정
                if (buttonEvent != null)
                {
                    buttonEvent.SetPlayer(player);
                }
                else
                {
                    Debug.LogError("ButtonEvent 스크립트가 설정되지 않았습니다. Inspector에서 설정해주세요.");
                }
            }
            else
            {
                Debug.LogError("Failed to instantiate player prefab");
            }        
        }
        else
        {
            Debug.LogError("PhotonNetwork is not connected or player is null or room is null");
        }
    }

    // 각 플레이어의 순서에 따라 스폰 위치를 반환하는 함수
    Vector3 GetSpawnPosition()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.LogError("PhotonNetwork.CurrentRoom is null");
            return Vector3.zero; // 기본 위치를 반환하여 오류 방지
        }
        
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log("Current player count: " + playerCount);

        if (playerCount == 1)
        {
            // 첫 번째 플레이어의 위치
            return new Vector3(-18.6f, -25.5f, 0f);
        }
        else if (playerCount == 2)
        {
            // 두 번째 플레이어의 위치
            return new Vector3(24.3f, -28.3f, 0f);
        }
        else
        {
            // 기본 위치 (혹은 추가 플레이어를 위한 위치)
            return new Vector3(0f, 0f, 0f);
        }
    }
}