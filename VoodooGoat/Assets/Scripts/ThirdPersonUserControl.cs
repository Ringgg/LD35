using System;
using UnityEngine;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private Transform m_Cam;

    private void Start()
    {
        m_Character = GetComponent<ThirdPersonCharacter>();
        m_Cam = Camera.main.transform;
    }
        
    private void FixedUpdate()
    {
        //m_Move =
        //        Input.GetAxis("Vertical") * Vector3.forward
        //    + Input.GetAxis("Horizontal") * Vector3.right;

        m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        m_Move = Input.GetAxis("Vertical") * m_CamForward + Input.GetAxis("Horizontal") * m_Cam.right;

        m_Character.Move(m_Move);
    }
}
