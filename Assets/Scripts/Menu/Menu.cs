using UnityEngine;
using System.Collections;


public class Menu : MonoBehaviour {


	private Animator _animtor;

	public bool IsOpen
	{
		get
		{
			return _animtor.GetBool("IsOpen");
		}
		set
		{
			_animtor.SetBool("IsOpen", value);
		}
	}

	
	public void Awake () 
	{
		_animtor = GetComponent<Animator> ();

		var rect = GetComponent<RectTransform> ();
		rect.offsetMax = rect.offsetMin = new Vector2 (0, 0);
	}

	public void ResetObject()
	{
		gameObject.SetActive (false);
	}
	
	public void DisableObject(string gameObjectName)
	{
		GameObject gameObject= GameObject.Find (gameObjectName);
		if (gameObject != null) 
		{
			if (gameObject.activeSelf) 
			{
				gameObject.SetActive (false);
			}
		}
	}


}
