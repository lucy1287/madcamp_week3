using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public Transform[] exitPortals;

    public void TeleportPlayer(GameObject player, Portal currentPortal)
    {
        if (exitPortals.Length > 0)
        {
            Transform randomPortal;
            do
            {
                randomPortal = exitPortals[Random.Range(0, exitPortals.Length)];
            } while (randomPortal == currentPortal.transform);

            player.transform.position = randomPortal.position;
            Debug.Log("플레이어를 다른 포털로 이동시킵니다: " + randomPortal.name); // 디버그 메시지 추가
        }
        else
        {
            Debug.LogWarning("출구 포털이 설정되지 않았습니다.");
        }
    }
}
