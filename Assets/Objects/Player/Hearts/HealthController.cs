using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controla o Estado e Sprite de Hearts no Health System
/// </summary>
public class HealthController : MonoBehaviour
{
    /// <summary>
    /// Se o Heart está preenchido
    /// </summary>
    public bool Full = true;

    [SerializeField] private Sprite _emptySprite;
    [SerializeField] private Sprite _fullSprite;

    private Image _image;

    void Awake()
    {
        _image = GetComponent<Image>();
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Muda o estado do Heart
    /// </summary>
    /// <param name="value">Novo estado: false (vazio), ou true (preenchido)</param>
    public void ChangeHeart(bool value)
    {
        Full = value;
        _image.sprite = (Full == true) ? _fullSprite : _emptySprite;
    }
}
