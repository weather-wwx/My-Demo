using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAttackSound : MonoBehaviour
{
    public AudioClip attackClip;

    private void OnEnable()
    {
        MusicController.Instance.PlaySFX(attackClip);
    }
}
