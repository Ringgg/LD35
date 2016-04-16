using System;
using UnityEngine;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
        
    private void Start()
    {
        m_Character = GetComponent<ThirdPersonCharacter>();
    }
        
    private void FixedUpdate()
    {
        m_Move =
                Input.GetAxis("Vertical") * Vector3.forward
            + Input.GetAxis("Horizontal") * Vector3.right;
            
        m_Character.Move(m_Move);
    }
}
