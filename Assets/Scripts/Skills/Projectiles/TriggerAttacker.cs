using UnityEngine;

public class TriggerAttacker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character owner  = GetComponent<GuidedBulletMover>().owner;
        int       damage = GetComponent<GuidedBulletMover>().damage;

        if (collision.GetComponent<Character>()             == null             ){ return; }
        if (collision.GetComponent<Character>().playerIndex == owner.playerIndex){ return; }
	
    
        Managers.Stat.GiveDamage(1 - owner.playerIndex, damage);

        owner.Target.GetComponent<CharacterStatus>().SetKnockbackEffect(1f, 30f, transform.position);

        Destroy(this.gameObject);
    }
}
