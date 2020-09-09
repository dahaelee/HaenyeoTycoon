using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sea_coin : MonoBehaviour
{
    public GameObject gold_string, silver_string, bronze_string;
    public AudioSource coin;

    public void Start()
    {
        gold_string.gameObject.SetActive(false);
        silver_string.gameObject.SetActive(false);
        bronze_string.gameObject.SetActive(false);

        coin.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
    }

    public void OnTriggerEnter2D(Collider2D col) // 해녀와 충돌하면
    {
        if (col.gameObject.CompareTag("haenyeo"))
        {
            coin.PlayOneShot(coin.clip);

            if (this.gameObject.GetComponent<SpriteRenderer>().sprite.name == "coin_gold")
            {
                StartCoroutine("gold_effect");
                Haenyeo.money += 20000; //해녀 돈 추가

                //효민 - daily quest 관련
                if (quest_Data.daily_quest_list[3].state != -1 && quest_Data.daily_quest_list[3].state != 2)
                {
                    PlayerPrefs.SetInt("quest_gold", PlayerPrefs.GetInt("quest_gold") + 1);
                }
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
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); // 동전 투명
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false; // 충돌 불가능
        float string_time = 0.2f;
        float time = 0f;
        Vector3 string_pos = gold_string.transform.localPosition;
        string_pos.y = 0f;
        gold_string.gameObject.SetActive(true); //문구 활성화

        while (string_pos.y < 0.5)
        {
            time += Time.deltaTime / string_time;
            string_pos.y = Mathf.Lerp(-0f, 0.5f, time);
            gold_string.transform.localPosition = new Vector3(0, string_pos.y, 0);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        gold_string.gameObject.SetActive(false); // 문구 비활성화
        Destroy(this.gameObject); // 코인 클론 제거
    }

    public IEnumerator silver_effect()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); // 동전 투명
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        float string_time = 0.2f;
        float time = 0f;
        Vector3 string_pos = silver_string.transform.localPosition;
        string_pos.y = 0f;
        silver_string.gameObject.SetActive(true); //문구 활성화

        while (string_pos.y < 0.5)
        {
            time += Time.deltaTime / string_time;
            string_pos.y = Mathf.Lerp(-0f, 0.5f, time);
            silver_string.transform.localPosition = new Vector3(0, string_pos.y, 0);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        silver_string.gameObject.SetActive(false); //문구 비활성화
        Destroy(this.gameObject); // 코인 클론 제거 
    }

    public IEnumerator bronze_effect()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); // 동전 투명
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        float string_time = 0.2f;
        float time = 0f;
        Vector3 string_pos = bronze_string.transform.localPosition;
        string_pos.y = 0f;
        bronze_string.gameObject.SetActive(true); //문구 활성화

        while (string_pos.y < 0.5)
        {
            time += Time.deltaTime / string_time;
            string_pos.y = Mathf.Lerp(-0f, 0.5f, time);
            bronze_string.transform.localPosition = new Vector3(0, string_pos.y, 0);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        bronze_string.gameObject.SetActive(false); //문구 비활성화
        Destroy(this.gameObject); // 코인 클론 제거
    }
}
