using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Town
{
    public class MainScenePlayerController : MonoBehaviour
    {

        private Camera mainCamera;
        private NavMeshAgent playerNavMesh;
        private Animator anim;
        [SerializeField] private float speed = 10f;
        public LayerMask ground;
        public LayerMask obstacle;
        public bool isMovingToSpecificPos;
        public Vector3 targetPos;
        public Vector3 curPos;
        public float stopDistance = 0.5f;
        public LocationController curTarget;

        //ĳ      ִϸ  ̼ǿ 
        //private Animator animator;
        public bool isMove = true;

        void Awake()
        {
            mainCamera = Camera.main;
            playerNavMesh = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            //animator = GetComponent<Animator>();
        }

        void Start()
        {
            curPos = transform.position;
        }
        void Update()
        {
            if (isMove)
            {
                var distance = curPos - transform.position;
                var velocity = (distance).magnitude;
                anim.SetFloat("speed", velocity);
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    var mousePos = mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(mousePos, out hit, Mathf.Infinity, ground))
                    {
                        Debug.Log($"gr Name : {hit.point} {hit.transform.gameObject.name}");
                        curPos = hit.point;
                        playerNavMesh.SetDestination(hit.point);
                        isMovingToSpecificPos = false;
                        playerNavMesh.isStopped = false;
                    }

                    if (Physics.Raycast(mousePos, out hit, Mathf.Infinity, obstacle))
                    {
                        Debug.Log($"obs Name : {hit.point} {hit.transform.gameObject.name}");
                        curTarget = hit.transform.GetComponent<LocationController>();
                        targetPos = curTarget.location.position;
                        targetPos.y = transform.position.y;
                        curPos = targetPos;
                        isMovingToSpecificPos = true;
                        playerNavMesh.isStopped = false;
                        playerNavMesh.SetDestination(targetPos);

                        //playerNavMesh.SetDestination(hit.point);
                    }
                }
                UnitStop();
            }
        }

        public void UnitStop()
		{
			if (!isMovingToSpecificPos) return;
			////Debug.Log("달려");
			if (Vector3.SqrMagnitude(targetPos - transform.position) <= stopDistance)
			{
				isMovingToSpecificPos = false;
				playerNavMesh.isStopped = true;
				curTarget.OpenWindow();
				isMove = false;
				Debug.Log("도착함");
			}
		}



		//void Move()
		//{
		//	if (isMove)
		//	{
		//		var dir = destination - transform.position;
		//		transform.forward = dir;
		//		transform.position += dir.normalized * Time.deltaTime * speed;
		//	}
		//	if (Vector3.Distance(transform.position, destination) <= 0.1f)
		//	{
		//		isMove = false;
		//		//animator.SetBool("isMove", false);
		//	}
		//}
	}

}
