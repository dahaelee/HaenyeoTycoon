using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;

public class Haenyeo : MonoBehaviour
{
    public static int money, debt, diving_time, moving_speed, farm_number, day, level, hp;

    public static int[] sea_item_number = new int[9]; //보유하고있는 자원 개수
    public Image hp100, hp80, hp60, hp40, hp20, hp0;
    public Text hp_text;

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
    }
    void Update()
    {
        hp_text.GetComponent<Text>().text = hp.ToString();

        if (hp >= 90)
        {
            hp_off();
            hp100.gameObject.SetActive(true);
        }
        else if (hp < 90 && hp >= 70)
        {
            hp_off();
            hp80.gameObject.SetActive(true);
        }
        else if (hp < 70 && hp >= 50)
        {
            hp_off();
            hp60.gameObject.SetActive(true);
        }
        else if (hp < 50 && hp >= 30)
        {
            hp_off();
            hp40.gameObject.SetActive(true);
        }
        else if (hp < 30 && hp >= 10)
        {
            hp_off();
            hp20.gameObject.SetActive(true);
        }
        else if (hp < 10)
        {
            hp_off();
            hp0.gameObject.SetActive(true);
        }


    }

    void hp_off()
    {
        hp100.gameObject.SetActive(false);
        hp80.gameObject.SetActive(false);
        hp60.gameObject.SetActive(false);
        hp40.gameObject.SetActive(false);
        hp20.gameObject.SetActive(false);
        hp0.gameObject.SetActive(false);
    }





}
