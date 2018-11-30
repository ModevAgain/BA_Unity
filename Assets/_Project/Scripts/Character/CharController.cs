using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {

    Rigidbody _rigid;
    public float MoveSpeed = 2;

    private Vector3 camOffset;

    [SerializeField]
    private bool _onMobile;

    private void Awake()
    {

#if UNITY_ANDROID || UNITY_IOS

        _onMobile = true;
#endif
    }

    // Use this for initialization
    void Start () {
        _rigid = GetComponent<Rigidbody>();
        camOffset = Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 inputVector = Vector3.negativeInfinity;

        if (_onMobile)
        {
            
        }
        else
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            inputVector = new Vector3(x, 0, y);
        }

        if (inputVector == Vector3.negativeInfinity)
            return;

        Vector3 dir = transform.position + inputVector;

        _rigid.MovePosition(transform.position + inputVector * Time.deltaTime * MoveSpeed);

        

        transform.LookAt(dir);

        Vector3 camPos = camOffset + transform.position;
        camPos.y = camOffset.y;

        Camera.main.transform.position = camPos;

    }
}
