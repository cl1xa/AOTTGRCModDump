using UnityEngine;

[AddComponentMenu("NGUI/Examples/Item Attachment Point")]
public class InvAttachmentPoint : MonoBehaviour
{
	private GameObject mChild;

	private GameObject mPrefab;

	public InvBaseItem.Slot slot;

	public GameObject Attach(GameObject prefab)
	{
		if (mPrefab != prefab)
		{
			mPrefab = prefab;
			if (mChild != null)
			{
				Object.Destroy(mChild);
			}
			if (mPrefab != null)
			{
				Transform transform = base.transform;
				mChild = Object.Instantiate(mPrefab, transform.position, transform.rotation) as GameObject;
				Transform obj = mChild.transform;
				obj.parent = transform;
				obj.localPosition = Vector3.zero;
				obj.localRotation = Quaternion.identity;
				obj.localScale = Vector3.one;
			}
		}
		return mChild;
	}
}
