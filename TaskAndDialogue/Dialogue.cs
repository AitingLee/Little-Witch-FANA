using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
	public Villager talkPerson;
	public string name;

	[TextArea(3, 10)]
	public string[] sentences;

	public bool clickAccept;
	public bool openShop;
	public Task triggerTask;
	public Task finishTask;
	public Dialogue nextDialogue;
}
