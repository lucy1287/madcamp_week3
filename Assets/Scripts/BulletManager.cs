
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BulletManager : MonoBehaviour
{   
    public float speed = 10f;
    private float screenRightEdge;
    private PhotonView photonView;
    private Collider2D bulletCollider;
    private bool isInitialized = false;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        bulletCollider = GetComponent<Collider2D>();
        if (bulletCollider != null)
        {
            bulletCollider.enabled = false; // 초기화 동안 충돌 비활성화
        }

        Initialize();
    }

    void Initialize()
    {
        screenRightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        Debug.Log("Bullet Start - PhotonView ID: " + photonView.ViewID);

        if (bulletCollider != null)
        {
            bulletCollider.enabled = true; // 초기화 완료 후 충돌 활성화
        }

        isInitialized = true;
        Debug.Log("Bullet initialized - Owner: " + photonView.OwnerActorNr);
    }

    // Update is called once per frame
    void Update()
    {
        // 총알을 오른쪽으로 이동시킵니다.
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // 총알이 화면의 오른쪽 끝에 도달하면 삭제합니다.
        if (transform.position.x > screenRightEdge)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PhotonView playerPhotonView = collision.GetComponent<PhotonView>();
            Debug.Log("playerPhotonView.OwnerActorNr: "+ playerPhotonView.OwnerActorNr);
            Debug.Log("photonView.OwnerActorNr: "+ photonView.OwnerActorNr);
            if (playerPhotonView != null && playerPhotonView.OwnerActorNr != photonView.OwnerActorNr)
            {
                Debug.Log("shooted player: " + playerPhotonView.OwnerActorNr);
                Debug.Log("shooting player: " + photonView.OwnerActorNr);

                Astronaut shootedPlayer = collision.GetComponent<Astronaut>();
                playerPhotonView.RPC("ResetPosition", RpcTarget.All, playerPhotonView.ViewID);

                // 소유권을 가지지 않은 클라이언트에서 총알을 삭제하려면 RPC 호출
                if (!photonView.IsMine)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.Destroy(gameObject);
                        
                    }
                    else
                    {
                        photonView.RPC("RequestBulletDestroy", RpcTarget.MasterClient, photonView.ViewID);
                    }
                }
                else
                {
                    PhotonNetwork.Destroy(gameObject);
                    Debug.Log("Destroy Bullet successfully.");
                }
            }
        }
    }

    [PunRPC]
    void RequestBulletDestroy(int viewID)
    {
        PhotonView bulletPhotonView = PhotonView.Find(viewID);
        if (bulletPhotonView != null && bulletPhotonView.IsMine)
        {
            PhotonNetwork.Destroy(bulletPhotonView.gameObject);
            Debug.Log("Destroy Bullet successfully.");
        }
    }

}
