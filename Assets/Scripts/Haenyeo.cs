using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;
using System.Diagnostics;
using System.Runtime.Versioning;

public class Haenyeo : MonoBehaviour
{
    public static int money, debt, diving_time, moving_speed, farm_number, day, level, hp;

    public static int[] sea_item_number = new int[9]; //보유하고있는 자원 개수
    public Text hp_text;
    public Image hp_bg;
    public Sprite hp100, hp80, hp60, hp40, hp20, hp0;

    public enum sea_item_index
    {
        starfish = 0,
        seaweed,
        shell,
        shrimp,
        jellyfish,
        crab,
        octopus,
        abalone,
        turtle

    };

    public void Start()
    {
        hp_text.GetComponent<Text>().text = hp.ToString();
        Sprite hp100 = Resources.Load<Sprite>("heart100");
        Sprite hp80 = Resources.Load<Sprite>("heart80");
        Sprite hp60 = Resources.Load<Sprite>("heart60");
        Sprite hp40 = Resources.Load<Sprite>("heart40");
        Sprite hp20 = Resources.Load<Sprite>("heart20");
        Sprite hp0 = Resources.Load<Sprite>("heart0");
    }
    void Update()
    {
        
       
        hp_text.GetComponent<Text>().text = hp.ToString();
        if (hp >= 90)
        {
            hp_bg.GetComponent<Image>().sprite = hp100;
        }
        else if (hp < 90 && hp >= 70)
        {
            hp_bg.GetComponent<Image>().sprite = hp80;
        }
        else if (hp < 70 && hp >= 50)
        {
            hp_bg.GetComponent<Image>().sprite = hp60;
        }
        else if (hp < 50 && hp >= 30)
        {
            hp_bg.GetComponent<Image>().sprite = hp40;
        }
        else if (hp < 30 && hp >= 10)
        {
            hp_bg.GetComponent<Image>().sprite = hp20;
        }
        else if (hp < 10)
        {
            hp_bg.GetComponent<SpriteRenderer>().sprite = hp0;
        }
    }

    void SetHP()
    {
        
        
    }

}
