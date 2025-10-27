#nullable disable

using Dithering.Algorithms;
using Dithering.Palettes;
using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.Imaging;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using PaintDotNet.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Cryptography.Xml;
using CheckboxControl = System.Boolean;
using ListBoxControl = System.Byte;

[assembly: AssemblyTitle("DitheringEffect plugin for Paint.NET")]
[assembly: AssemblyDescription("Dithering Effect selected pixels")]
[assembly: AssemblyConfiguration("dithering")]
[assembly: AssemblyCompany("Robert J Lebowitz")]
[assembly: AssemblyProduct("DitheringEffect")]
[assembly: AssemblyCopyright("Copyright ©2025 by Robert J Lebowitz")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyMetadata("BuiltByCodeLab", "Version=6.13.9087.35650")]
[assembly: SupportedOSPlatform("Windows")]

namespace Dithering
{
    public class DitheringEffectSupportInfo : IPluginSupportInfo
    {
        public string Author => GetType().Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
        public string Copyright => GetType().Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
        public string DisplayName => GetType().Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
        public Version Version => GetType().Assembly.GetName().Version;
        public Uri WebsiteUri => new("http://github.com/rjlebow/DitheringEffect");
    }

    [PluginSupportInfo<DitheringEffectSupportInfo>(DisplayName = "Dithering Effect")]
    public class DitheringEffectPlugin : PropertyBasedBitmapEffect
    {
        public static string StaticName => "Dithering Effect";
        public static Image StaticIcon => new Bitmap(typeof(DitheringEffectPlugin), "ErrorDiffusionDithering.png");
        public static string SubmenuName => SubmenuNames.Photo;

        public DitheringEffectPlugin()
            : base(StaticName, StaticIcon, SubmenuName, BitmapEffectOptionsFactory.Create() with { IsConfigurable = true })
        {
        }

#nullable enable
        private ErrorDiffusionDithering? ChosenAlgorithm { get; set; }
        private Palette? ChosenPalette { get; set; }
#nullable disable
        public enum PropertyNames
        {
            Algorithm,
            PaletteType,
            RenderingMode
        }

        public enum AlgorithmOptions
        {
            AlgorithmOption1,
            AlgorithmOption2,
            //AlgorithmOption3,
            //AlgorithmOption4,
            //AlgorithmOption5,
            AlgorithmOption6,
            AlgorithmOption7,
            AlgorithmOption8,
            AlgorithmOption9,
            AlgorithmOption10,
            AlgorithmOption11

        }

        public enum PaletteTypeOptions
        {
            PaletteTypeOption1,
            PaletteTypeOption2,
            PaletteTypeOption3,
            PaletteTypeOption4,
            PaletteTypeOption5,
            PaletteTypeOption6,
            PaletteTypeOption7,
            PaletteTypeOption8
        }
        protected override PropertyCollection OnCreatePropertyCollection()
        {
            List<Property> props =
            [
                StaticListChoiceProperty.CreateForEnum<AlgorithmOptions>(PropertyNames.Algorithm, 0, false),
                StaticListChoiceProperty.CreateForEnum<PaletteTypeOptions>(PropertyNames.PaletteType, 0, false),
                new BooleanProperty(PropertyNames.RenderingMode, false)
            ];

            return new PropertyCollection(props);
        }

        protected override ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultConfigUI(props);

