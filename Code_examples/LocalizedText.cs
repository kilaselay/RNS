using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//Данный скрипт с классом ставится на любой объект типа Text и служит для отображения текущего перевода текста
//Данный код был взят из официальной документации по Unity и доработан под собственные цели
public class LocalizedText : MonoBehaviour
{
    [SerializeField]
    private string key;

    private Text text;

    void Awake()
    {
        if(text == null)
        {
            text = GetComponent<Text>();
        }

        LocalizationManager.OnLanguageChanged += UpdateText;//Класс LocalizationManager отвечает за загрузку действующего перевода
    }

    void Start()
    {
        UpdateText();
    }

    private void OnDestroy()
    {
        LocalizationManager.OnLanguageChanged -= UpdateText;
    }

	//Метод активируется по событию смены языка и в методе Start()
    virtual protected void UpdateText()
    {
        if (gameObject == null) return;

        if (text == null)
        {
            text = GetComponent<Text>();
        }

        if (LocalizationManager.localizedText.ContainsKey(key))//Проверяем наличие ключа в словаре
        {
            text.text = LocalizationManager.localizedText[key];//localizedText - словарь с переводами для текущего языка.
															//Ключи не меняются независимо от перевода, значение зависит от перевода
        }
        else
        {
            throw new Exception("Localization text with key \"" + key + "\" not found");
        }
    }
}
