﻿// Generated by TinyPG v1.3 available at www.codeproject.com

#pragma warning disable CRR0026 // Unused member
#pragma warning disable CS0168 // Variable is declared but never used
#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable CRR0052
#pragma warning disable CRR0044

using System;
using System.Collections.Generic;

namespace <%Namespace%>
{
    #region Parser

    public partial class Parser <%IParser%>
    {
        private Scanner scanner;
        private ParseTree tree;
        
        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
        }

        public <%IParseTree%> Parse(string input)
        {
            tree = new ParseTree();
            return Parse(input, tree);
        }

        public ParseTree Parse(string input, ParseTree tree)
        {
            scanner.Init(input);

            this.tree = tree;
            ParseStart(tree);
            tree.Skipped = scanner.Skipped;

            return tree;
        }

<%ParseNonTerminals%>
    }

    #endregion Parser
}