            configUI.SetPropertyControlValue(PropertyNames.Algorithm, ControlInfoPropertyNames.DisplayName, "Dithering Algorithm");
            PropertyControlInfo AlgorithmControl = configUI.FindControlForPropertyName(PropertyNames.Algorithm);
            AlgorithmControl.SetValueDisplayName(AlgorithmOptions.AlgorithmOption1, "Floyd-Steinberg");
            AlgorithmControl.SetValueDisplayName(AlgorithmOptions.AlgorithmOption2, "Jarvis, Judice and Ninke");
            //AlgorithmControl.SetValueDisplayName(AlgorithmOptions.AlgorithmOption3, "Fan");
            //AlgorithmControl.SetValueDisplayName(AlgorithmOptions.AlgorithmOption4, "4-cell Shiau-Fan");
            //AlgorithmControl.SetValueDisplayName(AlgorithmOptions.AlgorithmOption5, "5-cell Shiau-Fan");
            AlgorithmControl.SetValueDisplayName(AlgorithmOptions.AlgorithmOption6, "Stucki");
            AlgorithmControl.SetValueDisplayName(AlgorithmOptions.AlgorithmOption7, "Burkes");
            AlgorithmControl.SetValueDisplayName(AlgorithmOptions.AlgorithmOption8, "Sierra");
            AlgorithmControl.SetValueDisplayName(AlgorithmOptions.AlgorithmOption9, "Two-row Sierra");
            AlgorithmControl.SetValueDisplayName(AlgorithmOptions.AlgorithmOption10, "Sierra Lite");
            AlgorithmControl.SetValueDisplayName(AlgorithmOptions.AlgorithmOption11, "Atkinson");
            configUI.SetPropertyControlValue(PropertyNames.Algorithm, ControlInfoPropertyNames.ShowHeaderLine, false);
            configUI.SetPropertyControlValue(PropertyNames.PaletteType, ControlInfoPropertyNames.DisplayName, "Palette");
            PropertyControlInfo PaletteTypeControl = configUI.FindControlForPropertyName(PropertyNames.PaletteType);
            PaletteTypeControl.SetValueDisplayName(PaletteTypeOptions.PaletteTypeOption1, "Black and white 2-color palette");
            PaletteTypeControl.SetValueDisplayName(PaletteTypeOptions.PaletteTypeOption2, "Black,grey and white palette");
            PaletteTypeControl.SetValueDisplayName(PaletteTypeOptions.PaletteTypeOption3, "Black, grey, silver and white palette");
            PaletteTypeControl.SetValueDisplayName(PaletteTypeOptions.PaletteTypeOption4, "Windows 16-color palette");
            PaletteTypeControl.SetValueDisplayName(PaletteTypeOptions.PaletteTypeOption5, "Apple 16-color palette");
            PaletteTypeControl.SetValueDisplayName(PaletteTypeOptions.PaletteTypeOption6, "Windows 20-color palette");
            PaletteTypeControl.SetValueDisplayName(PaletteTypeOptions.PaletteTypeOption7, "RISC OS default 16-color palette");
            PaletteTypeControl.SetValueDisplayName(PaletteTypeOptions.PaletteTypeOption8, "64-color palette");
            configUI.SetPropertyControlValue(PropertyNames.PaletteType, ControlInfoPropertyNames.ShowHeaderLine, false);
            configUI.SetPropertyControlValue(PropertyNames.RenderingMode, ControlInfoPropertyNames.DisplayName, string.Empty);
            configUI.SetPropertyControlValue(PropertyNames.RenderingMode, ControlInfoPropertyNames.Description, "Enable Rendering");
            configUI.SetPropertyControlValue(PropertyNames.RenderingMode, ControlInfoPropertyNames.ShowHeaderLine, false);

            return configUI;
        }

        protected override void OnCustomizeConfigUIWindowProperties(PropertyCollection props)
        {
            // Change the effect's window title
            props[ControlInfoPropertyNames.WindowTitle].Value = "Dithering Effects";
            // Add help button to effect UI
            props[ControlInfoPropertyNames.WindowHelpContentType].Value = WindowHelpContentType.PlainText;
            props[ControlInfoPropertyNames.WindowHelpContent].Value = "Dithering Effects v1.0\nCopyright ©2025 by Robert J Lebowitz\nAll rights reserved.";
            base.OnCustomizeConfigUIWindowProperties(props);
        }
        /// <summary>
        /// Method is called only once per effect instance, before rendering starts.
        /// </summary>
        /// <param name="renderInfo"></param>
        protected override void OnInitializeRenderInfo(IBitmapEffectRenderInfo renderInfo)
        {
            //https://forums.getpaint.net/topic/134083-bitmap-effect-how-to-call-blur/#comment-647850
            renderInfo.Schedule = BitmapEffectRenderingSchedule.None;
            base.OnInitializeRenderInfo(renderInfo);
        }

