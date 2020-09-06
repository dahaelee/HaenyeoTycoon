using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class farmed_item_sell : MonoBehaviour
{
    public sell_item_info[] farmed_items;
    public Text Haenyeo_money;

    //사운드 이펙트
    public AudioSource bgm, sell_click, updown_click;

    public GameObject sell_ui, wild_ui, farmed_ui, no_farmed, return_ui;

    public Image plus_money;
    public int temp_index, temp_num, temp_money;
    int if_no_farmed = 0;
    
    private void Awake()
    {
        bgm.volume = PlayerPrefs.GetFloat("Bgm_volume", 1);
        sell_click.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        updown_click.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        if_no_farmed = 0;
        data_load();
        item_noshow();
        item_UI();
    }

    public void return_to_home()
    {
        data_save();
        return_ui.SetActive(false);
        farmed_ui.gameObject.SetActive(false);
        wild_ui.gameObject.SetActive(false);
    }

    public void tab_change()
    {
        data_save();
        farmed_ui.gameObject.SetActive(false);
        wild_ui.gameObject.SetActive(true);
    }

    // 해녀가 가진 양식자원만 보이도록 설정
    public void item_UI()
    {
        temp_num = 0;
        temp_money = 0;
        temp_index = -1;
        item_noshow();
        Haenyeo_money.text = Haenyeo.money.ToString("N0");
        for (int i = 0; i < farmed_items.Length; i++)
        {
            if (Haenyeo.farm_item_number[i] > 0)
            {
                if_no_farmed++;
                farmed_items[i].gameObject.SetActive(true);

                farmed_items[i].sell_number = 0; // 팔 자원개수 0으로 초기화
                farmed_items[i].sell_price = farmed_items[i].sell_number * farmed_items[i].raw_price;
                farmed_items[i].total_number = Haenyeo.farm_item_number[i];

                farmed_items[i].sell_number_text.text = farmed_items[i].sell_number.ToString();
                farmed_items[i].total_number_text.text = farmed_items[i].total_number.ToString();
                farmed_items[i].sell_price_text.text = farmed_items[i].sell_price.ToString();

                up(i);
                down(i);
            }
        }
        if (if_no_farmed == 0)
        {
            no_farmed.gameObject.SetActive(true);
        }
    }

    //모든 자원창을 끔
    public void item_noshow()
    {
        for (int i = 0; i < farmed_items.Length; i++)
        {
            farmed_items[i].gameObject.SetActive(false);
        }
    }

    // 팔 자원 개수 늘이는 버튼
    public void number_up(int item_index)
    {
        updown_click.PlayOneShot(updown_click.clip);
        farmed_items[item_index].sell_number++;
        farmed_items[item_index].sell_price += farmed_items[item_index].raw_price;
        farmed_items[item_index].sell_number_text.text = farmed_items[item_index].sell_number.ToString();
        farmed_items[item_index].sell_price_text.text = (farmed_items[item_index].raw_price * farmed_items[item_index].sell_number).ToString("N0");
        up(item_index);
        down(item_index);
    }

    // 팔 자원 개수 줄이는 버튼
    public void number_down(int item_index)
    {
        updown_click.PlayOneShot(updown_click.clip);
        farmed_items[item_index].sell_number--;
        farmed_items[item_index].sell_price -= farmed_items[item_index].raw_price;
        farmed_items[item_index].sell_number_text.text = farmed_items[item_index].sell_number.ToString();
        farmed_items[item_index].sell_price_text.text = (farmed_items[item_index].raw_price * farmed_items[item_index].sell_number).ToString("N0");
        up(item_index);
        down(item_index);
    }

    // 팔기 버튼
    public void sell_button_click(int item_index)
    {
        //ask_ui.SetActive(true);
        sell_click.PlayOneShot(sell_click.clip);
        temp_index = item_index;
        farmed_items[item_index].sell_price_text.text = (farmed_items[item_index].raw_price * farmed_items[item_index].sell_number).ToString("N0");
        temp_money = farmed_items[item_index].raw_price * farmed_items[item_index].sell_number; // 해녀돈 += 자원 팔 개수 * raw_price;
        temp_num = farmed_items[item_index].sell_number; // 해녀가 가진 자원 개수 -= 자원 팔 개수
        //item_UI();
        Haenyeo.money += temp_money;
        Haenyeo.farm_item_number[temp_index] -= temp_num;
        //touch_x.SetActive(false);
        //ask_ui.gameObject.SetActive(false);
        item_UI();
        StartCoroutine(reward_effect());
        data_save();
    }

    // 업버튼 활성화/비활성화 
    public void up(int item_index)
    {
        farmed_items[item_index].number_up_button.SetActive(true);

        if (farmed_items[item_index].sell_number < farmed_items[item_index].total_number) // 총 자원보다 적으면 활성화
        {
            farmed_items[item_index].number_up_button.GetComponent<Button>().interactable = true;
            //sea_items[item_index].number_up_button.SetActive(true);
        }
        else if (farmed_items[item_index].sell_number >= farmed_items[item_index].total_number) // 총 자원보다 많거나 같아지면 비활성화 (안되는 부분)
        {
            farmed_items[item_index].number_up_button.GetComponent<Button>().interactable = false;
            //sea_items[item_index].number_up_button.SetActive(false);
        }
    }

    // 다운버튼 활성화/비활성화 
    public void down(int item_index)
    {

        farmed_items[item_index].number_down_button.SetActive(true);

        if (farmed_items[item_index].sell_number > 0) // 팔 자원 개수가 0보다 많아지면 활성화
        {
            farmed_items[item_index].number_down_button.GetComponent<Button>().interactable = true;
            farmed_items[item_index].sell_button.GetComponent<Button>().interactable = true;
            //sea_items[item_index].number_down_button.SetActive(true);
        }
        else if (farmed_items[item_index].sell_number <= 0) // 팔 자원 개수가 0과 같거나 작아지면 비활성화 (안되는 부분)
        {
            farmed_items[item_index].number_down_button.GetComponent<Button>().interactable = false;
            farmed_items[item_index].sell_button.GetComponent<Button>().interactable = false;
            //sea_items[item_index].number_down_button.SetActive(false);
        }
    }
    IEnumerator reward_effect()
    {
        plus_money.gameObject.SetActive(true); //동전들 화면에 띄우기
        plus_money.color = new Vector4(1, 1, 1, 1);    //투명도 0%인 상태
        for (int i = 0; i < 10; i++)      //투명도 점점 없어짐
        {
            plus_money.rectTransform.localPosition = new Vector3(426, 312 + i, 0);
            yield return new WaitForSeconds(0.0001f);
        }
        yield return new WaitForSeconds(0.001f);
        StartCoroutine(FadeOut(plus_money));
    }


    IEnumerator FadeOut(Image image, float sec = 0) //페이드 아웃 되듯이 사라지는 이펙트 함수
    {
        for (float i = 1f; i >= 0; i -= 0.1f)
        {
            Color color = new Vector4(1, 1, 1, i);
            image.color = color;
            yield return new WaitForSeconds(sec);
        }
        image.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.001f);
        //image2.gameObject.SetActive(false);
        //yield return new WaitForSeconds(0.05f);
        //image3.gameObject.SetActive(true);
    }
    public void data_save()
    {
        //데이터 저장

        PlayerPrefs.SetInt("Haenyeo" + "_" + "money", Haenyeo.money);

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
    public void data_load()
    {

        Haenyeo.money = PlayerPrefs.GetInt("Haenyeo_money", 500000);

        //해녀 보유한 자원 개수 초기화

        Haenyeo.farm_item_number[0] = PlayerPrefs.GetInt("Haenyeo_farm_item_number0", 0);
        Haenyeo.farm_item_number[1] = PlayerPrefs.GetInt("Haenyeo_farm_item_number1", 0);
        Haenyeo.farm_item_number[2] = PlayerPrefs.GetInt("Haenyeo_farm_item_number2", 0);
        Haenyeo.farm_item_number[3] = PlayerPrefs.GetInt("Haenyeo_farm_item_number3", 0);
        Haenyeo.farm_item_number[4] = PlayerPrefs.GetInt("Haenyeo_farm_item_number4", 0);
        Haenyeo.farm_item_number[5] = PlayerPrefs.GetInt("Haenyeo_farm_item_number5", 0);
        Haenyeo.farm_item_number[6] = PlayerPrefs.GetInt("Haenyeo_farm_item_number6", 0);
        Haenyeo.farm_item_number[7] = PlayerPrefs.GetInt("Haenyeo_farm_item_number7", 0);
        Haenyeo.farm_item_number[8] = PlayerPrefs.GetInt("Haenyeo_farm_item_number8", 0);

    }
}
