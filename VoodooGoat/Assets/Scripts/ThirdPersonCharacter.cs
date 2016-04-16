using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class ThirdPersonCharacter : MonoBehaviour
{
    [SerializeField] float m_MovementSpeed = 5;
    [SerializeField] float m_MovementAcceleration = 5;
	[SerializeField] float m_MovingTurnSpeed = 360;
	[SerializeField] float m_StationaryTurnSpeed = 180;

	Rigidbody m_Rigidbody;
	float m_TurnAmount;
	float m_ForwardAmount;

	void Start()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
	}
        
	public void Move(Vector3 move)
	{
        //movement
        if (move.magnitude > 1f) move.Normalize();
        m_Rigidbody.velocity = Vector3.MoveTowards(m_Rigidbody.velocity, move * m_MovementSpeed, m_MovementAcceleration * Time.fixedDeltaTime);

        Debug.Log(m_Rigidbody.velocity);
        //rotation
        move = transform.InverseTransformDirection(move);
		m_TurnAmount = Mathf.Atan2(move.x, move.z);
		m_ForwardAmount = move.z;

        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }
}