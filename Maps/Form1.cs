using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Maps
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        static Font TitleFont = new Font(AppearanceObject.DefaultFont.FontFamily, 10, FontStyle.Regular);
        readonly MapEditorOverlayManager overlayManager = new MapEditorOverlayManager();
        readonly Dictionary<Type, long> itemIndexes = new Dictionary<Type, long>();
        protected MapOverlay[] Overlays { get { return mapControl1.Overlays.ToArray(); } }
        MapEditor Editor { get { return mapControl1.MapEditor; } }

        public Form1()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            InitializeComponent();
            Editor.MapItemCreating += OnMapItemCreating;


            var path = AppDomain.CurrentDomain.BaseDirectory + "Cache";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            openStreetMapDataProvider1.CacheOptions.DiskFolder = path;
            openStreetMapDataProvider1.CacheOptions.DiskExpireTime = new TimeSpan(1111, 00, 00);
            openStreetMapDataProvider1.CacheOptions.MemoryLimit = 64111;
            openStreetMapDataProvider1.CacheOptions.DiskLimit = 20011;
        }


        void OnMapItemCreating(object sender, MapItemCreatingEventArgs e)
        {
            e.Item.Attributes.Add(new MapItemAttribute() { Name = "name", Value = GenerateName(e.Item) });
            ApplyColors(e.Item);
        }

        void ApplyColors(MapItem item)
        {
            item.Fill = fillEdit.Color;
            item.Stroke = strokeEdit.Color;
        }
        string GenerateName(MapItem item)
        {
            string res = "";
            CControls.InputBox("МФЙ номи", "", ref res);
            return res;
        }

        private void openStreetMapDataProvider1_WebRequest(object sender, DevExpress.XtraMap.MapWebRequestEventArgs e)
        {
            e.UserAgent = "Sample app with OSM tiles";
            e.Referer = "https://www.mycompanysite.com/";
        }



        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (var dialog = new XtraSaveFileDialog())
            {
                dialog.Filter = "KML files|*.kml";
                dialog.CreatePrompt = true;
                dialog.OverwritePrompt = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Editor.ActiveLayer.ExportToKml(dialog.FileName);
                    XtraMessageBox.Show(ParentForm, string.Format("Items successfully exported to {0} file", dialog.FileName), "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnLoad_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (var dialog = new XtraOpenFileDialog())
            {
                dialog.Filter = "KML files|*.kml";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    kmlFileDataAdapter1.FileUri = new Uri("file:\\\\" + dialog.FileName, UriKind.RelativeOrAbsolute);
                    Editor.ActiveLayer = vectorItemsLayer1;
                }
            }
        }

        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string r = "";
            if (CControls.InputBox("Кордината киритинг", "", ref r) == DialogResult.OK)
            {
                string[] spearator = { ",", " " };
                var sa = r.Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
                IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
                mapControl1.CenterPoint = new GeoPoint(double.Parse(sa[0], formatter), double.Parse(sa[1], formatter));
            }
        }
    }
}
