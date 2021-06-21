using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Скрипт с данным классом находится на пустышке с UI-элементами внутри Canvas и отвечает за отображение времени действия и вида бонуса
public class TemporaryBonuses : MonoBehaviour
{
    public Slider BonusBar;

    public Text[] Bonuses = new Text[4];//Названия бонусов
    private int Bonus_ID;//Номер задействованного бонуса

    [SerializeField] private float duration = 10f;//Длительность действия бонуса
    private float timer = 0;

    public delegate void EndBonus(int bonus_ID);
    public static event EndBonus StopBonus; //Событие запускает методы, для восстановления значений без модификатора бонуса
    public static event EndBonus CorrectionBonus; //Событие позволяет понижать вероятность выпадения бонусов до 0 пока работает действующий бонус

    void Start()
    {
        if(PlayerPrefs.HasKey("TempDurationLvl"))
        {
            int temp1 = PlayerPrefs.GetInt("TempDurationLvl");//Значение одного из глобальных улучшений, увеличивающих время действия бонусов
            float temp2 = temp1 * 2.5f;

            duration += temp2;
        }
        else
        {
            PlayerPrefs.SetInt("TempDurationLvl", 0);
        }

        BonusBar.maxValue = duration;
    }
	
	//Метод вызывается при подборе бонуса и принимает значение: номер конкретного бонуса
    public void ActivateBonus(int bonus_ID)
    {
        CorrectionBonus?.Invoke(1);//Понижаем вероятность выпадения бонусов до 0

        if (timer > 0)
        {
            StopCoroutine(UpdateTimer());
            Stopping();
        }

        Bonus_ID = bonus_ID;

        timer = duration;
        BonusBar.value = timer;

        BonusBar.gameObject.SetActive(true);//Показываем Slider
        Bonuses[Bonus_ID].gameObject.SetActive(true);//Показываем название бонуса

        StartCoroutine(UpdateTimer());

        if (PlayerPrefs.HasKey("BonusCollectorProgress"))//Проверяем, есть ли ежедневное задание "Собрать n временных бонусов"
        {
            int progress = PlayerPrefs.GetInt("BonusCollectorProgress");
            progress++;
            PlayerPrefs.SetInt("BonusCollectorProgress", progress);//Обновляем прогресс в исполнении данного задания
        }
    }
	
	//Сопрограмма, предназначенная для отсчитывания время действия бонуса
    IEnumerator UpdateTimer()
    {
        if(timer > 0)
        {
            while (timer > 0)
            {
                timer -= 0.1f;
                BonusBar.value = timer;//Отображаем уменьшение значение элемента Slider
                yield return new WaitForSeconds(0.1f);
            }
        }

        if(timer <= 0)
        {
            Stopping();
        }

        StopCoroutine(UpdateTimer());
    }

	//Метод запусает оповещение других классов о завершении действия бонуса
    private void Stopping()
    {
        BonusBar.gameObject.SetActive(false);
        Bonuses[Bonus_ID].gameObject.SetActive(false);

        StopBonus?.Invoke(Bonus_ID);//Активируем через событие методы, возвращающие стандартные значения вне бонусов
        CorrectionBonus?.Invoke(0);//Снова возвращаем стандартные вероятности выпадения бонусов
    }
}
