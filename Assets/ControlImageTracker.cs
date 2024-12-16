using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zappar;

public class ControlImageTracker : MonoBehaviour
{
    private List<ImageUpdate> Data = null;
    private ImageUpdateStatus DataResponse = null;
    // Start is called before the first frame update
    public GameObject g_timeout;
    public TextMeshProUGUI _TimeOutText;
    public ParticleSystem _Effect;
    public Image Background;
    public ApiManager ApiManager;
    // time out
    public float timeLimit = 180f; // Thời gian đếm ngược (3 phút = 180 giây)
    private float timer; // Biến lưu thời gian

    // Backgground Image popup
    // Backfground khi nhận được babytree
    public Sprite GiftCompleteBackground;
    // Background khi trên sever không còn quà
    public Sprite GiftFail;
    // confirm neu da nhan qua
    public Sprite GiftConfirm;
    // qua han thoi gian de nhan qua
    public Sprite GiftTimeout;

    public ZapparImageTrackingTarget _TrackingTarget;
    private ImageUpdate _ImageTrackerInfo;
    public Animation Loading;
    public Button BackButton;
    private void Awake()
    {
        // fake data
        ResultImageTracker();
    }
    private void OnEnable()
    {
        //Loading.Play();
    }
    void Start()
    {
        _TimeOutText = g_timeout.GetComponent<TextMeshProUGUI>();
        timer = timeLimit; // Khởi tạo thời gian
        BackButton.onClick.AddListener(() => OnClickBackButton());
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            if (ZapparCamera.Instance.AnchorOrigin.enabled)
            {
                ZapparCamera.Instance.AnchorOrigin.enabled = false;
                if (DataResponse != null && DataResponse.status == 1)
                {
                }
                this.Background.sprite = this.GiftCompleteBackground;
            }
            //if (Loading.isPlaying)
            //{
            //    return;
            //}
            if (timer > 0)
            {
                timer -= Time.deltaTime; // Giảm thời gian
                UpdateTimerText();
            }
            else
            {
                timer = 0;
            }

            //if (!_Effect.isPlaying)
            //    _Effect.Play();
            if (ApiManager.GetDataImage() != null)
            {
                if (DataResponse != null) return;
                DataResponse = ApiManager.GetDataImage();
                string jsonData = JsonUtility.ToJson(DataResponse);
                Debug.Log("Form: !" + jsonData);
                //Debug.Log("Form: !" + DataResponse.id);
                Debug.Log(ZapparCamera.Instance.TrackerAtOrigin.name);
            }
        }
        else
        {
            //if (_Effect.isPlaying || _Effect.isPaused)
            //    _Effect.Stop();
        }
    }
    void UpdateTimerText()
    {
        // Chuyển đổi thời gian còn lại thành phút và giây
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        _TimeOutText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds); // Cập nhật text
    }
    void ResultImageTracker()
    {
        Data = Data ?? new List<ImageUpdate>()
        {
            new ImageUpdate()
            {

                id = 1,
                updateFlag = false
            },
            //new ImageUpdate()
            //{
            //    UserToken = "HuyMV2",
            //    ImageId = "2",
            //    IsAvailable = false
            //}
        };
    }
    void OnDisable()
    {
        timer = timeLimit; // Khởi tạo thời gian
    }
    public void GetZapperCamera(ZapparImageTrackingTarget camera)
    {
        var a = camera;
        _ImageTrackerInfo = new ImageUpdate()
        {
            id = Convert.ToInt32(camera.Target.Split(".")[0]),
            updateFlag = true
        };
        Debug.Log(_ImageTrackerInfo);
        // Check if image available
        ApiManager.gameObject.SetActive(true);
        string url = ApiManager.apiUrl + ApiManager.Checkupdate_Endpoint;
        ApiManager.SendData(_ImageTrackerInfo, url);
        //if (ApiManager.IsPostCompleted)
        //ApiManager.GetRequest(url, Data[0]);

    }
    public void ShowPopupConfirm()
    {

    }
    public void OnClickBackButton()
    {
        this.gameObject.SetActive(false);
        ZapparCamera.Instance.AnchorOrigin.enabled = true;
        //timer = 0;
    }
}
