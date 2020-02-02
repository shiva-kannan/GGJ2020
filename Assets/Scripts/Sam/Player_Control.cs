using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{
    public PlayerNumber _playerNumber;

    public float moveSpd;

    private bool isMoving = false;
    private Vector3 forward;
    private Vector3 right;
    private Rigidbody myrgbody;
    private Vector3 faceDirect = Vector3.zero; // A normalized face direction


    // Start is called before the first frame update
    void Start()
    {
        myrgbody = GetComponent<Rigidbody>();

        forward = Camera.main.transform.forward; // Set forward to equal the camera's forward vector
        forward.y = 0; // make sure y is 0
        forward = Vector3.Normalize(forward); // make sure the length of vector is set to a max of 1.0
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward; // set the right-facing vector to be facing right relative to the camera's forward vector

        transform.LookAt(transform.position + forward);
        faceDirect = forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            //Vector3 direction = new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis("VerticalKey")); // setup a direction Vector based on keyboard input. GetAxis returns a value between -1.0 and 1.0. If the A key is pressed, GetAxis(HorizontalKey) will return -1.0. If D is pressed, it will return 1.0
            Vector3 rightMovement = Vector3.zero;
            Vector3 upMovement = Vector3.zero;
            if (this.gameObject.tag == "Player")
            {
                rightMovement = right * Input.GetAxis("Horizontal1");
                upMovement = forward * Input.GetAxis("Vertical1");
            }else
            {
                rightMovement = right * Input.GetAxis("Horizontal2");
                upMovement = forward * Input.GetAxis("Vertical2");
            }

            Vector3 heading = rightMovement + upMovement; // This creates our new direction. By combining our right and forward movements and normalizing them, we create a new vector that points in the appropriate direction with a length no greater than 1.0transform.forward = heading; // Sets forward direction of our game object to whatever direction we're moving intransform.position += rightMovement; // move our transform's position right/left     transform.position += upMovement; // Move our transform's position up/down


            //Vector3 velo = new Vector3(Input.GetAxis("Horizontal"), myrgbody.velocity.y, Input.GetAxis("Vertical"));

            myrgbody.velocity = heading * moveSpd * Time.deltaTime;

            // Update face direct while it's moving
            faceDirect = heading.normalized;

            // Look at the faceDirect
            Vector3 placeToLook = transform.position + faceDirect;
            transform.LookAt(placeToLook);


            if (this.gameObject.tag == "Player")
            {
                if (Mathf.Abs(Input.GetAxis("Horizontal1")) < 0.1f && Mathf.Abs(Input.GetAxis("Vertical1")) < 0.1f)
                {
                    myrgbody.velocity = Vector3.zero;
                    isMoving = false;
                }
            }
            else
            {
                if (Mathf.Abs(Input.GetAxis("Horizontal2")) < 0.1f && Mathf.Abs(Input.GetAxis("Vertical2")) < 0.1f)
                {
                    myrgbody.velocity = Vector3.zero;
                    isMoving = false;
                }
            }

        }
        else
        {
            if (this.gameObject.tag == "Player")
            {
                if (Mathf.Abs(Input.GetAxis("Horizontal1")) >= 0.1f || Mathf.Abs(Input.GetAxis("Vertical1")) >= 0.1f)
                {
                    isMoving = true;
                }
            }
            else
            {
                if (Mathf.Abs(Input.GetAxis("Horizontal2")) >= 0.1f || Mathf.Abs(Input.GetAxis("Vertical2")) >= 0.1f)
                {
                    isMoving = true;
                }
            }

        }

    }

    public Vector3 GetFaceDirect()
    {
        return faceDirect;
    }

}

public enum PlayerNumber
{
    None,
    Player1,
    Player2,
}