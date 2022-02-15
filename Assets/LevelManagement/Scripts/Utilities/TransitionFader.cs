using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

/// <summary>
/// Implements a transition screen controller, using the base ScreenFader script. Used to transition between scenes
/// and hide potential loading freezes.
/// </summary>
public class TransitionFader : ScreenFader
{
    [SerializeField] float _lifetime = 1f;
    [SerializeField] float _delay = 0.3f;

    public float Delay => _delay;

    protected void Awake()
    {
        _lifetime = Mathf.Clamp(_lifetime, FadeOnDuration + FadeOffDuration + _delay, 10f);
    }

    IEnumerator PlayRoutine()
    {
        SetAlpha(_clearAlpha);
        yield return new WaitForSeconds(_delay);
        
        FadeOn();
        float onTime = _lifetime - FadeOffDuration - _delay;

        yield return new WaitForSeconds(onTime);
        
        FadeOff();
        Destroy(gameObject, FadeOffDuration);
    }

    public void Play()
    {
        StartCoroutine(PlayRoutine());
    }

    public static void PlayTransition(TransitionFader trainsitionPrefab)
    {
        if (trainsitionPrefab == null) return;

        TransitionFader instance = Instantiate(trainsitionPrefab, Vector3.zero, Quaternion.identity);
        instance.Play();
    }
}
