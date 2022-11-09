using DevExpress.Utils;
using DevExpress.Utils.DPI;
using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Maps
{
    public static class OverlayUtils
    {
        static MapOverlay medalsOverlay;

        public static MapOverlay MedalsOverlay
        {
            get
            {
                if (medalsOverlay == null)
                    medalsOverlay = CreateMedalsOverlay();
                return medalsOverlay;
            }
        }
        static MapOverlay CreateMedalsOverlay()
        {
            return CreateOverlay("2016 Summer Olympics Medal Result");
        }
        static MapOverlay CreateOverlay(string text)
        {
            MapOverlay overlay = new MapOverlay() { Alignment = ContentAlignment.TopCenter, Margin = new Padding(16) };
            MapOverlayTextItem medalsItem = new MapOverlayTextItem() { Padding = new Padding(5), Text = text };
            medalsItem.TextStyle.Font = new Font(AppearanceObject.DefaultFont.FontFamily, 16, FontStyle.Regular);
            overlay.Items.Add(medalsItem);
            return overlay;
        }

        public static MapOverlay[] GetMedalsOverlay()
        {
            return new MapOverlay[] { MedalsOverlay };
        }
        public static MapOverlayItemBase GetClickedOverlayItem(MapHitInfo hitInfo)
        {
            if (hitInfo.InUIElement)
            {
                MapOverlayHitInfo overlayHitInfo = hitInfo.UiHitInfo as MapOverlayHitInfo;
                if (overlayHitInfo != null)
                    return overlayHitInfo.OverlayItem;
            }
            return null;
        }
    }
    public abstract class OverlayManagerBase : IDisposable
    {
        readonly Dictionary<string, Font> fontsCollection;

        protected Dictionary<string, Font> FontsCollection { get { return fontsCollection; } }

        protected OverlayManagerBase()
        {
            this.fontsCollection = CreateFonts();
        }

        protected abstract Dictionary<string, Font> CreateFonts();

        #region IDisposable implementation
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                IEnumerable<string> keysCollection = new List<string>(this.fontsCollection.Keys);
                foreach (string key in keysCollection)
                {
                    if (fontsCollection[key] != null)
                    {
                        this.fontsCollection[key].Dispose();
                        this.fontsCollection[key] = null;
                    }
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~OverlayManagerBase()
        {
            Dispose(false);
        }
        #endregion
    }


    public class MapEditorOverlayManager : OverlayManagerBase
    {
        const int padding = 8;
        Point fillEditLocation = Point.Empty;
        Point strokeEditLocation = Point.Empty;

        public Point FillEditLocation { get { return fillEditLocation; } }
        public Point StrokeEditLocation { get { return strokeEditLocation; } }

        protected override Dictionary<string, Font> CreateFonts()
        {
            return new Dictionary<string, Font>();
        }
        public void ArrangeOverlays(ScaleHelper scaleDPI, OverlayArrangement[] arrangements, Size colorPickerSize)
        {
            //Rectangle panelRect = arrangements[0].OverlayLayout;
            //Rectangle rulerRect = arrangements[1].OverlayLayout;
            //Rectangle fillRect = arrangements[2].OverlayLayout;
            //Rectangle strokeRect = arrangements[3].OverlayLayout;
            //arrangements[2].OverlayLayout = new Rectangle(new Point(rulerRect.Location.X, fillRect.Location.Y),
            //                                       new Size(fillRect.Width + colorPickerSize.Width, panelRect.Height));
            //int colorPickTop = (int)(panelRect.Top + (panelRect.Height - colorPickerSize.Height) / 2);
            //this.fillEditLocation = new Point(rulerRect.Location.X + fillRect.Width, colorPickTop);
            //int strokeLayoutStart = arrangements[2].OverlayLayout.Right;
            //arrangements[3].OverlayLayout = new Rectangle(strokeLayoutStart, strokeRect.Top,
            //    strokeRect.Width + colorPickerSize.Width, panelRect.Height);
            //this.strokeEditLocation = new Point(strokeLayoutStart + strokeRect.Width, colorPickTop);
            //arrangements[1].OverlayLayout = new Rectangle(new Point(panelRect.Left, panelRect.Bottom + scaleDPI.ScaleVertical(padding)), rulerRect.Size);
        }
    }

}
