using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Game.Scripts.Common.Characters;
using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.Locations.Configs.Inventory;

namespace Assets.Game.Editor
{
    public class InventoryItemWizard : EditorWindow
    {
        private InventoryItemDataBase _database;
        private string _id = "";
        private string _itemName = "";
        private Sprite _sprite;
        private ItemType _itemType = ItemType.None;
        private EquipmentType _equipmentType = EquipmentType.Head;
        private bool _isStackable = true;
        private Stats _stats = new Stats();
        private List<BattleAbility> _abilities = new List<BattleAbility>();
        private string _description = "";

        [MenuItem("Tools/Inventory Item Wizard")]
        public static void ShowWindow()
        {
            GetWindow<InventoryItemWizard>("Item Wizard");
        }

        private void OnGUI()
        {
            DrawDatabaseSection();
            if (_database == null) return;

            DrawItemCreationSection();
        }

        private void DrawDatabaseSection()
        {
            GUILayout.Label("Database Configuration", EditorStyles.boldLabel);
            _database = (InventoryItemDataBase)EditorGUILayout.ObjectField(
                "Item Database",
                _database,
                typeof(InventoryItemDataBase),
                false
            );

            if (_database == null)
            {
                EditorGUILayout.HelpBox("Database is not assigned", MessageType.Warning);
                if (GUILayout.Button("Create New Database"))
                {
                    CreateNewDatabase();
                }
                return;
            }
        }

        private void DrawItemCreationSection()
        {
            GUILayout.Space(20);
            GUILayout.Label("Item Configuration", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            _id = EditorGUILayout.TextField("ID", _id);
            if (GUILayout.Button("Generate ID", GUILayout.Width(100)))
            {
                _id = Guid.NewGuid().ToString();
            }
            EditorGUILayout.EndHorizontal();

            _itemName = EditorGUILayout.TextField("Name", _itemName);
            _sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", _sprite, typeof(Sprite), false);
            _itemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", _itemType);

            DrawEquipmentSpecificFields();
            DrawAbilitiesList();

            if (_itemType != ItemType.Equipment)
            {
                _isStackable = EditorGUILayout.Toggle("Stackable", _isStackable);
            }
            else
            {
                _isStackable = false;
            }

            _description = EditorGUILayout.TextArea(_description, GUILayout.MinHeight(80));

            if (GUILayout.Button("Create Item", GUILayout.Height(40)))
            {
                CreateItem();
            }
        }

        private void DrawEquipmentSpecificFields()
        {
            if (_itemType != ItemType.Equipment) return;

            _equipmentType = (EquipmentType)EditorGUILayout.EnumPopup("Equipment Type", _equipmentType);

            GUILayout.Space(10);
            GUILayout.Label("Stats", EditorStyles.boldLabel);
            _stats.Hp = EditorGUILayout.IntField("HP", _stats.Hp);
            _stats.Attack = EditorGUILayout.IntField("Attack", _stats.Attack);
            _stats.MagicalAttack = EditorGUILayout.IntField("Magical Attack", _stats.MagicalAttack);
            _stats.Defence = EditorGUILayout.IntField("Defence", _stats.Defence);
            _stats.MagicalDefence = EditorGUILayout.IntField("Magical Defence", _stats.MagicalDefence);
            _stats.Speed = EditorGUILayout.IntField("Speed", _stats.Speed);
        }

        private void DrawAbilitiesList()
        {
            if (_itemType != ItemType.Equipment) return;

            GUILayout.Label("Abilities", EditorStyles.boldLabel);
            for (int i = 0; i < _abilities.Count; i++)
            {
                _abilities[i] = (BattleAbility)EditorGUILayout.ObjectField(
                    $"Ability {i + 1}",
                    _abilities[i],
                    typeof(BattleAbility),
                    false
                );
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Ability"))
            {
                _abilities.Add(null);
            }
            if (GUILayout.Button("Remove Ability") && _abilities.Count > 0)
            {
                _abilities.RemoveAt(_abilities.Count - 1);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void CreateNewDatabase()
        {
            InventoryItemDataBase newDatabase = ScriptableObject.CreateInstance<InventoryItemDataBase>();
            string path = EditorUtility.SaveFilePanelInProject(
                "Save Item Database",
                "Item_data_base",
                "asset",
                "Select save location"
            );

            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.CreateAsset(newDatabase, path);
                AssetDatabase.SaveAssets();
                _database = newDatabase;
            }
        }

        private void CreateItem()
        {
            if (string.IsNullOrEmpty(_id))
            {
                EditorUtility.DisplayDialog("Error", "ID cannot be empty", "OK");
                return;
            }

            if (string.IsNullOrEmpty(_itemName))
            {
                EditorUtility.DisplayDialog("Error", "Name cannot be empty", "OK");
                return;
            }

            InventoryItemConfig newItem = ScriptableObject.CreateInstance<InventoryItemConfig>();
            newItem.name = _itemName;

#if UNITY_EDITOR
            newItem.Initialize(
                _id,
                _itemName,
                _sprite,
                _itemType,
                _equipmentType,
                _itemType == ItemType.Equipment ? false : _isStackable,
                _itemType == ItemType.Equipment ? _stats : null,
                new List<BattleAbility>(_abilities),
                _description
            );
#endif

            string databasePath = AssetDatabase.GetAssetPath(_database);
            string folder = System.IO.Path.GetDirectoryName(databasePath);
            string assetPath = $"{folder}/{_itemName}.asset";
            assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            AssetDatabase.CreateAsset(newItem, assetPath);
            AssetDatabase.SaveAssets();

#if UNITY_EDITOR
            _database.AddItem(newItem);
#endif

            EditorUtility.SetDirty(_database);
            AssetDatabase.SaveAssets();
            Selection.activeObject = newItem;

            Debug.Log($"Successfully created item: {_itemName} and added to database");
            ResetForm();
        }

        private void ResetForm()
        {
            _id = "";
            _itemName = "";
            _sprite = null;
            _itemType = ItemType.None;
            _equipmentType = EquipmentType.Head;
            _isStackable = true;
            _stats = new Stats();
            _abilities.Clear();
            _description = "";
        }
    }

}
