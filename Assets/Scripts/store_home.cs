using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class store_home : MonoBehaviour
{
    void Start()
    {
        int storeNew = PlayerPrefs.GetInt("storeNew", 1);
        if (storeNew == 1)
        {
            quest_Data.tutorial_quest_list[3].state = -1;
            PlayerPrefs.SetInt("storeNew", 0);
        }
    }

    public GameObject sell_tab, buy_tab,return_from_buy, return_from_sell;
    public int rand_text = -1;
    public string[] seller_text;
    public Text bubble_text;
    public Image bubble_image;
    public AudioSource bgm;
    public AudioSource seller_rand;
    public AudioClip[] seller_sound;

    public void Awake()
    {
        bgm.volume = PlayerPrefs.GetFloat("Bgm_volume", 1);
        //seller_temp.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        //seller_rand = GetComponent<AudioSource>();
        //seller_rand.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        //for (int i = 0; i < seller_sound.Length; i++)
        //{
        //    seller_sound[i].volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        //}

        bubble_image.gameObject.SetActive(false);
        bubble_text.gameObject.SetActive(false);
    }
    
    public void buy_panel_click()
    {
        return_from_buy.SetActive(true);
        buy_tab.SetActive(true);
    }
    public void sell_panel_click()
    {
        return_from_sell.SetActive(true);
        sell_tab.SetActive(true);
    }
    public void return_to_farm()
    {
        data_save();
        SceneManager.LoadScene("farm");
    }

    public void seller_talk()
    {
        seller_text = new string[]
        {
            "허허, 오늘도 수고하는구나~",
            "아저씨는 내기를 좋아한단다. 가위바위보 한 판 할테니?",
            "그래, 오늘은 무슨 자원을 팔러 왔니?",
            "장비를 업그레이드 하면 물질이 더 편해질거란다~",
            "자연산과 양식산은 가격이 다르다는거, 알고 있지?",
            "가위바위보에서 이기면 자연산 가격을 좀 더 짭짤하게 쳐주마."
        };
        rand_text = Random.Range(0, seller_text.Length);
        bubble_text.text = seller_text[rand_text];
        //seller_rand.clip = seller_sound[Random.Range(0, seller_sound.Length)];
        //seller_rand.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
    }

    public void seller_click()
    {
        seller_rand.PlayOneShot(seller_sound[Random.Range(0, seller_sound.Length)]);
        seller_rand.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        seller_talk();
        StartCoroutine(UI_On(bubble_image, bubble_text));

    }

    public IEnumerator UI_On(Image image, Text text)
    {
        yield return new WaitForSeconds(0.1f);
        image.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            image.rectTransform.localScale = new Vector3((float)(0.95 + i * 0.01), (float)(0.95 + i * 0.01), (float)(0.95 + i * 0.01));
            text.rectTransform.localScale = new Vector3((float)(0.95 + i * 0.01), (float)(0.95 + i * 0.01), (float)(0.95 + i * 0.01));
            yield return 0;
        }
        yield return new WaitForSeconds(2.5f);
        image.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public void data_save()
    {
        //데이터 저장

        PlayerPrefs.SetInt("Haenyeo" + "_" + "money", Haenyeo.money);

        PlayerPrefs.SetInt("Haenyeo_sea_item_number0", Haenyeo.sea_item_number[0]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number1", Haenyeo.sea_item_number[1]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number2", Haenyeo.sea_item_number[2]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number3", Haenyeo.sea_item_number[3]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number4", Haenyeo.sea_item_number[4]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number5", Haenyeo.sea_item_number[5]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number6", Haenyeo.sea_item_number[6]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number7", Haenyeo.sea_item_number[7]);
        PlayerPrefs.SetInt("Haenyeo_sea_item_number8", Haenyeo.sea_item_number[8]);

        PlayerPrefs.SetInt("Haenyeo_farm_item_number0", Haenyeo.farm_item_number[0]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number1", Haenyeo.farm_item_number[1]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number2", Haenyeo.farm_item_number[2]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number3", Haenyeo.farm_item_number[3]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number4", Haenyeo.farm_item_number[4]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number5", Haenyeo.farm_item_number[5]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number6", Haenyeo.farm_item_number[6]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number7", Haenyeo.farm_item_number[7]);
        PlayerPrefs.SetInt("Haenyeo_farm_item_number8", Haenyeo.farm_item_number[8]);

        PlayerPrefs.Save();
    }
}
