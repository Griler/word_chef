using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using UnityEngine.UI;

public class LevelsParser : MonoBehaviour {

	public XmlDocument xmlDocument;
	public XmlNodeList packs;

	public int lastUnlockedPack;
	public int lastUnlockedWorld;
	public int lastUnlockedLevel;

	public static int selectedPack;
	public static int selectedWorld;
	public static int selectedLevel;

	
	public GameObject packPrefab;
	public GameObject worldTabPrefab;
	public GameObject levelPrefab;

	public static LevelsParser levelParser;

	void Awake()
	{
		xmlDocument = new XmlDocument();
		TextAsset xmlData = new TextAsset();
		xmlData = Resources.Load("WordSearch") as TextAsset;

		xmlDocument.LoadXml(xmlData.text);

		packs = xmlDocument.SelectNodes("/xml/category");

		if (GlobalVariables.stars == 0)
		{
			lastUnlockedPack = 0;
			lastUnlockedWorld = 0;
			lastUnlockedLevel = 0;

			if (!PlayerPrefs.HasKey("LastUnlockedPack"))
				PlayerPrefs.SetInt("LastUnlockedPack", 0);

			if (!PlayerPrefs.HasKey("LastUnlockedWorld"))
				PlayerPrefs.SetInt("LastUnlockedWorld", 0);

			if (!PlayerPrefs.HasKey("LastUnlockedLevel"))
				PlayerPrefs.SetInt("LastUnlockedLevel", 0);
		}
		else
		{
			lastUnlockedPack = PlayerPrefs.GetInt("LastUnlockedPack");
			lastUnlockedWorld = PlayerPrefs.GetInt("LastUnlockedWorld");
			lastUnlockedLevel = PlayerPrefs.GetInt("LastUnlockedLevel");
		}
			
		levelParser = this;
		DontDestroyOnLoad(this);
	}

	public void SetPackWorlds()
	{
		Transform packsHolder = GameObject.Find("WorldPacksHolder").transform;

		for (int i = 0; i < packs.Count; i++)
		{
			GameObject newPack = Instantiate(packPrefab, packsHolder) as GameObject;
			newPack.transform.localScale = Vector3.one;

			for (int j = 0; j < packs[i].ChildNodes.Count; j++)
			{
				int numberOfStarsForThisWorld = 0;

				GameObject newWorld = Instantiate(worldTabPrefab, newPack.transform.GetChild(0).transform) as GameObject;
				newWorld.transform.localScale = Vector3.one;

				if (i == lastUnlockedPack)
				{
					if (j > lastUnlockedWorld)
					{
						newWorld.GetComponent<CanvasGroup>().alpha = 0.3f;
					}
					else if (j < lastUnlockedWorld) 
					{
						newWorld.transform.Find("AnimationHolder/BadgeImage").gameObject.SetActive(true);
					}
				}
				else if (i > lastUnlockedPack) 
				{
					newWorld.GetComponent<CanvasGroup>().alpha = 0.3f;
				}
				else 
				{
					newWorld.transform.Find("AnimationHolder/BadgeImage").gameObject.SetActive(true);
				}

				newWorld.transform.GetChild(1).GetComponent<WorldLevelScript>().packIndex = i;
				newWorld.transform.GetChild(1).GetComponent<WorldLevelScript>().worldIndex = j;

				newWorld.transform.Find("AnimationHolder/WorldName").GetComponent<Text>().text = packs[i].ChildNodes[j].Attributes["title"].Value;
				newWorld.transform.Find("AnimationHolder/LetterNumberText").GetComponent<Text>().text = packs[i].ChildNodes[j].Attributes["maxletters"].Value + " letters max";

				for (int k = 0; k < packs[i].ChildNodes[j].ChildNodes.Count; k++)
				{
					numberOfStarsForThisWorld += packs[i].ChildNodes[j].ChildNodes[k].SelectSingleNode("words").InnerText.Split(',').Length;
				}

				newWorld.transform.Find("AnimationHolder/StarNumber").GetComponent<Text>().text = numberOfStarsForThisWorld.ToString();







			}

			newPack.transform.Find("Tabs/TitleHolder").GetComponent<Text>().text = packs[i].Attributes["name"].Value;

			if (i < lastUnlockedPack)
				newPack.transform.Find("Tabs/TitleHolder/WorldCompletedHolder").gameObject.SetActive(true);

			newPack.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(newPack.GetComponent<RectTransform>().anchoredPosition3D.x, newPack.GetComponent<RectTransform>().anchoredPosition3D.y, 0f);
		}

		StartCoroutine(SetTabsAppropriately(packsHolder));
	}

