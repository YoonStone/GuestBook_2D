using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

[System.Serializable]
public class Data
{
    public string _name;
    public string _content;
}

public class GameManager : MonoBehaviour
{
    public Data data;

    public TMP_InputField input_Name;
    public TMP_InputField input_Content;
    public GameObject popup, txt_Normal, txt_Save;
    public Transform popupImg;
    public TextMeshProUGUI popupTxt;
    public Button yBtn, nBtn;

    [TextArea]
    public string text_Emty;
    [TextArea]
    public string text_Name;
    [TextArea]
    public string text_Submit;

    bool isSavePop;

    private void Start()
    {
        print(Application.persistentDataPath);
    }

    public void ClickSubmit()
    {
        string _name = input_Name.text;
        string _content = input_Content.text;

        print(_name);
        // 내용물이 비어있으면 취소
        if (_name == null || _name == "" || _content == null || _content == "")
        {
            StartCoroutine(PopUp(text_Emty));
        }
        else if (CheckName(_name)) // 이름이 중복된다면
        {
            StartCoroutine(PopUp(text_Name));
        }
        else // 이름이 중복되지 않는다면
        {
            StartCoroutine(SavePopOpen()); // 저장확인 팝업창
        }
    }

    IEnumerator PopUp(string text)
    {
        // 문구, 크기 준비
        popupTxt.text = text;
        popupImg.localScale = Vector3.zero;
        txt_Normal.SetActive(true);
        txt_Save.SetActive(false);
        popup.SetActive(true);

        float time = 0;
        while (time < 1) // 커졌다
        {
            time += Time.deltaTime * 2;
            popupImg.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        time = 0;
        while (time < 1) // 작아졌다
        {
            time += Time.deltaTime * 2;
            popupImg.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, time);
            yield return null;
        }

        popup.SetActive(false);

        // 제출이 완료되었다면 초기화
        if (text == text_Submit)
        {
            input_Name.text = "";
            input_Content.text = "";
        }
    }

    IEnumerator SavePopOpen() // 저장 팝업창 켜기
    {
        // 버튼, 크기 준비
        yBtn.interactable = false;
        nBtn.interactable = false;

        popupImg.localScale = Vector3.zero;
        txt_Normal.SetActive(false);
        txt_Save.SetActive(true);
        popup.SetActive(true);

        float time = 0;
        while (time < 1) // 커졌다
        {
            time += Time.deltaTime * 2;
            popupImg.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time);
            yield return null;
        }

        yBtn.interactable = true;
        nBtn.interactable = true;
    }

    IEnumerator SavePopClose() // 저장 팝업창 끄기
    {
        float time = 0;
        while (time < 1) // 작아졌다
        {
            time += Time.deltaTime * 2;
            popupImg.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, time);
            yield return null;
        }

        popup.SetActive(false);
    }

    bool CheckName(string _name)
    {
        string path = Application.persistentDataPath + "/" + _name + ".json";
        return File.Exists(path);
    }

    public void Save()
    {
        string _name = input_Name.text;
        string _content = input_Content.text;

        string path = Application.persistentDataPath + "/" + _name + ".json";

        data._name = _name;
        data._content = _content;

        // 저장
        string saveData = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, saveData);

        isSavePop = false;

        // 팝업창
        StartCoroutine(PopUp(text_Submit));
    }

    public void No()
    {
        StartCoroutine(SavePopClose());
    }
}
