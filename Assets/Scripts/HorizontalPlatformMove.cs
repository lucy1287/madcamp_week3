using UnityEngine;

public class HorizontalPlatformMove : MonoBehaviour
{
    public float speed = 2.0f; // 속도
    public float distance = 1.0f; // 이동할 최대 거리
    public float delay = 1.0f; // 방향 전환 시 대기 시간

    private Vector3 originalPosition; // 초기 위치
    private bool goingRight = true; // 플랫폼이 오른쪽으로 가는지 왼쪽으로 가는지 확인
    private float delayTimer = 0.0f; // 대기 시간 타이머

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
            return;
        }

        float newX;

        if (goingRight)
        {
            newX = transform.position.x + speed * Time.deltaTime;
            if (newX >= originalPosition.x + distance)
            {
                newX = originalPosition.x + distance;
                goingRight = false;
                delayTimer = delay;
            }
        }
        else
        {
            newX = transform.position.x - speed * Time.deltaTime;
            if (newX <= originalPosition.x - distance)
            {
                newX = originalPosition.x - distance;
                goingRight = true;
                delayTimer = delay;
            }
        }

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}