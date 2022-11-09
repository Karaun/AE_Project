using ESRI.ArcGIS.Carto;
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
    public partial class attribute : Form
    {
        public attribute(Form1 f1)
        {
            this.mainfrm = f1;

            InitializeComponent();
        }

        Form1 mainfrm;

        ILayer pLayer = null;

        private void attribute_Load(object sender, EventArgs e)
        {
            try
            {
                IMap Map = mainfrm.axMapControl1.Map;
                int MapCount = mainfrm.axMapControl1.LayerCount;

                //从主图层中添加图层名称到cboLayer中
                for (int i = 0; i < MapCount; i++)
                {
                    ILayer pLayer = mainfrm.axMapControl1.get_Layer(i);
                    if (pLayer is IFeatureLayer)
                    {
                        cboLayers.Items.Add(pLayer.Name);
                    }

                }

                //lstopts中添加运算符
                lstopts.Items.Add(">");
                lstopts.Items.Add("<");
                lstopts.Items.Add("=");
                lstopts.Items.Add("<>");
                cboLayers.SelectedIndex = 0;
            }
            catch
            { }

        }



        private void lstFields_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstFields_DoubleClick_1(object sender, EventArgs e)
        {
            txtWhereClause.Text += lstFields.Text;
        }

        private void cboLayers_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            lstFields.Items.Clear();//优先清空使用界面

            int MapCount = mainfrm.axMapControl1.LayerCount;//创造图层数的整形变量

            for (int i = 0; i < MapCount; i++)
            {
                ILayer layer = mainfrm.axMapControl1.get_Layer(i);
                if (layer.Name == cboLayers.Text)//若图层名相同，显示图层要素
                {
                    pLayer = layer;
                }
            }

            if (pLayer == null)
            {
                MessageBox.Show("数据有误，无法执行");
                return;
            }
            IFeatureLayer pFtlayer = pLayer as IFeatureLayer;//要素对象

            IFeatureClass pFtClass = pFtlayer.FeatureClass;//要素类

            for (int i = 0; i < pFtClass.Fields.FieldCount; i++)
            {
                string Name = pFtClass.Fields.get_Field(i).Name;
                lstFields.Items.Add(Name);
            }

            label3.Text = cboLayers.Text;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            lstvalue.Items.Clear();

            IFeatureClass pFtClass = (pLayer as IFeatureLayer).FeatureClass;
            IFeatureCursor pFtCursor = pFtClass.Search(null, false);
            //如果为true,即Cursor每一次都从第一个开始，导致重复查询，若为false，游标每次停留最后一个；
            IFeature pFt = pFtCursor.NextFeature();
            int FieldsIndex = pFtClass.FindField(lstFields.Text);
            while (pFt != null)
            {
                string value = pFt.get_Value(FieldsIndex).ToString();
                if (pFt.Fields.get_Field(FieldsIndex).Type == esriFieldType.esriFieldTypeString)
                    value = "'" + value + "'";
                lstvalue.Items.Add(value);

                pFt = pFtCursor.NextFeature();
            }
        }

        private void lstopts_DoubleClick_1(object sender, EventArgs e)
        {
            txtWhereClause.Text += " " + lstopts.Text;
        }

        private void lstvalue_DoubleClick_1(object sender, EventArgs e)
        {
            txtWhereClause.Text += " " + lstvalue.Text;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                mainfrm.axMapControl1.Map.ClearSelection();
                mainfrm.axMapControl1.ActiveView.Refresh();

                IFeatureClass pFtClass = (pLayer as IFeatureLayer).FeatureClass;
                IQueryFilter pFilter = new QueryFilter() as IQueryFilter;
                pFilter.WhereClause = txtWhereClause.Text;
                IFeatureCursor pFtCursor = pFtClass.Search(pFilter, false);
                IFeature pFt = pFtCursor.NextFeature();
                while (pFt != null)
                {
                    mainfrm.axMapControl1.Map.SelectFeature(pLayer, pFt); //选择要素，高亮显示
                    pFt = pFtCursor.NextFeature();
                }
                mainfrm.axMapControl1.ActiveView.Refresh();
            }
            catch
            { }
        }
    }
}
