using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public GameObject popup;
    public Transform popupImg;
    public TextMeshProUGUI popupTxt;

    [TextArea]
    public string text_Emty;
    [TextArea]
    public string text_Name;
    [TextArea]
    public string text_Submit;

    public void ClickSubmit()
    {
        string _name = input_Name.text;
        string _content = input_Content.text;

        // 내용물이 비어있으면 취소
        if (_name == null || _name == "" || _content == null || _content == "")
        {
            StartCoroutine(PopUp(text_Emty));
        }
        else // 비어있지 않다면
        {
            if (CheckName(_name)) // 이름이 중복된다면
            {
                StartCoroutine(PopUp(text_Name));
            }
            else // 이름이 중복되지 않는다면
            {
                Save(_name, _content); // 저장
            }
        }
    }

    IEnumerator PopUp(string text)
    {
        // 문구, 크기 준비
        popupTxt.text = text;
        popupImg.localScale = Vector3.zero;
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

    bool CheckName(string _name)
    {
        string path = Application.persistentDataPath + "/" + _name + ".json";
        return File.Exists(path);
    }

    void Save(string _name, string _content)
    {
        string path = Application.persistentDataPath + "/" + _name + ".json";

        data._name = _name;
        data._content = _content;

        // 저장
        string saveData = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, saveData);

        // 팝업창
        StartCoroutine(PopUp(text_Submit));
    }
}