        protected override void OnSetToken(PropertyBasedEffectConfigToken newToken)
        {
            Algorithm = (byte)(int)newToken.GetProperty<StaticListChoiceProperty>(PropertyNames.Algorithm).Value;
            PaletteType = (byte)(int)newToken.GetProperty<StaticListChoiceProperty>(PropertyNames.PaletteType).Value;
            ChosenAlgorithm = DitheringCollection.Ditherings[Algorithm];
            ChosenPalette = PaletteCollection.Palettes[PaletteType];
            ChosenPalette.Clear();
            Render = newToken.GetProperty<BooleanProperty>(PropertyNames.RenderingMode).Value;
            base.OnSetToken(newToken);
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

        // For help writing a Bitmap plugin: https://boltbait.com/pdn/CodeLab/help/tutorial/bitmap/

        #region UICode
        ListBoxControl Algorithm = 0; // Dithering Algorithm|Floyd-Steinberg|Jarvis, Judice and Ninke|Stucki|Burkes|Sierra|Two-row Sierra|Sierra Lite|Atkinson
        ListBoxControl PaletteType = 0; // Palette|Black and white 2-color palette|Windows 16-color palette|Windows 20-color palette|Apple 16-color paletteApple 16-color palette|RISC OS default 16-color palette
        CheckboxControl Render = false; // Enable Rendering
        #endregion

        protected override void OnRender(IBitmapEffectOutput output)
        {
            using IEffectInputBitmap<ColorBgra32> sourceBitmap = Environment.GetSourceBitmapBgra32();
            using IBitmapLock<ColorBgra32> sourceLock = Environment.GetSourceBitmapBgra32().Lock(new RectInt32(0, 0, sourceBitmap.Size));
            RegionPtr<ColorBgra32> sourceRegion = sourceLock.AsRegionPtr();

            RectInt32 outputBounds = output.Bounds;
            using IBitmapLock<ColorBgra32> outputLock = output.LockBgra32();
            RegionPtr<ColorBgra32> outputSubRegion = outputLock.AsRegionPtr();
            var outputRegion = outputSubRegion.OffsetView(-outputBounds.Location);

            if (!Render)
            {
                for (int y = outputBounds.Top; y < outputBounds.Bottom; ++y)
                {
                    if (IsCancelRequested) return;

                    for (int x = outputBounds.Left; x < outputBounds.Right; ++x)
                    {
                        outputRegion[x, y] = sourceRegion[x, y];
                    }
                }
                return;
            }

            IImagingFactory factory = Services.GetService<IImagingFactory>();  // don't use Dispose on services
            using IBitmap<ColorBgra32> workBitmap = factory.CreateBitmap<ColorBgra32>(sourceBitmap.Size);
            using IBitmapLock<ColorBgra32> workLock = workBitmap.Lock(new RectInt32(0, 0, sourceBitmap.Size), BitmapLockOptions.ReadWrite);
            RegionPtr<ColorBgra32> workRegion = workLock.AsRegionPtr();
            //   workRegion.Fill(Color.White);

            sourceRegion.CopyTo(workRegion);
            ColorBgra32 current = Color.Transparent;
            // Alter the palette for the work region
            for (int y = outputBounds.Top; y < outputBounds.Bottom; ++y)
            {
                if (IsCancelRequested) return;

                for (int x = outputBounds.Left; x < outputBounds.Right; ++x)
                {
                    // Get the workspace pixel
                    current = workRegion[x, y];
                    workRegion[x, y] = ChosenPalette.FindClosestColor(workRegion[x, y]);
                }
            }
            // Dither the image
            for (int y = outputBounds.Top; y < outputBounds.Bottom; ++y)
            {
                if (IsCancelRequested) return;

                for (int x = outputBounds.Left; x < outputBounds.Right; ++x)
                {
                    ChosenAlgorithm?.Diffuse(workRegion, current, x, y, outputBounds);
                    // Save your pixel to the output canvas
                    outputRegion[x, y] = workRegion[x, y];
                }
            }

        }


        #endregion
    }
}
