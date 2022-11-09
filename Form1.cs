using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Threading;
using System.Windows.Forms;

namespace AE教程2
{
    public partial class Form1 : Form
    {
        public static Form1 temp;

        public Form1()
        {
            InitializeComponent();
            temp = this;
        }

        private string sMapUnits;
        private void Form1_Load(object sender, EventArgs e)
        {

            sMapUnits = "UnKnown";
            panelniaokan.Visible = false;
            panel1.Visible = false;
            label1.Visible = false;
          
            

        }

        private void axToolbarControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IToolbarControlEvents_OnMouseDownEvent e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void 退出ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定退出吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Application.Exit();
            }

        }

        private void 添加数据AddDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "矢量文件|*.shp|栅格文件|*.bmp; *.jpg; *.tif; *.img; *.dat; *.png;|CAD文件|*.dwg|Gird文件|*。gird|all|*.*";
            ofd.Title = "请选择打开的图形文件";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filename = ofd.FileName;
                int r = filename.LastIndexOf('\\');
                string path = filename.Substring(0, r);
                string file = filename.Substring(r + 1); ;
                string extension = System.IO.Path.GetExtension(filename);
                //加载shp文件
                if (extension == ".shp")
                {
                    axMapControl1.AddShapeFile(path, file);
                    axMapControl1.ActiveView.Refresh();
                }
                //加载gird
                else if (extension == ".gird")
                {
                    IRasterLayer prasterLayer = new RasterLayer(); //引用DataSourcesRaster
                    //读取栅格文件
                    prasterLayer.CreateFromFilePath(path + file);
                    //加载栅格数据
                    axMapControl1.AddLayer(prasterLayer, 0);
                }
                //栅格图形加载
                else if (extension == ".bmp" || extension == ".jpg" || extension == ".tif" || extension == ".img" || extension == ".dat" || extension == ".png")
                {
                    IWorkspaceFactory pWorkspaceFactory = new RasterWorkspaceFactory();
                    //工作工厂 
                    IRasterWorkspace pRasterspace = pWorkspaceFactory.OpenFromFile(path, 0) as IRasterWorkspace;
                    //创建栅格空间
                    IRasterDataset prasterDataset = pRasterspace.OpenRasterDataset(file);
                    //影像金字塔判断与创建

                    IRasterPyramid3 pRaspyramid;
                    pRaspyramid = prasterDataset as IRasterPyramid3;
                    if (pRaspyramid != null)
                    {
                        if (!(pRaspyramid.Present))
                        {
                            pRaspyramid.Create();//创建金字塔
                        }
                    }
                    IRaster praster;
                    praster = prasterDataset.CreateDefaultRaster();
                    IRasterLayer prasterLayer;
                    prasterLayer = new RasterLayerClass();
                    prasterLayer.CreateFromRaster(praster);
                    axMapControl1.AddLayer(prasterLayer);

                }
                //加载dwg文件
                else if (extension == ".dwg")
                {
                    //通过遍历CAD数据集，依次加载中点线面注记等图层，所加载的数据均为一个独立的图层。
                    IWorkspaceFactory pWorkspaceFactory;
                    IFeatureWorkspace pFeatureWorkspace;
                    IFeatureLayer pFeatureLayer;
                    IFeatureDataset pFeatureDataset;
                    //打开CAD数据集
                    pWorkspaceFactory = new CadWorkspaceFactoryClass();
                    pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(path, 0);
                    //打开一个要素集
                    pFeatureDataset = pFeatureWorkspace.OpenFeatureDataset(file);
                    //IFeaturClassContainer可以管理IFeatureDataset中的每个要素类
                    IFeatureClassContainer pFeatureClassContainer = (IFeatureClassContainer)pFeatureDataset;
                    //对CAD文件中的要素进行遍历处理
                    for (int i = 0; i < pFeatureClassContainer.ClassCount - 1; i++)
                    {
                        IFeatureClass pFeatureClass = pFeatureClassContainer.get_Class(i);
                        if (pFeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                        {
                            //如果是注记，则添加注记层
                            pFeatureLayer = new CadAnnotationLayerClass();
                        }
                        else//如果是点、线、面，则添加要素层
                        {
                            pFeatureLayer = new FeatureLayerClass();
                            pFeatureLayer.Name = pFeatureClass.AliasName;
                            pFeatureLayer.FeatureClass = pFeatureClass;
                            this.axMapControl1.Map.AddLayer(pFeatureLayer);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("类型错误", "提示");
                }

            }


        }

        private void 保存SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IMapDocument MapDoc = new MapDocumentClass();
                MapDoc.Open(axMapControl1.DocumentFilename, string.Empty);
                MapDoc.ReplaceContents((IMxdContents)axMapControl1.Object);
                MapDoc.Save(MapDoc.UsesRelativePaths, false);
                MapDoc.Close();
                axMapControl1.ActiveView.Refresh();

            }
            catch
            { }

        }

        private void 打开ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string file = string.Empty;
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Mxd|*.mxd|All|*.*";
                ofd.Title = "请选择打开的文件";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    file = ofd.FileName;
                    axMapControl1.LoadMxFile(file);
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.SetBuddyControl(axMapControl1);
                }
            }
            catch
            {
                MessageBox.Show("打开异常，请重新确认文件能否正常打开");
            }
        }

        private void 另存为SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog pSaveDialog = new System.Windows.Forms.SaveFileDialog();
                pSaveDialog.Title = "另存为";
                pSaveDialog.OverwritePrompt = true;//当相同的文件存在是提示错误
                pSaveDialog.Filter = "ArcMap文档(*.mxd)|*.mxd|ArcMap模板(*.mxt)|*.mxt";
                pSaveDialog.RestoreDirectory = true;
                if (pSaveDialog.ShowDialog() == DialogResult.OK)
                {
                    string sFilePath = pSaveDialog.FileName;
                    IMapDocument pMapDocument = new MapDocumentClass();
                    pMapDocument.New(sFilePath);
                    pMapDocument.ReplaceContents(axMapControl1.Map as IMxdContents);
                    pMapDocument.Save(true, true);
                    pMapDocument.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 视图转换
        /// </summary>
        private void CopyToPageLayout()
        {
            IObjectCopy objectCopy = new ObjectCopyClass();
            object copyFormMap = axMapControl1.Map;
            object copyMap = objectCopy.Copy(copyFormMap);
            object copyToMap = axPageLayoutControl1.ActiveView.FocusMap;
            objectCopy.Overwrite(copyMap, ref copyToMap);

        }


        /// <summary>
        /// 视图转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMapControl1_OnAfterScreenDraw(object sender, IMapControlEvents2_OnAfterScreenDrawEvent e)
        {
            IActiveView activeView = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;
            IDisplayTransformation displayTransformation = activeView.ScreenDisplay.DisplayTransformation;
            displayTransformation.VisibleBounds = axMapControl1.Extent;
            axPageLayoutControl1.ActiveView.Refresh();
            CopyToPageLayout();

        }


        // 鸟瞰图
        private void 鸟瞰图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yingyan();
            panelniaokan.Visible = true;
        }

        /// 新建文档(MXD)
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            #region 鹰眼模组
            IEnvelope pEnvelop = e.newEnvelope as IEnvelope;//将参数e转换成Ienvelop接口对象，相当于把新的空间范围存放
                                                            //pEnvelop接口对象中了
            IGraphicsContainer pGraphicsContainer = axMapControl2.Map as IGraphicsContainer;//获得索引窗口的图形要素容器；
            //该容器可以简单的理解为在索引地图的上面蒙上一层透明的层
            //该层中可以绘制点、线、面、文本等多种图形要素

            pGraphicsContainer.DeleteAllElements();//删除图形要素容器中原来的所有要素
            IElement pElemet = new RectangleElement();//创建一个矩形要素
            pElemet.Geometry = pEnvelop;//使该矩形要素的几何实体等于pEnvelope接口对象，
                                        //相当于该矩形要素的空间范围等于新传进来的空间范围

            IRgbColor pColor = new RgbColor();//创建一个RGB颜色对象
            pColor.Red = 255;//红色值
            pColor.Green = 0;//绿色值
            pColor.Blue = 0;//蓝色值
            pColor.Transparency = 255;//该颜色对象的透明度0~255，0为纯透明，255为不透明

            ILineSymbol pLine = new SimpleLineSymbol();//创建一个简单的线符号
            pLine.Color = pColor;//给该线符号附上颜色，目前为红色
            pLine.Width = 3;//线的宽度为3个像素

            pColor.Transparency = 0;//使颜色对象全透明

            IFillSymbol pFillSymbol = new SimpleFillSymbol();//创建一个简单的填充符号
            pFillSymbol.Color = pColor;//让该填充符号纯透明
            pFillSymbol.Outline = pLine;//给该填充符号的边界附上一个红色的边框

            IFillShapeElement pFillShapeEle = pElemet as IFillShapeElement;//通过接口查询，获取填充要素的接口
            pFillShapeEle.Symbol = pFillSymbol;//给填充要素的符号赋值为刚才创建的具有红色边框的填充符号

            pGraphicsContainer.AddElement((IElement)pFillShapeEle, 0);//将新获得的具有红色边框的填充要素添加进索引窗口的图形要素容器中
            axMapControl2.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);//让索引窗口的图形绘制内容刷新

            #endregion

        }
         
        private IScreenDisplay m_Dispaly;
        private bool m_Pan;

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            #region 实现下方状态显示
            //显示坐标
            CoordinateLabel.Text = " 当前坐标 X = " + e.mapX.ToString() + " Y = " + e.mapY.ToString() + " " + this.axMapControl1.MapUnits;

            // 显示当前比例尺 
            ScaleLabel.Text = " 比例尺 1:" + ((long)this.axMapControl1.MapScale).ToString();

            #endregion

            #region 实现中键平移
            switch (e.button)
            {
                case 1:
                    axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;
                    break;
                case 2:
                    axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;
                    break;
                case 4:
                    IPoint point = new PointClass();
                    point.PutCoords(e.mapX, e.mapY);
                    m_Dispaly = axMapControl1.ActiveView.ScreenDisplay;
                    m_Dispaly.PanStart(point);
                    m_Pan = true;

                    if (axMapControl1.CurrentTool == null)
                    {
                        axMapControl1.MousePointer = esriControlsMousePointer.esriPointerHand;
                        axMapControl1.Pan();
                    }
                    break;
                default:
                    break;
            }
            #endregion

            #region 显示经纬度
            double temx = 0;
            double temy = 0;
            string outx = null;
            string outy = null;
            IActiveView pActiveView = axMapControl1.ActiveView;
            bj54tojingweiduAo(pActiveView, e.mapX, e.mapY, ref outx, ref outy, ref temx, ref temy);
            label1.Text = "地理坐标： 经度：" + outx + " 纬度：" + outy + "\n" + "\n" + "地理坐标 ：经度：" + temx + "纬度：" + temy ;
            //IFeatureLayer pFeatLyr; 
            //pFeatLyr = axMapControl1.Map.get_Layer(2) as IFeatureLayer; 
            //pFeatLyr.DisplayField = "面积"; 
            //pFeatLyr.ShowTips = true; 
            //string pTips; 
            //pTips = pFeatLyr.get_TipText(e.mapX, e.mapY, pActiveView.FullExtent.Width / 100); 
            //toolTip1.SetToolTip(axMapControl1, pTips)
            #endregion

        }


        //坐标系转为北京54
        #region 坐标系转为北京54
        public static void bj54tojingweiduAo(IActiveView pActiveView, double inx, double iny, ref string outx, ref string outy,ref double temx, ref double temy)
        {
            try
            {
                IMap pMap = pActiveView.FocusMap;
                SpatialReferenceEnvironment pSpRE = new SpatialReferenceEnvironment();
                IGeographicCoordinateSystem pGeoCS = pSpRE.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_Beijing1954);
                ISpatialReference pSpr = pGeoCS;
                IPoint pPoint = new ESRI.ArcGIS.Geometry.Point();
                pPoint.X = inx;
                pPoint.Y = iny;
                IGeometry pGeo = pPoint;
                pGeo.SpatialReference = pMap.SpatialReference;
                pGeo.Project(pSpr);//坐标转换，由当前地图坐标转为北京 54 经纬度坐标
                double jwd_jd = pPoint.X;
                double jwd_wd = pPoint.Y;
                //转化成度、分、秒
                //经度
                int Jd, Wd, Jf, Wf;
                double temp;
                Single Jm, Wm;
                Jd = (int)jwd_jd; //度
                temp = (jwd_jd - Jd) * 60;
                Jf = (int)temp; //分
                temp = (temp - Jf) * 60;
                Jm = Convert.ToInt32(temp); //秒
                                            //纬度
                Wd = (int)jwd_wd; //度
                temp = (jwd_wd - Wd) * 60;
                Wf = (int)temp; //分
                temp = (temp - Wf) * 60;
                Wm = Convert.ToInt32(temp); //秒
                outx = Jd + "度" + Jf + "分" + Jm + "秒";
                outy = Wd + "度" + Wf + "分" + Wm + "秒";
                temx = jwd_jd;
                temy = jwd_wd;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region 鹰眼代码
        void yingyan()
        {
            for (int i = 0; i < axMapControl1.ActiveView.FocusMap.LayerCount; i++)//依据axmapcontrol1（主地图窗口）中正处于打开
                                                                                  //状态的地图的图层数进行循环
                axMapControl2.AddLayer(axMapControl1.ActiveView.FocusMap.get_Layer(i));//利用索引窗口控件的addlayer方法依次把主窗口                                                                        //中的图层添加所引窗口中

            axMapControl2.Extent = axMapControl1.FullExtent;//将索引窗口的地图设为整图显示
            axMapControl2.Refresh();//刷新索引窗口
        }
        #endregion

        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            #region 鹰眼代码
            Thread th = new Thread(yingyan);
            th.IsBackground = true;
            th.Start();
            #endregion

            #region 更改坐标单位代码
            esriUnits mapUnits = axMapControl1.MapUnits;
            switch (mapUnits)
            {
                case esriUnits.esriCentimeters:
                    sMapUnits = "Centimeters";
                    break;

                case esriUnits.esriDecimalDegrees:
                    sMapUnits = "Decimal Degrees";
                    break;

                case esriUnits.esriDecimeters:
                    sMapUnits = "Decimeters";
                    break;

                case esriUnits.esriFeet:
                    sMapUnits = "Feet";
                    break;

                case esriUnits.esriInches:
                    sMapUnits = "Inches";
                    break;

                case esriUnits.esriKilometers:
                    sMapUnits = "Kilometers";
                    break;

                case esriUnits.esriMeters:
                    sMapUnits = "Meters";
                    break;

                case esriUnits.esriMiles:
                    sMapUnits = "Miles";
                    break;

                case esriUnits.esriMillimeters:
                    sMapUnits = "Millimeters";
                    break;

                case esriUnits.esriNauticalMiles:
                    sMapUnits = "NauticalMiles";
                    break;

                case esriUnits.esriPoints:
                    sMapUnits = "Points";
                    break;

                case esriUnits.esriUnknownUnits:
                    sMapUnits = "Unknown";
                    break;

                case esriUnits.esriYards:
                    sMapUnits = "Yards";
                    break;
            }
            #endregion

        }

        private void button1_Click(object sender, EventArgs e)
        {
            panelniaokan.Visible = false;
        }


        private void axMapControl2_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            #region 鹰眼代码
            if (e.button == 1)//判定鼠标左键是否按下
            {
                IPoint pPoint = new ESRI.ArcGIS.Geometry.Point();//创建一个点对象
                pPoint.PutCoords(e.mapX, e.mapY);//给点对象的xy赋值为鼠标所在位置的xy坐标
                axMapControl1.CenterAt(pPoint);//将地图主窗口的中心点定位到新的位置
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);//主窗口地图刷新
            }
            else if (e.button == 2)//判定鼠标右键是否按下
            {
                IEnvelope pEnvelop = axMapControl2.TrackRectangle();//调用axmapcontrol2的矩形框跟踪功能进行拉框操作
                //将拉框完成后的范围赋值给一个envelope
                axMapControl1.Extent = pEnvelop;//将新的拉框范围赋值给地图主窗口的空间范围
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);//主窗口地图刷新

            }

            #endregion

        }

        private void 按属性查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            attribute f2 = new attribute(this);
            f2.ShowDialog();
        }

        private void 清空选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;
            this.axMapControl1.Map.ClearSelection();///////////与二维不同
            axMapControl1.ActiveView.Refresh();
        }

        private void 属性toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }



        //创建图层区的右击图层（在MouseUp的控件上进行操作）
        ILayer Player = null;//创建图层对象
        private void axTOCControl1_OnMouseUp(object sender, ITOCControlEvents_OnMouseUpEvent e)
        {
            #region 判断右击菜单位置
            esriTOCControlItem item = new esriTOCControlItem();//创建esri的操作对象

            IBasicMap map = new Map() as IBasicMap;//创建地图对象
            object o = new object();
            axTOCControl1.HitTest(e.x, e.y, ref item, ref map, ref Player, ref o, ref o);

            if (e.button == 2)//右击鼠标
            {
                contextMenuStrip1.Show(axTOCControl1, e.x, e.y);//显示菜单的位置
            }
            #endregion

            #region 实现中键位移
            if (e.button == 4)
            {
                if (m_Dispaly == null)
                {
                    return;
                }
                IEnvelope extent = m_Dispaly.PanStop();
                if (extent != null)
                {
                    m_Dispaly.DisplayTransformation.VisibleBounds = extent;
                    m_Dispaly.Invalidate(null, true, (short)esriScreenCache.esriAllScreenCaches);

                }
                if (axMapControl1.CurrentTool == null)
                {
                    axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
                }
            }
            #endregion

        }

        private void 实时地图对照ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 同屏模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RealMap r1 = new RealMap();
            r1.MdiParent = this;
            r1.Parent = this.axMapControl1;
            r1.Dock = DockStyle.None;
            r1.Show();


        }

        private void 删除toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除吗", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (Player != null)
                {
                    axMapControl1.Map.DeleteLayer(Player);
                }
            }
        }

        private void 重启ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定重启？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);

                Application.Exit();
            }
        }

        private void 窗体模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            RealMap r2 = new RealMap();
            r2.Show();
        }

        //添加下方状态栏
        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            #region 添加状态栏
            // 取得鼠标所在工具的索引号 
            int index = axToolbarControl1.HitTest(e.x, e.y, false);
            if (index != -1)
            {
                // 取得鼠标所在工具的 ToolbarItem 
                IToolbarItem toolbarItem = axToolbarControl1.GetItem(index);
                // 设置状态栏信息 
                MessageLabel.Text = toolbarItem.Command.Message;
            }
            else
            {
                MessageLabel.Text = " 就绪 ";
            }
            #endregion

        }


        
        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {

        }

        private void axMapControl1_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {

        }

        private void 刷新视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yingyan();
        }

        private void axTOCControl1_OnDoubleClick(object sender, ITOCControlEvents_OnDoubleClickEvent e)
        {
            esriTOCControlItem itemType = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap basicMap = null;
            ILayer layer = null;
            object unk = null;
            object data = null;
            axTOCControl1.HitTest(e.x, e.y, ref itemType, ref basicMap, ref layer, ref unk, ref data);
            if (e.button == 1)
            {
                if (itemType == esriTOCControlItem.esriTOCControlItemLegendClass)
                {
                    //取得图例
                    ILegendClass pLegendClass = ((ILegendGroup)unk).get_Class((int)data);
                    //创建符号选择器 SymbolSelector 实例
                    SymbolSelectorForm SymbolSelectorFrm = new SymbolSelectorForm(pLegendClass, layer);
                    if (SymbolSelectorFrm.ShowDialog() == DialogResult.OK)
                    {
                        //局部更新主 Map 控件
                        axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                        //设置新的符号
                        pLegendClass.Symbol = SymbolSelectorFrm.pSymbol;
                        //更新主 Map 控件和图层控件
                        this.axMapControl1.ActiveView.Refresh();
                        this.axTOCControl1.Refresh();
                    }
                }
            }

        }

        private void 开启编辑器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (开启编辑器ToolStripMenuItem.Text == "开启编辑器")
            {
                panel1.Visible = true;
                开启编辑器ToolStripMenuItem.Text = "关闭编辑器";
            }
            else if(开启编辑器ToolStripMenuItem.Text == "关闭编辑器")
            {
                panel1.Visible = false;
                开启编辑器ToolStripMenuItem.Text = "开启编辑器";
            }

        }

        private void 经纬度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (经纬度ToolStripMenuItem.Text == "经纬度")
            {
                label1.Visible = true;
                经纬度ToolStripMenuItem.Text = "关闭经纬度";
            }
            else if (经纬度ToolStripMenuItem.Text == "关闭经纬度")
            {
                label1.Visible = false;
                经纬度ToolStripMenuItem.Text = "经纬度";
            }
        }

        private void 新建Shp图层ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #region 创建shp点元素
        private void 点shpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pointshppath = "";

            SaveFileDialog spointdlg = new SaveFileDialog(); /* 打开保存文件对话框，设置保存路径和shp文件名 */
            spointdlg.Filter = "shp文件|*.shp";

            if (spointdlg.ShowDialog() == DialogResult.OK)

            {
                pointshppath = spointdlg.FileName;
            }
            else
            {
                return;
            }

            /* 准备好要素类空对象 */

            IFeatureClass m_pointfeatureclass = null;

            /* 从文件路径中分解出文件夹路径和文件名称 */

            int count = pointshppath.LastIndexOf("\\");

            string folder = pointshppath.Substring(0, count);

            string name = pointshppath.Substring(count + 1, pointshppath.Length - count - 1);

            //根据文件夹路径创建工作空间工厂和工作空间

            IWorkspace ipws;

            IWorkspaceFactory ipwsf = new ShapefileWorkspaceFactoryClass();

            ipws = ipwsf.OpenFromFile(folder, 0);

            //转为要素工作空间

            IFeatureWorkspace ifeatws;

            ifeatws = ipws as IFeatureWorkspace;

            //对shp文件的一些必要设置，除了红字部分外都不用改

            IFields pFields = new FieldsClass();

            IField pField = new FieldClass();

            IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

            IFieldEdit pFieldEdit = pField as IFieldEdit;

            IGeometryDef ipGeodef = new GeometryDefClass();

            IGeometryDefEdit ipGeodefEdit = ipGeodef as IGeometryDefEdit;

            ISpatialReference ipSpatialRef;

            IUnknownCoordinateSystem iunknowncoord = new UnknownCoordinateSystemClass();

            ipSpatialRef = iunknowncoord;

            ipGeodefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;//确定你要生成的shp的几何类型（点线面）

            ipSpatialRef.SetMDomain(-10000, 10000);

            ipGeodefEdit.HasM_2 = false;

            ipGeodefEdit.HasZ_2 = false;

            ipGeodefEdit.SpatialReference_2 = ipSpatialRef;

            pFieldEdit.Name_2 = "Shape ";

            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

            pFieldEdit.GeometryDef_2 = ipGeodef;

            pFieldsEdit.AddField(pField);

            //////////////////////////////////////////

            //创建要素类并导出shp文件于预设文件路径

            m_pointfeatureclass = ifeatws.CreateFeatureClass(name, pFields, null, null, esriFeatureType.esriFTSimple, "Shape ", " ");

            //加载新创建的shp文件并调到图层显示顺序的最顶层

            axMapControl1.AddShapeFile(folder, name);

            axMapControl1.MoveLayerTo(0, axMapControl1.LayerCount - 1);

            return;
        }
        #endregion

        #region 创建shp线要素
        private void 线要素ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string lineshppath = " ";

            SaveFileDialog slinedlg = new SaveFileDialog();//打开保存文件对话框，设置保存路径和shp文件名
            slinedlg.Filter = "shp文件|*.shp";

            if (slinedlg.ShowDialog() == DialogResult.OK)

            {

                lineshppath = slinedlg.FileName;

            }

            else

            {

                return;

            }

            //准备好要素类空对象

            IFeatureClass m_linefeatureclass = null;

            //从文件路径中分解出文件夹路径和文件名称

            int count = lineshppath.LastIndexOf("\\");

            string folder = lineshppath.Substring(0, count);

            string name = lineshppath.Substring(count + 1, lineshppath.Length - count - 1);

            //根据文件夹路径创建工作空间工厂和工作空间

            IWorkspace ipws;

            IWorkspaceFactory ipwsf = new ShapefileWorkspaceFactoryClass();

            ipws = ipwsf.OpenFromFile(folder, 0);

            //转为要素工作空间

            IFeatureWorkspace ifeatws;

            ifeatws = ipws as IFeatureWorkspace;

            //对shp文件的一些必要设置，除了红字部分外都不用改

            IFields pFields = new FieldsClass();

            IField pField = new FieldClass();

            IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

            IFieldEdit pFieldEdit = pField as IFieldEdit;

            IGeometryDef ipGeodef = new GeometryDefClass();

            IGeometryDefEdit ipGeodefEdit = ipGeodef as IGeometryDefEdit;

            ISpatialReference ipSpatialRef;

            IUnknownCoordinateSystem iunknowncoord = new UnknownCoordinateSystemClass();

            ipSpatialRef = iunknowncoord;

            ipGeodefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;//确定你要生成的shp的几何类型（点线面）

            ipSpatialRef.SetMDomain(-10000, 10000);

            ipGeodefEdit.HasM_2 = false;

            ipGeodefEdit.HasZ_2 = false;

            ipGeodefEdit.SpatialReference_2 = ipSpatialRef;

            pFieldEdit.Name_2 = "Shape ";

            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

            pFieldEdit.GeometryDef_2 = ipGeodef;

            pFieldsEdit.AddField(pField);

            //////////////////////////////////////////

            //创建要素类并导出shp文件于预设文件路径

            m_linefeatureclass = ifeatws.CreateFeatureClass(name, pFields, null, null, esriFeatureType.esriFTSimple, "Shape ", " ");

            //加载新创建的shp文件并调到图层显示顺序的最顶层

            axMapControl1.AddShapeFile(folder, name);

            axMapControl1.MoveLayerTo(0, axMapControl1.LayerCount - 1);

            return;
        }
        #endregion

        #region 创建shp面元素
        private void 面shpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string polygonshppath = " ";

            SaveFileDialog spolygondlg = new SaveFileDialog();//打开保存文件对话框，设置保存路径和shp文件名
            spolygondlg.Filter = "shp文件|*.shp";

            if (spolygondlg.ShowDialog() == DialogResult.OK)

            {

                polygonshppath = spolygondlg.FileName;

            }

            else

            {

                return;

            }

            //准备好要素类空对象

            IFeatureClass m_polygonfeatureclass = null;

            //从文件路径中分解出文件夹路径和文件名称

            int count = polygonshppath.LastIndexOf("\\");

            string folder = polygonshppath.Substring(0, count);

            string name = polygonshppath.Substring(count + 1, polygonshppath.Length - count - 1);

            //根据文件夹路径创建工作空间工厂和工作空间

            IWorkspace ipws;

            IWorkspaceFactory ipwsf = new ShapefileWorkspaceFactoryClass();

            ipws = ipwsf.OpenFromFile(folder, 0);

            //转为要素工作空间

            IFeatureWorkspace ifeatws;

            ifeatws = ipws as IFeatureWorkspace;

            //对shp文件的一些必要设置，除了红字部分外都不用改

            IFields pFields = new FieldsClass();

            IField pField = new FieldClass();

            IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

            IFieldEdit pFieldEdit = pField as IFieldEdit;

            IGeometryDef ipGeodef = new GeometryDefClass();

            IGeometryDefEdit ipGeodefEdit = ipGeodef as IGeometryDefEdit;

            ISpatialReference ipSpatialRef;

            IUnknownCoordinateSystem iunknowncoord = new UnknownCoordinateSystemClass();

            ipSpatialRef = iunknowncoord;

            ipGeodefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;//确定你要生成的shp的几何类型（点线面）

            ipSpatialRef.SetMDomain(-10000, 10000);

            ipGeodefEdit.HasM_2 = false;

            ipGeodefEdit.HasZ_2 = false;

            ipGeodefEdit.SpatialReference_2 = ipSpatialRef;

            pFieldEdit.Name_2 = "Shape ";

            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

            pFieldEdit.GeometryDef_2 = ipGeodef;

            pFieldsEdit.AddField(pField);

            //////////////////////////////////////////

            //创建要素类并导出shp文件于预设文件路径

            m_polygonfeatureclass = ifeatws.CreateFeatureClass(name, pFields, null, null, esriFeatureType.esriFTSimple, "Shape ", " ");

            //加载新创建的shp文件并调到图层显示顺序的最顶层

            axMapControl1.AddShapeFile(folder, name);

            axMapControl1.MoveLayerTo(0, axMapControl1.LayerCount - 1);

            return;
        }
        #endregion


        #region 绘制图形

        //定义一个Operation枚举
        

        private void 开启绘图模块ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           


        }



        #endregion
     

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            
        }

        private void 打开属性表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (axMapControl1.LayerCount > 0)
            {
                Form3 f3 = new Form3(this.axMapControl1);
                f3.Show();
            }

       
        }
    }
}
