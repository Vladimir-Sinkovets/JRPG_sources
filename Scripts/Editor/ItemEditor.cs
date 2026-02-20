//#if UNITY_EDITOR
//using Assets.Game.Scripts.Locations.Inventories;
//using TriInspector.Editors;
//using UnityEditor;
//using UnityEditor.AddressableAssets;
//using UnityEditor.AddressableAssets.Settings;
//using UnityEngine;

//[CustomEditor(typeof(InventoryItemConfig))]
//public class InventoryItemEditor : TriEditor
//{
//    protected override void OnEnable()
//    {
//        base.OnEnable();

//        InventoryItemConfig InventoryItem = (InventoryItemConfig)target;
//        SerializedObject so = new SerializedObject(InventoryItem);

//        // Генерируем ключ только если он пустой
//        if (string.IsNullOrEmpty(InventoryItem.AddressableKey))
//        {
//            // Получаем настройки Addressables
//            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

//            // Получаем GUID ассета
//            string assetPath = AssetDatabase.GetAssetPath(InventoryItem);
//            string guid = AssetDatabase.AssetPathToGUID(assetPath);

//            // Проверяем, добавлен ли уже ассет в Addressables
//            AddressableAssetEntry entry = settings.FindAssetEntry(guid);

//            if (entry == null)
//            {
//                // Добавляем в группу "InventoryItems" если не добавлен
//                AddressableAssetGroup group = settings.FindGroup("InventoryItems") ?? settings.DefaultGroup;
//                entry = settings.CreateOrMoveEntry(guid, group);
//            }

//            // Устанавливаем адрес = имени ассета
//            entry.address = InventoryItem.name;

//            // Сохраняем ключ в ScriptableObject
//            SerializedProperty keyProp = so.FindProperty("_addressableKey");
//            keyProp.stringValue = entry.address;
//            so.ApplyModifiedProperties();

//            // Сохраняем изменения
//            EditorUtility.SetDirty(InventoryItem);
//            AssetDatabase.SaveAssets();
//            Debug.Log($"Addressable key set: {InventoryItem.name} -> {entry.address}");
//        }
//    }
//}
//#endif