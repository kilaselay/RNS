using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] Image LoadingBar;
    [SerializeField] Text Progress;

    public void ActivatePanel(bool status)
    {
        gameObject.SetActive(status);
    }

    public void Run(float progress)
    {
        progress /= 0.9f;
        LoadingBar.fillAmount = progress;
        Progress.text = string.Format("{0:0}%", progress * 100);
    }
}