	public void SetTabs(Transform packsHolder)
	{
		StartCoroutine(SetTabsAppropriately(packsHolder));
	}

	IEnumerator SetTabsAppropriately(Transform packsHolder)
	{
		yield return new WaitForEndOfFrame();

		for (int i = 0; i < packsHolder.childCount; i++)
		{
			packsHolder.GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(packsHolder.GetChild(i).GetComponent<RectTransform>().sizeDelta.x, packsHolder.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
		}

		packsHolder.GetComponent<ContentSizeFitter>().enabled = false;
		packsHolder.GetComponent<ContentSizeFitter>().enabled = true;

		

		yield return new WaitForEndOfFrame();

		if (packsHolder.name == "LevelPacksHolder")
		{
			if (selectedPack == lastUnlockedPack && selectedWorld == lastUnlockedWorld)
			{
				SnapTo(packsHolder.GetChild(0).GetChild(0).GetChild(selectedLevel + 1).GetComponent<RectTransform>(), 
					packsHolder.GetComponent<RectTransform>(), packsHolder.parent.GetComponent<ScrollRect>());
			}
			else
			{
				packsHolder.GetComponent<RectTransform>().anchoredPosition = new Vector2(packsHolder.GetComponent<RectTransform>().anchoredPosition.x, -packsHolder.GetComponent<RectTransform>().sizeDelta.y / 2 + 200f);
			}


		}
		else if (packsHolder.name == "WorldPacksHolder")
		{








			SnapTo(packsHolder.GetChild(selectedPack).GetChild(0).GetChild(selectedWorld).GetComponent<RectTransform>(), 
				packsHolder.GetComponent<RectTransform>(), packsHolder.parent.GetComponent<ScrollRect>());

		}
	}

	public void SnapTo(RectTransform target, RectTransform contentPanel, ScrollRect scrollRect)
	{
		Canvas.ForceUpdateCanvases();

		contentPanel.anchoredPosition =
			(Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
			- (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
	}
		
	public void SetWorldLevels(int packIndex, int worldIndex)
	{
		string worldName = packs[packIndex].ChildNodes[worldIndex].Attributes["title"].Value;

		Transform packsHolder = GameObject.Find("LevelPacksHolder").transform;

		GameObject newPack;
		if (packsHolder.childCount == 0)
		{
			newPack = Instantiate(packPrefab, packsHolder) as GameObject;
			newPack.transform.localScale = Vector3.one;
		}
		else
		{
			newPack = packsHolder.GetChild(0).gameObject;
		}

		
		if (packsHolder.GetChild(0).GetChild(0).childCount == 1)
		{
			for (int i = 0; i < packs[packIndex].ChildNodes[worldIndex].ChildNodes.Count; i++)
			{
				GameObject newLevel = Instantiate(levelPrefab, newPack.transform.GetChild(0).transform) as GameObject;
				newLevel.transform.localScale = Vector3.one;
				newLevel.name = "Level" + (i + 1).ToString();

				if (selectedPack == lastUnlockedPack)
				{
					if (selectedWorld == lastUnlockedWorld)
					{
						if (i >= lastUnlockedLevel) 
						{
							if (i > lastUnlockedLevel)
							{
								newLevel.GetComponent<CanvasGroup>().alpha = 0.3f;
							}

							newLevel.transform.Find("AnimationHolder/LevelSolvedText").GetComponent<Text>().text = "Not solved";
						}
						else if (i < lastUnlockedLevel) 
						{
							
							newLevel.transform.Find("AnimationHolder/LevelSolvedText").GetComponent<Text>().text = "Solved";
						}
					}
					else 
					{
						newLevel.transform.Find("AnimationHolder/LevelSolvedText").GetComponent<Text>().text = "Solved";
					}
				}

				
				newLevel.transform.Find("AnimationHolder/LevelNumberName").GetComponent<Text>().text = "Level " + (i + 1).ToString();

				
				newLevel.transform.GetChild(1).GetComponent<WorldLevelScript>().levelIndex = i;









			}
		}
		else 
		{
			
			if (packsHolder.GetChild(0).GetChild(0).Find("NativeAdHolder") != null)
			{
				if (!GlobalVariables.removeAdsOwned)
					packsHolder.GetChild(0).GetChild(0).Find("NativeAdHolder").SetAsLastSibling();
				else
					Destroy(packsHolder.GetChild(0).GetChild(0).Find("NativeAdHolder").gameObject);
			}

			
			if (packsHolder.GetChild(0).GetChild(0).childCount > packs[packIndex].ChildNodes[worldIndex].ChildNodes.Count + 1) 
			{
				
				for (int i = packs[packIndex].ChildNodes[worldIndex].ChildNodes.Count + 1; i < packsHolder.GetChild(0).GetChild(0).childCount - 1; i++)
				{
					packsHolder.GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(false);	
				}
			}
			else if (packsHolder.GetChild(0).GetChild(0).childCount < packs[packIndex].ChildNodes[worldIndex].ChildNodes.Count + 1)
			{
				
				for (int i = packsHolder.GetChild(0).GetChild(0).childCount; i < packs[packIndex].ChildNodes[worldIndex].ChildNodes.Count + 1; i ++)
				{
					GameObject newLevel = Instantiate(levelPrefab, newPack.transform.GetChild(0).transform) as GameObject;
					newLevel.transform.localScale = Vector3.one;
					newLevel.name = "Level" + (i + 1).ToString();

					
					newLevel.transform.GetChild(1).GetComponent<WorldLevelScript>().levelIndex = i;
				}
			}

			
			for (int i = 0; i < packs[packIndex].ChildNodes[worldIndex].ChildNodes.Count; i++)
			{
				
				if (selectedPack == lastUnlockedPack)
				{
					
					if (selectedWorld == lastUnlockedWorld)
					{
						if (i >= lastUnlockedLevel) 
						{
							if (i > lastUnlockedLevel)
							{
								
								packsHolder.GetChild(0).GetChild(0).GetChild(i + 1).GetComponent<CanvasGroup>().alpha = 0.3f;
							}

							
							packsHolder.GetChild(0).GetChild(0).GetChild(i + 1).transform.Find("AnimationHolder/LevelSolvedText").GetComponent<Text>().text = "Not solved";
						}
						else if (i < lastUnlockedLevel) 
						{
							
							packsHolder.GetChild(0).GetChild(0).GetChild(i + 1).transform.Find("AnimationHolder/LevelSolvedText").GetComponent<Text>().text = "Solved";
						}
					}
					else 
					{
						
						packsHolder.GetChild(0).GetChild(0).GetChild(i + 1).transform.Find("AnimationHolder/LevelSolvedText").GetComponent<Text>().text = "Solved";
					}
				}

				
				packsHolder.GetChild(0).GetChild(0).GetChild(i + 1).transform.Find("AnimationHolder/LevelNumberName").GetComponent<Text>().text = "Level " + (i + 1).ToString();

				
				packsHolder.GetChild(0).GetChild(0).GetChild(i + 1).gameObject.SetActive(true);
			}

			
			for (int i = selectedLevel + 1; i < packs[packIndex].ChildNodes[worldIndex].ChildNodes.Count; i++)
			{
				packsHolder.GetChild(0).GetChild(0).GetChild(selectedLevel + 2).SetAsLastSibling();
			}
		}

		
		newPack.transform.Find("Tabs/TitleHolder").GetComponent<Text>().text = packs[packIndex].ChildNodes[worldIndex].Attributes["title"].Value;

		newPack.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(newPack.GetComponent<RectTransform>().anchoredPosition3D.x, newPack.GetComponent<RectTransform>().anchoredPosition3D.y, 0f);

		
		StartCoroutine(SetTabsAppropriately(packsHolder));
	}

	
	
	
	
	


	
	public void WorldSelected()
	{
		
		LevelSelectManager.levelSelectManager.WorldSelected();
	}

	
	public void LevelSelected()
	{
		
		LevelSelectManager.levelSelectManager.LevelSelected();
	}

	
	public void SetLevelParameters()
	{
		string letters = packs[selectedPack].ChildNodes[selectedWorld].ChildNodes[selectedLevel].SelectSingleNode("letters").InnerText;
		string words = packs[selectedPack].ChildNodes[selectedWorld].ChildNodes[selectedLevel].SelectSingleNode("words").InnerXml;
		string additionalWords = packs[selectedPack].ChildNodes[selectedWorld].ChildNodes[selectedLevel].SelectSingleNode("additional_words").InnerText;
		string solvedWords = packs[selectedPack].ChildNodes[selectedWorld].ChildNodes[selectedLevel].SelectSingleNode("solved_words").InnerText;

		GameplayManager.gameplayManager.offeredLetters.Clear();
		GameplayManager.gameplayManager.targetWords.Clear();
		GameplayManager.gameplayManager.additionalWords.Clear();
		GameplayManager.gameplayManager.solvedWords.Clear();

		if (packs[selectedPack].ChildNodes[selectedWorld].ChildNodes[selectedLevel].Attributes["bonus"].Value == "yes")
			GameplayManager.isBonus = true;

		GameplayManager.gameplayManager.offeredLetters = letters.Split(',').ToList();

		
		if (GameplayManager.isBonus)
		{
			GameplayManager.gameplayManager.offeredBonusLetters = packs[selectedPack].ChildNodes[selectedWorld].ChildNodes[selectedLevel].SelectSingleNode("additional_letters").InnerText.Split(',').ToList();

		}

		GameplayManager.gameplayManager.targetWords = words.Split(',').ToList();
		GameplayManager.gameplayManager.additionalWords = additionalWords.Split(',').ToList();
		GameplayManager.gameplayManager.solvedWords = solvedWords.Split(',').ToList();
	}

	
	public void CheckIfLastLevelWasFinished()
	{
		if (lastUnlockedPack == selectedPack && lastUnlockedWorld == selectedWorld && lastUnlockedLevel == selectedLevel)
		{
			
			if (selectedLevel < packs[selectedPack].ChildNodes[selectedWorld].ChildNodes.Count - 1)
			{
				
				lastUnlockedLevel += 1;
				PlayerPrefs.SetInt("LastUnlockedLevel", lastUnlockedLevel);
				PlayerPrefs.Save();

				Debug.Log("Level povecan");
			}
			else if (selectedWorld < packs[selectedPack].ChildNodes.Count - 1) 
			{
				lastUnlockedLevel = 0;
				lastUnlockedWorld += 1;
				PlayerPrefs.SetInt("LastUnlockedLevel", lastUnlockedLevel);
				PlayerPrefs.SetInt("LastUnlockedWorld", lastUnlockedWorld);
				PlayerPrefs.Save();
				Debug.Log("World povecan");
			}
			else if (lastUnlockedPack < packs.Count - 1) 
			{
				lastUnlockedPack += 1;
				lastUnlockedWorld = 0;
				lastUnlockedLevel = 0;
				PlayerPrefs.SetInt("LastUnlockedLevel", lastUnlockedLevel);
				PlayerPrefs.SetInt("LastUnlockedWorld", lastUnlockedWorld);
				PlayerPrefs.SetInt("LastUnlockedPack", lastUnlockedPack);
				PlayerPrefs.Save();
				Debug.Log("Pack povecan");
			}

			GlobalVariables.stars += GameplayManager.gameplayManager.targetWords.Count;
			PlayerPrefs.SetInt("Stars", GlobalVariables.stars);
			PlayerPrefs.Save();
		}
	}

	public void IncrementLastSelectedLevel()
	{
		
		if (selectedLevel < packs[selectedPack].ChildNodes[selectedWorld].ChildNodes.Count - 1)
		{
			selectedLevel++;
		}
		else if (selectedWorld < packs[selectedPack].ChildNodes.Count - 1) 
		{
			selectedWorld++;
			selectedLevel = 0;
		}
		else if (selectedPack < packs.Count - 1)
		{
			selectedPack++;
			selectedWorld = 0;
			selectedLevel = 0;
		}
	}
}
