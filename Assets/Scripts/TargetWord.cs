using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetWord : MonoBehaviour {

	public string word;
	public bool wordSolved;

	void Awake()
	{
		wordSolved = false;
	}

	
	public void ShowWord()
	{
		StartCoroutine("ShowLettersPeriodically");

		wordSolved = true;
	}

	IEnumerator ShowLettersPeriodically()
	{
		
		for (int i = 0; i < transform.childCount; i++)
		{
			
			


			if (transform.GetChild(i).GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Empty"))
				transform.GetChild(i).GetChild(0).GetComponent<Animator>().Play("Solved", 0, 0);
			else if (transform.GetChild(i).GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Hinted"))
				transform.GetChild(i).GetChild(0).GetComponent<Animator>().Play("HintedSolved", 0, 0);
			else
				transform.GetChild(i).GetChild(0).GetComponent<Animator>().Play("BonusLetterSolved", 0, 0);

			if (GameplayManager.gameplayManager.bonusLetters.Contains(transform.GetChild(i).gameObject))
			{
				GlobalVariables.globalVariables.AddCoins(1);
				GameplayManager.gameplayManager.coinsText.text = GlobalVariables.coins.ToString();
				GameplayManager.gameplayManager.coinsTextShop.text = GlobalVariables.coins.ToString();

				
				SoundManager.Instance.Play_Sound(SoundManager.Instance.bonuscoin);
			}

			yield return new WaitForSeconds(0.12f);

			
			
			
		}
	}
}
