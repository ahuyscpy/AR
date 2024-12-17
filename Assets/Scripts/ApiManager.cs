using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{

    public const string apiUrl = "http://54.206.201.163:8080/v1/image";
    public const string Checkupdate_Endpoint = "/check-update";
    public const string ConfirmGift_Endpoint = "update";
    private string UserToken = "";
    public bool IsPostCompleted { get; private set; }
    public bool IsGetCompleted { get; private set; }
    private ImageConfirmStatus data = null;
    private void Awake()
    {
        UserToken = PlayerPrefs.GetString("token");
    }
    public void SendCheckIsAvailableData(ImageUpdate data, string url)
    {
        StartCoroutine(PostCheckUpdateCoroutine(data, url));
    }
    public void SendConfirmReceivedData(ImageUpdate data, string url)
    {
        StartCoroutine(PostCheckUpdateCoroutine(data, url));
    }
    #region Draft
    //public void GetData(string apiUrl)
    //{
    //    StartCoroutine(GetRequest(apiUrl));
    //}
    #endregion

    private IEnumerator PostCheckUpdateCoroutine(ImageUpdate stateEntity, string url = "")
    {
        var data = JsonUtility.ToJson(stateEntity);
        using (UnityWebRequest www = UnityWebRequest.Post(url, data, "application/json"))
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
            }
        }
    }
    #region Draft
    //IEnumerator GetRequest(string url)
    //{
    //    using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
    //    {
    //        yield return webRequest.SendWebRequest();

    //        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
    //        {
    //            Debug.LogError(webRequest.error);
    //        }
    //        else
    //        {
    //            Debug.Log(webRequest.downloadHandler.text);
    //        }
    //        if (webRequest.result == UnityWebRequest.Result.Success)
    //        {
    //            GetIamgeAvailableStatus(true);
    //        }
    //        else
    //            GetIamgeAvailableStatus(false);
    //    }
    //}
    //private bool PostStatus(bool IsCompleted)
    //{
    //    return IsPostCompleted = IsCompleted;
    //}
    //private bool GetIamgeAvailableStatus(bool IsCompleted)
    //{
    //    return IsGetCompleted = IsCompleted;
    //}
    #endregion
    private ImageConfirmStatus SetDataImage(string data)
    {
        return this.data = JsonUtility.FromJson<ImageConfirmStatus>(data) ?? new ImageConfirmStatus();
    }
    public ImageConfirmStatus GetDataImage()
    {
        return data;
    }
}
