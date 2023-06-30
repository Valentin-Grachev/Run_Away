using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace DrawRoad
{
    [CreateAssetMenu(menuName = "Data/DrawRoad/Levels", fileName = "Levels")]
    public class LevelStore : ScriptableObject
    {
        public List<GameObject> levels;

        [SerializeField] private int _runLevel;


        [Button("Установить")]
        private void SetLevel()
        {
            PlayerPrefs.SetInt("Level", _runLevel);
            Debug.Log("Установлен " + _runLevel + " уровень"); 
        }

    }





}




