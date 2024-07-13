using UnityEngine;

public class Portal : MonoBehaviour
{
    public PortalManager portalManager;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("포털과 충돌 발생!"); // 디버그 메시지 추가
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 충돌함"); // 디버그 메시지 추가
            portalManager.TeleportPlayer(other.gameObject, this);
        }
    }
}
