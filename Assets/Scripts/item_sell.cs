using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class item_sell : MonoBehaviour
{
    public sell_item_info[] sea_items;
    //public sell_item_info starfish, seaweed, shell, shrimp, jellyfish, crab, octopus, abalone, turtle;
    public Text Haenyeo_money;

    // 사운드 이펙트
    public AudioSource bgm, sell_click, updown_click;

    void Awake()
    {
        bgm.volume = PlayerPrefs.GetFloat("Bgm_volume", 1);
        sell_click.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        updown_click.volume = PlayerPrefs.GetFloat("Effect_volume", 1);

        data_load();
        // 임의로 설정한 자원 개수
        //Haenyeo.sea_item_number[0] = 10;
        //Haenyeo.sea_item_number[3] = 5;
        item_UI();
        
        
    }

    public void return_to_farm()
    {
        data_save();
        SceneManager.LoadScene("farm");
    }

    // 해녀가 가진 자원만 보이도록 설정
    public void item_UI()
    {
        //item_noshow();
        Haenyeo_money.text = Haenyeo.money.ToString("N0");
        for (int i=0; i< sea_items.Length; i++)
        {
            if(Haenyeo.sea_item_number[i] >= 0)
            {
                sea_items[i].gameObject.SetActive(true);
                //sea_items[i].number_up_button.gameObject.SetActive(false);
                //sea_items[i].number_down_button.gameObject.SetActive(false);

                sea_items[i].sell_number = 0; // 팔 자원개수 0으로 초기화
                sea_items[i].sell_price = sea_items[i].sell_number * sea_items[i].raw_price;
                sea_items[i].total_number = Haenyeo.sea_item_number[i];

                sea_items[i].sell_number_text.text = sea_items[i].sell_number.ToString();
                sea_items[i].total_number_text.text = sea_items[i].total_number.ToString();
                sea_items[i].sell_price_text.text = sea_items[i].sell_price.ToString();

                up(i);
                down(i);
            }
        }
    }

    //모든 자원창을 끔
    public void item_noshow()
    {
        for(int i = 0; i < sea_items.Length; i++)
        {
            sea_items[i].gameObject.SetActive(false);
        }
    }

    // 팔 자원 개수 늘이는 버튼
    public void number_up(int item_index)
    {
        updown_click.PlayOneShot(updown_click.clip);
        sea_items[item_index].sell_number++;
        sea_items[item_index].sell_price += sea_items[item_index].raw_price;
        sea_items[item_index].sell_number_text.text = sea_items[item_index].sell_number.ToString();
        sea_items[item_index].sell_price_text.text = (sea_items[item_index].raw_price * sea_items[item_index].sell_number).ToString("N0");
        up(item_index);
        down(item_index);

    }

    // 팔 자원 개수 줄이는 버튼
    public void number_down(int item_index)
    {
        updown_click.PlayOneShot(updown_click.clip);
        sea_items[item_index].sell_number--;
        sea_items[item_index].sell_price -= sea_items[item_index].raw_price;
        sea_items[item_index].sell_number_text.text = sea_items[item_index].sell_number.ToString();
        sea_items[item_index].sell_price_text.text = (sea_items[item_index].raw_price * sea_items[item_index].sell_number).ToString("N0");
        up(item_index);
        down(item_index);
    }

    // 팔기 버튼
    public void sell_button_click(int item_index)
    {
        sell_click.PlayOneShot(sell_click.clip);
        sea_items[item_index].sell_price_text.text = (sea_items[item_index].raw_price * sea_items[item_index].sell_number).ToString("N0");
        Haenyeo.money += sea_items[item_index].raw_price * sea_items[item_index].sell_number; // 해녀돈 += 자원 팔 개수 * raw_price;
        Haenyeo.sea_item_number[item_index] -= sea_items[item_index].sell_number; // 해녀가 가진 자원 개수 -= 자원 팔 개수
        item_UI();
    }

    // 업버튼 활성화/비활성화 
    public void up(int item_index)
    {
        sea_items[item_index].number_up_button.SetActive(true);

        if (sea_items[item_index].sell_number < sea_items[item_index].total_number) // 총 자원보다 적으면 활성화
        {
            sea_items[item_index].number_up_button.GetComponent<Button>().interactable = true;
            //sea_items[item_index].number_up_button.SetActive(true);
        }
        else if (sea_items[item_index].sell_number >= sea_items[item_index].total_number) // 총 자원보다 많거나 같아지면 비활성화 (안되는 부분)
        {
            sea_items[item_index].number_up_button.GetComponent<Button>().interactable = false;
            //sea_items[item_index].number_up_button.SetActive(false);
        }
    }

    // 다운버튼 활성화/비활성화 
    public void down(int item_index)
    {
       
        sea_items[item_index].number_down_button.SetActive(true);

        if (sea_items[item_index].sell_number > 0) // 팔 자원 개수가 0보다 많아지면 활성화
        {
            sea_items[item_index].number_down_button.GetComponent<Button>().interactable = true;
            //sea_items[item_index].number_down_button.SetActive(true);
        }
        else if (sea_items[item_index].sell_number <= 0) // 팔 자원 개수가 0과 같거나 작아지면 비활성화 (안되는 부분)
        {
            sea_items[item_index].number_down_button.GetComponent<Button>().interactable = false;
            //sea_items[item_index].number_down_button.SetActive(false);
        }
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
        
        PlayerPrefs.Save();
    }
    public void data_load()
    {

        Haenyeo.money = PlayerPrefs.GetInt("Haenyeo_money", 500000);



        //해녀 보유한 자원 개수 초기화

        Haenyeo.sea_item_number[0] = PlayerPrefs.GetInt("Haenyeo_sea_item_number0", 10);
        Haenyeo.sea_item_number[1] = PlayerPrefs.GetInt("Haenyeo_sea_item_number1", 10);
        Haenyeo.sea_item_number[2] = PlayerPrefs.GetInt("Haenyeo_sea_item_number2", 10);
        Haenyeo.sea_item_number[3] = PlayerPrefs.GetInt("Haenyeo_sea_item_number3", 10);
        Haenyeo.sea_item_number[4] = PlayerPrefs.GetInt("Haenyeo_sea_item_number4", 10);
        Haenyeo.sea_item_number[5] = PlayerPrefs.GetInt("Haenyeo_sea_item_number5", 10);
        Haenyeo.sea_item_number[6] = PlayerPrefs.GetInt("Haenyeo_sea_item_number6", 10);
        Haenyeo.sea_item_number[7] = PlayerPrefs.GetInt("Haenyeo_sea_item_number7", 10);
        Haenyeo.sea_item_number[8] = PlayerPrefs.GetInt("Haenyeo_sea_item_number8", 10);

    }
}
