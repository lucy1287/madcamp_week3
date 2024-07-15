using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMove : MonoBehaviour
{
    // 이동 범위와 속도를 설정하는 public 변수
    public float moveRange = 10f;  // 왔다갔다하는 총 거리
    public float moveSpeed = 2f;   // 이동 속도

    // 초기 위치를 저장하는 private 변수
    private float initialX;

    // Start는 게임이 시작될 때 호출됩니다.
    void Start()
    {
        // 초기 x 위치를 저장합니다.
        initialX = transform.position.x;
    }

    // Update는 매 프레임마다 호출됩니다.
    void Update()
    {
        // 사인파를 사용하여 새로운 위치 계산
        float newX = initialX + Mathf.PingPong(Time.time * moveSpeed, moveRange) - (moveRange / 2);
        
        // 물체의 위치를 업데이트합니다.
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
