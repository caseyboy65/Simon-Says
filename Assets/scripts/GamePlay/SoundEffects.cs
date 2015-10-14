using UnityEngine;
using System.Collections;

public class SoundEffects : MonoBehaviour {
	//Audio settings
	private AudioSource source;
	public float lowPitch = 0.5f;
	public float highPitch = 1.0f;

	// Use this for initialization
	void Start () {
		source = gameObject.GetComponent<AudioSource>();
	}
	
	public void playSound() {
		source.pitch = Random.Range (lowPitch, highPitch);
		source.Play ();
	}
}
