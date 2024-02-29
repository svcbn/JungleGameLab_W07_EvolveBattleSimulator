using UnityEngine;

public class Test : MonoBehaviour
{
	private UISkillSelector _selector;

	private void Awake()
	{
		
	}

	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			GameManager.Instance.ChangeState(GameManager.GameState.PickSkill);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			GameManager.Instance.HideSkillPickUI();
			GameManager.Instance.ChangeState(GameManager.GameState.PreRound);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			Managers.Stat.GiveDamage(1, 50);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			Character[] characters = FindObjectsOfType<Character>();
			foreach (Character character in characters)
			{
				if (character.playerIndex == 0)
				{
					character.GetComponent<CharacterStatus>().SetFaintEffect(20f);
				}
			}
		}

#endif
	}
}
