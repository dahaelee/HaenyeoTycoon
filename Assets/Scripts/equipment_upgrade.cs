using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// 각 장비 이름 넣기
// 각 중급, 고급 해녀 -> 중간바다 깊은바다


public class equipment_upgrade : MonoBehaviour
{
    // 스크롤 이펙트 관련
    public RectTransform[] List;
    public int count;
    private float suit_pos, goggle_pos, flipper_pos;
    private float suit_movepos, goggle_movepos, flipper_movepos;
    private bool suit_IsScroll = false, goggle_IsScroll = false, flipper_IsScroll = false;
    // 사운드 이펙트
    public AudioSource upgrade_click;

    public Text Haenyeo_money;

    public equipment_info[] suits;
    public equipment_info[] goggles;
    public equipment_info[] flippers;

    public Text[] suit_upgrade_price_text;        // 업그레이드에 필요한 가격 텍스트. 활성화,비활성화 2개
    public Text[] goggle_upgrade_price_text;
    public Text[] flipper_upgrade_price_text;

    public GameObject suit_enable_button;      // 해녀복 활성화 버튼
    public GameObject suit_disable_button;     // 해녀복 비활성화 버튼
    public GameObject goggle_enable_button;    // 물안경 활성화 버튼
    public GameObject goggle_disable_button;   // 물안경 비활성화 버튼
    public GameObject flipper_enable_button;   // 오리발 활성화 버튼
    public GameObject flipper_disable_button;  // 오리발 비활성화 버튼

    public GameObject buy_ui, equip_ui, item_ui, return_ui;

    // 해녀가 장착하고 있는 장비
    public static int my_suit = 0;
    public static int my_goggle = 0;
    public static int my_flipper = 0;


    //IEnumerator upgrade_effect()

    public void return_to_home()
    {
        data_save();
        return_ui.SetActive(false);
        equip_ui.gameObject.SetActive(false);
        item_ui.gameObject.SetActive(false);
    }

    public void tab_change()
    {
        equip_ui.gameObject.SetActive(false);
        item_ui.gameObject.SetActive(true);
    }

    public void tab_change_temp()
    {
        item_ui.gameObject.SetActive(false);
        equip_ui.gameObject.SetActive(true);
    }

    void Update()
    {
        init_equipment();
        if ((Haenyeo.money >= suits[my_suit].next_upgrade_price))
        {
            suit_disable_button.SetActive(false);
            suit_enable_button.SetActive(true);
        }
        else
        {
            suit_disable_button.SetActive(true);
            suit_enable_button.SetActive(false);
        }
        if ((Haenyeo.money >= goggles[my_goggle].next_upgrade_price))
        {
            goggle_disable_button.SetActive(false);
            goggle_enable_button.SetActive(true);
        }
        else
        {
            goggle_disable_button.SetActive(true);
            goggle_enable_button.SetActive(false);
        }
        if ((Haenyeo.money >= flippers[my_flipper].next_upgrade_price))
        {
            flipper_disable_button.SetActive(false);
            flipper_enable_button.SetActive(true);
        }
        else
        {
            flipper_disable_button.SetActive(true);
            flipper_enable_button.SetActive(false);
        }

    }
    public void scroll_up(int index)
    {
        switch (index)
        {
            case 0:
                if (List[index].rect.yMin - List[index].rect.yMax / count == suit_movepos)
                {

                }
                else
                {
                    List[0].gameObject.SetActive(true);
                    suit_IsScroll = true;
                    suit_movepos = suit_pos - List[index].rect.height / count;
                    suit_pos = suit_movepos;
                    StartCoroutine(scroll(index));
                }
                break;

            case 1:
                if (List[index].rect.yMin - List[index].rect.yMax / count == goggle_movepos)
                {

                }
                else
                {
                    List[1].gameObject.SetActive(true);
                    goggle_IsScroll = true;
                    goggle_movepos = goggle_pos - List[index].rect.height / count;
                    goggle_pos = goggle_movepos;
                    StartCoroutine(scroll(index));
                }
                break;

            case 2:
                if (List[index].rect.yMin - List[index].rect.yMax / count == flipper_movepos)
                {

                }
                else
                {
                    List[2].gameObject.SetActive(true);
                    flipper_IsScroll = true;
                    flipper_movepos = flipper_pos - List[index].rect.height / count;
                    flipper_pos = flipper_movepos;
                    StartCoroutine(scroll(index));
                }
                break;
        }

    }

