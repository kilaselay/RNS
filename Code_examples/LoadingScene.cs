using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Скрипт с классом находится на UI-панели с элементами. Изначально панель неактивна
public class LoadingScene : MonoBehaviour
{
    public int Scene_ID;//Номер загруаемой сцены из менеджера сцен

    public Slider LoadingProgress;
    public Text LoadingText;

    public Image Logo;

	
    private void OnEnable()
    {
        LoadingProgress.value = 0;
        LoadingText.text = "0%";

        SelectViewLevel();

        StartCoroutine(AsyncLoadScene());
    }

	//Сопрограмма с асинхронной загрузкой сцены. Также отображает прогресс загрузки на UI
    IEnumerator AsyncLoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(Scene_ID);

        float progress = 0;

        while (!async.isDone)
        {
            Logo.transform.Rotate(new Vector3(0, 0, 100) * Time.deltaTime);//Вращаем изображение
            progress = async.progress / 0.9f;
            LoadingProgress.value = progress;//Двигаем Slider
            LoadingText.text = string.Format("{0:0}%", progress * 100);//Отображаем процент загрузки
            yield return null;
        }
    }

	//Метод служит для выбора скайбокса, который будет отрисован в методе Awake загружаемой сцены
    private void SelectViewLevel()
    {
        string temp_s = PlayerPrefs.GetString("SelectedLevels");//В строке хранятся номера купленных и выбранных для отображения уровней(скайбоксов)
        string temp_s2;
        int temp_i;
        int sky;

        List<int> Sky = new List<int>();

        if (temp_s.Length > 1)//Если для загрузки существуют уровни кроме базового бесплатного
        {
            foreach (char temp_ch in temp_s)
            {
                temp_s2 = temp_ch.ToString();//Каждое символ строки делаем временной строкой
                temp_i = int.Parse(temp_s2);//Превращаем "число-строку" в целое число
                Sky.Add(temp_i);
            }

            int sky_random = Random.Range(0, Sky.Count);//Из нескольких выбранных для загрузки скайбоксов,выбираем 1 случайно
            sky = Sky[sky_random];
        }
        else
        {
            sky = int.Parse(temp_s);
        }

        PlayerPrefs.SetInt("LoadLevel", sky);//Из этого файла реестра следующая сцена сразу узнает номер скайбокса, который нужно отрендерить
    }
}
