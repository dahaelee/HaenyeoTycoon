using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class buy_item : MonoBehaviour
{
    public store_item_info[] store_items; // net, bonus, 5m, 15m, 25m
    //public int[] item_inven = new int[5]; //아이템 개수
    public Text[] inven_text;
    public Text Haenyeo_money;
    public GameObject buy_ui, equip_ui, item_ui, return_ui;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void return_to_home()
    {
        data_save();
        return_ui.SetActive(false);
        equip_ui.gameObject.SetActive(false);
        item_ui.gameObject.SetActive(false);
    }

    public void tab_change()
    {
        item_ui.gameObject.SetActive(false);
        equip_ui.gameObject.SetActive(true);
    }

    void Awake()
    {
        //data_load();
        //init_inven();
        //check_price();
        inven_UI();
    }

    // Update is called once per frame
    void Update()
    {
        check_price();
    }

    public void check_price() // 사는 버튼 켜고 끄는 함수
    {
        for (int i = 0; i < store_items.Length; i++)
        {
            store_items[i].price_text[0].text = store_items[i].price.ToString("N0");
            store_items[i].price_text[1].text = store_items[i].price.ToString("N0");

            if (Haenyeo.money >= store_items[i].price)
            {
                store_items[i].enable_btn.SetActive(true);
                store_items[i].disable_btn.SetActive(false);
            }
            else
            {
                store_items[i].enable_btn.SetActive(false);
                store_items[i].disable_btn.SetActive(true);
            }
        }
    }

    public void buy_click(int item) // 사는 버튼 클릭 (돈 줄어들고, 버튼 업데이트, 인벤 업데이트)
    {
        Haenyeo.item_inven[item]++;
        Haenyeo.money -= store_items[item].price;
        Haenyeo_money.text = Haenyeo.money.ToString("N0");
        check_price();
        inven_UI();
        //data_save();
    }

    public void init_inven()
    {
        for (int i = 0; i < store_items.Length; i++)
        {
            //inven_text[i].text = "0";
            Haenyeo.item_inven[i] = 0;
        }
    }

    public void inven_UI() //인벤토리 개수 업데이트하는 함수
    {
        for (int i = 0; i < store_items.Length; i++)
        {
            inven_text[i].text = Haenyeo.item_inven[i].ToString();
        }
        data_save();
    }

    public void data_save()
    {
        //데이터 저장

        PlayerPrefs.SetInt("Haenyeo" + "_" + "money", Haenyeo.money);

        PlayerPrefs.SetInt("Haenyeo_item_inven_number0", Haenyeo.item_inven[0]);
        PlayerPrefs.SetInt("Haenyeo_item_inven_number1", Haenyeo.item_inven[1]);
        PlayerPrefs.SetInt("Haenyeo_item_inven_number2", Haenyeo.item_inven[2]);
        PlayerPrefs.SetInt("Haenyeo_item_inven_number3", Haenyeo.item_inven[3]);
        //PlayerPrefs.SetInt("Haenyeo_item_inven_number4", Haenyeo.item_inven[4]);

        PlayerPrefs.Save();
    }
    
}
