using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Скрипт с классом находится на пустышке с коллайдером-триггером определённого размера.
//Отвечает за контроль границ действия объектов "Вражеская звезда" и "Астероид"
public class KillZone : MonoBehaviour
{
    public Transform Player;
    public GameController gameController;

    void FixedUpdate()
    {
        transform.position = Player.transform.position;//Перемещаем зону действия объектов за игроком
    }

    private void OnTriggerExit(Collider other)//Реагируем на выход из коллайдера определённых объектов
    {
        if(other.tag == "DangerStar")
        {
            other.gameObject.SetActive(false);//В игре реализован пул объектов.
												//Вместо использования ресурсозатратных методов Destroy и Instantiate - применяем более лёгкий SetActive
            gameController.ReactivatoinStars();//Данный метод выдаёт новую позицию для звезды и активирует её
        }

		//Всё аналогично действию на тэг "Вражеской звезды", только для астероидов
        if (other.tag == "Asteroid")
        {
            other.gameObject.SetActive(false);
            gameController.ReactivationAsteroids();
        }
    }
}
