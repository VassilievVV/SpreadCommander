﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public enum ChartType
    {
        Combo                       = -2,
        ColumnClustered             = 0,
        ColumnStacked               = 1,
        ColumnFullStacked           = 2,
        Column3DClustered           = 3,
        Column3DStacked             = 4,
        Column3DFullStacked         = 5,
        Column3DStandard            = 6,
        Column3DClusteredCylinder   = 7,
        Column3DStackedCylinder     = 8,
        Column3DFullStackedCylinder = 9,
        Column3DStandardCylinder    = 10,
        Column3DClusteredCone       = 11,
        Column3DStackedCone         = 12,
        Column3DFullStackedCone     = 13,
        Column3DStandardCone        = 14,
        Column3DClusteredPyramid    = 15,
        Column3DStackedPyramid      = 16,
        Column3DFullStackedPyramid  = 17,
        Column3DStandardPyramid     = 18,
        Line                        = 19,
        LineStacked                 = 20,
        LineFullStacked             = 21,
        LineMarker                  = 22,
        LineStackedMarker           = 23,
        LineFullStackedMarker       = 24,
        Line3D                      = 25,
        Pie                         = 26,
        Pie3D                       = 27,
        PieExploded                 = 28,
        Pie3DExploded               = 29,
        PieOfPie                    = 30,
        BarOfPie                    = 31,
        BarClustered                = 32,
        BarStacked                  = 33,
        BarFullStacked              = 34,
        Bar3DClustered              = 35,
        Bar3DStacked                = 36,
        Bar3DFullStacked            = 37,
        Bar3DClusteredCylinder      = 38,
        Bar3DStackedCylinder        = 39,
        Bar3DFullStackedCylinder    = 40,
        Bar3DClusteredCone          = 41,
        Bar3DStackedCone            = 42,
        Bar3DFullStackedCone        = 43,
        Bar3DClusteredPyramid       = 44,
        Bar3DStackedPyramid         = 45,
        Bar3DFullStackedPyramid     = 46,
        Area                        = 47,
        AreaStacked                 = 48,
        AreaFullStacked             = 49,
        Area3D                      = 50,
        Area3DStacked               = 51,
        Area3DFullStacked           = 52,
        ScatterMarkers              = 53,
        ScatterSmoothMarkers        = 54,
        ScatterSmooth               = 55,
        ScatterLine                 = 56,
        ScatterLineMarkers          = 57,
        StockHighLowClose           = 58,
        StockOpenHighLowClose       = 59,
        StockVolumeHighLowClose     = 60,
        StockVolumeOpenHighLowClose = 61,
        Surface                     = 62,
        SurfaceWireframe            = 63,
        Surface3D                   = 64,
        Surface3DWireframe          = 65,
        Doughnut                    = 66,
        DoughnutExploded            = 67,
        Bubble                      = 68,
        Bubble3D                    = 69,
        Radar                       = 70,
        RadarMarkers                = 71,
        RadarFilled                 = 72,
        BoxAndWhisker               = 73,
        Funnel                      = 74,
        Histogram                   = 75,
        Pareto                      = 76,
        Sunburst                    = 77,
        Treemap                     = 78,
        Waterfall                   = 79
    }
}