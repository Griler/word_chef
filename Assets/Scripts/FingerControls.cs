using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FingerControls : MonoBehaviour {

	private RectTransform canvasRect;
	private Vector2 screenPosition;
	private Vector2 viewportPosition;

	public List<OfferedLetter> listOfSelectedLetters;

	public bool holdingDownFinger;

	public List<string> finishedWords;
	public List<string> finishedAdditionalWords;

	public int consecutiveWordsFound;
	public Animator consecutiveWordsAnimator;
	public GameObject[] consecutiveWordsSprites;

	public LineRenderer line;

	void Awake()
	{
		canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
		listOfSelectedLetters = new List<OfferedLetter>();
		finishedWords = new List<string>();
		finishedAdditionalWords = new List<string>();
		consecutiveWordsFound = 0;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			transform.SetAsLastSibling();
			holdingDownFinger = true;
			GetComponent<BoxCollider2D>().enabled = true;

			if (!GameplayManager.gameFinished)
				transform.GetChild(0).GetComponent<ParticleSystem>().Play();
		}

		if (Input.GetMouseButton(0) && !GameplayManager.gameFinished && holdingDownFinger)
		{
			SetObjectPositionToCanvasPosition(gameObject, Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}

		if (Input.GetMouseButtonUp(0))
		{
			holdingDownFinger = false;
			GetComponent<BoxCollider2D>().enabled = false;
			CheckIfSelectedLettersAreValid();
			transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
		}
	}

	public void SetObjectPositionToCanvasPosition(GameObject obj, Vector3 worldPosition)
	{
		viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);

		screenPosition = new Vector2(viewportPosition.x * canvasRect.sizeDelta.x - canvasRect.sizeDelta.x * 0.5f, viewportPosition.y * canvasRect.sizeDelta.y - canvasRect.sizeDelta.y * 0.5f);


		obj.GetComponent<RectTransform>().anchoredPosition = screenPosition;  
	}

	public Vector3 GetObjectPositionInCanvasPosition(RectTransform rt)
	{




		Vector3[] corners = new Vector3[4];
		rt.GetWorldCorners(corners);

		Vector3 worldPosition = new Vector3((corners[0].x + corners[3].x) / 2, (corners[0].y + corners[1].y) / 2, 0);

		return worldPosition;
	}

	public void OnTriggerEnter2D(Collider2D coll)
	{
		if (!GameplayManager.gameFinished && coll.tag == "Letter" && !listOfSelectedLetters.Contains(coll.GetComponent<OfferedLetter>()))
		{
			if (listOfSelectedLetters.Count == 0)
			{
				for (int i = 0; i < GameplayManager.gameplayManager.selectedLettersHolder.transform.childCount; i++)
				{
					GameplayManager.gameplayManager.selectedLettersHolder.transform.GetChild(i).gameObject.SetActive(false);
				}

				GameplayManager.gameplayManager.selectedLettersHolder.transform.parent.GetComponent<Animator>().Play("SelectedLettersVisibleIdle", 0, 0);
			}

			listOfSelectedLetters.Add(coll.GetComponent<OfferedLetter>());

			GameplayManager.gameplayManager.selectedLettersHolder.transform.Find(coll.GetComponent<OfferedLetter>().letter).gameObject.SetActive(true);
			GameplayManager.gameplayManager.selectedLettersHolder.transform.Find(coll.GetComponent<OfferedLetter>().letter).SetAsLastSibling();

			
			SoundManager.Instance.Play_Sound(SoundManager.Instance.letterSelected);


		}
		else if (coll.tag == "Letter" && listOfSelectedLetters.Contains(coll.GetComponent<OfferedLetter>()) && listOfSelectedLetters.IndexOf(coll.GetComponent<OfferedLetter>()) == listOfSelectedLetters.Count - 1)
		{
			listOfSelectedLetters.RemoveAt(listOfSelectedLetters.Count - 1);

			GameplayManager.gameplayManager.selectedLettersHolder.transform.Find(coll.GetComponent<OfferedLetter>().letter).gameObject.SetActive(false);

			if (listOfSelectedLetters.Count == 0)
			{
				GameplayManager.gameplayManager.selectedLettersHolder.transform.parent.GetComponent<Animator>().Play("SelectedLettersIdle", 0, 0);

				line.GetComponent<Main>().ClearPoints();
			}
		}
	}

	public void ExtraWordFound()
	{
		GlobalVariables.bonusWordsCollected += 1;
		GameplayManager.gameplayManager.bonusWordsCollecedOnThisLevel += 1;

		if (GlobalVariables.bonusWordsCollected >= GlobalVariables.bonusWordsRequired)
			StartCoroutine("PlayFullJarAnimation");

		GameplayManager.gameplayManager.wordIntoCookieJarAnimator.Play("ExtraWordFound", 0, 0);
		GameplayManager.gameplayManager.extraWordButton.transform.GetChild(0).GetComponent<Animator>().Play("JarExtraWord", 0, 0);
	}

	IEnumerator PlayFullJarAnimation()
	{
		yield return new WaitForSeconds(1.6f);
		GameplayManager.gameplayManager.extraWordButton.transform.GetChild(0).GetComponent<Animator>().Play("ExtraWordsJarOpenAnimation", 0, 0);
	}
		
	public void CheckIfSelectedLettersAreValid()
	{
		if (listOfSelectedLetters.Count > 1)
		{
			string word = "";

			for (int i = 0; i < listOfSelectedLetters.Count; i++)
				word += listOfSelectedLetters[i].letter;

			if (finishedWords.Contains(word))
			{
				GameplayManager.gameplayManager.selectedLettersHolder.transform.parent.GetComponent<Animator>().Play("AlreadyFoundWord", 0, 0);
			}
			else if (GameplayManager.gameplayManager.targetWords.Contains(word))
			{
				finishedWords.Add(word);

				GameplayManager.gameplayManager.selectedLettersHolder.transform.parent.GetComponent<Animator>().Play("CorrectWord", 0, 0);

				
				SoundManager.Instance.Play_Sound(SoundManager.Instance.wordSolved);

				if (GameplayManager.gameplayManager.targetWords.Count <= 3)
				{
					
					for (int i = 0; i < GameplayManager.gameplayManager.targetWords.Count; i++)
					{
						if (GameplayManager.gameplayManager.targetWordsHolder.transform.GetChild(i).GetComponent<TargetWord>().word == word)
							GameplayManager.gameplayManager.targetWordsHolder.transform.GetChild(i).GetComponent<TargetWord>().ShowWord();
					}
				}
				else
				{
					bool wordFound = false;

					
					for (int i = 0; i < GameplayManager.gameplayManager.targetWordsHolder1.transform.childCount; i++)
					{
						if (GameplayManager.gameplayManager.targetWordsHolder1.transform.GetChild(i).GetComponent<TargetWord>().word == word)
						{
							GameplayManager.gameplayManager.targetWordsHolder1.transform.GetChild(i).GetComponent<TargetWord>().ShowWord();
							wordFound = true;
							break;
						}
					}

					
					if (!wordFound)
					{
						for (int i = 0; i < GameplayManager.gameplayManager.targetWordsHolder2.transform.childCount; i++)
						{
							if (GameplayManager.gameplayManager.targetWordsHolder2.transform.GetChild(i).GetComponent<TargetWord>().word == word)
							{
								GameplayManager.gameplayManager.targetWordsHolder2.transform.GetChild(i).GetComponent<TargetWord>().ShowWord();
								wordFound = true;
								break;
							}
						}
					}
				}

				
				if (finishedWords.Count == GameplayManager.gameplayManager.targetWords.Count)
				{
					GameplayManager.gameFinished = true;
					GameplayManager.gameplayManager.LevelFinished();
				}
				else
				{
					
					consecutiveWordsFound += 1;

					if (consecutiveWordsFound % 3 == 0)
					{
						consecutiveWordsAnimator.Play("GameTextAnimation", 0, 0);
						int randomConsWord = Random.Range(0, consecutiveWordsSprites.Length);

						for(int i = 0; i < consecutiveWordsSprites.Length; i++)
						{
							if (i != randomConsWord)
								consecutiveWordsSprites[i].SetActive(false);
							else
								consecutiveWordsSprites[i].SetActive(true);
						}
					}
				}
			}
			else if (finishedAdditionalWords.Contains(word)) 
			{

				GameplayManager.gameplayManager.selectedLettersHolder.transform.parent.GetComponent<Animator>().Play("AlreadyFoundWord", 0, 0);
			}
			else if (GameplayManager.gameplayManager.additionalWords.Contains(word)) 
			{
				finishedAdditionalWords.Add(word);

				
				if (LevelsParser.selectedPack == LevelsParser.levelParser.lastUnlockedPack && LevelsParser.selectedWorld == LevelsParser.levelParser.lastUnlockedWorld && LevelsParser.selectedLevel == LevelsParser.levelParser.lastUnlockedLevel)
				{
					ExtraWordFound();


					GameplayManager.gameplayManager.selectedLettersHolder.transform.parent.GetComponent<Animator>().Play("AdditionalWord", 0, 0);
				}

				
				consecutiveWordsFound += 1;

				if (consecutiveWordsFound % 3 == 0)
				{
					consecutiveWordsAnimator.Play("GameTextAnimation", 0, 0);
					int randomConsWord = Random.Range(0, consecutiveWordsSprites.Length);

					for(int i = 0; i < consecutiveWordsSprites.Length; i++)
					{
						if (i != randomConsWord)
							consecutiveWordsSprites[i].SetActive(false);
						else
							consecutiveWordsSprites[i].SetActive(true);
					}
				}

				
				SoundManager.Instance.Play_Sound(SoundManager.Instance.wordSolved);
			}
			else if (GameplayManager.gameplayManager.solvedWords.Contains(word)) 
			{

				GameplayManager.gameplayManager.selectedLettersHolder.transform.parent.GetComponent<Animator>().Play("AlreadyFoundWord", 0, 0);
			}
			else 
			{
				GameplayManager.gameplayManager.selectedLettersHolder.transform.parent.GetComponent<Animator>().Play("WrongWord", 0, 0);

				
				consecutiveWordsFound = 0;

				
				SoundManager.Instance.Play_Sound(SoundManager.Instance.wrongWord);
			}
		}
		else
		{
			
			if (listOfSelectedLetters.Count > 0)
			{
				GameplayManager.gameplayManager.selectedLettersHolder.transform.parent.GetComponent<Animator>().Play("WrongWord", 0, 0);

				
				consecutiveWordsFound = 0;

				
				SoundManager.Instance.Play_Sound(SoundManager.Instance.wrongWord);
			}
		}

		
		listOfSelectedLetters.Clear();

		
		line.positionCount = 0;
	}
}
