using System.Collections.Generic;
using SodaCraft.Localizations;
using UnityEngine;

namespace ShowMoreInfo
{
    public class ItemInfoLocalization
    {
        public const string NoTags = "NoTags";
        public const string NonStackable = "NonStackable";
        public const string Stackable = "Stackable ({0})";
        public const string DurabilityFormat = "Durability: {0}/{1}";
        public const string QualityLabel = "Quality: {0}";
        public const string TagsLabel = "Tags: {0}";

        // 新增开发者信息本地化键（带占位符）
        public const string StatsLabel = "Stats: {0}";
        public const string SlotsLabel = "Slots: {0}";
        public const string InventoryItemsLabel = "InventoryItems: {0}";
        public const string VariablesLabel = "Variables: {0}";

        private static readonly Dictionary<SystemLanguage, Dictionary<string, string>> Translations =
            new Dictionary<SystemLanguage, Dictionary<string, string>>()
        {
            {
                SystemLanguage.ChineseSimplified, new Dictionary<string, string>()
                {
                    { NoTags, "无标签" },
                    { NonStackable, "不可堆叠" },
                    { Stackable, "可堆叠 ({0})" },
                    { DurabilityFormat, "耐久: {0}/{1}" },
                    { QualityLabel, "品质: {0}" },
                    { TagsLabel, "标签: {0}" },

                    // 开发者信息本地化（简体中文）
                    { StatsLabel, "属性/状态效果: {0}" },
                    { SlotsLabel, "插槽/部件: {0}" },
                    { InventoryItemsLabel, "子物品: {0}" },
                    { VariablesLabel, "变量: {0}" }
                }
            },
            {
                SystemLanguage.ChineseTraditional, new Dictionary<string, string>()
                {
                    { NoTags, "無標籤" },
                    { NonStackable, "不可堆疊" },
                    { Stackable, "可堆疊 ({0})" },
                    { DurabilityFormat, "耐久: {0}/{1}" },
                    { QualityLabel, "品質: {0}" },
                    { TagsLabel, "標籤: {0}" },

                    // 開發者資訊本地化（繁體中文）
                    { StatsLabel, "屬性/狀態效果: {0}" },
                    { SlotsLabel, "插槽/部件: {0}" },
                    { InventoryItemsLabel, "子物品: {0}" },
                    { VariablesLabel, "變量: {0}" }
                }
            },
            {
                SystemLanguage.English, new Dictionary<string, string>()
                {
                    { NoTags, "No tags" },
                    { NonStackable, "NonStackable" },
                    { Stackable, "Stackable ({0})" },
                    { DurabilityFormat, "Durability: {0}/{1}" },
                    { QualityLabel, "Quality: {0}" },
                    { TagsLabel, "Tags: {0}" },

                    // Developer info localization (English)
                    { StatsLabel, "Stats: {0}" },
                    { SlotsLabel, "Slots: {0}" },
                    { InventoryItemsLabel, "Inventory Items: {0}" },
                    { VariablesLabel, "Variables: {0}" }
                }
            },
            {
                SystemLanguage.Japanese, new Dictionary<string, string>()
                {
                    { NoTags, "タグなし" },
                    { NonStackable, "スタック不可" },
                    { Stackable, "スタック可能 ({0})" },
                    { DurabilityFormat, "耐久: {0}/{1}" },
                    { QualityLabel, "品質: {0}" },
                    { TagsLabel, "タグ: {0}" },

                    // 開発者情報（日本語）
                    { StatsLabel, "ステータス/効果: {0}" },
                    { SlotsLabel, "スロット/部品: {0}" },
                    { InventoryItemsLabel, "内部アイテム: {0}" },
                    { VariablesLabel, "変数: {0}" }
                }
            },
            {
                SystemLanguage.Korean, new Dictionary<string, string>()
                {
                    { NoTags, "태그 없음" },
                    { NonStackable, "비축불가" },
                    { Stackable, "쌓을 수 있음 ({0})" },
                    { DurabilityFormat, "내구도: {0}/{1}" },
                    { QualityLabel, "품질: {0}" },
                    { TagsLabel, "태그: {0}" },

                    // 개발자 정보 (한국어)
                    { StatsLabel, "스탯/효과: {0}" },
                    { SlotsLabel, "슬롯/부품: {0}" },
                    { InventoryItemsLabel, "내부 아이템: {0}" },
                    { VariablesLabel, "변수: {0}" }
                }
            },
            {
                SystemLanguage.French, new Dictionary<string, string>()
                {
                    { NoTags, "Aucun tag" },
                    { NonStackable, "Non empilable" },
                    { Stackable, "Empilable ({0})" },
                    { DurabilityFormat, "Durabilité: {0}/{1}" },
                    { QualityLabel, "Qualité: {0}" },
                    { TagsLabel, "Tags: {0}" },

                    // Informations développeur (Français)
                    { StatsLabel, "Stats/effets: {0}" },
                    { SlotsLabel, "Emplacements/parties: {0}" },
                    { InventoryItemsLabel, "Objets internes: {0}" },
                    { VariablesLabel, "Variables: {0}" }
                }
            },
            {
                SystemLanguage.Russian, new Dictionary<string, string>()
                {
                    { NoTags, "Без тегов" },
                    { NonStackable, "Не складывается" },
                    { Stackable, "Можно сложить ({0})" },
                    { DurabilityFormat, "Прочность: {0}/{1}" },
                    { QualityLabel, "Качество: {0}" },
                    { TagsLabel, "Теги: {0}" },

                    // Информация для разработчиков (Русский)
                    { StatsLabel, "Статы/эффекты: {0}" },
                    { SlotsLabel, "Слоты/детали: {0}" },
                    { InventoryItemsLabel, "Вложенные предметы: {0}" },
                    { VariablesLabel, "Переменные: {0}" }
                }
            },
            {
                SystemLanguage.German, new Dictionary<string, string>()
                {
                    { NoTags, "Keine Tags" },
                    { NonStackable, "Nicht stapelbar" },
                    { Stackable, "Stapeln ({0})" },
                    { DurabilityFormat, "Haltbarkeit: {0}/{1}" },
                    { QualityLabel, "Qualität: {0}" },
                    { TagsLabel, "Tags: {0}" },

                    // Entwicklerinformationen (Deutsch)
                    { StatsLabel, "Stats/Effekte: {0}" },
                    { SlotsLabel, "Slots/Teile: {0}" },
                    { InventoryItemsLabel, "Interne Gegenstände: {0}" },
                    { VariablesLabel, "Variablen: {0}" }
                }
            },
            {
                SystemLanguage.Spanish, new Dictionary<string, string>()
                {
                    { NoTags, "Sin etiquetas" },
                    { NonStackable, "No apilable" },
                    { Stackable, "Apilable ({0})" },
                    { DurabilityFormat, "Durabilidad: {0}/{1}" },
                    { QualityLabel, "Calidad: {0}" },
                    { TagsLabel, "Etiquetas: {0}" },

                    // Información para desarrolladores (Español)
                    { StatsLabel, "Stats/efectos: {0}" },
                    { SlotsLabel, "Ranuras/partes: {0}" },
                    { InventoryItemsLabel, "Objetos internos: {0}" },
                    { VariablesLabel, "Variables: {0}" }
                }
            }
        };

        public static string GetTranslation(string key)
        {
            var defaultLang = SystemLanguage.English;
            var currentLanguage = LocalizationManager.CurrentLanguage;

            if (Translations.TryGetValue(currentLanguage, out var langDict))
            {
                if (langDict.TryGetValue(key, out var translation))
                {
                    return translation;
                }
            }

            return Translations[defaultLang][key];
        }
    }
}
