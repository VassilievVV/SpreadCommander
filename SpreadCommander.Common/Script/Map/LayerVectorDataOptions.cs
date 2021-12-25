using DevExpress.XtraMap;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class LayerVectorDataOptions
    {
        [Description("Layer's name.")]
        public string Name { get; set; }

        [Description("When set - layer is added to mini map.")]
        public bool MiniMap { get; set; }

        [Description("Default map item type.")]
        public MapItemType DefaultMapItemType { get; set; }

        [Description("Index of the image assigned to the map item.")]
        public int? ItemImageIndex { get; set; }

        [Description("Color used to fill a map item.")]
        public string FillColor { get; set; }

        [Description("Map item's font.")]
        public string Font { get; set; }

        [Description("Color that specifies how the map item outline is painted.")]
        public string StrokeColor { get; set; }

        [Description("Width of the stroke on the current map item.")]
        public int? StrokeWidth { get; set; }

        [Description("Map item's text glow color.")]
        public string TextGlowColor { get; set; }

        [Description("Pattern used to generate shape titles.")]
        public string ShapeTitlesPattern { get; set; }

        [Description("Data field to which Latitude is bound.")]
        [DefaultValue("Latitude")]
        public string LatitudeField { get; set; } = "Latitude";

        [Description("Data field to which Longitude is bound.")]
        [DefaultValue("Longitude")]
        public string LongitudeField { get; set; } = "Longitude";

        [Description("Data field to which ImageIndex is bound.")]
        public string ImageIndexField { get; set; }

        [Description("Data field to which Text is bound.")]
        public string TextField { get; set; }

        [Description("Data field to which MapItemType is bound. Data source shall return integer: 1 - dot, 2 - ellipse, 3 - line, 4 - path, 5 - polygon, 6 - polyline, 7 - rectangle, 8 - pushpin, 10 - callout.")]
        public string MapItemTypeField { get; set; }

        [Description("Data field to which X coordinate is bound.")]
        public string XCoordinateField { get; set; }

        [Description("Data field to which Y coordinate is bound.")]
        public string YCoordinateField { get; set; }

        [Description("Data field to which Bubble group is bound.")]
        public string BubbleField { get; set; }

        [Description("Data field to which pie segment is bound.")]
        public string PieField { get; set; }

        [Description("Data field to which pie value is bound.")]
        public string ValueField { get; set; }

        [Description("Default item color.")]
        public string DefaultItemColor { get; set; }

        [Description("Item color's field.")]
        public string ItemColorField { get; set; }

        [Description("Default stroke color.")]
        public string DefaultStrokeColor { get; set; }

        [Description("Stroke color's field.")]
        public string StrokeColorField { get; set; }

        [Description("Default dot shape kind.")]
        public MapDotShapeKind? DefaultDotShapeKind { get; set; }

        [Description("Dot shape kind's field. Data source shall return an integer: 0 - for rectangle, 1 - for circle.")]
        public string DotShapeKindField { get; set; }

        [Description("Default dot size.")]
        public double? DefaultDotSize { get; set; }

        [Description("Dot size's field.")]
        public string DotSizeField { get; set; }

        [Description("Default ellipse height.")]
        public double? DefaultEllipseHeight { get; set; }

        [Description("Ellipse height's field.")]
        public string EllipseHeightField { get; set; }

        [Description("Default ellipse width.")]
        public double? DefaultEllipseWidth { get; set; }

        [Description("Ellipse width's field.")]
        public string EllipseWidthField { get; set; }

        [Description("Default rectangle height.")]
        public double? DefaultRectangleHeight { get; set; }

        [Description("Rectangle height's field.")]
        public string RectangleHeightField { get; set; }

        [Description("Default rectangle width.")]
        public double? DefaultRectangleWidth { get; set; }

        [Description("Rectangle width's field.")]
        public string RectangleWidthField { get; set; }

        [Description("Collection of attribute mappings")]
        public string[] Attributes { get; set; }
    }

    public partial class SCMap
    {
        public SCMap AddLayerVectorData(object dataSource, LayerVectorDataOptions options = null)
        {
            options ??= new LayerVectorDataOptions();

            DataSourceAdapterBase adapter;

            if (!string.IsNullOrWhiteSpace(options.BubbleField))
            {
                var bubbleAdapter = new BubbleChartDataAdapter()
                {
                    BubbleItemDataMember = options.BubbleField
                };
                adapter = bubbleAdapter;

                //Mappings
                bubbleAdapter.Mappings.ImageIndex  = options.ImageIndexField;
                bubbleAdapter.Mappings.Latitude    = options.LatitudeField;
                bubbleAdapter.Mappings.Longitude   = options.LongitudeField;
                bubbleAdapter.Mappings.Text        = options.TextField;
                bubbleAdapter.Mappings.Type        = options.MapItemTypeField;
                bubbleAdapter.Mappings.XCoordinate = options.XCoordinateField;
                bubbleAdapter.Mappings.YCoordinate = options.YCoordinateField;
                bubbleAdapter.Mappings.Value       = options.ValueField;
            }
            else if (!string.IsNullOrWhiteSpace(options.PieField))
            {
                var pieAdapter = new PieChartDataAdapter()
                {
                    PieItemDataMember = options.PieField
                };
                adapter= pieAdapter;

                //Mappings
                pieAdapter.Mappings.ImageIndex  = options.ImageIndexField;
                pieAdapter.Mappings.Latitude    = options.LatitudeField;
                pieAdapter.Mappings.Longitude   = options.LongitudeField;
                pieAdapter.Mappings.Text        = options.TextField;
                pieAdapter.Mappings.Type        = options.MapItemTypeField;
                pieAdapter.Mappings.XCoordinate = options.XCoordinateField;
                pieAdapter.Mappings.YCoordinate = options.YCoordinateField;
                pieAdapter.Mappings.Value       = options.ValueField;
            }
            else
            {
                var listAdapter = new ListSourceDataAdapter()
                {
                    DefaultMapItemType = (DevExpress.XtraMap.MapItemType)options.DefaultMapItemType
                };
                adapter = listAdapter;

                //Mappings
                listAdapter.Mappings.ImageIndex  = options.ImageIndexField;
                listAdapter.Mappings.Latitude    = options.LatitudeField;
                listAdapter.Mappings.Longitude   = options.LongitudeField;
                listAdapter.Mappings.Text        = options.TextField;
                listAdapter.Mappings.Type        = options.MapItemTypeField;
                listAdapter.Mappings.XCoordinate = options.XCoordinateField;
                listAdapter.Mappings.YCoordinate = options.YCoordinateField;
            }

            //Property mappings
            var defaultItemColor = Utils.ColorFromString(options.DefaultItemColor);
            if (defaultItemColor != Color.Empty || !string.IsNullOrWhiteSpace(options.ItemColorField))
            {
                var mapItem = new MapItemFillMapping();
                if (defaultItemColor != Color.Empty)
                    mapItem.DefaultValue = defaultItemColor;
                if (!string.IsNullOrWhiteSpace(options.ItemColorField))
                    mapItem.Member = options.ItemColorField;

                adapter.PropertyMappings.Add(mapItem);
            }

            var defaultStrokeColor = Utils.ColorFromString(options.DefaultStrokeColor);
            if (defaultStrokeColor != Color.Empty || !string.IsNullOrWhiteSpace(options.StrokeColorField))
            {
                var mapItem = new MapItemStrokeMapping();
                if (defaultStrokeColor != Color.Empty)
                    mapItem.DefaultValue = defaultStrokeColor;
                if (!string.IsNullOrWhiteSpace(options.StrokeColorField))
                    mapItem.Member = options.StrokeColorField;

                adapter.PropertyMappings.Add(mapItem);
            }

            if (options.DefaultDotShapeKind.HasValue || !string.IsNullOrWhiteSpace(options.DotShapeKindField))
            {
                var mapItem = new MapDotShapeKindMapping();
                if (options.DefaultDotShapeKind.HasValue)
                    mapItem.DefaultValue = (DevExpress.XtraMap.MapDotShapeKind)options.DefaultDotShapeKind.Value;
                if (!string.IsNullOrWhiteSpace(options.DotShapeKindField))
                    mapItem.Member = options.DotShapeKindField;

                adapter.PropertyMappings.Add(mapItem);
            }

            if (options.DefaultDotSize.HasValue || !string.IsNullOrWhiteSpace(options.DotSizeField))
            {
                var mapItem = new MapDotSizeMapping();
                if (options.DefaultDotSize.HasValue)
                    mapItem.DefaultValue = options.DefaultDotSize.Value;
                if (!string.IsNullOrWhiteSpace(options.DotSizeField))
                    mapItem.Member = options.DotSizeField;

                adapter.PropertyMappings.Add(mapItem);
            }

            if (options.DefaultEllipseHeight.HasValue || !string.IsNullOrWhiteSpace(options.EllipseHeightField))
            {
                var mapItem = new MapEllipseHeightMapping();
                if (options.DefaultEllipseHeight.HasValue)
                    mapItem.DefaultValue = options.DefaultEllipseHeight.Value;
                if (!string.IsNullOrWhiteSpace(options.EllipseHeightField))
                    mapItem.Member = options.EllipseHeightField;

                adapter.PropertyMappings.Add(mapItem);
            }

            if (options.DefaultEllipseWidth.HasValue || !string.IsNullOrWhiteSpace(options.EllipseWidthField))
            {
                var mapItem = new MapEllipseWidthMapping();
                if (options.DefaultEllipseWidth.HasValue)
                    mapItem.DefaultValue = options.DefaultEllipseWidth.Value;
                if (!string.IsNullOrWhiteSpace(options.EllipseWidthField))
                    mapItem.Member = options.EllipseWidthField;

                adapter.PropertyMappings.Add(mapItem);
            }

            if (options.DefaultRectangleHeight.HasValue || !string.IsNullOrWhiteSpace(options.RectangleHeightField))
            {
                var mapItem = new MapRectangleHeightMapping();
                if (options.DefaultRectangleHeight.HasValue)
                    mapItem.DefaultValue = options.DefaultRectangleHeight.Value;
                if (!string.IsNullOrWhiteSpace(options.RectangleHeightField))
                    mapItem.Member = options.RectangleHeightField;

                adapter.PropertyMappings.Add(mapItem);
            }

            if (options.DefaultRectangleWidth.HasValue || !string.IsNullOrWhiteSpace(options.RectangleWidthField))
            {
                var mapItem = new MapRectangleWidthMapping();
                if (options.DefaultRectangleWidth.HasValue)
                    mapItem.DefaultValue = options.DefaultRectangleWidth.Value;
                if (!string.IsNullOrWhiteSpace(options.RectangleWidthField))
                    mapItem.Member = options.RectangleWidthField;

                adapter.PropertyMappings.Add(mapItem);
            }

            //Attribute mappings
            if (options.Attributes != null)
            {
                foreach (string attribute in options.Attributes)
                    adapter.AttributeMappings.Add(new MapItemAttributeMapping() { Member = attribute, Name = attribute });
            }

            if (dataSource != null)
                adapter.DataSource = GetDataSource(dataSource, new DataSourceParameters());

            var map = Map;
            MapItemStyle itemStyle;

            if (!options.MiniMap)
            {
                var layer = new VectorItemsLayer()
                {
                    AllowEditItems     = false,
                    EnableHighlighting = false,
                    EnableSelection    = false
                };

                if (!string.IsNullOrWhiteSpace(options.ShapeTitlesPattern))
                {
                    layer.ShapeTitlesPattern    = options.ShapeTitlesPattern;
                    layer.ShapeTitlesVisibility = VisibilityMode.Visible;
                }

                if (!string.IsNullOrWhiteSpace(options.Name))
                    layer.Name = options.Name;
                if (options.ItemImageIndex.HasValue)
                    layer.ItemImageIndex = options.ItemImageIndex.Value;
                itemStyle = layer.ItemStyle;

                layer.Data = adapter;

                map.Layers.Add(layer);
                CurrentLayer = layer;
            }
            else
            {
                var layerMini = new MiniMapVectorItemsLayer();

                if (!string.IsNullOrWhiteSpace(options.Name))
                    layerMini.Name = options.Name;
                if (options.ItemImageIndex.HasValue)
                    layerMini.ItemImageIndex = options.ItemImageIndex.Value;
                itemStyle = layerMini.ItemStyle;

                layerMini.Data = adapter;

                if (map.MiniMap == null)
                    throw new Exception("Mini map is not created.");

                map.MiniMap.Layers.Add(layerMini);
                CurrentLayer = layerMini;
            }

            if (itemStyle != null)
            {
                var fillColor = Utils.ColorFromString(options.FillColor);
                if (fillColor != Color.Empty)
                    itemStyle.Fill = fillColor;

                var font = Utils.StringToFont(options.Font, out Color textColor);
                if (font != null)
                    itemStyle.Font = font;
                if (textColor != Color.Empty)
                    itemStyle.TextColor = textColor;

                var strokeColor = Utils.ColorFromString(options.StrokeColor);
                if (strokeColor != Color.Empty)
                    itemStyle.Stroke = strokeColor;
                if (options.StrokeWidth.HasValue)
                    itemStyle.StrokeWidth = options.StrokeWidth.Value;

                var textGlowColor = Utils.ColorFromString(options.TextGlowColor);
                if (textGlowColor != Color.Empty)
                    itemStyle.TextGlowColor = textGlowColor;
            }

            return this;
        }
    }
}
