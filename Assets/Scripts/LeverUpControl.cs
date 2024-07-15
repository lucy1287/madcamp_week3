using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverUpControl : MonoBehaviour
{
    public Sprite leverDownSprite;
    public Sprite leverUpSprite;
    public ManualPlatform manualPlatform;

    private SpriteRenderer spriteRenderer;
    private bool isLeverUp = true; // 현재 상태를 나타내는 변수
    private bool isPlayerNearby = false; // 플레이어가 레버 근처에 있는지 나타내는 변수

    void Start()
    {
        // SpriteRenderer 컴포넌트를 가져옵니다.
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component missing from this game object. Please add one.");
        }

        // 초기 스프라이트를 levelDownSprite로 설정합니다.
        spriteRenderer.sprite = leverUpSprite;
    }

    // 플레이어가 레버에 닿았을 때 상태 변경
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 충돌한 오브젝트의 태그가 "Player"인지 확인합니다.
        {
            // 플레이어가 처음 레버에 닿았을 때만 상태를 변경합니다.
            if (!isPlayerNearby)
            {   
                manualPlatform.ToggleMovement(); // 수동플랫폼 움직이기

                isPlayerNearby = true;

                // 현재 상태를 반대로 변경합니다.
                isLeverUp = !isLeverUp;

                // 상태에 따라 스프라이트를 변경합니다.
                if (isLeverUp)
                {
                    spriteRenderer.sprite = leverUpSprite;
                }
                else
                {
                    spriteRenderer.sprite = leverDownSprite;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어가 레버에서 멀어졌을 때 isPlayerNearby를 false로 설정합니다.
            isPlayerNearby = false;
        }
    }
}
