using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    private static CameraControl instance;
    public static CameraControl Instance => instance;

    private CinemachineVirtualCamera virtualCamera;
    public CinemachineImpulseSource impulseSource;
    public VoidEventSo cameraShakeEvent;
    public GameObject playerPrefab;
    public Transform player;

    private void Awake()
    {
        instance = this;
        virtualCamera =GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
    }

    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
    }

    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    public void SetLookAt(Vector3 target, Vector3 scale)
    {
        player = Instantiate(playerPrefab, target, Quaternion.identity).GetComponent<Transform>();
        player.localScale = scale;
        virtualCamera.Follow = player;
        virtualCamera.LookAt = player;
    }
}