    IEnumerator scroll(int index)
    {
        switch (index)
        {
            case 0:
                while (suit_IsScroll)
                {
                    List[index].localPosition = Vector2.Lerp(List[index].localPosition, new Vector2(0, suit_movepos), Time.deltaTime * 5);
                    if (Vector2.Distance(new Vector2(0, suit_movepos), List[index].localPosition) < 0.1f)
                    {
                        suit_IsScroll = false;
                    }
                    yield return null;
                }
                break;

            case 1:
                while (goggle_IsScroll)
                {
                    List[index].localPosition = Vector2.Lerp(List[index].localPosition, new Vector2(0, goggle_movepos), Time.deltaTime * 5);
                    if (Vector2.Distance(new Vector2(0, goggle_movepos), List[index].localPosition) < 0.1f)
                    {
                        goggle_IsScroll = false;
                    }
                    yield return null;
                }
                break;
            case 2:
                while (flipper_IsScroll)
                {
                    List[index].localPosition = Vector2.Lerp(List[index].localPosition, new Vector2(0, flipper_movepos), Time.deltaTime * 5);
                    if (Vector2.Distance(new Vector2(0, flipper_movepos), List[index].localPosition) < 0.1f)
                    {
                        flipper_IsScroll = false;
                    }
                    yield return null;
                }
                break;
        }
    }

