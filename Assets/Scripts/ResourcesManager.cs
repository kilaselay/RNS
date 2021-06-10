using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour
{
    public Text Title;
    public RawImage[] Cells = new RawImage[4];

    [SerializeField] LoadingPanel Loading;

    [SerializeField] InfoPanel infoPanel;

    [SerializeField] GameObject ErrorPanel;

    private void Start()
    {
        //StartCoroutine(SendRequest());
    }

    public void LoadingImages(int rover_number)
    {
        string rover_name = "";
        int[] sol = new int[4];
        string camera = "";

        if (rover_number == 0)
        {
            rover_name = "curiosity";

            sol[0] = 100;
            sol[1] = 400;
            sol[2] = 700;
            sol[3] = 1000;

            camera = "fhaz";
            
        }
        else if (rover_number == 1)
        {
            rover_name = "opportunity";

            sol[0] = 10;
            sol[1] = 100;
            sol[2] = 150;
            sol[3] = 1000;

            camera = "pancam";
        }
        else if(rover_number == 2)
        {
            rover_name = "spirit";

            sol[0] = 15;
            sol[1] = 50;
            sol[2] = 250;
            sol[3] = 1000;

            camera = "navcam";
        }

        StartCoroutine(StartLoading(rover_name, sol, camera));
    }

    IEnumerator StartLoading(string rover_name, int[] sol, string camera)
    {
        Title.text = "Фотографии с марсохода " + rover_name;
        float progress = 0;

        Loading.Run(progress);
        Loading.ActivatePanel(true);

        for(int i=0; i<4; i++)
        {
            yield return StartCoroutine(SendRequest(rover_name, sol[i], camera, i));
            progress += 0.25f;
            Loading.Run(progress);
        }

        Loading.ActivatePanel(false);
    }

    IEnumerator SendRequest(string rover_name, int sol, string camera, int i)
    {
        string url = "https://api.nasa.gov/mars-photos/api/v1/rovers/" + rover_name + "/photos?sol=" + sol + "&camera=" + camera + "&api_key=DEMO_KEY";

        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.ConnectionError)
        {
            PlayerPrefs.SetString("Photo" + i, request.downloadHandler.text);

            Response response = JsonUtility.FromJson<Response>(request.downloadHandler.text);

            url = response.photos[0].img_src;

            var image_request = UnityWebRequestTexture.GetTexture(url);

            yield return image_request.SendWebRequest();

            Texture2D image = DownloadHandlerTexture.GetContent(image_request);

            Cells[i].texture = image;
        }
        else
        {
            ErrorPanel.SetActive(true);

            yield return null;
        }
    }

    public void ViewingInfo(int cell_number)
    {
        if(PlayerPrefs.HasKey("Photo" + cell_number))
        {
            string json = PlayerPrefs.GetString("Photo" + cell_number);

            Response response = JsonUtility.FromJson<Response>(json);

            string[] data = new string[4];

            data[0] = response.photos[0].sol.ToString();
            data[1] = response.photos[0].rover.status;
            data[2] = response.photos[0].camera.name;
            data[3] = response.photos[0].earth_date;

            infoPanel.ActivatePanel(data);
        }
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    [System.Serializable]
    public struct Response
    {
        public Photo[] photos;
    }

    [System.Serializable]
    public struct Photo
    {
        public int id;
        public int sol;
        public Camera camera;
        public string img_src;
        public string earth_date;
        public Rover rover;
    }

    [System.Serializable]
    public struct Camera
    {
        public int id;
        public string name;
        public int rover_id;
        public string full_name;
    }

    [System.Serializable]
    public struct Rover
    {
        public int id;
        public string name;
        public string landing_date;
        public string launch_date;
        public string status;
    }
}
