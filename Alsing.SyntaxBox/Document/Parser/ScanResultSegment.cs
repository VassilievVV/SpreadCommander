using Alsing.Windows.Forms.Document.DocumentStructure.Span;
using Alsing.Windows.Forms.Document.SyntaxDefinition.Pattern;
using Alsing.Windows.Forms.Document.SyntaxDefinition.Scope;
using Alsing.Windows.Forms.Document.SyntaxDefinition.SpanDefinition;

namespace Alsing.Windows.Forms.Document.Parser
{
    public class ScanResultSegment
    {
        public SpanDefinition spanDefinition;
        public bool HasContent;
        public bool IsEndSegment;
        public Pattern Pattern;
        public int Position;
        public Scope Scope;
        public Span span;
        public string Token = "";
    }
}
