using UnityEngine;

public class OnClickLoadSomething : MonoBehaviour
{
	public enum ResourceTypeOption : byte
	{
		Scene,
		Web
	}

	public string ResourceToLoad;

	public ResourceTypeOption ResourceTypeToLoad;

	public void OnClick()
	{
		switch (ResourceTypeToLoad)
		{
		case ResourceTypeOption.Scene:
			Application.LoadLevel(ResourceToLoad);
			break;
		case ResourceTypeOption.Web:
			Application.OpenURL(ResourceToLoad);
			break;
		}
	}
}
