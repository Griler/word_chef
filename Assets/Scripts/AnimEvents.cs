using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimEvents : MonoBehaviour {

	public void LoadScene()
	{
		Application.LoadLevel("Loading");
	}

	public void DeactivateLetters()
	{
		for (int i = 0; i < transform.GetChild(0).childCount; i++)
		{
			transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
		}
	}

	public void PlayChildElementParticle()
	{
		transform.GetChild(0).GetComponent<ParticleSystem>().Play();
	}

	public void StopChildElementParticle()
	{
		transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
	}

	public void PlayLoadingArriveSound()
	{
		SoundManager.Instance.Play_Sound(SoundManager.Instance.loadingArrive);
	}

	public void PlayLoadingDepartSound()
	{
		SoundManager.Instance.Play_Sound(SoundManager.Instance.loadingDepart);
	}
}
