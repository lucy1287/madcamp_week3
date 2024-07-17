using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class JewelControl : MonoBehaviour
{
    public int jewel_num = 1;
    private TMP_Text jewelNumText;

    // Start is called before the first frame update
    void Start()
    {
        // TMP_Text를 동적으로 찾기
        GameObject jewelNumTextObject = GameObject.FindGameObjectWithTag("JewelText");
        if (jewelNumTextObject != null)
        {
            jewelNumText = jewelNumTextObject.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogError("JewelNumText object not found.");
            return;
        }

        if (jewelNumText == null)
        {
            Debug.LogError("TMP_Text component not found on JewelNumText object.");
            return;
        }
        // 보유한 보석 개수 표시
        jewelNumText.text = "Jewel: " + 0.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Jewel Trigger 호출됨");
        if(collision.CompareTag("Player"))
        {
            // 충돌한 플레이어 객체를 가져옵니다.
            GameObject player = collision.gameObject;

            // 보석을 제거하고 아이템 획득 메서드를 호출합니다.
            Destroy(gameObject);
            ItemGain(player);
        }   
    }

    protected virtual void ItemGain(GameObject player) 
    { 
        if (player == null)
        {
            Debug.LogError("Player object not set.");
            return;
        }
        
        PhotonView playerPhotonView = player.GetComponent<PhotonView>();
        Astronaut astronaut = player.GetComponent<Astronaut>();
        if (playerPhotonView != null && playerPhotonView.IsMine)
        {
            if (astronaut != null)
            {
                astronaut.jewel += jewel_num;
                jewelNumText.text = "Jewel: " + astronaut.jewel.ToString();
                Debug.Log("Jewel acquired! Total jewels: " + astronaut.jewel);
            }
            else
            {
                Debug.LogError("Astronaut component not found on player object.");
            }
        }
        else
        {
            Debug.Log("This player object is not owned by the local player.");
        }
    }
}
