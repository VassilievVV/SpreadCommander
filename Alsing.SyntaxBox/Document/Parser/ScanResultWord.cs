using Alsing.Windows.Forms.Document.SyntaxDefinition.Pattern;

namespace Alsing.Windows.Forms.Document.Parser
{

    public class ScanResultWord
    {
        public bool HasContent;
        public PatternList ParentList;
        public Pattern Pattern;
        public int Position;
        public string Token = "";
    }
}
