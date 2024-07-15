using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float moveDistance = 5.0f;   // 움직일 거리
    public float moveSpeed = 5.0f;      // 움직임 속도
    public float pauseTime = 1.0f;      // 방향 전환 전 대기 시간

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 nextPos;
    private float timer;
    private bool movingUp = true;

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + Vector3.up * moveDistance;
        nextPos = endPos;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > pauseTime)
        {
            timer = 0;

            if (movingUp)
                nextPos = startPos;
            else
                nextPos = endPos;

            movingUp = !movingUp;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);
    }
}