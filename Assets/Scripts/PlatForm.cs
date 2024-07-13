using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2.0f; // 속도
    public float height = 1.0f; // 이동할 최대 높이
    public float delay = 1.0f; // 방향 전환 시 대기 시간

    private Vector3 originalPosition; // 초기 위치
    private bool goingUp = true; // 플랫폼이 올라가는지 내려가는지 확인
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

        float newY;

        if (goingUp)
        {
            newY = transform.position.y + speed * Time.deltaTime;
            if (newY >= originalPosition.y + height)
            {
                newY = originalPosition.y + height;
                goingUp = false;
                delayTimer = delay;
            }
        }
        else
        {
            newY = transform.position.y - speed * Time.deltaTime;
            if (newY <= originalPosition.y)
            {
                newY = originalPosition.y;
                goingUp = true;
                delayTimer = delay;
            }
        }

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}