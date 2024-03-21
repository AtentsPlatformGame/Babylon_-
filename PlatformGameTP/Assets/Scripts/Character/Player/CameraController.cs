using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform[] cameraPos;
    [SerializeField] Transform orgPos;
    [SerializeField] CinemachineVirtualCamera playerCam;
    float keyDownTime = 0.0f;
    bool isCamera3D = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isCamera3D)SetCamera3D();
        MoveCameraFocus();
    }

    void MoveCameraFocus()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerCam.Follow = cameraPos[0];
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            playerCam.Follow = orgPos;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerCam.Follow = cameraPos[1];
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            playerCam.Follow = orgPos;
        }

        if (Input.GetKey(KeyCode.S))
        {
            keyDownTime += Time.deltaTime;

            if(keyDownTime >= 1.0f)
            {
                playerCam.Follow = cameraPos[2];
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            keyDownTime = 0.0f;
            playerCam.Follow = orgPos;
        }
    }

    public void SetTrackOX(int _dir)
    {
        Debug.Log(playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset);
        playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(_dir * 5.0f, 4.0f, 0f);
    }

    // �÷��̸�尡 2D�� �� ī�޶� ��ġ ����
    public void SetCamera2D()
    {
        // localRoation x = 0, y = 270, z = 0;
        Vector3 rot2D = new Vector3(0.0f, 270.0f, 0.0f);
        playerCam.transform.localEulerAngles = rot2D;

    }

    // �÷��� ��尡 3D �� �� ī�޶� ��ġ ���� >> ���� ���� ���� ����, ���콺�� �����ӿ� ���� ī�޶� ���� �Ÿ����� ������� �������
    public void SetCamera3D() // ���⼱ Lerp
    {
        // localRoation x = 30, y = 300, z = 0;
        Vector3 originCamRot = playerCam.transform.localEulerAngles;
        Vector3 rot3D = new Vector3(30.0f, 300.0f, 0.0f);

        Vector3 targetRot = Vector3.Lerp(originCamRot, rot3D, Time.deltaTime * 10.0f);

        playerCam.transform.localEulerAngles = rot3D;
    }

    // �÷��̸�尡 ž ���� �� ī�޶� ��ġ ����
    public void SetCameraTopView()
    {
        // localRoation x = 90, y = 0, z = 0;
        Vector3 rotTopView = new Vector3(90.0f, 0.0f, 0.0f);
        playerCam.transform.localEulerAngles = rotTopView;

    }

    public void Camera3DToggle(bool _is3D)
    {
        isCamera3D = _is3D;
    }
}
