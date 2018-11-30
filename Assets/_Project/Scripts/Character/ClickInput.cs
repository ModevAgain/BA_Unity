using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class ClickInput : MonoBehaviour {

    public const float FIX_Y = -0.027f;

    public float speed = 50;

    [SerializeField]
    private Vector3 Direction;
    [SerializeField]
    private Vector3 target;
    private Rigidbody _rigid;
    public float DesiredDistanceToTarget;
    public float CurrentDistanceToTarget;

    private bool shouldMove;

    private NavMeshAgent _agent;

	// Use this for initialization
	void Start () {
        _rigid = GetComponent<Rigidbody>();
        target = Vector3.negativeInfinity;
        _agent = GetComponent<NavMeshAgent>();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindObjectOfType<NavMeshSurface>().BuildNavMesh();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 inputPos = Input.mousePosition;



            Ray ray = Camera.main.ScreenPointToRay(inputPos);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, 1 << 10))
            {
                target = hit.point;
                Direction = (target - transform.position).normalized;
            }
            //target.y = FIX_Y;

            Direction = (target - transform.position).normalized;
            //transform.LookAt(target);
            //transform.DOLookAt(target, 0.3f);

            _agent.SetDestination(target);

            shouldMove = true;

        }

        if (target == Vector3.negativeInfinity || !shouldMove)
            return;


        CurrentDistanceToTarget = Vector3.Distance(transform.position, target);

        if (CurrentDistanceToTarget < DesiredDistanceToTarget)
        {
            target = Vector3.negativeInfinity;
            shouldMove = false;
            return;
        }
        else
        {
            //Vector3 tempTargetPos = Vector3.Lerp(transform.position, target, 1 / (speed * CurrentDistanceToTarget));
            
           // _rigid.MovePosition(tempTargetPos);
        }

    }


}
