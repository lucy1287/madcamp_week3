using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    void Start()
    {
        Debug.Log("확인 호출됨");
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing from this game object. Please add one.");
            return;
        }

        // Z축 회전 고정
        rb.freezeRotation = true;

        // groundCheck GameObject를 캐릭터의 발 아래에 위치한 빈 GameObject로 설정
        groundCheck = transform.Find("GroundCheck").gameObject;
        if (groundCheck == null)
        {
            Debug.LogError("Ground check GameObject not assigned in the inspector. Please assign it.");
            return;
        }

        // BoxCollider2D 추가
        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(4.0f, 4.0f); // 적절한 크기 설정

        if (portalManager == null)
        {
            portalManager = FindObjectOfType<PortalManager>();
        }
    }

    void Update()
    {
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
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
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