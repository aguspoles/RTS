using UnityEngine;
using System.Collections;
using PathFinding;

public class Unit : MonoBehaviour {

	const float minPathUpdateTime = .2f;
	const float pathUpdateMoveThreshold = .5f;
    public static Vector3 InvalidVector3 = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);

    public float speed = 20;
    public float turnSpeed = 3;
	public float turnDst = 5;
	public float stoppingDst = 10;
    public bool selected = false;
    public bool followingPath = false;
    public bool targetReached = false;

	Path path;
    FlockAgent agentComponent;
    public Vector3 target;
    private Vector3 previousTarget;

    Animator animatorController;

    void Start()
	{
		agentComponent = GetComponent<FlockAgent>();
        animatorController = GetComponent<Animator>();
        target = InvalidVector3;
    }

    void Update()
    {
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animatorController.SetTrigger("Attack");
        }
        else animatorController.ResetTrigger("Attack");
    }

    public void MoveToPosition(Vector3 target) {
        if (previousTarget == target)
            return;
        this.previousTarget = target;
        this.target = target;
        targetReached = false;
        StartCoroutine (UpdatePath (target));
	}

	public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) {
		if (pathSuccessful) {
			path = new Path(waypoints, transform.position, turnDst, stoppingDst);

			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator UpdatePath(Vector3 target) {

		if (Time.timeSinceLevelLoad < .3f) {
			yield return new WaitForSeconds (.3f);
		}
		PathRequestManager.RequestPath (new PathRequest(transform.position, target, OnPathFound));

		float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
		Vector3 targetPosOld = target;

		while (true) {
			yield return new WaitForSeconds (minPathUpdateTime);
			//print (((target - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
			if ((target - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
				PathRequestManager.RequestPath (new PathRequest(transform.position, target, OnPathFound));
				targetPosOld = target;
			}
		}
	}

	IEnumerator FollowPath() {
        followingPath = true;
        targetReached = false;
		int pathIndex = 0;
		transform.LookAt (path.lookPoints [0]);

		float speedPercent = 1;

		while (followingPath) {
			Vector2 pos2D = new Vector2 (transform.position.x, transform.position.z);
			while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D)) {
				if (pathIndex == path.finishLineIndex) {
					followingPath = false;
					break;
				} else {
					pathIndex++;
				}
			}

			if (followingPath) {

				if (pathIndex >= path.slowDownIndex && stoppingDst > 0) {
					speedPercent = Mathf.Clamp01 (path.turnBoundaries [path.finishLineIndex].DistanceFromPoint (pos2D) / stoppingDst);
					if (speedPercent < 0.01f) {
						followingPath = false;
                        targetReached = true;
                    }
				}
				
				Vector3 lookRotationDirection = (path.lookPoints [pathIndex] - transform.position);
				Quaternion targetRotation = Quaternion.LookRotation(lookRotationDirection);
				transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
				transform.Translate (Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
            }

			yield return null;
		}
        target = InvalidVector3;
    }

	public void OnDrawGizmos() {
		if (path != null) {
			path.DrawWithGizmos ();
		}
	}

    public void ResetPath()
    {

    }
}
