using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AE教程2
{
    public partial class Form3 : Form
    {
        public AxMapControl mapControl;

        public Form3(AxMapControl pMapControl)
        {
            InitializeComponent();
            mapControl = pMapControl;
            comboBox1.Items.Clear();
            for (int i = 0; i < mapControl.LayerCount; i++)
            {
                comboBox1.Items.Add(mapControl.get_Layer(i).Name);
            }
            comboBox1.Text = comboBox1.Items[0].ToString();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            dataUpdate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataUpdate();
        }

        private void dataUpdate()
        {
            try
            {
                IFeatureLayer pFeatureLayer = mapControl.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
                IFeatureCursor featureCursor = pFeatureLayer.Search(null, false);
                IFeature pFeature = featureCursor.NextFeature();
                DataTable dt = new DataTable();

                DataColumn column = null;
                for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                {
                    column = new DataColumn(pFeature.Fields.get_Field(i).Name);
                    dt.Columns.Add(column);
                }

                DataRow row = null;
                while (pFeature != null)
                {
                    row = dt.NewRow();
                    for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                    {
                        row[i] = pFeature.get_Value(i).ToString();
                    }
                    dt.Rows.Add(row);
                    pFeature = featureCursor.NextFeature();
                }
                dataGridView1.DataSource = dt;
            }
            catch
            {
                MessageBox.Show("无法查询");
            }
        }
    }
}
