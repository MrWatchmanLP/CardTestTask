using UnityEngine;

[CreateAssetMenu(fileName="New Card", menuName="ScriptableObject/Card")]
public class CardBase : ScriptableObject
{
    [SerializeField] string cardName;
    [SerializeField] [TextArea] string description;
    [SerializeField] [TextArea] string followUpDescription;
    [SerializeField] int atk;
    [SerializeField] int hp;
    [SerializeField] int mana;
	[SerializeField] Sprite art = null;
	
	public string CardName { get { return cardName; } }
	public string Description { get { return description; } }
	public string FollowUpDescription { get { return followUpDescription; } }
	public int Atk { get { return atk; } }
	public int HP { get { return hp; } }
	public int Mana { get { return mana; } }
	public Sprite Art { get { return art; } set { art = value; } }
}