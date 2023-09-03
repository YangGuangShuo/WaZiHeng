using UnityEngine;
using System.Collections;

public class MeteorCrashSound : MonoBehaviour {

	public GameObject m_pPrefab;

private ParticleCollisionEvent[] collisionEvents = new ParticleCollisionEvent[16];

	void OnParticleCollision(GameObject other) {
		int safeLength = GetComponent<ParticleSystem>().GetSafeCollisionEventSize();
		if (collisionEvents.Length < safeLength)
			collisionEvents = new ParticleCollisionEvent[safeLength];
		
		int numCollisionEvents = GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);
		int i = 0;
		while (i < numCollisionEvents) {
			Vector3 CrashLocation = collisionEvents[i].intersection;


			GameObject MeteorSound;
			MeteorSound=Instantiate(m_pPrefab,CrashLocation,Quaternion.identity) as GameObject;	
			MeteorSound.transform.parent=transform;
			Destroy(MeteorSound, 7.5f); // destroy object after clip duration

			i++;
		}
	}
}


