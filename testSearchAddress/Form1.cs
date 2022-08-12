using System;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using System.Collections.Generic;

namespace testSearchAddress
{
    public partial class Form1 : Form
    {
        public static string API_KEY = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                System.Net.IPHostEntry e1 =
                     System.Net.Dns.GetHostEntry("www.google.com");
            }
            catch
            {
                gMapControl1.Manager.Mode = AccessMode.CacheOnly;
                MessageBox.Show("No internet connection avaible, going to CacheOnly mode.",
                      "GMap.NET - Demo.WindowsForms", MessageBoxButtons.OK,
                      MessageBoxIcon.Warning);
            }

            // center red cross 제거
            gMapControl1.ShowCenter = false;

            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            gMapControl1.CacheLocation = Application.StartupPath + "data.gmdp";
            GMapProviders.GoogleMap.ApiKey = API_KEY;
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.DragButton = MouseButtons.Left;

            gMapControl1.MaxZoom = 20;
            gMapControl1.MinZoom = 2;
            gMapControl1.Zoom = 16;

        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Button1_Click(sender, e);
            }
        }

        // Search
        private void Button1_Click(object sender, EventArgs e)
        {
            string keyword = textBox1.Text;
            if (keyword.Length > 0)
            {
                GeoCoderStatusCode result = gMapControl1.GetPositionByKeywords(keyword, out PointLatLng point);

                if (result == GeoCoderStatusCode.OK)
                {
                    gMapControl1.Position = point;
                }
            }
        }

        // point -> address 표시
        private void GetAddress(PointLatLng point)
        {
            GeoCoderStatusCode result = GMapProviders.GoogleMap.GetPlacemarks(point, out List<Placemark> place);
            if (result == GeoCoderStatusCode.OK && place != null)
            {
                foreach (var pl in place)
                {
                    if (!string.IsNullOrEmpty(pl.PostalCodeNumber))
                    {
                        Console.WriteLine("Accuracy: " + pl.Accuracy + ", " + pl.Address + ", PostalCodeNumber: " + pl.PostalCodeNumber);
                    }
                }
            }
        }
    }
}
