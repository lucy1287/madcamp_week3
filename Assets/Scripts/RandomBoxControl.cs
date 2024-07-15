using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomBoxControl : MonoBehaviour
{
    private GameObject player;
    public int bullet_num = 1;
    public int jewel_num = 1;
    public TMP_Text bulletNumText;
    public TMP_Text jewelNumText;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
            Destroy(gameObject);
            ItemGain();
        }   
    }

    protected virtual void ItemGain() 
    { 
        Astronaut astronaut = player.GetComponent<Astronaut>();
        if(astronaut != null)
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
}