    void Awake()
    {
        init_equipment();
        if (my_suit == 1)
        {
            suits[0].gameObject.transform.SetAsFirstSibling();
            suits[1].gameObject.transform.SetAsLastSibling();
        }
        if (my_suit == 2)
        {
            suits[2].gameObject.transform.SetAsLastSibling();
        }
        if (my_goggle == 1)
        {
            goggles[0].gameObject.transform.SetAsFirstSibling();
            goggles[1].gameObject.transform.SetAsLastSibling();
        }
        if (my_goggle == 2)
        {
            goggles[2].gameObject.transform.SetAsLastSibling();
        }
        if (my_flipper == 1)
        {
            flippers[0].gameObject.transform.SetAsFirstSibling();
            flippers[1].gameObject.transform.SetAsLastSibling();
        }
        if (my_flipper == 2)
        {
            flippers[2].gameObject.transform.SetAsLastSibling();
        }
        upgrade_click.volume = PlayerPrefs.GetFloat("Effect_volume", 1);
        
        Haenyeo_money.text = Haenyeo.money.ToString("N0");
        //init_equipment();

        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:
                    suit_pos = List[i].localPosition.y;
                    suit_movepos = List[i].rect.yMax - List[i].rect.yMax / count;
                    break;

                case 1:
                    goggle_pos = List[i].localPosition.y;
                    goggle_movepos = List[i].rect.yMax - List[i].rect.yMax / count;
                    break;

                case 2:
                    flipper_pos = List[i].localPosition.y;
                    flipper_movepos = List[i].rect.yMax - List[i].rect.yMax / count;
                    break;
            }
        }
    }

    // 장비 정보 초기화
    public void init_equipment()
    {
        my_suit = PlayerPrefs.GetInt("PLAYER_SUIT", 0);
        my_goggle = PlayerPrefs.GetInt("PLAYER_GOGGLE", 0);
        my_flipper = PlayerPrefs.GetInt("PLAYER_FLIPPER", 0);

        // 해녀복
        for (int i = 0; i < suits.Length; i++)
        {
            if (i == my_suit)
            {
                suits[i].equip_name.gameObject.SetActive(true);

                if (suits.Length - 1 == i)    // 업그레이드가 최대치인 경우
                {
                    suit_enable_button.SetActive(false);
                    suit_disable_button.SetActive(true);
                    for (int j = 0; j < suit_upgrade_price_text.Length; j++)
                    {
                        suit_upgrade_price_text[j].text = "Max";
                    }
                }
                else
                {
                    if (Haenyeo.money >= suits[i].next_upgrade_price) // 업그레이드에 필요한 돈이 충분하다면
                    {
                        suit_enable_button.SetActive(true);
                        suit_disable_button.SetActive(false);
                    }
                    else
                    {
                        suit_enable_button.SetActive(false);
                        suit_disable_button.SetActive(true);
                    }

                    for (int j = 0; j < suit_upgrade_price_text.Length; j++)
                    {
                        suit_upgrade_price_text[j].text = suits[i].next_upgrade_price.ToString("N0");   // "NO" 적으면 돈처럼 콤마 표시. 
                    }
                }
            }
            else
            {
                suits[i].equip_name.gameObject.SetActive(false);
            }
        }

        // 물안경
        for (int i = 0; i < goggles.Length; i++)
        {
            if (i == my_goggle)
            {
                goggles[i].equip_name.gameObject.SetActive(true);

                if (goggles.Length - 1 == i)    // 업그레이드가 최대치인 경우
                {
                    goggle_enable_button.SetActive(false);
                    goggle_disable_button.SetActive(true);
                    for (int j = 0; j < goggle_upgrade_price_text.Length; j++)
                    {
                        goggle_upgrade_price_text[j].text = "Max";
                    }
                }
                else
                {
                    if (Haenyeo.money >= goggles[i].next_upgrade_price) // 업그레이드에 필요한 돈이 충분하다면
                    {
                        //Debug.Log("I can buy");
                        goggle_enable_button.SetActive(true);
                        goggle_disable_button.SetActive(false);
                    }
                    else
                    {
                        //Debug.Log("I cannot buy");
                        goggle_enable_button.SetActive(false);
                        goggle_disable_button.SetActive(true);
                    }

                    for (int j = 0; j < goggle_upgrade_price_text.Length; j++)
                    {
                        goggle_upgrade_price_text[j].text = goggles[i].next_upgrade_price.ToString("N0");
                    }
                }
            }
            else
            {
                goggles[i].equip_name.gameObject.SetActive(false);
            }
        }

        // 오리발
        for (int i = 0; i < flippers.Length; i++)
        {
            if (i == my_flipper)
            {
                flippers[i].equip_name.gameObject.SetActive(true);

                if (flippers.Length - 1 == i)    // 업그레이드가 최대치인 경우
                {
                    flipper_enable_button.SetActive(false);
                    flipper_disable_button.SetActive(true);
                    for (int j = 0; j < flipper_upgrade_price_text.Length; j++)
                    {
                        flipper_upgrade_price_text[j].text = "Max";
                    }
                }
                else
                {
                    if (Haenyeo.money >= flippers[i].next_upgrade_price) // 업그레이드에 필요한 돈이 충분하다면
                    {
                        flipper_enable_button.SetActive(true);
                        flipper_disable_button.SetActive(false);
                    }
                    else
                    {
                        flipper_enable_button.SetActive(false);
                        flipper_disable_button.SetActive(true);
                    }

                    for (int j = 0; j < flipper_upgrade_price_text.Length; j++)
                    {
                        flipper_upgrade_price_text[j].text = flippers[i].next_upgrade_price.ToString("N0");
                    }
                }
            }
            else
            {
                flippers[i].equip_name.gameObject.SetActive(false);
            }
        }

    }

    // 해녀복 레이아웃 보이기
    public void suit_info()
    {
        suits[my_suit].equipment_info_obj.gameObject.SetActive(!suits[my_suit].equipment_info_obj.gameObject.activeSelf);
    }
    // 물안경 레이아웃 켜기/끄기
    public void goggle_info()
    {
        goggles[my_goggle].equipment_info_obj.gameObject.SetActive(!goggles[my_goggle].equipment_info_obj.gameObject.activeSelf);
    }
    // 오리발 레이아웃 켜기/끄기
    public void flipper_info()
    {
        flippers[my_flipper].equipment_info_obj.gameObject.SetActive(!flippers[my_flipper].equipment_info_obj.gameObject.activeSelf);
    }

    // 해녀복 업그레이드 버튼 클릭
    public void suit_upgrade_click()
    {
        for (int i = 0; i < suits.Length; i++)
        {
            suits[i].equipment_info_obj.gameObject.SetActive(false);
        }
        if (Haenyeo.money >= suits[my_suit].next_upgrade_price && (my_suit != 2))
        {
            upgrade_click.PlayOneShot(upgrade_click.clip);
            Haenyeo.money -= suits[my_suit].next_upgrade_price;
            if(my_suit == 0)
            {
                Haenyeo.level = 2;
            }
            if(my_suit == 1)
            {
                Haenyeo.level = 3;
            }
            my_suit++;
            PlayerPrefs.SetInt("PLAYER_SUIT", my_suit);
            init_equipment();
            scroll_up(0);
            Haenyeo_money.text = Haenyeo.money.ToString("N0");
        }
        data_save();
    }

    // 물안경 업그레이드 버튼 클릭
    public void goggle_upgrade_click()
    {
        for (int i = 0; i < goggles.Length; i++)
        {
            goggles[i].equipment_info_obj.gameObject.SetActive(false);
        }
        if (Haenyeo.money >= goggles[my_goggle].next_upgrade_price && (my_goggle != 2))
        {
            upgrade_click.PlayOneShot(upgrade_click.clip);
            Haenyeo.money -= goggles[my_goggle].next_upgrade_price;
            if (my_goggle == 0)
            {
                Haenyeo.coin_time = 7;
            }
            if (my_goggle == 1)
            {
                Haenyeo.coin_time = 6;
            }
            my_goggle++;
            PlayerPrefs.SetInt("PLAYER_GOGGLE", my_goggle);
            init_equipment();
            scroll_up(1);
            Haenyeo_money.text = Haenyeo.money.ToString("N0");
        }
        data_save();
    }


    // 오리발 업그레이드 버튼 클릭
    public void flipper_upgrade_click()
    {
        for (int i = 0; i < flippers.Length; i++)
        {
            flippers[i].equipment_info_obj.gameObject.SetActive(false);
        }
        if (Haenyeo.money >= flippers[my_flipper].next_upgrade_price && (my_flipper != 2))
        {
            upgrade_click.PlayOneShot(upgrade_click.clip);
            Haenyeo.money -= flippers[my_flipper].next_upgrade_price;
            if (my_flipper == 0)
            {
                Haenyeo.moving_speed = 8.4f;
            }
            if (my_flipper == 1)
            {
                Haenyeo.moving_speed = 9.8f;
            }
            my_flipper++;
            PlayerPrefs.SetInt("PLAYER_FLIPPER", my_flipper);
            init_equipment();
            scroll_up(2);
            Haenyeo_money.text = Haenyeo.money.ToString("N0");
        }
        data_save();
    }


    public void data_save()
    {
        //데이터 저장

        PlayerPrefs.SetInt("Haenyeo" + "_" + "money", Haenyeo.money);

        PlayerPrefs.SetInt("PLAYER_SUIT", my_suit);
        PlayerPrefs.SetInt("PLAYER_GOGGLE", my_goggle);
        PlayerPrefs.SetInt("PLAYER_FLIPPER", my_flipper);

        PlayerPrefs.SetInt("Haenyeo_level", Haenyeo.level); // 해녀복 효과 
        PlayerPrefs.SetInt("Haenyeo_coin_time", Haenyeo.coin_time); // 물안경 효과
        PlayerPrefs.SetFloat("Haenyeo_moving_speed", Haenyeo.moving_speed); // 오리발 효과  

        PlayerPrefs.Save();
    }

    //public void data_load()
    //{

    //    Haenyeo.money = PlayerPrefs.GetInt("Haenyeo_money", 500000);
    //    Haenyeo.debt = PlayerPrefs.GetInt("Haenyeo_debt", 10000000);
    //    Haenyeo.moving_speed = PlayerPrefs.GetInt("Haenyeo_moving_speed", 5);
    //    PlayerPrefs.SetInt("PLAYER_SUIT", my_suit);
    //    PlayerPrefs.SetInt("PLAYER_GOGGLE", my_goggle);
    //    PlayerPrefs.SetInt("PLAYER_FLIPPER", my_flipper);
    //}
}




