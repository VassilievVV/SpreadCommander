using System;
using System.Linq;

namespace SpreadCommander.Common.Code
{
    public class GridFormatCondition
    {
        public string TableName         { get; set; }
        public string ColumnName        { get; set; }
        public string Expression        { get; set; }

        public string TargetColumn      { get; set; }
        public bool ApplyToRow          { get; set; }

        public string FormatRule        { get; set; }	//Expresssion, DataBar, IconSet, ColorScale, AboveAverage, BelowAverage, Unique, Duplicate, Top, Bottom, DateOccuring, Rule
        public string DataBar           { get; set; }
        public string IconSet           { get; set; }

        public string Rank              { get; set; }    //For Top/Bottom - count or percentage of column cells with the highest or lowest values
        public string DateOccuring      { get; set; }	//For DateOccuring
        public string Condition         { get; set; }
        public string Value1            { get; set; }
        public string Value2            { get; set; }

        public string AppearanceName    { get; set; }
        public string BackColor         { get; set; }
        public string BackColor2        { get; set; }
        public string BorderColor       { get; set; }
        public string ColorScale        { get; set; }
        public string ForeColor         { get; set; }
        public string Font              { get; set; }
        public string Gradient          { get; set; }
    }
}
