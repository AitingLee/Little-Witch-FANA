using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
	private static DialogueManager _instance;
	public static DialogueManager instance
	{
		get
		{
			return _instance;
		}
		private set
		{
			_instance = value;
		}
	}

	public GameObject nextButton;
	public GameObject taskAcceptButtons;
	public GameObject shopButton;

	public TextMeshProUGUI nameText;
	public TextMeshProUGUI dialogueText;

	public Animator animator;

	private Queue<string> sentences;

	public Villager talkTo;
	private Dialogue currentDialogue;

	public Villager playerVillager;
	public Villager[] allVillagers;
	public GameObject[] villagerIcons;

	private void Awake()
    {
		_instance = this;

	}
    // Use this for initialization
    void Start()
	{
		allVillagers = TaskManager.instance.allVillagers;
		playerVillager = PlayerManager.instance.gameObject.GetComponent<Villager>();
		sentences = new Queue<string>();
		InitDialoug();
	}

	private void InitDialoug()
    {
		allVillagers[0].dialogues[1].nextDialogue = playerVillager.dialogues[9];
		playerVillager.dialogues[9].nextDialogue = allVillagers[0].dialogues[2];
		allVillagers[0].dialogues[2].nextDialogue = playerVillager.dialogues[10];
		playerVillager.dialogues[10].nextDialogue = allVillagers[0].dialogues[3];
		allVillagers[0].dialogues[3].nextDialogue = allVillagers[0].dialogues[4];
	}

	public void StartDialogue(Dialogue dialogue)
	{
		foreach (Villager villager in allVillagers)
        {
			villager.dialogueIcon.SetActive(false);

		}

		talkTo = dialogue.talkPerson;
		currentDialogue = dialogue;

		if (dialogue.talkPerson != null)
        {
			dialogue.talkPerson.dialogueIcon.SetActive(true);
        }

		nextButton.SetActive(true);
		animator.SetBool("IsOpen", true);

		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		PlayerManager.instance.InvokeFreezeTimeScale(1);
		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		if (sentences.Count <= 1)
		{
			if (currentDialogue.clickAccept)
			{
				taskAcceptButtons.SetActive(true);
				nextButton.SetActive(false);
			}
			else
			{
				taskAcceptButtons.SetActive(false);
				nextButton.SetActive(true);
			}

			if (currentDialogue.openShop)
			{
				shopButton.SetActive(true);
			}
			else
			{
				shopButton.SetActive(false);
			}
		}
		else
        {
			taskAcceptButtons.SetActive(false);
			nextButton.SetActive(true);
			shopButton.SetActive(false);
		}


		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));


	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{

		if (currentDialogue != null)
        {
			if (currentDialogue.finishTask != null)
			{
				Debug.Log($"currentDialogue.finishTask {currentDialogue.name} {currentDialogue.finishTask} ");
				TaskManager.instance.FinishTask(currentDialogue.finishTask);
			}

			if (!currentDialogue.clickAccept && currentDialogue.triggerTask != null)
			{
				TaskManager.instance.StartExecuteTask(currentDialogue.triggerTask);
			}


		}
		if (currentDialogue.nextDialogue != null)
		{
			StartDialogue(currentDialogue.nextDialogue);
			return;
		}
		PlayerManager.instance.DefreezeTimeScale();
		ClosePanel();
		talkTo = null;
	}

	void ClosePanel()
    {
		animator.SetBool("IsOpen", false);
		currentDialogue = null;
	}
	public void AcceptTaskOnClick()
    {
		TaskManager.instance.StartExecuteTask(currentDialogue.triggerTask);
		EndDialogue();
	}

	public void RefuseTaskOnClick()
    {
		EndDialogue();
	}

	public void ShopButtonOnClick()
    {
		ClosePanel();
		CanvasManager.instance.shopSystem.OpenCertainVendor(talkTo.characterName);
	}



}
