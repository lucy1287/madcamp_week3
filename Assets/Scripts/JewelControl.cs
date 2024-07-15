using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JewelControl : MonoBehaviour
{
    private GameObject player;
    public int jewel_num = 1;
    public TMP_Text jewelNumText;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // 보유한 총알 개수 표시
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
            Destroy(gameObject);
            ItemGain();
        }   
    }

    protected virtual void ItemGain() 
    { 
        Astronaut astronaut = player.GetComponent<Astronaut>();
        if(astronaut != null)
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
}
