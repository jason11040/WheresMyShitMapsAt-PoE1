using System.Collections.Generic;
using ExileCore.Shared.Helpers;
using SharpDX;
using WheresMyShitMapsAt.Settings;
using WheresMyShitMapsAt.Types;
using Graphics = ExileCore.Graphics;

namespace WheresMyShitMapsAt.Core;

public sealed class MapHighlighter
{
    private Graphics _graphics;

    public void Initialise(Graphics graphics)
    {
        _graphics = graphics;
    }

    public void RenderHighlights(
        IReadOnlyDictionary<long, MapHighlightInfo> highlights,
        WheresMyShitMapsAtSettings settings)
    {
        var badModColor = settings.BadModColor.Value;
        var goodModColor = settings.GoodModColor.Value;
        var mixedModColor = settings.MixedModColor.Value;

        foreach (var highlight in highlights.Values)
        {
            try
            {
                var color = GetHighlightColor(highlight, badModColor, goodModColor, mixedModColor);
                _graphics.DrawRectFilledMultiColor(
                    highlight.Item.GetClientRectCache.TopLeft.ToVector2Num(),
                    highlight.Item.GetClientRectCache.BottomRight.ToVector2Num(),
                    color,
                    color,
                    color,
                    color);
            }
            catch { }
        }
    }

    private static Color GetHighlightColor(
        MapHighlightInfo highlight,
        Color badModColor,
        Color goodModColor,
        Color mixedModColor)
    {
        if (highlight.HasBadMod && highlight.HasGoodMod)
            return mixedModColor;

        if (highlight.HasBadMod)
            return badModColor;

        return goodModColor;
    }
}
