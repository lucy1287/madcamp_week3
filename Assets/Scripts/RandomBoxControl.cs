using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RandomBoxControl : MonoBehaviour
{
    public int bullet_num = 1;
    public int jewel_num = 1;
    public TMP_Text bulletNumText;
    public TMP_Text jewelNumText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("RandomBox Trigger 호출됨");
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
                System.Random rand = new System.Random();
                int randomValue = rand.Next(0, 2); // 0 또는 1 랜덤 값 생성
                if(randomValue == 0)
                {
                    astronaut.bullet += bullet_num;
                    bulletNumText.text = "Bullet: " + astronaut.bullet.ToString();
                    Debug.Log("Bullet acquired! Total bullets: " + astronaut.bullet);
                }
                else
                {
                    astronaut.jewel += jewel_num;
                    jewelNumText.text = "Jewel: " + astronaut.jewel.ToString();
                    Debug.Log("Jewel acquired! Total jewels: " + astronaut.jewel);
                }
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
