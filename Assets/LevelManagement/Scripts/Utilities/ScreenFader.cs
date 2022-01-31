using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] protected float _solidAlpha = 1f;
    [SerializeField] protected float _clearAlpha = 0f;
    [SerializeField] float _fadeOnDuration = 2f;
    [SerializeField] float _fadeOffDuration = 2f;
    [SerializeField] MaskableGraphic[] _graphicsToFade;
    
    public float FadeOnDuration => _fadeOnDuration;
    public float FadeOffDuration => _fadeOffDuration;

    void Start()
    {
        // FadeOff();
    }

    protected void SetAlpha(float alpha)
    {
        foreach (MaskableGraphic graphic in _graphicsToFade)
        {
            if (graphic != null)
            {
                graphic.canvasRenderer.SetAlpha(alpha);
            }
        }
    }

    void Fade(float targetAlpha, float duration)
    {
        foreach (MaskableGraphic graphic in _graphicsToFade)
        {
            graphic.CrossFadeAlpha(targetAlpha, duration, true);
        }
    }

    public void FadeOff()
    {
        SetAlpha(_solidAlpha);
        Fade(_clearAlpha, _fadeOffDuration);
    }
    
    public void FadeOn()
    {
        SetAlpha(_clearAlpha);
        Fade(_solidAlpha, _fadeOnDuration);
    }
}
