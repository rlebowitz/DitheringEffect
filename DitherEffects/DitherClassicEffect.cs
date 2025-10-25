using Dithering.Algorithms;
using Dithering.Palettes;
using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using CheckboxControl = System.Boolean;
using ListBoxControl = System.Byte;

//[assembly: AssemblyTitle("DitherEffect plugin for Paint.NET")]
//[assembly: AssemblyDescription("DitherEffect selected pixels")]
//[assembly: AssemblyConfiguration("dithereffect")]
//[assembly: AssemblyCompany("Robert J Lebowitz")]
//[assembly: AssemblyProduct("DitherEffect")]
//[assembly: AssemblyCopyright("Copyright ©2025 by Robert J Lebowitz")]
//[assembly: AssemblyTrademark("")]
//[assembly: AssemblyCulture("")]
//[assembly: ComVisible(false)]
//[assembly: AssemblyVersion("1.0.*")]
//[assembly: AssemblyMetadata("BuiltByCodeLab", "Version=6.13.9087.35650")]
//[assembly: SupportedOSPlatform("Windows")]

namespace Dithering
{
    public class DitherEffectSupportInfo : IPluginSupportInfo
    {
        public string Author => GetType().Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
        public string Copyright => GetType().Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
        public string DisplayName => GetType().Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
        public Version Version => GetType().Assembly.GetName().Version;
        public Uri WebsiteUri => new Uri("http://github.com/rjlebow");
    }

    [PluginSupportInfo<DitherEffectSupportInfo>(DisplayName = "DitherEffect")]
    public partial class DitherEffectPlugin : PropertyBasedEffect
    {
        public static string StaticName => "DitherEffect";
        public static Image StaticIcon => null;
        public static string SubmenuName => SubmenuNames.Photo;
        private PdnRegion SelectionRegion { get; set; } = null;
        private ErrorDiffusionDithering Algorithm { get; set; } = null;
        private Palette Palette { get; set; } = null;
        private Surface WorkSurface { get; set; } = null;
        private ColorBgra NullColor = ColorBgra.FromBgr(0, 0, byte.MaxValue);

        public DitherEffectPlugin()
#pragma warning disable CS0618 // Type or member is obsolete
            : base(StaticName, StaticIcon, SubmenuName, new EffectOptions { Flags = EffectFlags.Configurable })
#pragma warning restore CS0618 // Type or member is obsolete
        {
            RandomNumberInstanceSeed = unchecked((uint)DateTime.Now.Ticks);
        }

        public enum PropertyNames
        {
            AlgorithmIndex,
            Serpentine,
            PaletteIndex
        }

        public enum AlgorithmIndexOptions
        {
            AlgorithmIndexOption1,
            AlgorithmIndexOption2,
            AlgorithmIndexOption3,
            AlgorithmIndexOption4,
            AlgorithmIndexOption5,
            AlgorithmIndexOption6,
            AlgorithmIndexOption7,
            AlgorithmIndexOption8,
        }

        public enum PaletteIndexOptions
        {
            PaletteIndexOption1,
            PaletteIndexOption2,
            PaletteIndexOption3,
            PaletteIndexOption4,
            PaletteIndexOption5
        }

        #region Random Number Support
        private readonly uint RandomNumberInstanceSeed;
        private uint RandomNumberRenderSeed = 0;
        #endregion


        protected override PropertyCollection OnCreatePropertyCollection()
        {
            List<Property> props = new List<Property>();

            props.Add(StaticListChoiceProperty.CreateForEnum<AlgorithmIndexOptions>(PropertyNames.AlgorithmIndex, 0, false));
            props.Add(new BooleanProperty(PropertyNames.Serpentine, false));
            props.Add(StaticListChoiceProperty.CreateForEnum<PaletteIndexOptions>(PropertyNames.PaletteIndex, 0, false));

            return new PropertyCollection(props);
        }

        protected override ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultConfigUI(props);

