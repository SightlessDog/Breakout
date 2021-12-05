using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public AudioSource EffectsSource;

	public static SoundManager Instance = null;
	
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	public void Play(AudioClip clip, bool loop)
	{
		EffectsSource.clip = clip;
		EffectsSource.loop = loop;
		EffectsSource.Play();
	}

	public void PlayOnce(AudioClip clip)
	{
		EffectsSource.PlayOneShot(clip, 0.7f);
	}
}
