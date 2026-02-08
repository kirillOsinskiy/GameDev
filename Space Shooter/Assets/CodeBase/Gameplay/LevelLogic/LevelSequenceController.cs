using Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class LevelSequenceController : SingletonBase<LevelSequenceController>
    {
        public LevelSequence LevelSequence;

        public bool IsCurrentLevelLast()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            string lastLevelSceneName = LevelSequence.Levels[LevelSequence.Levels.Length - 1].SceneName;

            return sceneName == lastLevelSceneName;
        }

        public LevelProperties GetCurrentLevel()
        {
            string sceneName = SceneManager.GetActiveScene().name;

            for (int i = 0; i < LevelSequence.Levels.Length; i++)
            {
                if (LevelSequence.Levels[i].SceneName == sceneName)
                {
                    return LevelSequence.Levels[i];
                }
            }

            Debug.LogError("Level not found " + sceneName);

            return null;
        }

        public LevelProperties GetNextLevelProperties()
        {
            return GetNextLevelProperties(GetCurrentLevel());
        }

        public LevelProperties GetNextLevelProperties(LevelProperties levelProperties)
        {
            for (int i = 0; i < LevelSequence.Levels.Length; i++)
            {
                if (LevelSequence.Levels[i].SceneName == levelProperties.SceneName)
                {
                    if (i <  LevelSequence.Levels.Length - 1)
                    {
                        return LevelSequence.Levels[i + 1];
                    }                    
                }
            }

            Debug.LogError("Not found next level properties for level " + levelProperties.SceneName);

            return null;
        }
    }
}