using DevExpress.XtraMap;
using SpreadCommander.Common.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
    [Cmdlet(VerbsCommon.Add, "MapLayerVectorData")]
    public class AddMapLayerVectorDataCmdlet : BaseMapWithContextCmdlet
    {
        [Parameter(HelpMessage = "Layer's name.")]
        public string Name { get; set; }

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Data source containing map's vector items.")]
        public PSObject DataSource { get; set; }

        [Parameter(HelpMessage = "When set - layer is added to mini map.")]
        public SwitchParameter MiniMap { get; set; }

        [Parameter(HelpMessage = "Default map item type.")]
        public MapItemType DefaultMapItemType { get; set; }

        [Parameter(HelpMessage = "Index of the image assigned to the map item.")]
        public int? ItemImageIndex { get; set; }

        [Parameter(HelpMessage = "Color used to fill a map item.")]
        public string FillColor { get; set; }

        [Parameter(HelpMessage = "Map item's font.")]
        public string Font { get; set; }

        [Parameter(HelpMessage = "Color that specifies how the map item outline is painted.")]
        public string StrokeColor { get; set; }

        [Parameter(HelpMessage = "Width of the stroke on the current map item.")]
        public int? StrokeWidth { get; set; }

        [Parameter(HelpMessage = "Map item's text glow color.")]
        public string TextGlowColor { get; set; }

        [Parameter(HelpMessage = "Pattern used to generate shape titles.")]
        public string ShapeTitlesPattern { get; set; }

        [Parameter(HelpMessage = "Data field to which Latitude is bound.")]
        [PSDefaultValue(Value = "Latitude")]
        [DefaultValue("Latitude")]
        public string LatitudeField { get; set; } = "Latitude";

        [Parameter(HelpMessage = "Data field to which Longitude is bound.")]
        [PSDefaultValue(Value = "Longitude")]
        [DefaultValue("Longitude")]
        public string LongitudeField { get; set; } = "Longitude";

        [Parameter(HelpMessage = "Data field to which ImageIndex is bound.")]
        public string ImageIndexField { get; set; }

        [Parameter(HelpMessage = "Data field to which Text is bound.")]
        public string TextField { get; set; }

        [Parameter(HelpMessage = "Data field to which MapItemType is bound. Data source shall return integer: 1 - dot, 2 - ellipse, 3 - line, 4 - path, 5 - polygon, 6 - polyline, 7 - rectangle, 8 - pushpin, 10 - callout.")]
        public string MapItemTypeField { get; set; }

        [Parameter(HelpMessage = "Data field to which X coordinate is bound.")]
        public string XCoordinateField { get; set; }

        [Parameter(HelpMessage = "Data field to which Y coordinate is bound.")]
        public string YCoordinateField { get; set; }

        [Parameter(HelpMessage = "Data field to which Bubble group is bound.")]
        public string BubbleGroupField { get; set; }

        [Parameter(HelpMessage = "Data field to which pie segment is bound.")]
        public string PieSegmentField { get; set; }

        [Parameter(HelpMessage = "Data field to which pie value is bound.")]
        public string ValueField { get; set; }

        [Parameter(HelpMessage = "Default item color.")]
        public string DefaultItemColor { get; set; }

        [Parameter(HelpMessage = "Item color's field.")]
        public string ItemColorField { get; set; }

        [Parameter(HelpMessage = "Default stroke color.")]
        public string DefaultStrokeColor { get; set; }

        [Parameter(HelpMessage = "Stroke color's field.")]
        public string StrokeColorField { get; set; }

        [Parameter(HelpMessage = "Default dot shape kind.")]
        public MapDotShapeKind? DefaultDotShapeKind { get; set; }

        [Parameter(HelpMessage = "Dot shape kind's field. Data source shall return an integer: 0 - for rectangle, 1 - for circle.")]
        public string DotShapeKindField { get; set; }

        [Parameter(HelpMessage = "Default dot size.")]
        public double? DefaultDotSize { get; set; }

        [Parameter(HelpMessage = "Dot size's field.")]
        public string DotSizeField { get; set; }

        [Parameter(HelpMessage = "Default ellipse height.")]
        public double? DefaultEllipseHeight { get; set; }

        [Parameter(HelpMessage = "Ellipse height's field.")]
        public string EllipseHeightField { get; set; }

        [Parameter(HelpMessage = "Default ellipse width.")]
        public double? DefaultEllipseWidth { get; set; }

        [Parameter(HelpMessage = "Ellipse width's field.")]
        public string EllipseWidthField { get; set; }

        [Parameter(HelpMessage = "Default rectangle height.")]
        public double? DefaultRectangleHeight { get; set; }

        [Parameter(HelpMessage = "Rectangle height's field.")]
        public string RectangleHeightField { get; set; }

        [Parameter(HelpMessage = "Default rectangle width.")]
        public double? DefaultRectangleWidth { get; set; }

        [Parameter(HelpMessage = "Rectangle width's field.")]
        public string RectangleWidthField { get; set; }


        protected override void UpdateMap()
        {
            var adapter = new ListSourceDataAdapter()
            {
                DefaultMapItemType = (DevExpress.XtraMap.MapItemType)DefaultMapItemType,
            };
            
            //Mappings
            adapter.Mappings.ImageIndex  = ImageIndexField;
            adapter.Mappings.Latitude    = LatitudeField;
            adapter.Mappings.Longitude   = LongitudeField;
            adapter.Mappings.Text        = TextField;
            adapter.Mappings.Type        = MapItemTypeField;
            adapter.Mappings.XCoordinate = XCoordinateField;
            adapter.Mappings.YCoordinate = YCoordinateField;

            //Property mappings
            var defaultItemColor = Utils.ColorFromString(DefaultItemColor);
            if (defaultItemColor != Color.Empty || !string.IsNullOrWhiteSpace(ItemColorField))
            {
                var mapItem = new MapItemFillMapping();
                if (defaultItemColor != Color.Empty)
                    mapItem.DefaultValue = defaultItemColor;
                if (!string.IsNullOrWhiteSpace(ItemColorField))
                    mapItem.Member = ItemColorField;

                adapter.PropertyMappings.Add(mapItem);
            }

            var defaultStrokeColor = Utils.ColorFromString(DefaultStrokeColor);
            if (defaultStrokeColor != Color.Empty || !string.IsNullOrWhiteSpace(StrokeColorField))
            {
                var mapItem = new MapItemStrokeMapping();
                if (defaultStrokeColor != Color.Empty)
                    mapItem.DefaultValue = defaultStrokeColor;
                if (!string.IsNullOrWhiteSpace(StrokeColorField))
                    mapItem.Member = StrokeColorField;

                adapter.PropertyMappings.Add(mapItem);
            }

            if (DefaultDotShapeKind.HasValue || !string.IsNullOrWhiteSpace(DotShapeKindField))
            {
                var mapItem = new MapDotShapeKindMapping();
                if (DefaultDotShapeKind.HasValue)
                    mapItem.DefaultValue = DefaultDotShapeKind.Value;
                if (!string.IsNullOrWhiteSpace(DotShapeKindField))
                    mapItem.Member = DotShapeKindField;

                adapter.PropertyMappings.Add(mapItem);
            }

            if (DefaultDotSize.HasValue || !string.IsNullOrWhiteSpace(DotSizeField))
            {
                var mapItem = new MapDotSizeMapping();
                if (DefaultDotSize.HasValue)
                    mapItem.DefaultValue = DefaultDotSize.Value;
                if (!string.IsNullOrWhiteSpace(DotSizeField))
                    mapItem.Member = DotSizeField;

                adapter.PropertyMappings.Add(mapItem);
            }

            if (DefaultEllipseHeight.HasValue || !string.IsNullOrWhiteSpace(EllipseHeightField))
            {
                var mapItem = new MapEllipseHeightMapping();
                if (DefaultEllipseHeight.HasValue)
                    mapItem.DefaultValue = DefaultEllipseHeight.Value;
                if (!string.IsNullOrWhiteSpace(EllipseHeightField))
                    mapItem.Member = EllipseHeightField;

                adapter.PropertyMappings.Add(mapItem);
            }

            if (DefaultEllipseWidth.HasValue || !string.IsNullOrWhiteSpace(EllipseWidthField))
            {
                var mapItem = new MapEllipseWidthMapping();
                if (DefaultEllipseWidth.HasValue)
                    mapItem.DefaultValue = DefaultEllipseWidth.Value;
                if (!string.IsNullOrWhiteSpace(EllipseWidthField))
                    mapItem.Member = EllipseWidthField;

                adapter.PropertyMappings.Add(mapItem);
            }

            if (DefaultRectangleHeight.HasValue || !string.IsNullOrWhiteSpace(RectangleHeightField))
            {
                var mapItem = new MapRectangleHeightMapping();
                if (DefaultRectangleHeight.HasValue)
                    mapItem.DefaultValue = DefaultRectangleHeight.Value;
                if (!string.IsNullOrWhiteSpace(RectangleHeightField))
                    mapItem.Member = RectangleHeightField;

                adapter.PropertyMappings.Add(mapItem);
            }

            if (DefaultRectangleWidth.HasValue || !string.IsNullOrWhiteSpace(RectangleWidthField))
            {
                var mapItem = new MapRectangleWidthMapping();
                if (DefaultRectangleWidth.HasValue)
                    mapItem.DefaultValue = DefaultRectangleWidth.Value;
                if (!string.IsNullOrWhiteSpace(RectangleWidthField))
                    mapItem.Member = RectangleWidthField;

                adapter.PropertyMappings.Add(mapItem);
            }

            //Data Source
            if (DataSource?.BaseObject == null)
                throw new Exception("Please provide data source with vector items.");

            if (DataSource.BaseObject is DataTable dataTable)
                adapter.DataSource = dataTable;
            else if (DataSource.BaseObject is DataView dataView)
                adapter.DataSource = dataView;
            else
            {
                ITypedList list = null;
                if (DataSource.BaseObject is ITypedList)
                    list = (ITypedList)DataSource.BaseObject;
                else if (DataSource.BaseObject is IListSource listSource)
                    list = listSource.GetList() as ITypedList;

                if (list != null)
                {
                    var ds = new TypedListDataReader(DataSource.BaseObject);
                    adapter.DataSource = ds;
                }
                else if (DataSource.BaseObject is IList objectList && IsPSList(objectList))
                {
                    var psList = new List<PSObject>();
                    foreach (var psObj in objectList)
                        psList.Add((PSObject)psObj);

                    var ds = new PSObjectDataReader(psList);
                    var dt = new DataTable("GeoItems");
                    dt.Load(ds);
                    adapter.DataSource = dt;
                }
                else
                    throw new Exception("Cannot use specified data source.");
            }

            var map = MapContext.Map;
            MapItemStyle itemStyle;

            if (!MiniMap)
            {
                var layer = new VectorItemsLayer()
                {
                    AllowEditItems     = false,
                    EnableHighlighting = false,
                    EnableSelection    = false
                };

                if (!string.IsNullOrWhiteSpace(ShapeTitlesPattern))
                {
                    layer.ShapeTitlesPattern    = ShapeTitlesPattern;
                    layer.ShapeTitlesVisibility = VisibilityMode.Visible;
                }

                if (!string.IsNullOrWhiteSpace(Name))
                    layer.Name = Name;
                if (ItemImageIndex.HasValue)
                    layer.ItemImageIndex = ItemImageIndex.Value;
                itemStyle = layer.ItemStyle;

                layer.Data = adapter;

                map.Layers.Add(layer);
                MapContext.CurrentLayer = layer;
            }
            else
            {
                var layerMini = new MiniMapVectorItemsLayer();

                if (!string.IsNullOrWhiteSpace(Name))
                    layerMini.Name = Name;
                if (ItemImageIndex.HasValue)
                    layerMini.ItemImageIndex = ItemImageIndex.Value;
                itemStyle = layerMini.ItemStyle;

                layerMini.Data = adapter;

                if (map.MiniMap == null)
                    throw new Exception("Mini map is not created.");

                map.MiniMap.Layers.Add(layerMini);
                MapContext.CurrentLayer = layerMini;
            }

            if (itemStyle != null)
            {
                var fillColor = Utils.ColorFromString(FillColor);
                if (fillColor != Color.Empty)
                    itemStyle.Fill = fillColor;

                var font = Utils.StringToFont(Font, out Color textColor);
                if (font != null)
                    itemStyle.Font = font;
                if (textColor != Color.Empty)
                    itemStyle.TextColor = textColor;

                var strokeColor = Utils.ColorFromString(StrokeColor);
                if (strokeColor != Color.Empty)
                    itemStyle.Stroke = strokeColor;
                if (StrokeWidth.HasValue)
                    itemStyle.StrokeWidth = StrokeWidth.Value;

                var textGlowColor = Utils.ColorFromString(TextGlowColor);
                if (textGlowColor != Color.Empty)
                    itemStyle.TextGlowColor = textGlowColor;
            }


            static bool IsPSList(IList objects)
            {
                foreach (var obj in objects)
                    if (!(obj is PSObject))
                        return false;

                return true;
            }
        }
    }
}
