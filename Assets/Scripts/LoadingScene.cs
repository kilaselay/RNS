using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] int Scene_ID;
    [SerializeField] LoadingPanel Loading;

    void Start()
    {
        StartCoroutine(AsyncLoadScene());
    }

    IEnumerator AsyncLoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(Scene_ID);

        Loading.Run(0);
        Loading.ActivatePanel(true);

        while (!async.isDone)
        {
            Loading.Run(async.progress);

            yield return null;
        }
    }
}
