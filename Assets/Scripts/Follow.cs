using UnityEngine;


public class Follow : MonoBehaviour
{
	GameObject VRplayer;
    Camera desktopCamera;
	
	#region fields

		public Transform target;
		public float positionSmooth = 0.02f;

		Vector3 vrOffset = new Vector3(0, 0, 1.2f);
		Vector3 desktopOffset = new Vector3(0, 0, 1f);

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
		if ( target == null ) {
			VRplayer = GameObject.Find("VRCamera");
        	desktopCamera = GameObject.Find("DesktopCamera").GetComponent<Camera>();
			Debug.Log(desktopCamera);
			
			if (ExperimentManager.Instance.isVR) {
				target = VRplayer.transform;
				Debug.Log("Bound to VR");
			} else {
				target = desktopCamera.transform;
				Debug.Log("Bound to Desktop");
			}
		}
			
	}

}
