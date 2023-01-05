using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; set;}

    public CinemachineVirtualCamera cam;
    [Tooltip("For Zoom and Rotation")] public float speed;

    [Header("For Zoom Effect")]
    [SerializeField] float Current_focus;
    [SerializeField] float Changed_focus = 17;
    [SerializeField] float Current_Angle = 0;
    [SerializeField] bool RotateRight = false;

    float shakeTimer = 0;

    #region Script Initialization
    private void Awake()
    {
        MakeInstance();

        Current_focus = cam.m_Lens.OrthographicSize;

    }

    void MakeInstance()
    {        
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    #region Working
    private void Update()
    {
        ManageZoom();
        ManageShake();
        ManageRotation();
    }
    void ManageZoom()
    {
        if (Current_focus != Changed_focus)
        {
            Current_focus = Mathf.Lerp(Current_focus, Changed_focus, speed * Time.deltaTime);
            cam.m_Lens.OrthographicSize = Current_focus;
        }
    }
    void ManageShake()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
    void ManageRotation()
    {
        if (Current_Angle != 0)
        {
            Current_Angle = Mathf.Lerp(Current_Angle, 0, speed * Time.deltaTime);
            cam.m_Lens.Dutch = -Current_Angle;
        }
    }
    #endregion

    #region Public Function
    public void Zoom(float f = 10 )
    {
        if(f==17)
        {
            f = 12;
        }
        Changed_focus = f;
    }

    public void Shake( float intensity , float time)
    {
        CinemachineBasicMultiChannelPerlin CVChannelPerlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        CVChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    public void RotateCam(int angle)
    {
        if (RotateRight)
        {
            Current_Angle = angle;
        }
        else
        {
            Current_Angle = -angle;
        }
        RotateRight = !RotateRight;
    }
    #endregion
}
