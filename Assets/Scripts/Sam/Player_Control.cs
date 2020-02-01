using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{
    public float moveSpd;
    private Rigidbody myrgbody;


    // Start is called before the first frame update
    void Start()
    {
        myrgbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velo = Vector3.zero;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            velo = new Vector3(1, velo.y, velo.z);
        }else if (Input.GetKey(KeyCode.LeftArrow))
        {
            velo = new Vector3(-1, velo.y, velo.z);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            velo = new Vector3(velo.x, velo.y, 1);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            velo = new Vector3(velo.x, velo.y, -1);
        }

        myrgbody.velocity = velo * moveSpd * Time.deltaTime;
    }

    // ------------- Remove after you implement -------------------
    //public void ShitPoop()
    //{
    //    TileMap.Instance.GetTileUnderPoint(transform.position);
    //}
}
