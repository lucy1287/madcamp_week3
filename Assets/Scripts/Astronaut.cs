using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Astronaut : MonoBehaviour
{
    public PortalManager portalManager;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public GameObject groundCheck; // GameObject로 변경
    public float groundCheckRadius = 0.2f;
    public int bullet = 0;
    public int jewel = 0;
    public GameObject bulletPrefab; // 총알 프리팹 연결
    public Transform bulletSpawnPoint; // 총알이 생성될 위치
    public TMP_Text bulletNumText;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isClimbing;
    private float verticalMove;
    private Vector3 initialPosition;
    private PhotonView photonView;
    // public GameObject popup; 
    private BoxCollider2D boxCollider; 

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Debug.Log("Astronaut Start method called");

        // 스크립트 시작 시 `TMP_Text`를 동적으로 할당
        if (bulletNumText == null)
        {
            bulletNumText = GameObject.Find("BulletNumText").GetComponent<TMP_Text>();
            if (bulletNumText == null)
            {
                Debug.LogError("BulletNumText not found in the scene. Please ensure the object exists and has a TMP_Text component.");
            }
        }
        
        Debug.Log("확인 호출됨");

        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing from this game object. Please add one.");
        }

        if (photonView == null)
        {
            Debug.LogError("PhotonView component missing from this game object. Please add one.");
            return;
        }

        Debug.Log("PhotonView: " + photonView);

        // Z축 회전 고정
        rb.freezeRotation = true;
        // Z축 회전 고정
        photonView.RPC("SetFreezeRotation", RpcTarget.AllBuffered, true);
        photonView.RPC("SyncBoxCollider", RpcTarget.AllBuffered, boxCollider.size, boxCollider.offset);

        // groundCheck GameObject를 캐릭터의 발 아래에 위치한 빈 GameObject로 설정
        groundCheck = transform.Find("GroundCheck").gameObject;
        if (groundCheck == null)
        {
            Debug.LogError("Ground check GameObject not assigned in the inspector. Please assign it.");
            return;
        }

         // 초기 위치 저장
        initialPosition = transform.position;
        Debug.Log("처음 위치 : " + initialPosition);
        photonView = GetComponent<PhotonView>();

        // BoxCollider2D 추가
        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(4.0f, 4.0f); // 적절한 크기 설정

        if (portalManager == null)
        {
            portalManager = FindObjectOfType<PortalManager>();
        }

        // popup.SetActive(false); 
    }

    [PunRPC]
    void SetFreezeRotation(bool freeze)
    {
        rb.freezeRotation = freeze;
    }

    [PunRPC]
    void SyncBoxCollider(Vector2 size, Vector2 offset)
    {
        if (boxCollider != null)
        {
            boxCollider.size = size;
            boxCollider.offset = offset;
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        // Rigidbody2D가 없으면 업데이트 중단
        if (rb == null) return;

        // 방향키 입력 받기
        float horizontalMove = Input.GetAxis("Horizontal");

        // 이동
        rb.velocity = new Vector2(horizontalMove * moveSpeed, rb.velocity.y);

        // 캐릭터의 발 위치에 맞춰 groundCheck 위치 업데이트
        groundCheck.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); // 예시: 캐릭터의 발 아래로 조정

        // 점프
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, groundCheckRadius, groundLayer);
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // 사다리 오르기
        if (isClimbing)
        {
            verticalMove = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, verticalMove * moveSpeed);
        }

        // 총 쏘기
        if (Input.GetKeyDown(KeyCode.W) && bullet > 0)
        {
            Debug.Log("shoot");
            Shoot();
        }
    }

    void Shoot()
    {
        // 총알 생성
        Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet--;
        bulletNumText.text = "Bullet: " + bullet.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("트리거 호출됨");
        if (collision.gameObject.CompareTag("Platform"))
        {
            Debug.Log("플랫폼 호출됨");
            transform.SetParent(collision.transform);
        }
        if (collision.gameObject.CompareTag("ManualPlatform"))
        {
            Debug.Log("수동플랫폼 호출됨");
            transform.SetParent(collision.transform);
        }
        if (collision.gameObject.CompareTag("Alien"))
        {
            Debug.Log("외계인 호출됨");
            transform.position = initialPosition;
        }

        if (collision.gameObject.CompareTag("Spike"))
        {
            Debug.Log("움직가시 호출됨");
            transform.position = initialPosition;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(null);
        }
        if (collision.gameObject.CompareTag("ManualPlatform"))
        {
            transform.SetParent(null);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            Debug.Log("사다리 호출됨");
            isClimbing = true;
            rb.gravityScale = 0f;
        }

        if (collision.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.CompareTag("Portal"))
        {
            Debug.Log("포털 호출됨");
            portalManager.TeleportPlayer(gameObject, collision.GetComponent<Portal>());
            // popup.SetActive(true);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = 1f;
        }

        if (collision.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}