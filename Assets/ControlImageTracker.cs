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
    private ImageConfirmStatus DataResponse = null;
    // Start is called before the first frame update
    public GameObject g_timeout;
    public TextMeshProUGUI _TimeOutText;
    public ParticleSystem _Effect;
    public Image Background;
    public ApiManager ApiManager;
    // time out
    public float timeLimit = 5f; // Thời gian đếm ngược (3 phút = 180 giây)
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
    public Button ConfirmButton;

    string _ChecupdateApiUrl = "";
    string _ConfirmGiftApiUrl = "";


    // state received api data 1 time
    bool _IsReceivedData = false;
    private void Awake()
    {
        // fake data
        ResultImageTracker();
    }
    private void OnEnable()
    {
        //Loading.Play();

        ApiManager.gameObject.SetActive(true);
        Debug.Log("OnEnable image tracker");
        _ChecupdateApiUrl = ApiManager.apiUrl + ApiManager.Checkupdate_Endpoint;

        string token = PlayerPrefs.GetString("token", "default_value");
        if (token != "default_value")
        {
            // Sử dụng token
            Debug.Log("Token: " + token);
        }
        else
        {
            Debug.Log("Không tìm thấy token.");
        }
        BackButton.gameObject.SetActive(true);


    }
    void Start()
    {
        _TimeOutText = g_timeout.GetComponent<TextMeshProUGUI>();
        timer = timeLimit; // Khởi tạo thời gian
        BackButton.onClick.AddListener(() => OnClickBackButton());
        ConfirmButton.onClick.AddListener(() => OnClickConfirmButton());

    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            if (ZapparCamera.Instance.AnchorOrigin.enabled)
            {
                ZapparCamera.Instance.AnchorOrigin.enabled = false;
                //if (DataResponse != null && DataResponse.status == 1)
                //{
                //}
                //this.Background.sprite = this.GiftCompleteBackground;
            }
            //if (Loading.isPlaying)
            //{
            //    return;
            //}
            //if (!_Effect.isPlaying)
            //    _Effect.Play();
            //if (ApiManager.GetDataImage() != null)
            if (DataResponse != null && !_IsReceivedData)
            {
                int i = DataResponse.content;
                if (i == 0)
                {
                    OnReceiveApi(i);
                    return;
                }
                OnReceiveApi(i);
                DataResponse = ApiManager.GetDataImage();
                string jsonData = JsonUtility.ToJson(DataResponse);
                _IsReceivedData = true;
            }
            if (timer > 0 && _IsReceivedData)
            {
                _TimeOutText.gameObject.SetActive(true);
                timer -= Time.deltaTime; // Giảm thời gian
                UpdateTimerText();
                if (timer <= 0)
                {
                    OnEndingScanImage();
                }
            }
            else
            {
                timer = 0;
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
             new ImageUpdate()
            {
                id = 2,
                updateFlag = true
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
        DataResponse = null;
        BackButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(false);
        _IsReceivedData = false;
    }
    public void GetZapperCamera(ZapparImageTrackingTarget camera)
    {
        var a = camera;
        _ImageTrackerInfo = new ImageUpdate()
        {
            id = Convert.ToInt32(camera.Target.Split(".")[0]),
            updateFlag = true
        };
        FakeApi(_ImageTrackerInfo.id);

    }
    public void ShowPopupConfirm()
    {

    }
    public void OnClickBackButton()
    {
        OnEndingScanImage();
        //timer = 0;
    }
    public void OnClickConfirmButton()
    {
        _ConfirmGiftApiUrl = ApiManager.apiUrl + ApiManager.ConfirmGift_Endpoint;
        ApiManager.SendConfirmReceivedData(_ImageTrackerInfo, _ConfirmGiftApiUrl);
    }
    void FakeApi(int i = 1)
    {
        switch (i)
        {
            case 1:
                DataResponse = new ImageConfirmStatus()
                {
                    status = 1,
                    content = _ImageTrackerInfo.updateFlag == true ? 1 : 0
                };
                break;
            case 8:
                DataResponse = new ImageConfirmStatus()
                {
                    status = 1,
                    content = 0
                };
                break;
            default:
                Debug.Log("wrongg rs");
                break;
        }

    }
    void OnReceiveApi(int i)
    {
        switch (i)
        {
            case 0:
                ConfirmButton.gameObject.SetActive(false);
                this.Background.sprite = this.GiftFail;
                _TimeOutText.gameObject.SetActive(false);
                break;
            case 1:
                ConfirmButton.gameObject.SetActive(true);
                _TimeOutText.gameObject.SetActive(false);
                this.Background.sprite = this.GiftCompleteBackground;
                break;
        }
    }
    void OnEndingScanImage()
    {
        this.gameObject.SetActive(false);
        ZapparCamera.Instance.AnchorOrigin.enabled = true;
    }
}
