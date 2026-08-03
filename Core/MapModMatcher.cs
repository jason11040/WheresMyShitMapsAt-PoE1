using System;
using System.Collections.Generic;
using System.Linq;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using WheresMyShitMapsAt.Settings;

namespace WheresMyShitMapsAt.Core;

public sealed class MapModMatcher
{
    public readonly record struct ModMatchResult(bool HasBadMod, bool HasGoodMod)
    {
        public bool HasAnyMatch => HasBadMod || HasGoodMod;
    }

    public static ModMatchResult MatchMods(Mods mods, IEnumerable<TableEntry> entries)
    {
        var activeEntries = entries.Where(x => x.Active).ToList();

        if (activeEntries.Count == 0)
            return new ModMatchResult(false, false);

        var badMods = activeEntries.Where(x => x.Type == ModType.Bad);
        var goodMods = activeEntries.Where(x => x.Type == ModType.Good);

        return new ModMatchResult(
            HasBadMod: HasMatchingMod(mods, badMods),
            HasGoodMod: HasMatchingMod(mods, goodMods)
        );
    }

    private static bool HasMatchingMod(
        Mods mods,
        IEnumerable<TableEntry> targetMods)
    {
        var modTexts = GetSearchableModTexts(mods).ToList();
        var targets = targetMods.ToList();

        if (modTexts.Count == 0 || targets.Count == 0)
            return false;

        return targets.Any(entry =>
            modTexts.Any(modText =>
                modText.Contains(entry.Name, StringComparison.OrdinalIgnoreCase)
            )
        );
    }

    public static IEnumerable<string> GetSearchableModTexts(Mods mods)
    {
        return GetModTexts(mods.HumanStats)
            .Concat(GetModTexts(mods.HumanImpStats))
            .Concat(GetModTexts(mods.HumanCraftedStats))
            .Concat(GetModTexts(mods.EnchantedStats))
            .Concat(GetModTexts(mods.FracturedStats))
            .Concat(GetModTexts(mods.CrucibleStats))
            .Concat(GetModTexts(mods.ExplicitMods))
            .Distinct(StringComparer.OrdinalIgnoreCase);
    }

    private static IEnumerable<string> GetModTexts(IEnumerable<string> mods)
    {
        return mods
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim());
    }

    private static IEnumerable<string> GetModTexts(IEnumerable<ItemMod> mods)
    {
        return mods.SelectMany(mod => new[]
            {
                mod.DisplayName,
                mod.Translation,
                mod.RawName,
                mod.Name
            })
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim());
    }
}