            configUI.SetPropertyControlValue(PropertyNames.AlgorithmIndex, ControlInfoPropertyNames.DisplayName, "Dithering Algorithm");
            PropertyControlInfo AlgorithmIndexControl = configUI.FindControlForPropertyName(PropertyNames.AlgorithmIndex);
            AlgorithmIndexControl.SetValueDisplayName(AlgorithmIndexOptions.AlgorithmIndexOption1, "Floyd-Steinberg");
            AlgorithmIndexControl.SetValueDisplayName(AlgorithmIndexOptions.AlgorithmIndexOption2, "Jarvis, Judice and Ninke");
            AlgorithmIndexControl.SetValueDisplayName(AlgorithmIndexOptions.AlgorithmIndexOption3, "Stucki");
            AlgorithmIndexControl.SetValueDisplayName(AlgorithmIndexOptions.AlgorithmIndexOption4, "Burkes");
            AlgorithmIndexControl.SetValueDisplayName(AlgorithmIndexOptions.AlgorithmIndexOption5, "Sierra");
            AlgorithmIndexControl.SetValueDisplayName(AlgorithmIndexOptions.AlgorithmIndexOption6, "Two-row Sierra");
            AlgorithmIndexControl.SetValueDisplayName(AlgorithmIndexOptions.AlgorithmIndexOption7, "Sierra Lite");
            AlgorithmIndexControl.SetValueDisplayName(AlgorithmIndexOptions.AlgorithmIndexOption8, "Atkinson");
            configUI.SetPropertyControlValue(PropertyNames.AlgorithmIndex, ControlInfoPropertyNames.ShowHeaderLine, false);
            configUI.SetPropertyControlValue(PropertyNames.Serpentine, ControlInfoPropertyNames.DisplayName, string.Empty);
            configUI.SetPropertyControlValue(PropertyNames.Serpentine, ControlInfoPropertyNames.Description, "Serpentine");
            configUI.SetPropertyControlValue(PropertyNames.Serpentine, ControlInfoPropertyNames.ShowHeaderLine, false);
            configUI.SetPropertyControlValue(PropertyNames.PaletteIndex, ControlInfoPropertyNames.DisplayName, "Palette");
            PropertyControlInfo PaletteIndexControl = configUI.FindControlForPropertyName(PropertyNames.PaletteIndex);
            PaletteIndexControl.SetValueDisplayName(PaletteIndexOptions.PaletteIndexOption1, "Black and white 2-color palette");
            PaletteIndexControl.SetValueDisplayName(PaletteIndexOptions.PaletteIndexOption2, "Windows 16-color palette");
            PaletteIndexControl.SetValueDisplayName(PaletteIndexOptions.PaletteIndexOption3, "Windows 20-color palette");
            PaletteIndexControl.SetValueDisplayName(PaletteIndexOptions.PaletteIndexOption4, "Apple 16-color palette");
            PaletteIndexControl.SetValueDisplayName(PaletteIndexOptions.PaletteIndexOption5, "RISC OS default 16-color palette");
            configUI.SetPropertyControlValue(PropertyNames.PaletteIndex, ControlInfoPropertyNames.ShowHeaderLine, false);

            return configUI;
        }

        protected override void OnCustomizeConfigUIWindowProperties(PropertyCollection props)
        {
            // Change the effect's window title
            props[ControlInfoPropertyNames.WindowTitle].Value = "Dithering";
            // Add help button to effect UI
            props[ControlInfoPropertyNames.WindowHelpContentType].Value = WindowHelpContentType.PlainText;
            props[ControlInfoPropertyNames.WindowHelpContent].Value = "DitherClassicEffect v1.0\nCopyright ©2025 by \nAll rights reserved.";
            base.OnCustomizeConfigUIWindowProperties(props);
        }

        protected override void OnSetRenderInfo(PropertyBasedEffectConfigToken token, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            AlgorithmIndex = (byte)(int)token.GetProperty<StaticListChoiceProperty>(PropertyNames.AlgorithmIndex).Value;
            Serpentine = token.GetProperty<BooleanProperty>(PropertyNames.Serpentine).Value;
            PaletteIndex = (byte)(int)token.GetProperty<StaticListChoiceProperty>(PropertyNames.PaletteIndex).Value;

            PreRender(dstArgs.Surface, srcArgs.Surface);

            base.OnSetRenderInfo(token, dstArgs, srcArgs);
        }

        protected override unsafe void OnRender(Rectangle[] rectangle, int startIndex, int length)
        {
            if (length == 0) return;

            for (int i = startIndex; i < startIndex + length; ++i)
            {
                Render(DstArgs.Surface,SrcArgs.Surface,rectangle[i]);
            }
        }

