using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceButtonLabel : MonoBehaviour
{
    [SerializeField] private string device;
    [SerializeField] private string button;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = InputButtonMapper.GetLabelFor(button, device);
	}
}
