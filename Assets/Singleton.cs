using UnityEngine;

namespace Game.Shared
{
	public abstract class Singleton<T> : MonoBehaviour where T : class
	{
		public static T Instance { get; private set; }

		protected virtual void Awake()
		{
			if (!Application.isPlaying) 
				return;

			if (Instance != null)
			{
				Destroy(gameObject); 
				return;
			}

			Instance = this as T;
		}
	}
}