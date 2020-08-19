using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class happy_ending : MonoBehaviour
{
    public Text text, touch;
    public Image cut0, cut1, cut2, cut3, gameclear, interactive, twinkle1, twinkle2;
    // Start is called before the first frame update
    void Start()
    {
        touch.gameObject.SetActive(false);
        interactive.gameObject.SetActive(false);
        cut0.gameObject.SetActive(true);
        cut1.gameObject.SetActive(false);
        cut2.gameObject.SetActive(false);
        cut3.gameObject.SetActive(false);
        StartCoroutine(typing());

    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator typing()
    {
        yield return new WaitForSeconds(1f);

        string sentence = "그렇게 "+Haenyeo.day+"일이 지났다...";
        for (int i = 0; i < sentence.Length; i++)
        {
            text.text = sentence.Substring(0, i + 1);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        for (float i = 1f; i >= 0; i -= 0.1f)
        {
            text.color = new Vector4(1, 1, 1, i);
            yield return new WaitForSeconds(0.1f);
        }
        text.gameObject.SetActive(false);

        cut1.gameObject.SetActive(true);        //2-1 장면
        cut1.color = new Vector4(1, 1, 1, 0);
        for (float i = 0f; i <= 1; i += 0.1f)
        {
            cut1.color = new Vector4(1, 1, 1, i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(4f);
        for (float i = 1f; i >= 0; i -= 0.1f)
        {
            cut1.color = new Vector4(1, 1, 1, i);
            yield return new WaitForSeconds(0.1f);
        }
        cut1.gameObject.SetActive(false);

        cut2.gameObject.SetActive(true);                //2-2 장면
        cut2.color = new Vector4(1, 1, 1, 0);
        for (float i = 0f; i <= 1; i += 0.1f)
        {
            cut2.color = new Vector4(1, 1, 1, i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(3f);
        for (float i = 1f; i >= 0; i -= 0.1f)
        {
            cut2.color = new Vector4(1, 1, 1, i);
            yield return new WaitForSeconds(0.1f);
        }
        cut2.gameObject.SetActive(false);
        cut3.gameObject.SetActive(true);

        StartCoroutine("effect_gameclear");          //게임오버 이펙트

    }






    IEnumerator FadeOut(Image image, Image image2 = null, Image image3 = null, float sec = 0) //페이드 아웃 되듯이 사라지는 이펙트 함수
    {
        for (float i = 1f; i >= 0; i -= 0.1f)
        {
            Color color = new Vector4(1, 1, 1, i);
            image.color = color;
            yield return new WaitForSeconds(sec);
        }
        image.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        image2.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        image3.gameObject.SetActive(true);
    }

    IEnumerator effect_gameclear()
    {
        StartCoroutine(twinkles());
        for (int i = 0; i < 20; i++)                     //게임오버 이펙트
        {
            gameclear.rectTransform.localScale = new Vector3((float)(0.8 + i * 0.01), (float)(0.8 + i * 0.01), (float)(0.8 + i * 0.01));
            yield return new WaitForSeconds(0.05f);
        }
        for (int i = 0; i < 20; i++)
        {
            gameclear.rectTransform.localScale = new Vector3((float)(1 - i * 0.01), (float)(1 - i * 0.01), (float)(1 - i * 0.01));
            yield return new WaitForSeconds(0.05f);
        }
        for (int i = 0; i < 20; i++)
        {
            gameclear.rectTransform.localScale = new Vector3((float)(0.8 + i * 0.01), (float)(0.8 + i * 0.01), (float)(0.8 + i * 0.01));
            yield return new WaitForSeconds(0.05f);
        }
        for (int i = 0; i < 20; i++)
        {
            gameclear.rectTransform.localScale = new Vector3((float)(1 - i * 0.01), (float)(1 - i * 0.01), (float)(1 - i * 0.01));
            yield return new WaitForSeconds(0.05f);
        }
        for (int i = 0; i < 20; i++)
        {
            gameclear.rectTransform.localScale = new Vector3((float)(0.8 + i * 0.01), (float)(0.8 + i * 0.01), (float)(0.8 + i * 0.01));
            yield return new WaitForSeconds(0.05f);
        }

        touch.gameObject.SetActive(true);
        interactive.gameObject.SetActive(true);

    }

    public void restart()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("start");
        //처음 화면으로 넘어가기
    }
    
    IEnumerator twinkles()
    {
        while (true)
        {
            twinkle2.gameObject.SetActive(false);
            twinkle1.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            twinkle1.gameObject.SetActive(false);
            twinkle2.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            
        }
    }


}
