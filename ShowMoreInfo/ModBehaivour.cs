using Duckov.Modding;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using ReplaceThisWithYourModNameSpace;
using SodaCraft.Localizations;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace ShowMoreInfo
{
    /*
    （中文详细说明）：
    - 在鼠标悬停物品时显示额外信息
    - 如果启用了开发者选项（showDeveloperId），则添加更多的调试/开发者信息行
    - 每个添加的信息行都带有中文注释，解释该字段的含义

    代码步骤：
    1. 当显示物品信息时，构建一个字符串列表 `lines`
    2. 如果 `config.showDeveloperId` 为 true：
       - 添加 `ID`（TypeID）行并注释说明其用途
       - 添加 `Order` 行并注释说明其用途
       - 根据 `Stackable` 添加堆叠计数说明、并注释
       - 添加可售卖、可丢弃、是否有手持代理、是否为角色、是否正在被销毁 等布尔字段，并注释每项含义
       - 如果使用耐久则显示耐久值/最大耐久并注释格式
       - 如果存在声音键则显示并注释
       - 显示显示品质（DisplayQuality）并注释
       - 显示 Stats、Slots、InventoryItems、Variables 的计数并注释这些集合表示什么
    3. 继续添加常规信息：品质、标签、堆叠、耐久、价值等（与原实现一致）
    4. 将 `lines` 列表合并为文本并设置到 `InfoText` 中，应用字体大小和颜色
    */

    [System.Serializable]
    public class ShowMoreInfoConfig
    {
        public bool showQuality = true;        // 显示品质
        public bool showTags = true;           // 显示标签
        public bool showStackable = true;      // 显示堆叠信息
        public bool showDurability = true;     // 显示耐久/可用次数
        public bool showValue = true;          // 显示价值

        public bool showDeveloperId = false;   // 开发者选项：显示物品ID（默认关）

        public float fontSize = 20f;           // 文本字体大小
        public string textColor = "#FFFFFF";   // 文本颜色

        public string configToken = "show_more_info_v1"; // 强制更新 token
    }

    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private TextMeshProUGUI _infoText = null;
        private TextMeshProUGUI InfoText
        {
            get
            {
                if (_infoText == null)
                {
                    _infoText = Instantiate(GameplayDataSettings.UIStyle.TemplateTextUGUI);
                    _infoText.gameObject.SetActive(false);
                }
                return _infoText;
            }
        }

        private ShowMoreInfoConfig config = new ShowMoreInfoConfig();
        private const string MOD_NAME = "ShowMoreInfo";

        void OnEnable()
        {
            ItemHoveringUI.onSetupItem += OnSetupItemHoveringUI;
            ItemHoveringUI.onSetupMeta += OnSetupMeta;

            // 设置 ModConfig
            if (ModConfigAPI.IsAvailable())
            {
                SetupModConfig();
                LoadConfigFromModConfig();
            }

            ModManager.OnModActivated += OnModActivated;
        }

        void OnDisable()
        {
            ItemHoveringUI.onSetupItem -= OnSetupItemHoveringUI;
            ItemHoveringUI.onSetupMeta -= OnSetupMeta;

            ModConfigAPI.SafeRemoveOnOptionsChangedDelegate(OnModConfigOptionsChanged);
            ModManager.OnModActivated -= OnModActivated;
        }

        private void OnModActivated(ModInfo info, Duckov.Modding.ModBehaviour behaviour)
        {
            if (info.name == ModConfigAPI.ModConfigName)
            {
                SetupModConfig();
                LoadConfigFromModConfig();
            }
        }

        private void SetupModConfig()
        {
            if (!ModConfigAPI.IsAvailable()) return;

            ModConfigAPI.SafeAddOnOptionsChangedDelegate(OnModConfigOptionsChanged);

            bool isChinese = new[] { SystemLanguage.Chinese, SystemLanguage.ChineseSimplified, SystemLanguage.ChineseTraditional }
                             .Contains(LocalizationManager.CurrentLanguage);

            ModConfigAPI.SafeAddBoolDropdownList(MOD_NAME, "showQuality", isChinese ? "显示品质" : "Show Quality", config.showQuality);
            ModConfigAPI.SafeAddBoolDropdownList(MOD_NAME, "showTags", isChinese ? "显示标签" : "Show Tags", config.showTags);
            ModConfigAPI.SafeAddBoolDropdownList(MOD_NAME, "showStackable", isChinese ? "显示堆叠信息" : "Show Stackable", config.showStackable);
            ModConfigAPI.SafeAddBoolDropdownList(MOD_NAME, "showDurability", isChinese ? "显示耐久" : "Show Durability", config.showDurability);
            ModConfigAPI.SafeAddBoolDropdownList(MOD_NAME, "showValue", isChinese ? "显示价值" : "Show Value", config.showValue);

            // 新增：开发者选项 - 显示物品ID（默认关闭）
            ModConfigAPI.SafeAddBoolDropdownList(MOD_NAME, "showDeveloperId", isChinese ? "更多开发者选项" : "More Developer info", config.showDeveloperId);

            ModConfigAPI.SafeAddInputWithSlider(MOD_NAME, "fontSize", isChinese ? "文字大小" : "Font Size", typeof(float), config.fontSize, new Vector2(10f, 40f));
            ModConfigAPI.SafeAddInputWithSlider(MOD_NAME, "textColor", isChinese ? "文字颜色" : "Text Color", typeof(string), config.textColor, null);
        }

        private void OnModConfigOptionsChanged(string key)
        {
            if (!key.StartsWith(MOD_NAME + "_")) return;

            LoadConfigFromModConfig();
        }

        private void LoadConfigFromModConfig()
        {
            config.showQuality = ModConfigAPI.SafeLoad<bool>(MOD_NAME, "showQuality", config.showQuality);
            config.showTags = ModConfigAPI.SafeLoad<bool>(MOD_NAME, "showTags", config.showTags);
            config.showStackable = ModConfigAPI.SafeLoad<bool>(MOD_NAME, "showStackable", config.showStackable);
            config.showDurability = ModConfigAPI.SafeLoad<bool>(MOD_NAME, "showDurability", config.showDurability);
            config.showValue = ModConfigAPI.SafeLoad<bool>(MOD_NAME, "showValue", config.showValue);

            // 加载开发者选项
            config.showDeveloperId = ModConfigAPI.SafeLoad<bool>(MOD_NAME, "showDeveloperId", config.showDeveloperId);

            config.fontSize = ModConfigAPI.SafeLoad<float>(MOD_NAME, "fontSize", config.fontSize);
            config.textColor = ModConfigAPI.SafeLoad<string>(MOD_NAME, "textColor", config.textColor);
        }

        private void OnSetupMeta(ItemHoveringUI uiInstance, ItemMetaData data)
        {
            InfoText.gameObject.SetActive(false);
        }

        private void OnSetupItemHoveringUI(ItemHoveringUI uiInstance, Item item)
        {
            if (item == null)
            {
                InfoText.gameObject.SetActive(false);
                return;
            }

            InfoText.gameObject.SetActive(true);
            InfoText.transform.SetParent(uiInstance.LayoutParent);
            InfoText.transform.localScale = Vector3.one;

            List<string> lines = new List<string>();
            //Debug.Log($"Hover Item: {item.DisplayName}");

            // 开发者信息（开启 showDeveloperId 时显示更多调试字段）
            if (config.showDeveloperId)
            {
                // 物品类型唯一标识符（TypeID），用于内部识别和调试
                lines.Add($"ID: {item.TypeID}");

                // 显示物品排序或优先级（Order），可能用于 UI 列表排序
                lines.Add(ShowMoreInfoLocalization.Get(ShowMoreInfoLocalization.OrderLabel, item.Order));

                // 堆叠相关信息：
                // - 如果可堆叠，则显示当前堆叠数量和最大堆叠数
                // - 否则明确标注为不可堆叠
                if (item.Stackable)
                    lines.Add(ShowMoreInfoLocalization.Get(ShowMoreInfoLocalization.StackCountLabel, item.StackCount, item.MaxStackCount)); // 当前数量 / 最大堆叠数

                // 是否可出售（CanBeSold），用于判断是否出现在商店等系统中
                lines.Add(ShowMoreInfoLocalization.Get(ShowMoreInfoLocalization.CanBeSoldLabel, ShowMoreInfoLocalization.FormatBool(item.CanBeSold)));

                // 是否可以丢弃（CanDrop），影响玩家交互（丢弃或扔掉物品）
                lines.Add(ShowMoreInfoLocalization.Get(ShowMoreInfoLocalization.CanDropLabel, ShowMoreInfoLocalization.FormatBool(item.CanDrop)));

                // 是否有手持代理（HasHandHeldAgent），指示物品是否有专门的手持表现或控制器
                lines.Add(ShowMoreInfoLocalization.Get(ShowMoreInfoLocalization.HasHandHeldAgentLabel, ShowMoreInfoLocalization.FormatBool(item.HasHandHeldAgent)));

                // 是否正在被销毁（IsBeingDestroyed），用于调试生命周期问题
                lines.Add(ShowMoreInfoLocalization.Get(ShowMoreInfoLocalization.IsBeingDestroyedLabel, ShowMoreInfoLocalization.FormatBool(item.IsBeingDestroyed)));

                // 声音键（SoundKey），如果物品关联特定声音则显示其键名，便于调试音效
                if (!string.IsNullOrWhiteSpace(item.SoundKey))
                    lines.Add($"SoundKey: {item.SoundKey}");

                // 显示用于 UI 的显示品质（DisplayQuality），通常为NONE
                lines.Add(ShowMoreInfoLocalization.Get(ShowMoreInfoLocalization.DisplayQualityLabel, item.DisplayQuality));
                //lines.Add($"Quality (num): {item.Quality}");

                // 各种集合的计数，用于快速查看该物品是否包含附加数据：
                // - Stats: 属性/状态效果集合数量
                // - Slots: 插槽或部件集合数量
                // - InventoryItems: 物品自身携带的子物品数量（如果是容器）
                // - Variables: 自定义变量数量（用于扩展属性）
                int statsCount = item.Stats != null ? item.Stats.Count : 0;
                int slotsCount = item.Slots != null ? item.Slots.Count : 0;
                int inventoryCount = item.Inventory != null ? item.Inventory.Count() : 0;
                int variablesCount = item.Variables != null ? item.Variables.Count : 0;

                // 仅在数量大于 0 时显示对应行，并使用本地化文本
                if (statsCount > 0)
                    lines.Add(string.Format(ItemInfoLocalization.GetTranslation(ItemInfoLocalization.StatsLabel), statsCount));
                if (slotsCount > 0)
                    lines.Add(string.Format(ItemInfoLocalization.GetTranslation(ItemInfoLocalization.SlotsLabel), slotsCount));
                if (inventoryCount > 0)
                    lines.Add(string.Format(ItemInfoLocalization.GetTranslation(ItemInfoLocalization.InventoryItemsLabel), inventoryCount));
                if (variablesCount > 0)
                    lines.Add(string.Format(ItemInfoLocalization.GetTranslation(ItemInfoLocalization.VariablesLabel), variablesCount));
            }

            // 品质（改为数字+单个星号，例如 1★ 到 9★）
            if (config.showQuality)
            {
                int quality = Mathf.Clamp(item.Quality, 0, 9);
                string qualityText = $"{quality}★";
                lines.Add(string.Format(ItemInfoLocalization.GetTranslation(ItemInfoLocalization.QualityLabel), qualityText));
            }

            // 标签
            if (config.showTags)
            {
                string tags = ItemInfoLocalization.GetTranslation(ItemInfoLocalization.NoTags);
                if (item.Tags != null)
                {
                    try
                    {
                        var tagStrings = item.Tags.Cast<object>().Select(t => t?.ToString()).Where(s => !string.IsNullOrEmpty(s)).ToList();
                        if (tagStrings.Count > 0)
                            tags = string.Join(", ", tagStrings);
                    }
                    catch { }
                }
                lines.Add(string.Format(ItemInfoLocalization.GetTranslation(ItemInfoLocalization.TagsLabel), tags));
            }

            // 堆叠信息
            if (config.showStackable)
                lines.Add(item.MaxStackCount <= 1 ? ItemInfoLocalization.GetTranslation(ItemInfoLocalization.NonStackable) :
                    string.Format(ItemInfoLocalization.GetTranslation(ItemInfoLocalization.Stackable), item.MaxStackCount));

            // 耐久（仅在物品实际使用耐久系统时显示）
            if (config.showDurability && item.UseDurability)
            {
                // 尝试显示为 "当前/最大" 格式，兼容当 MaxDurability 不可用或为 0 的情况
                try
                {
                    // 假设存在 MaxDurability 属性
                    var maxDurability = item.MaxDurability;
                    if (maxDurability > 0)
                    {
                        lines.Add(string.Format(ItemInfoLocalization.GetTranslation(ItemInfoLocalization.DurabilityFormat), item.Durability, maxDurability));
                    }
                    else
                    {
                        // 回退到只显示当前耐久
                        lines.Add(string.Format(ItemInfoLocalization.GetTranslation(ItemInfoLocalization.DurabilityFormat), item.Durability, maxDurability));
                    }
                }
                catch
                {
                    // 如果没有 MaxDurability 属性或读取失败，则仅显示当前耐久（兼容旧版）
                    lines.Add(string.Format(ItemInfoLocalization.GetTranslation(ItemInfoLocalization.DurabilityFormat), item.Durability, item.Durability));
                }
            }

            // 价值
            if (config.showValue && item.Value > 0)
            {
                float totalValue = item.GetTotalRawValue() / 2f;
                lines.Add($"${totalValue}");

                // 新增：价重比显示，参考 Waspite102 
                float weight = item.SelfWeight;
                if (weight > 0)
                {
                    float valuePerKg = totalValue / weight;
                    lines.Add(string.Format("${0:N0}/kg", valuePerKg));
                    //Debug.Log($"weight: {weight}, valuePerKg: {valuePerKg}");
                }
            
            }


            InfoText.text = string.Join("\n", lines);

            InfoText.fontSize = config.fontSize;
            if (ColorUtility.TryParseHtmlString(config.textColor, out var color))
                InfoText.color = color;

            InfoText.alignment = TextAlignmentOptions.Left;
        }
        // 差值颜色格式化
        private string FormatDiff(float diff)
        {
            if (diff > 0) return $"<color=#00FF00>(+{diff:N0})</color>";
            if (diff < 0) return $"<color=#FF4444>({diff:N0})</color>";
            return "";
        }
    }

    // 本地化辅助类：为开发者信息提供中/英文格式化文本
    internal static class ShowMoreInfoLocalization
    {
        public const string OrderLabel = "OrderLabel";
        public const string StackCountLabel = "StackCountLabel";
        public const string CanBeSoldLabel = "CanBeSoldLabel";
        public const string CanDropLabel = "CanDropLabel";
        public const string HasHandHeldAgentLabel = "HasHandHeldAgentLabel";
        public const string IsBeingDestroyedLabel = "IsBeingDestroyedLabel";
        public const string UseDurabilityLabel = "UseDurabilityLabel";
        public const string DisplayQualityLabel = "DisplayQualityLabel";

        private static bool IsChinese()
        {
            return new[] { SystemLanguage.Chinese, SystemLanguage.ChineseSimplified, SystemLanguage.ChineseTraditional }
                   .Contains(LocalizationManager.CurrentLanguage);
        }

        // 将布尔值格式化为本地化文本（中文: 是/否，其他: True/False）
        public static string FormatBool(bool b)
        {
            return IsChinese() ? (b ? "是" : "否") : (b ? "True" : "False");
        }

        // 根据键返回格式化文本，支持传入参数格式化
        public static string Get(string key, params object[] args)
        {
            string template = GetTemplate(key);
            if (args != null && args.Length > 0)
                return string.Format(template, args);
            return template;
        }

        private static string GetTemplate(string key)
        {
            bool cn = IsChinese();
            switch (key)
            {
                case OrderLabel:
                    return cn ? "Order(优先级？): {0}" : "Order: {0}";
                case StackCountLabel:
                    return cn ? "数量: {0}/{1}" : "Count: {0}/{1}";
                case CanBeSoldLabel:
                    return cn ? "可出售: {0}" : "CanBeSold: {0}";
                case CanDropLabel:
                    return cn ? "可丢弃: {0}" : "CanDrop: {0}";
                case HasHandHeldAgentLabel:
                    return cn ? "有手持代理: {0}" : "HasHandHeldAgent: {0}";
                case IsBeingDestroyedLabel:
                    return cn ? "正在销毁: {0}" : "IsBeingDestroyed: {0}";
                case UseDurabilityLabel:
                    return cn ? "使用耐久: {0}" : "UseDurability: {0}";
                case DisplayQualityLabel:
                    return cn ? "显示品质: {0}" : "DisplayQuality: {0}";
                default:
                    return key;
            }
        }
    }
}
