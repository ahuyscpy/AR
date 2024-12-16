using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{

    public const string apiUrl = "http://54.206.201.163:8080/v1/image";
    public const string Checkupdate_Endpoint = "/check-update";
    public const string ConfirmGift_Endpoint = "update";
    private string UserToken = "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJUaGFuaE5IMzMiLCJpYXQiOjE3MzQyNzQ5ODksImV4cCI6MTczNDM2MTM4OX0.5q-LKegag9XzjOBPRd1YnIGoSdnEcxwZyyHkHlW4uKBWQSvqlYCj2LZT8x0LKtZko6F1vRVwLEjsW4_6FLIgUw";
    public bool IsPostCompleted { get; private set; }
    public bool IsGetCompleted { get; private set; }
    private ImageUpdateStatus data = null;
    public void SendData(ImageUpdate data, string url)
    {
        StartCoroutine(PostCheckUpdateCoroutine(data, url));
    }
    public void GetData(string apiUrl)
    {
        StartCoroutine(GetRequest(apiUrl));
    }

    private IEnumerator PostCheckUpdateCoroutine(ImageUpdate stateEntity, string url = "")
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, "{ \"id\": 1 }", "application/json"))
        {
            www.SetRequestHeader("Authorization", "Bearer "+ UserToken);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {www.error} - Response: {www.downloadHandler.text}");
            }
            else
            {
                SetDataImage(www.downloadHandler.text);
                Debug.Log("Form upload complete!");
            }
        }
    }
    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
            }
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                GetIamgeAvailableStatus(true);
            }
            else
                GetIamgeAvailableStatus(false);
        }
    }
    private bool PostStatus(bool IsCompleted)
    {
        return IsPostCompleted = IsCompleted;
    }
    private bool GetIamgeAvailableStatus(bool IsCompleted)
    {
        return IsGetCompleted = IsCompleted;
    }
    private ImageUpdateStatus SetDataImage(string data)
    {
        return this.data = JsonUtility.FromJson<ImageUpdateStatus>(data) ?? new ImageUpdateStatus();
    }
    public ImageUpdateStatus GetDataImage()
    {
        return data;
    }
}
