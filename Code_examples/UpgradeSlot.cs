using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Скрипт с классом находится на префабе, представляющем собой UI-панель с элементами.
//Несколько таких префабов находятся во вкладке "УЛУЧШЕНИЯ" в игре.
//Служит для приобретения конкретного глобального улучшения.
public class UpgradeSlot : MonoBehaviour
{
    public string UpgradeName;

    public Button BuyUpgrade;
    public Text Price;
    public Text LevelData;
    public Text CompleteText;//Текст появляется вместо кнопки "Купить", когда достигнут максимальный уровень данного улучшения

    private int current_level;//Текущий уровень улучшения
    private int current_price;//Цена улучшения для текущего уровня
    private int current_money;//Общее количество игровой валюты в распоряжении игрока

    public delegate void ChangedMoney();
    public static event ChangedMoney OnUpgradeChanged;//Событие служит для оповещения об уменьшении количества игровой валюты после покупки

    void Start()
    {
        if(!PlayerPrefs.HasKey(UpgradeName))
        {
            PlayerPrefs.SetInt(UpgradeName, 0);
        }

        ChangedLevel();
        ChangedPrice();
        ChangeMoney();

        UpgradesSystem.OnUpdateSlots += UpdateSlot;
    }

	//Метод служит для отображения прогресса в покупке данного улучшения
    private void ChangedLevel()
    {
        int maxLevel = AllData.Upgrades[UpgradeName];// Класс AllData содержит множество статических методов и значений.
													//Upgrades - словарь, ключ которого - имя улучшения, значение - максимальный уровень улучшения
        current_level = PlayerPrefs.GetInt(UpgradeName);//Файл реестра содержит текущий уровень данного улучшения

        if(current_level >= maxLevel)//Если улучшение прокачено полностью
        {
            current_level = maxLevel;
            BuyUpgrade.gameObject.SetActive(false);
            CompleteText.gameObject.SetActive(true);
        }

        LevelData.text = current_level + " / " + maxLevel;
    }

	//Метод служит для отображения цены на текущий уровень улучшения
    private void ChangedPrice()
    {
        if(BuyUpgrade.IsActive())//Кнопка активна, пока улучшение не прокачено полностью
        {
            string ID = UpgradeName + current_level;
            current_price = AllData.Upgrades[ID];//В том же словаре содержатся цены для каждого уровня конкретного улучшения

            Price.text = current_price.ToString();
        }
    }
	
	//Метод служит для проверки наличия средств у игрока для покупки улучшения
    private void ChangeMoney()
    {
        if (BuyUpgrade.IsActive())
        {
            current_money = PlayerPrefs.GetInt("Money");//Получаем количество доступной игроку игровой валюты

            if (current_money < current_price)
            {
                BuyUpgrade.interactable = false;//Не даём игроку возможности купить улучшение, если недостаточно средств
            }
            else
            {
                BuyUpgrade.interactable = true;
            }
        }
    }
	
	//Метод активируется по нажатию кнопки покупки
    public void Upgrade()
    {
        current_level++;
        current_money = current_money - current_price;

        PlayerPrefs.SetInt(UpgradeName, current_level);//Обновляем в реестре текущий уровень улучшения
        PlayerPrefs.SetInt("Money", current_money);//Обновляем количество доступных игроку средств

        ChangedLevel();
        ChangedPrice();
        ChangeMoney();

        SecuritySystem.SetUpgrades(true);//Класс SecuritySystem отвечает за защиту от изменения данных реестра на устройстве пользователя с root-правами

        OnUpgradeChanged?.Invoke();//Оповещаем систему об изменении доступных средств и изменении поведения других объектов, взаимодействующих с игровой валютой
    }

	//Метод служит для обновления данных, срабатывает по активации события из другого класса
    private void UpdateSlot()
    {
        ChangedLevel();
        ChangedPrice();
        ChangeMoney();
    }

    private void OnDestroy()
    {
        UpgradesSystem.OnUpdateSlots -= UpdateSlot;
    }
}
