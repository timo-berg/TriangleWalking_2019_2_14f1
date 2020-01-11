using UnityEngine;


public class Follow : MonoBehaviour
{
	#region fields

		public Transform target;
		public float positionSmooth = 0.02f;

		Vector3 vrOffset = new Vector3(0, 0, 1.2f);
		Vector3 desktopOffset = new Vector3(0, 0.5f, 2f);

	#endregion



	public void AppearAtTargetPos()
	{
		transform.position = target.TransformPoint(ExperimentManager.Instance.isVR ? vrOffset : desktopOffset);
		transform.rotation = target.rotation;
	}




	void LateUpdate()
	{
		Vector3 targetPos = target.TransformPoint(ExperimentManager.Instance.isVR ? vrOffset : desktopOffset);
		Quaternion targetQ = target.rotation;

		if ( ExperimentManager.Instance.isVR )
		{
			transform.position = Vector3.Lerp(transform.position, targetPos, positionSmooth);
			//transform.rotation = Quaternion.Slerp(transform.rotation, targetQ, 0.1f);
			transform.rotation = targetQ;
		}
		else
		{
			transform.position = targetPos;
			transform.rotation = targetQ;
		}

	}



	void Start()
	{
		if ( target == null )
			target = Camera.main.transform;
	}

}
