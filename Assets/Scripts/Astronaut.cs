using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astronaut : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public GameObject groundCheck; // GameObject로 변경
    public float groundCheckRadius = 0.2f;

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

        // groundCheck GameObject를 캐릭터의 발 아래에 위치한 빈 GameObject로 설정
         groundCheck = transform.Find("GroundCheck").gameObject;
        if (groundCheck == null)
        {
            Debug.LogError("Ground check GameObject not assigned in the inspector. Please assign it.");
            return;
        }

         // BoxCollider2D 추가
        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(1.0f, 1.0f); // 적절한 크기 설정
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("트리거 호출됨");
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