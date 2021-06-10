using UnityEngine;
using UnityEngine.UI;

public class AuthorizationForm : MonoBehaviour
{
    [SerializeField] InputField Login;
    [SerializeField] InputField Password;

    [SerializeField] Text Error;

    [SerializeField] Text Username;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("User"))
        {
            CheckingUser();
            gameObject.SetActive(false);
        }
    }

    public void Registration()
    {
        if (Login.text != string.Empty && Password.text != string.Empty)
        {
            UserData user = new UserData
            {
                login = Login.text,
                password = Password.text
            };

            string json = JsonUtility.ToJson(user);

            //Место для отправки запроса на сервер

            PlayerPrefs.SetString("User", json);

            Username.gameObject.SetActive(true);
            Username.text = user.login;

            gameObject.SetActive(false);
        }
        else
            Error.gameObject.SetActive(true);
    }

    void CheckingUser()
    {
        string json = PlayerPrefs.GetString("User");

        UserData user = JsonUtility.FromJson<UserData>(json);

        //Место для отправки запроса на сервер

        Username.text = user.login;
        Username.gameObject.SetActive(true);
    }

    [System.Serializable]
    struct UserData
    {
        public string login;
        public string password;
    }
}
