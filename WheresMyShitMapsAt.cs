using System;
using System.Collections.Generic;
using System.Linq;
using WheresMyShitMapsAt.Core;
using WheresMyShitMapsAt.Types;
using WheresMyShitMapsAt.Cache;
using WheresMyShitMapsAt.Settings;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.PoEMemory;
using ExileCore.Shared.Enums;
using ExileCore;
using ExileCore.Shared.Helpers;

namespace WheresMyShitMapsAt;

public sealed class WheresMyShitMapsAt : BaseSettingsPlugin<WheresMyShitMapsAtSettings>
{
    private static WheresMyShitMapsAt _instance;
    private static readonly (string Name, global::WheresMyShitMapsAt.Settings.ModType Type)[] DefaultEntries =
    [
        ("reflect", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("cannot Regenerate", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("less Recovery Rate", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("less effect of Non-Curse Auras", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("maximum Player Resistances", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("Elemental Weakness", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("Vulnerability", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("Temporal Chains", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("increased Critical Strike Chance", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("extra Physical Damage as", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("additional Projectiles", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("increased Attack Speed", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("increased Cast Speed", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("increased Area of Effect", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("Avoid Elemental Ailments", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("Hexproof", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("cannot be Taunted", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("reduced effect of Curses", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("reduced Flask Charges gained", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("increased Quantity of Items", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("increased Rarity of Items", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("increased Pack size", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("Area contains many Totems", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("Area contains two Unique Bosses", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("Unique Bosses are Possessed", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("Area is inhabited by Sea Witches", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("Area is inhabited by Humanoids", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("Area is inhabited by Animals", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("Area is inhabited by Undead", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("Area is inhabited by Demons", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("Area contains additional packs", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("Magic Monster Packs", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("Rare Monsters", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("Monsters have increased Life", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("Monsters have increased Movement Speed", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartVoyageSoulEater", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("MapDeepwaterChartVoyageNoEquipmentDrops", global::WheresMyShitMapsAt.Settings.ModType.Bad),
        ("MapDeepwaterChartAdjacentLostMessage", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartAdjacentWisps", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartAdjacentDivinerBox", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartAdjacentArcanistBox", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartAdjacentOperativeBox", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartAdjacentGoldenLanterns", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartAdjacentUniqueRing", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartAdjacentUniqueAmulet", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartAdjacentUniqueBelt", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartAdjacentIncreasedRareMonsters", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartAdjacentIncreasedMagicMonsters", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartAdjacentStrongboxes", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartVoyagePackSize", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartVoyageQuantity", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartVoyageRarity", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartVoyageResourceFound", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartVoyageIncreasedRareMonsters", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartVoyageIncreasedMagicMonsters", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartVoyageMinimumMagicMonsters", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartVoyageMonstersEssenced", global::WheresMyShitMapsAt.Settings.ModType.Good),
        ("MapDeepwaterChartVoyageRareFracture", global::WheresMyShitMapsAt.Settings.ModType.Good)
    ];

    private readonly HighlightCache _highlightCache;
    private readonly MapHighlighter _highlighter;
    private NormalInventoryItem _previewItem = null;

    public static WheresMyShitMapsAt Instance { get => _instance; set => _instance = value; }

    public WheresMyShitMapsAt()
    {
        _highlightCache = new HighlightCache();
        _highlighter = new MapHighlighter();

        Instance = this;
    }

    public override bool Initialise()
    {
        SeedDefaultEntriesOnce();

        _highlighter.Initialise(Graphics);

        return true;
    }

    public override Job Tick()
    {
        if (!Settings.Enable.Value)
            return null;

        var newHighlights = new Dictionary<long, MapHighlightInfo>();

        ProcessInventory(newHighlights);
        ProcessStash(newHighlights);
        ProcessVoyageCharts(newHighlights);

        _highlightCache.Update(newHighlights);

        if (Settings.PreviewHotkey.PressedOnce())
        {
            var element = GameController.IngameState.UIHoverElement;
            if (element?.AsObject<Element>() is { } hoveredElement)
            {
                _previewItem = hoveredElement.AsObject<NormalInventoryItem>();
            }
        }

        return null;
    }

    public NormalInventoryItem GetPreviewItem() => _previewItem;

    private void SeedDefaultEntriesOnce()
    {
        if (!Settings.SeedDefaultEntries.Value || Settings.DefaultEntriesSeeded)
            return;

        if (Settings.Entries.Count == 0)
            EnsureDefaultEntries();

        Settings.DefaultEntriesSeeded = true;
    }

    private void EnsureDefaultEntries()
    {
        foreach (var (name, type) in DefaultEntries)
        {
            var existingEntry = Settings.Entries.FirstOrDefault(entry =>
                entry.Type == type &&
                string.Equals(entry.Name, name, StringComparison.OrdinalIgnoreCase));

            if (existingEntry != null)
            {
                if (Settings.ActivateDefaultEntries.Value)
                    existingEntry.Active = true;

                continue;
            }

            Settings.Entries.Add(new TableEntry(name, type)
            {
                Active = Settings.ActivateDefaultEntries.Value
            });
        }
    }

    private void ProcessInventory(Dictionary<long, MapHighlightInfo> highlights)
    {
        if (!Settings.FilterInventory.Value || !GameController.IngameState.IngameUi.InventoryPanel.IsVisible)
            return;

        var inventoryItems = GameController.IngameState.IngameUi
            .InventoryPanel[InventoryIndex.PlayerInventory]
            .VisibleInventoryItems;

        ProcessItems(inventoryItems, highlights);
    }

    private void ProcessStash(Dictionary<long, MapHighlightInfo> highlights)
    {
        if (!Settings.FilterStash.Value || !GameController.IngameState.IngameUi.StashElement.IsVisible)
            return;

        var visibleStash = GameController.IngameState.IngameUi
            .StashElement
            .VisibleStash;
        
        if (visibleStash == null)
            return;

        var stashItems = visibleStash.VisibleInventoryItems;
        ProcessItems(stashItems, highlights);
    }

    private void ProcessVoyageCharts(Dictionary<long, MapHighlightInfo> highlights)
    {
        if (!Settings.FilterInventory.Value ||
            !Settings.FilterDeepwaterCharts.Value ||
            !Settings.FilterVoyageWindowCharts.Value)
            return;

        var voyageWindow = GameController.IngameState.IngameUi.VoyageWindow;
        if (voyageWindow is not { IsValid: true, IsVisible: true })
            return;

        ProcessItems(voyageWindow.AvailableCharts, highlights);
    }

    private void ProcessItems(
        IEnumerable<NormalInventoryItem> items,
        Dictionary<long, MapHighlightInfo> highlights)
    {
        foreach (var item in items.Where(IsValidTargetItem))
        {
            var mods = item.Item.GetComponent<Mods>();
            var modMatch = MapModMatcher.MatchMods(mods, Settings.Entries);

            if (modMatch.HasAnyMatch)
            {
                highlights[item.Item.Address] = new MapHighlightInfo(
                    Center: item.GetClientRectCache.Center.ToVector2Num(),
                    HasBadMod: modMatch.HasBadMod,
                    HasGoodMod: modMatch.HasGoodMod,
                    Item: item
                );
            }
        }
    }

    public override void Render()
    {
        if (!Settings.Enable.Value)
            return;

        if (!Settings.FilterInventory.Value && !Settings.FilterStash.Value)
            return;

        _highlighter.RenderHighlights(_highlightCache.GetCurrentHighlights(), Settings);
    }

    private bool IsValidTargetItem(NormalInventoryItem inventoryItem)
    {
        try
        {
            var metadata = inventoryItem?.Item?.Metadata;
            var isMap = metadata?.StartsWith("Metadata/Items/Maps/", StringComparison.Ordinal) == true;
            var isDeepwaterChart = metadata?.StartsWith("Metadata/Items/Deepwater/", StringComparison.Ordinal) == true;

            return inventoryItem?.Item != null
                && inventoryItem.Item.TryGetComponent(out Mods mods)
                && ((Settings.FilterMaps.Value && isMap && mods.Identified) ||
                    (Settings.FilterDeepwaterCharts.Value && isDeepwaterChart));
        }
        catch (Exception)
        {
            return false;
        }
    }
}