        #region User Entered Code
        // Name:
        // Submenu:
        // Author:
        // Title:
        // Version:
        // Desc:
        // Keywords:
        // URL:
        // Help:
        #region UICode
        ListBoxControl AlgorithmIndex = 0; // Dithering Algorithm|Floyd-Steinberg|Jarvis, Judice and Ninke|Fan|4-cell Shiau-Fan|5-cell Shiau-Fan|Stucki|Burkes|Sierra|Two-row Sierra|Sierra Lite|Atkinson
        CheckboxControl Serpentine = false; // Serpentine
        ListBoxControl PaletteIndex = 0; // Palette|Black and white 2-color palette|Microsoft Windows default 16-color palette|Microsoft Windows default 20-color palette|Apple Macintosh default 16-color palette|RISC OS default 16-color palette|Automatic palette|
        #endregion
        // This single-threaded function is called after the UI changes and before the Render function is called
        // The purpose is to prepare anything you'll need in the Render function
        private void PreRender(Surface dst, Surface src)
        {
            SelectionRegion = EnvironmentParameters.GetSelectionAsPdnRegion();
            Algorithm = DitheringCollection.Ditherings[AlgorithmIndex];
            Palette = PaletteCollection.Palettes[PaletteIndex];
            Palette.Clear();
            WorkSurface ??= new Surface(src.Size);
        }


        // Here is the main multi-threaded render function
        // The dst canvas is broken up into rectangles and
        // your job is to write to each pixel of that rectangle
        private unsafe void Render(Surface dst, Surface src, Rectangle rect)
        {
            WorkSurface.CopySurface(src, rect.Location, rect);
            ProcessPixels(rect, Algorithm);
            dst.CopySurface(WorkSurface, rect.Location, rect);
            for (int top = rect.Top; top < rect.Bottom && !IsCancelRequested; ++top)
            {
                ColorBgra* pointerUnchecked = dst.GetPointPointerUnchecked(rect.Left, top);
                for (int left = rect.Left; left < rect.Right; ++left)
                {
                    if (Equals(*pointerUnchecked, ColorBgra.Gray))
                        *pointerUnchecked = NullColor;
                    ++pointerUnchecked;
                }
            }
        }

        private void ProcessPixels(Rectangle rect, IErrorDiffusion dither)
        {
            for (int y = rect.Top; y < rect.Bottom && !IsCancelRequested; ++y)
            {
                for (int x = rect.Left; x != rect.Right; x++)
                {
                    ColorBgra current = WorkSurface[x, y];
                    // apply a dither algorithm to this pixel
                    // assuming it wasn't done before
                   // dither?.Diffuse(WorkSurface, current, x, y, rect);
                }
            }
        }
        //private void X() { 
        //    for (int y = rect.Top; y < rect.Bottom && !IsCancelRequested; ++y)
        //    {
        //        bool leftToRight = !Serpentine || y % 2 == 0;
        //        int x1 = leftToRight ? rect.Left : rect.Right - 1;
        //        int x2 = leftToRight ? rect.Right : rect.Left - 1;
        //        int step = leftToRight ? 1 : -1;
        //        for (int x = x1; x != x2; x += step)
        //        {
        //            if (SelectionRegion.IsVisible(x, y))
        //            {
        //                ColorBgra oldColor = dst[x, y];
        //                ColorBgra newColor = Palette.FindClosestColor(oldColor);
        //                newColor.A = oldColor.A;
        //                dst[x, y] = newColor;
        //                int redError = oldColor.R - newColor.R;
        //                int greenError = oldColor.G - newColor.G;
        //                int blueError = oldColor.B - newColor.B;
        //                for (int i = 0; i < Algorithm.MatrixHeight; i++)
        //                {
        //                    for (int j = 0; j < Algorithm.MatrixWidth; j++)
        //                    {
        //                        int k = y + i;
        //                        int l = x + (j * step) - (Algorithm.MatrixOffset * step);
        //                        double coefficient = Algorithm.Matrix[i, j];
        //                        if (coefficient != 0 && SelectionRegion.IsVisible(l, k))
        //                        {
        //                            ColorBgra color = dst[l, k];
        //                            double redOffset = redError * coefficient;
        //                            double greenOffset = greenError * coefficient;
        //                            double blueOffset = blueError * coefficient;
        //                            color.R = (color.R + redOffset).ToByte();
        //                            color.G = (color.G + greenOffset).ToByte();
        //                            color.B = (color.B + blueOffset).ToByte();
        //                            dst[l, k] = color;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        #endregion
    }
}
