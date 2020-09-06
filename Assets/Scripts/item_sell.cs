using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class item_sell : MonoBehaviour
{
    public sell_item_info[] sea_items;
    public Text Haenyeo_money;

    // 사운드 이펙트
    public AudioSource bgm, sell_click, updown_click;

    public GameObject sell_ui, ask_ui, rsp_ui, wild_ui, farmed_ui, touch_x, no_wild, return_ui, black_bg;

    public Image plus_money;
    public int temp_index, temp_num, temp_money;
    int if_no_wild = 0;

    private void OnEnable()
    {
        item_UI();
    }
    void Awake()
    {
        bgm.volume = PlayerPrefs.GetFloat("Bgm_volume", 1);
        sell_click.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        updown_click.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        if_no_wild = 0;
        data_load();
        item_noshow();
        item_UI();        
    }

    public void return_to_home()
    {
        data_save();
        return_ui.SetActive(false);
        wild_ui.gameObject.SetActive(false);
        farmed_ui.gameObject.SetActive(false);
        item_UI();
    }

    public void tab_change()
    {
        data_save();
        wild_ui.gameObject.SetActive(false);
        farmed_ui.gameObject.SetActive(true);
    }

    public void rsp_yes()
    {
        black_bg.SetActive(true);
        touch_x.SetActive(false);
        ask_ui.gameObject.SetActive(false);
        rsp_ui.gameObject.SetActive(true);
    }
    public void rsp_no()
    {
        // 원래 지정한 금액만 짤랑
        Haenyeo.money += temp_money;
        Haenyeo.sea_item_number[temp_index] -= temp_num;
        touch_x.SetActive(false);
        ask_ui.gameObject.SetActive(false);
        item_UI();
        StartCoroutine(reward_effect());
    }

    public void sell_rsp()
    {
        black_bg.SetActive(false);
        rsp_ui.SetActive(false);
        sell_click.PlayOneShot(sell_click.clip);
        if (betting_rsp.rsp_result == 0)
        {
            Haenyeo.money += temp_money;
            //plus_money.text = "+ " + (temp_money).ToString("N0");
        }
        else if (betting_rsp.rsp_result == 1)
        {
            Haenyeo.money += temp_money/2;
            //plus_money.text = "+ " + (temp_money/2).ToString("N0");
        }
        else
        {
            Haenyeo.money += temp_money*5;
            //plus_money.text = "+ " + (temp_money*2).ToString("N0");
        }
        Haenyeo.sea_item_number[temp_index] -= temp_num;
        item_UI();
        StartCoroutine(reward_effect());
        data_save();
    }


    // 해녀가 가진 자원만 보이도록 설정
    public void item_UI()
    {
        temp_num = 0;
        temp_money = 0;
        temp_index = -1;
        item_noshow();
        Haenyeo_money.text = Haenyeo.money.ToString("N0");
        for (int i=0; i< sea_items.Length; i++)
        {
            if(Haenyeo.sea_item_number[i] > 0)
            {
                if_no_wild++;
                sea_items[i].gameObject.SetActive(true);

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
        if (if_no_wild == 0)
        {
            no_wild.gameObject.SetActive(true);
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
        touch_x.SetActive(true);
        ask_ui.SetActive(true);
        //sell_click.PlayOneShot(sell_click.clip);
        temp_index = item_index;
        //plus_money.text = "+ " + (sea_items[item_index].raw_price * sea_items[item_index].sell_number).ToString("N0");
        sea_items[item_index].sell_price_text.text = (sea_items[item_index].raw_price * sea_items[item_index].sell_number).ToString("N0");
        temp_money = sea_items[item_index].raw_price * sea_items[item_index].sell_number; // 해녀돈 += 자원 팔 개수 * raw_price;
        temp_num = sea_items[item_index].sell_number; // 해녀가 가진 자원 개수 -= 자원 팔 개수
        //item_UI();
        data_save();
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
            sea_items[item_index].sell_button.GetComponent<Button>().interactable = true;
            //sea_items[item_index].number_down_button.SetActive(true);
        }
        else if (sea_items[item_index].sell_number <= 0) // 팔 자원 개수가 0과 같거나 작아지면 비활성화 (안되는 부분)
        {
            sea_items[item_index].number_down_button.GetComponent<Button>().interactable = false;
            sea_items[item_index].sell_button.GetComponent<Button>().interactable = false;
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

        Haenyeo.sea_item_number[0] = PlayerPrefs.GetInt("Haenyeo_sea_item_number0", 0);
        Haenyeo.sea_item_number[1] = PlayerPrefs.GetInt("Haenyeo_sea_item_number1", 0);
        Haenyeo.sea_item_number[2] = PlayerPrefs.GetInt("Haenyeo_sea_item_number2", 0);
        Haenyeo.sea_item_number[3] = PlayerPrefs.GetInt("Haenyeo_sea_item_number3", 0);
        Haenyeo.sea_item_number[4] = PlayerPrefs.GetInt("Haenyeo_sea_item_number4", 0);
        Haenyeo.sea_item_number[5] = PlayerPrefs.GetInt("Haenyeo_sea_item_number5", 0);
        Haenyeo.sea_item_number[6] = PlayerPrefs.GetInt("Haenyeo_sea_item_number6", 0);
        Haenyeo.sea_item_number[7] = PlayerPrefs.GetInt("Haenyeo_sea_item_number7", 0);
        Haenyeo.sea_item_number[8] = PlayerPrefs.GetInt("Haenyeo_sea_item_number8", 0);

    }
}
