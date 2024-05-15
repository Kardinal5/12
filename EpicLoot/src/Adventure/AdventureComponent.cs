﻿using System;
using Newtonsoft.Json;
using UnityEngine;

namespace EpicLoot.Adventure
{
    [RequireComponent(typeof(Player))]
    public class AdventureComponent : MonoBehaviour
    {
        public const string SaveDataKey = EpicLoot.PluginId + "+" + nameof(AdventureSaveData);

        private Player _player;
        public AdventureSaveDataList SaveData = new AdventureSaveDataList();

        public void Awake()
        {
            _player = GetComponent<Player>();
            Load();
        }

        public void Load()
        {
            // Upgrade old bounty information to new save system
            if (_player.m_knownTexts.TryGetValue(SaveDataKey, out var oldData))
            {
                if (!_player.m_customData.ContainsKey(SaveDataKey))
                {
                    _player.m_customData.Add(SaveDataKey, oldData);
                }

                _player.m_knownTexts.Remove(SaveDataKey);
            }
            
            if (_player.m_customData.TryGetValue(SaveDataKey, out var data))
            {
                try
                {
                    SaveData = JsonConvert.DeserializeObject<AdventureSaveDataList>(data);

                    // Clean up old bounties
                    var removed = 0;
                    foreach (var saveData in SaveData.AllSaveData)
                    {
                        removed += saveData.Bounties.RemoveAll(x => x.State == BountyState.InProgress && x.PlayerID == 0);
                    }

                    if (removed > 0)
                    {
                        EpicLoot.LogWarning($"Removed {removed} invalid bounties");
                    }
                }
                catch (Exception)
                {
                    SaveData = new AdventureSaveDataList();
                }
            }
            else
            {
                SaveData = new AdventureSaveDataList();
            }
        }

        public void Save()
        {
            var data = JsonConvert.SerializeObject(SaveData, Formatting.None);
            _player.m_customData[SaveDataKey] = data;
        }
    }
}