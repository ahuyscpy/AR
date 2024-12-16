using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zappar;

public class TimeOutController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI _TimeOutText;
    // time out
    public float timeLimit = 180f; // Thời gian đếm ngược (3 phút = 180 giây)

    private float timer; // Biến lưu thời gian
    void Start()
    {
        timer = timeLimit; // Khởi tạo thời gian
        UpdateTimerText();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime; // Giảm thời gian
            UpdateTimerText();
        }
        else
        {
            timer = 0; // Đặt lại timer khi hết thời gian
            // Thực hiện hành động khi thời gian kết thúc (nếu cần)
            Debug.Log("Thời gian đã hết!");
        }
    }
    void UpdateTimerText()
    {
        // Chuyển đổi thời gian còn lại thành phút và giây
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        _TimeOutText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Cập nhật text
    }
}
