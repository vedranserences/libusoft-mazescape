using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterControl : MonoBehaviour
{
    public float m_Speed = 12f;                 // How fast the tank moves forward and back.
    public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.

    private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
    private string m_TurnAxisName;              // The name of the input axis for turning.
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.

    private GameObject currentFloor;

    bool m_IsGrounded;
	float m_OrigGroundCheckDistance=0.1f;
    Vector3 m_GroundNormal;
    private bool m_Jump;
    public float m_JumpPower=10;
    public float m_GroundCheckDistance;
    [Range(1f,4f)]
    public float m_GravityMultiplier=2f;

    private Animator mummyAnimation;

    public int numberOfTags;


    private void Awake ()
    {
        m_Rigidbody = GetComponent<Rigidbody> ();
        mummyAnimation = GetComponent<Animator>();
    }


    private void OnEnable ()
    {
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;

        // Also reset the input values.
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;

    }


    private void OnDisable ()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;
    }


    private void Start ()
    {
        // The axes names are based on player number.
        m_MovementAxisName = "Vertical";
        m_TurnAxisName = "Horizontal";

    }


    private void Update ()
    {
        // Store the value of both input axes.
        m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis (m_TurnAxisName);
        if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        if(CrossPlatformInputManager.GetButtonDown("Use")&& numberOfTags>0){
            GameObject[] floors=GameObject.FindGameObjectsWithTag("Floor");
            foreach(GameObject floor in floors){
                Rect rect=new Rect(floor.transform.position.x-5,floor.transform.position.z-5,10,10);
                if(rect.Contains(new Vector2(transform.position.x,transform.position.z))){
                    currentFloor=floor;
                }
            }
            currentFloor.GetComponent<Renderer>().material.color=Color.magenta;
            numberOfTags--;
        }

    }

    private void FixedUpdate ()
    {
        // Adjust the rigidbodies position and orientation in FixedUpdate.
        Move ();
        Turn ();
        m_Jump=false;
    }


    private void Move ()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        Vector3 noMovement = new Vector3(0,0,0);
        CheckGroundStatus();
        if(m_Jump && m_IsGrounded){
            m_Rigidbody.velocity=new Vector3(m_Rigidbody.velocity.x,m_JumpPower,m_Rigidbody.velocity.z);
            m_IsGrounded=false;
            //m_GroundCheckDistance = 5f;
        }
        else{
            Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
			m_Rigidbody.AddForce(extraGravityForce);

			//m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
        }
        if(movement.Equals(noMovement)){
            mummyAnimation.SetBool("isRun", false);
        }else{
            mummyAnimation.SetBool("isRun", true);
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }
        
        
    }


    private void Turn ()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        //Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
        }
    }
    

}