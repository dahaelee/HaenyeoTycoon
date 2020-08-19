using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sea_coin : MonoBehaviour
{
    public GameObject gold_string, silver_string, bronze_string;

    public void Start()
    {
        gold_string.gameObject.SetActive(false);
        silver_string.gameObject.SetActive(false);
        bronze_string.gameObject.SetActive(false);
    }

    public void OnTriggerStay2D(Collider2D col) // 해녀와 충돌하면
    {
        if (col.gameObject.CompareTag("haenyeo"))
        {
            if (this.gameObject.GetComponent<SpriteRenderer>().sprite.name == "coin_gold")
            {
                StartCoroutine("gold_effect");
                Haenyeo.money += 20000; //해녀 돈 추가
            }

            if (this.gameObject.GetComponent<SpriteRenderer>().sprite.name == "coin_silver")
            {
                StartCoroutine("silver_effect");
                Haenyeo.money += 15000; //해녀 돈 추가
            }

            if (this.gameObject.GetComponent<SpriteRenderer>().sprite.name == "coin_bronze")
            {
                StartCoroutine("bronze_effect");
                Haenyeo.money += 10000; //해녀 돈 추가
            }
        }
    }

    public IEnumerator gold_effect()
    {
        Destroy(this.gameObject); // 코인 클론 제거 
        gold_string.gameObject.SetActive(true); //문구 활성화
        yield return new WaitForSeconds(0.5f);
        gold_string.gameObject.SetActive(false); //문구 비활성화
    }

    public IEnumerator silver_effect()
    {
        Destroy(this.gameObject); // 코인 클론 제거
        silver_string.gameObject.SetActive(true); //문구 활성화
        yield return new WaitForSeconds(0.5f);
        silver_string.gameObject.SetActive(false); //문구 비활성화
    }

    public IEnumerator bronze_effect()
    {
        Destroy(this.gameObject); // 코인 클론 제거
        bronze_string.gameObject.SetActive(true); //문구 활성화
        yield return new WaitForSeconds(0.5f);
        bronze_string.gameObject.SetActive(false); //문구 비활성화
    }
}
