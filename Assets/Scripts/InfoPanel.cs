using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] Text[] Data = new Text[4];

    public void ActivatePanel(string[] data)
    {
        Data[0].text = "����������� ����: " + data[0];
        Data[1].text = "������ ������ ������: " + data[1];
        Data[2].text = "�������� ������: " + data[2];
        Data[3].text = "������ ����: " + data[3];

        gameObject.SetActive(true);
    }
}
