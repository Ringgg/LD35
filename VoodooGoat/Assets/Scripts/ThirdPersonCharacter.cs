using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class ThirdPersonCharacter : MonoBehaviour
{
    [SerializeField] float m_MovementSpeed = 5;
    [SerializeField] float m_MovementAcceleration = 5;
	[SerializeField] float m_MovingTurnSpeed = 360;
	[SerializeField] float m_StationaryTurnSpeed = 180;

    //funnyRotation
    [SerializeField]
    Transform dummy;
    public float rotationAngle = 10.0f;
    float rotationStrength = 0.0f;

	Rigidbody m_Rigidbody;
	float m_TurnAmount;
	float m_ForwardAmount;

	void Start()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
	}
        

    void Update()
    {
        rotationStrength = Mathf.Clamp(m_Rigidbody.velocity.magnitude / m_MovementSpeed, 0, 1);
        if (name == "rotationAngle")
            Debug.Log(rotationStrength);
        if (rotationStrength < 0.1f) return;
        dummy.LookAt(transform.position + transform.forward * 100);
        Vector3 axis = Vector3.right + new Vector3(0, Mathf.Sin(Time.time * 10), Mathf.Cos(20*Time.time));
        axis = dummy.InverseTransformDirection(axis);
        dummy.Rotate(axis, rotationAngle * rotationStrength * Mathf.Sin(Time.time * 15));
    }

	public void Move(Vector3 move)
	{
        move.Normalize();
        m_Rigidbody.velocity = Vector3.MoveTowards(m_Rigidbody.velocity, move * m_MovementSpeed, m_MovementAcceleration * Time.fixedDeltaTime);
        
        //rotation
        move = transform.InverseTransformDirection(move);
		m_TurnAmount = Mathf.Atan2(move.x, move.z);
		m_ForwardAmount = move.z;

        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }
}