using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject GiftResult;
    [SerializeField]
    void Start()
    {
        this.GetComponentInParent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
