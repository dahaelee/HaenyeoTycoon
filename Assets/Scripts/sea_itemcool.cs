using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sea_itemcool : MonoBehaviour
{
    public Image image, block_touch;
    public Button button;
    public Text text;
    public float coolTime = 10.0f;
    public bool isClicked = false;
    float leftTime;

    void Update()
    {
        if (isClicked)
            if (leftTime > 0)
            {
                if (!block_touch.gameObject.activeSelf) //터치 방지가 비활성화 상태여야만
                {
                    leftTime -= Time.deltaTime;
                }
               
                if (leftTime < 0)
                {
                    leftTime = 0;
                    if (button)
                        button.enabled = true;
                    isClicked = true;
                }

                float ratio = 1.0f - (leftTime / coolTime);
                if (image)
                    image.fillAmount = ratio;
            }

        if (gameObject.name == "net_active")
        {
            text.text = Haenyeo.item_inven[0].ToString();
            if (Haenyeo.item_inven[0] < 1)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }

        if (gameObject.name == "boost_active")
        {
            text.text = Haenyeo.item_inven[1].ToString();
            if (Haenyeo.item_inven[1] < 1)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }

        if (gameObject.name == "double_active")
        {
            text.text = Haenyeo.item_inven[2].ToString();
            if (Haenyeo.item_inven[2] < 1)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }
    }

    public void StartCoolTime()
    {
        leftTime = coolTime;
        isClicked = true;
        if (button)
            button.enabled = false; // 버튼 기능을 해지함.
    }
}
