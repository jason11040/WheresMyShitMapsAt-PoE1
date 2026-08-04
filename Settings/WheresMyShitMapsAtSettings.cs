using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using System.Collections.Generic;
using System.Windows.Forms;
using SharpDX;

namespace WheresMyShitMapsAt.Settings;

public sealed class WheresMyShitMapsAtSettings : ISettings
{
    public ToggleNode Enable { get; set; } = new(false);
    public HotkeyNodeV2 PreviewHotkey { get; set; } = new(Keys.None);
    public ToggleNode FilterStash { get; set; } = new(false);
    public ToggleNode FilterInventory { get; set; } = new(true);
    public ToggleNode FilterMaps { get; set; } = new(true);
    public ToggleNode FilterDeepwaterCharts { get; set; } = new(true);
    public ToggleNode FilterVoyageWindowCharts { get; set; } = new(true);
    public ToggleNode SeedDefaultEntries { get; set; } = new(true);
    public ToggleNode ActivateDefaultEntries { get; set; } = new(true);
    public ColorNode BadModColor { get; set; } = new Color(255, 0, 0, 100);
    public ColorNode GoodModColor { get; set; } = new Color(0, 255, 0, 100);
    public ColorNode MixedModColor { get; set; } = new Color(255, 200, 0, 120);
    public List<TableEntry> Entries { get; set; } = [];
    public WheresMyShitMapsAtSettingsMenu Menu { get; set; }

    public WheresMyShitMapsAtSettings()
    {
        Menu = new WheresMyShitMapsAtSettingsMenu(this);
    }
}
