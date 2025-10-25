// Compiler options:  /unsafe /optimize /res:"C:\Users\john\Code experiments\PLUGIN pack stuff\pdn plugin graphics\plugin pack icons\FurBlurIcon.png","FurBlurEffect.FurBlurIcon.png"  /debug- /target:library /out:"C:\Program Files\Paint.NET\Effects\FurBlur.dll"
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using PaintDotNet;
using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

[assembly: AssemblyTitle("FurBlurPlugin")]
[assembly: AssemblyDescription("FurBlur Plugin for Paint.NET. (Compiled by Code Lab v1.8)")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Red ochre (John Robbins)")]
[assembly: AssemblyProduct("FurBlurPlugin")]
[assembly: AssemblyCopyright("Copyright © Red ochre (John Robbins)")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.0.1")]

namespace FurBlurEffect
{
    public class PluginSupportInfo : IPluginSupportInfo
    {
        public string Author
        {
            get
            {
                return "Red ochre (John Robbins)";
            }
        }
        public string Copyright
        {
            get
            {
                return ((AssemblyCopyrightAttribute)base.GetType().Assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;
            }
        }

        public string DisplayName
        {
            get
            {
                return ((AssemblyProductAttribute)base.GetType().Assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0]).Product;
            }
        }

        public Version Version
        {
            get
            {
                return base.GetType().Assembly.GetName().Version;
            }
        }

        public Uri WebsiteUri
        {
            get
            {
                return new Uri("http://www.getpaint.net/redirect/plugins.html");
            }
        }
    }

    [PluginSupportInfo(typeof(PluginSupportInfo), DisplayName = "FurBlur")]
    public class FurBlurEffectPlugin : PropertyBasedEffect
    {// add another surface in "public class ....Plugin : PropertyBasedEffect"
        private Surface destSurface;// added

        protected override void OnDispose(bool disposing)//added
        {
            if (disposing)
            {
                if (destSurface != null)
                {
                    destSurface.Dispose();
                    destSurface = null;
                }
            }

            base.OnDispose(disposing);//added
        }
        public static string StaticName
        {
            get
            {
                return "Furblur";
            }
        }

        public static Image StaticIcon
        {
            get
            {
                return null;
            }
        }

        public FurBlurEffectPlugin()
            : base(StaticName, StaticIcon, "Blurs", EffectFlags.Configurable)//|EffectFlags.SingleThreaded)//single threaded makes NO difference!  ROIs still prevent access to other ROIs
        {
            instanceSeed = unchecked((int)DateTime.Now.Ticks);
        }

        public enum PropertyNames
        {
            StartC,
            Reps,
            MaxL,
            LenV,
            Mang,
            Bid,
            AngV,
            Curv,
            Sooo,
            BTrat,
            ReSeed
        }

        public enum StartColor
        {
            FromSrc,
            FromDst,
            PrimaryColor,
            SecondaryColor
        }

        [ThreadStatic]
        private static Random RandomNumber;

        private int instanceSeed;


        protected override PropertyCollection OnCreatePropertyCollection()
        {
            List<Property> props = new List<Property>();

            props.Add(StaticListChoiceProperty.CreateForEnum<StartColor>(PropertyNames.StartC, 0, false));
            props.Add(new DoubleProperty(PropertyNames.Reps, 10, 0, 100));
            props.Add(new Int32Property(PropertyNames.MaxL, 20, 1, 100));
            props.Add(new DoubleProperty(PropertyNames.LenV, 0, 0, 1));
            props.Add(new DoubleProperty(PropertyNames.Mang, 0, -180, +180));
            props.Add(new BooleanProperty(PropertyNames.Bid, true));
            props.Add(new DoubleProperty(PropertyNames.AngV, 0, 0, 1));
            props.Add(new DoubleProperty(PropertyNames.Curv, 0, 0, 1));
            props.Add(new BooleanProperty(PropertyNames.Sooo, true));
            props.Add(new DoubleProperty(PropertyNames.BTrat, 0, 0, 1));
            props.Add(new Int32Property(PropertyNames.ReSeed, 0, 0, 255));

            return new PropertyCollection(props);
        }

        protected override ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultConfigUI(props);

            configUI.SetPropertyControlValue(PropertyNames.StartC, ControlInfoPropertyNames.DisplayName, "Start colour");
            PropertyControlInfo Amount1Control = configUI.FindControlForPropertyName(PropertyNames.StartC);
            Amount1Control.SetValueDisplayName(StartColor.FromSrc, "from src");
            Amount1Control.SetValueDisplayName(StartColor.FromDst, "from dst (fur on fur)");
            Amount1Control.SetValueDisplayName(StartColor.PrimaryColor, "primary color");
            Amount1Control.SetValueDisplayName(StartColor.SecondaryColor, "secondary color");
            configUI.SetPropertyControlValue(PropertyNames.Reps, ControlInfoPropertyNames.DisplayName, "Number of reps");
            configUI.SetPropertyControlValue(PropertyNames.Reps, ControlInfoPropertyNames.SliderLargeChange, 0.25);
            configUI.SetPropertyControlValue(PropertyNames.Reps, ControlInfoPropertyNames.SliderSmallChange, 0.05);
            configUI.SetPropertyControlValue(PropertyNames.Reps, ControlInfoPropertyNames.UpDownIncrement, 0.01);
            configUI.SetPropertyControlValue(PropertyNames.MaxL, ControlInfoPropertyNames.DisplayName, "Max length");
            configUI.SetPropertyControlValue(PropertyNames.LenV, ControlInfoPropertyNames.DisplayName, "Length variation");
            configUI.SetPropertyControlValue(PropertyNames.LenV, ControlInfoPropertyNames.SliderLargeChange, 0.25);
            configUI.SetPropertyControlValue(PropertyNames.LenV, ControlInfoPropertyNames.SliderSmallChange, 0.05);
            configUI.SetPropertyControlValue(PropertyNames.LenV, ControlInfoPropertyNames.UpDownIncrement, 0.01);
            configUI.SetPropertyControlValue(PropertyNames.Mang, ControlInfoPropertyNames.DisplayName, "Main angle");
            configUI.SetPropertyControlType(PropertyNames.Mang, PropertyControlType.AngleChooser);
            configUI.SetPropertyControlValue(PropertyNames.Bid, ControlInfoPropertyNames.DisplayName, string.Empty);
            configUI.SetPropertyControlValue(PropertyNames.Bid, ControlInfoPropertyNames.Description, "Bi-directional");
            configUI.SetPropertyControlValue(PropertyNames.AngV, ControlInfoPropertyNames.DisplayName, "Angle variation");
            configUI.SetPropertyControlValue(PropertyNames.AngV, ControlInfoPropertyNames.SliderLargeChange, 0.25);
            configUI.SetPropertyControlValue(PropertyNames.AngV, ControlInfoPropertyNames.SliderSmallChange, 0.05);
            configUI.SetPropertyControlValue(PropertyNames.AngV, ControlInfoPropertyNames.UpDownIncrement, 0.01);
            configUI.SetPropertyControlValue(PropertyNames.Curv, ControlInfoPropertyNames.DisplayName, "Curvature");
            configUI.SetPropertyControlValue(PropertyNames.Curv, ControlInfoPropertyNames.SliderLargeChange, 0.25);
            configUI.SetPropertyControlValue(PropertyNames.Curv, ControlInfoPropertyNames.SliderSmallChange, 0.05);
            configUI.SetPropertyControlValue(PropertyNames.Curv, ControlInfoPropertyNames.UpDownIncrement, 0.01);
            configUI.SetPropertyControlValue(PropertyNames.Sooo, ControlInfoPropertyNames.DisplayName, string.Empty);
            configUI.SetPropertyControlValue(PropertyNames.Sooo, ControlInfoPropertyNames.Description, "Start on object only");
            configUI.SetPropertyControlValue(PropertyNames.BTrat, ControlInfoPropertyNames.DisplayName, "Blur ..................................................Trail");
            configUI.SetPropertyControlValue(PropertyNames.BTrat, ControlInfoPropertyNames.SliderLargeChange, 0.25);
            configUI.SetPropertyControlValue(PropertyNames.BTrat, ControlInfoPropertyNames.SliderSmallChange, 0.05);
            configUI.SetPropertyControlValue(PropertyNames.BTrat, ControlInfoPropertyNames.UpDownIncrement, 0.01);
            configUI.SetPropertyControlValue(PropertyNames.ReSeed, ControlInfoPropertyNames.DisplayName, string.Empty);
            configUI.SetPropertyControlType(PropertyNames.ReSeed, PropertyControlType.IncrementButton);
            configUI.SetPropertyControlValue(PropertyNames.ReSeed, ControlInfoPropertyNames.ButtonText, "Reseed button");

            return configUI;
        }

        protected override void OnSetRenderInfo(PropertyBasedEffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            this.startColour = (StartColor)newToken.GetProperty<StaticListChoiceProperty>(PropertyNames.StartC).Value;
            this.repCount = newToken.GetProperty<DoubleProperty>(PropertyNames.Reps).Value;
            this.maxLength = newToken.GetProperty<Int32Property>(PropertyNames.MaxL).Value;
            this.lengthVariation = newToken.GetProperty<DoubleProperty>(PropertyNames.LenV).Value;
            this.mainAngle = newToken.GetProperty<DoubleProperty>(PropertyNames.Mang).Value;
            this.biDirectional = newToken.GetProperty<BooleanProperty>(PropertyNames.Bid).Value;
            this.angleVariation = newToken.GetProperty<DoubleProperty>(PropertyNames.AngV).Value;
            this.curvature = newToken.GetProperty<DoubleProperty>(PropertyNames.Curv).Value;
            this.startOnObject = newToken.GetProperty<BooleanProperty>(PropertyNames.Sooo).Value;
            this.blurAmount = newToken.GetProperty<DoubleProperty>(PropertyNames.BTrat).Value;
            this.randomSeed = (byte)newToken.GetProperty<Int32Property>(PropertyNames.ReSeed).Value;

            //added
            if (destSurface == null)
            {
                destSurface = new Surface(dstArgs.Surface.Width, dstArgs.Surface.Height);
            }

            Rectangle selection = this.EnvironmentParameters.GetSelection(srcArgs.Surface.Bounds).GetBoundsInt();

            this.Render(destSurface, srcArgs.Surface, selection); // clip the rendering to the selection.

            base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
        }


        protected override void OnCustomizeConfigUIWindowProperties(PropertyCollection props)
        {
            // Change the effect's window title
            props[ControlInfoPropertyNames.WindowTitle].Value = "Fur Blur                           Aug 2013 Red Ochre";
            base.OnCustomizeConfigUIWindowProperties(props);
        }

        protected override unsafe void OnRender(Rectangle[] rois, int startIndex, int length)
        {
            if (length == 0) return;

            DstArgs.Surface.CopySurface(destSurface, rois, startIndex, length);
        }

        #region User Entered Code
        /* =================================================== */
        /* Furblur    */
        /* Name    */
        /* (c) 2013 Red Ochre     */
        /* comment   */
        /* Description:random trail/blur */
        /*     */
        /* ========================================== ======== */

        // Name: Furblur
        // Author: Red ochre (John Robbins)
        // Submenu: Blur
        // URL: http://www.getpaint.net/redirect/plugins.html
        // Title:Fur Blur                           Aug 2013 Red Ochre

        #region UICode
        StartColor startColour = StartColor.FromSrc; // Start colour|from src|from dst (fur on fur)|primary color|secondary color
        double repCount = 10; // [0,100] Number of reps
        int maxLength = 20; // [1,100] Max length
        double lengthVariation = 0; // [0,1] Length variation
        double mainAngle = 45; // [-180,180] Main angle
        bool biDirectional = true; // [0,1] Bi-directional
        double angleVariation = 0; // [0,1] Angle variation
        double curvature = 0; // [0,1] Curvature
        bool startOnObject = true; // [0,1] Start on object only
        double blurAmount = 0; // [0,1] Blur ..................................................Trail
        byte randomSeed = 0; // [255] Reseed button
        #endregion

        void Render(Surface dest, Surface src, Rectangle rect)
        {
            dest.CopySurface(src, rect.Location, rect);// copy surface quicker than looping through

            int H = rect.Bottom - rect.Top;
            int W = rect.Right - rect.Left;// should I use selection? - or is rect based on selection?
            int N = (int)((repCount * W * H) / (100));
            int Lmain = maxLength;
            double Lvari = lengthVariation;
            double PI = Math.PI;
            double Amain = mainAngle + 360;// add 360 so that angle can vary less than 0 (360)
            bool Bidi = biDirectional;
            double Avar = angleVariation;
            double Curve = curvature / 10;
            bool Obtest = startOnObject;
            double BlurV = 1 - blurAmount;

            if (RandomNumber == null)
            {
                RandomNumber = new Random(instanceSeed ^ (randomSeed << 16) ^ (rect.X << 8) ^ rect.Y); 
            }

            ColorBgra PrimC = base.EnvironmentParameters.PrimaryColor;
            ColorBgra SecoC = base.EnvironmentParameters.SecondaryColor;

            ColorBgra LT, RT, LB, RB, stLT, stRT, stLB, stRB;
            int nB = 0; int nG = 0; int nR = 0; int nA = 255;// just to set a value

            for (int n = 0; n < N; n++)//number of 'stabs' at random locations
            {
                double Randx = RandomNumber.Next(rect.Left, rect.Right - 1); int Randxi = (int)(Randx);
                double Randy = RandomNumber.Next(rect.Top, rect.Bottom - 1); int Randyi = (int)(Randy);
                // note: start position to allow for 4 pixel square, so Randx < Right - 1 etc
                int Lmax = Lmain + (int)(Lmain * Lvari);
                int Lmin = Lmain - (int)(Lmain * Lvari);
                int L = (int)RandomNumber.Next(Lmin, Lmax);//Length of line
                int AngMax = (int)(Amain * (1 + Avar));
                int AngMin = (int)(Amain * (1 - Avar));
                //protection Min MUST be less than Max or out of bounds exception
                if (AngMin > AngMax) { AngMin = (int)(Amain * (1 + Avar)); AngMax = (int)(Amain * (1 - Avar)); }
                double Angle = (RandomNumber.Next(AngMin, AngMax) * PI) / 180.0;//Angle of line
                int AngDir = (int)RandomNumber.Next(0, 2);//Angle direction random 0 or 1
                if (Bidi) { Angle += (PI * AngDir); }//randomly reverse angle // slight bug here?
                int kB = 1; if (Bidi && (AngDir > 1)) { kB = -1; }
                int CurDir = (int)RandomNumber.Next(0, 2);//Curve direction random 0 or 1
                Curve -= (Curve * CurDir * 2);//small amount to multiply by length to change angle 

                for (int l = 0; l < L; l++)// distance to loop out from that random location
                {
                    double Nx = Randx + (l * Math.Cos(Angle + (l * Curve))); int Lxi = (int)(Nx); int Rxi = Lxi + 1;
                    double Ny = Randy + (l * -Math.Sin(Angle + (l * Curve))); int Tyi = (int)(Ny); int Byi = Tyi + 1;

                    //start position is within bounds, however the loop may take it out of bounds!
                    // therefore protection required or use get sample clamped?

                    if (Lxi > rect.Left && Rxi < rect.Right && Tyi > rect.Top && Byi < rect.Bottom)
                    {

                        double blur = (double)(L - (l * BlurV * kB)) / (double)(L);

                        // need to know start alpha later
                        stLT = src.GetPointUnchecked(Randxi, Randyi);
                        stRT = src.GetPointUnchecked(Randxi + 1, Randyi);
                        stLB = src.GetPointUnchecked(Randxi, Randyi + 1);
                        stRB = src.GetPointUnchecked(Randxi + 1, Randyi + 1);
                        int startA = (stLT.A + stRT.A + stLB.A + stRB.A) / 4;

                        switch (startColour)
                        {
                            case StartColor.FromSrc://from src
                                nB = (stLT.B + stRT.B + stLB.B + stRB.B) / 4;
                                nG = (stLT.G + stRT.G + stLB.G + stRB.G) / 4;
                                nR = (stLT.R + stRT.R + stLB.R + stRB.R) / 4;
                                nA = (stLT.A + stRT.A + stLB.A + stRB.A) / 4;//nA = 255;
                                break;
                            case StartColor.FromDst://from dst
                                stLT = dest.GetPointUnchecked(Randxi, Randyi);
                                stRT = dest.GetPointUnchecked(Randxi + 1, Randyi);
                                stLB = dest.GetPointUnchecked(Randxi, Randyi + 1);
                                stRB = dest.GetPointUnchecked(Randxi + 1, Randyi + 1);
                                nB = (stLT.B + stRT.B + stLB.B + stRB.B) / 4;
                                nG = (stLT.G + stRT.G + stLB.G + stRB.G) / 4;
                                nR = (stLT.R + stRT.R + stLB.R + stRB.R) / 4;
                                nA = (stLT.A + stRT.A + stLB.A + stRB.A) / 4;
                                break;
                            case StartColor.PrimaryColor://from Primary Color
                                nB = PrimC.B; nG = PrimC.G; nR = PrimC.R; nA = PrimC.A;
                                break;
                            case StartColor.SecondaryColor://from Secondary Color
                                nB = SecoC.B; nG = SecoC.G; nR = SecoC.R; nA = SecoC.A;
                                break;
                        }


                        if (!Obtest || startA > 127)// assume objects ahve alpha greater than 127
                        {
                            LT = dest.GetPointUnchecked(Lxi, Tyi); int Blt = LT.B; int Glt = LT.G; int Rlt = LT.R; int Alt = LT.A;
                            RT = dest.GetPointUnchecked(Rxi, Tyi); int Brt = RT.B; int Grt = RT.G; int Rrt = RT.R; int Art = RT.A;
                            LB = dest.GetPointUnchecked(Lxi, Byi); int Blb = LB.B; int Glb = LB.G; int Rlb = LB.R; int Alb = LB.A;
                            RB = dest.GetPointUnchecked(Rxi, Byi); int Brb = RB.B; int Grb = RB.G; int Rrb = RB.R; int Arb = RB.A;

                            double Nxd = Nx - Lxi; double iNxd = 1 - Nxd;
                            double Nyd = Ny - Tyi; double iNyd = 1 - Nyd;

                            double LTk1 = iNxd * iNyd * blur; double LTk2 = 1 - LTk1;
                            double RTk1 = Nxd * iNyd * blur; double RTk2 = 1 - RTk1;
                            double LBk1 = iNxd * Nyd * blur; double LBk2 = 1 - LBk1;
                            double RBk1 = Nxd * Nyd * blur; double RBk2 = 1 - RBk1;


                            //left top
                            int nBlt = (int)((nB * LTk1) + (Blt * LTk2));
                            int nGlt = (int)((nG * LTk1) + (Glt * LTk2));
                            int nRlt = (int)((nR * LTk1) + (Rlt * LTk2));
                            int nAlt = (int)((nA * LTk1) + (Alt * LTk2));
                            //nAlt = 255;
                            LT = ColorBgra.FromBgra(Int32Util.ClampToByte(nBlt), Int32Util.ClampToByte(nGlt), Int32Util.ClampToByte(nRlt), Int32Util.ClampToByte(nAlt));
                            dest[Lxi, Tyi] = LT;

                            //right top
                            int nBrt = (int)((nB * RTk1) + (Brt * RTk2));
                            int nGrt = (int)((nG * RTk1) + (Grt * RTk2));
                            int nRrt = (int)((nR * RTk1) + (Rrt * RTk2));
                            int nArt = (int)((nA * RTk1) + (Art * RTk2));
                            //nArt = 255;
                            RT = ColorBgra.FromBgra(Int32Util.ClampToByte(nBrt), Int32Util.ClampToByte(nGrt), Int32Util.ClampToByte(nRrt), Int32Util.ClampToByte(nArt));
                            dest[Rxi, Tyi] = RT;

                            //left bottom
                            int nBlb = (int)((nB * LBk1) + (Blb * LBk2));
                            int nGlb = (int)((nB * LBk1) + (Glb * LBk2));
                            int nRlb = (int)((nG * LBk1) + (Rlb * LBk2));
                            int nAlb = (int)((nA * LBk1) + (Alb * LBk2));
                            //nAlb = 255;
                            LB = ColorBgra.FromBgra(Int32Util.ClampToByte(nBlb), Int32Util.ClampToByte(nGlb), Int32Util.ClampToByte(nRlb), Int32Util.ClampToByte(nAlb));
                            dest[Lxi, Byi] = LB;

                            //right bottom
                            int nBrb = (int)((nB * RBk1) + (Brb * RBk2));
                            int nGrb = (int)((nG * RBk1) + (Grb * RBk2));
                            int nRrb = (int)((nR * RBk1) + (Rrb * RBk2));
                            int nArb = (int)((nA * RBk1) + (Arb * RBk2));
                            //nArb = 255;
                            RB = ColorBgra.FromBgra(Int32Util.ClampToByte(nBrb), Int32Util.ClampToByte(nGrb), Int32Util.ClampToByte(nRrb), Int32Util.ClampToByte(nArb));
                            dest[Rxi, Byi] = RB;

                        }
                        Angle = Angle + (angleVariation * CurDir);
                    }
                }
            }
        }

        #endregion
    }
}