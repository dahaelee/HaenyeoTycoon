using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class store_home : MonoBehaviour
{
    //public Button change_to_upgrade, change_to_sell;
    public GameObject sell_tab, buy_tab;

    //public void upgrade_tab_click()
    //{
    //    sell_tab.SetActive(false);
    //    upgrade_tab.SetActive(true);
    //}
    //public void sell_tab_click()
    //{
    //    upgrade_tab.SetActive(false);
    //    sell_tab.SetActive(true);
    //}
    public void buy_panel_click()
    {
        buy_tab.SetActive(true);
    }
    public void sell_panel_click()
    {
        sell_tab.SetActive(true);
    }
    public void return_to_farm()
    {
        //data_save();
        SceneManager.LoadScene("farm");
    }
}
