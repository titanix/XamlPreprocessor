using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using XamlPreprocessor.Evaluator;

namespace XamlPreprocessor
{
    public enum CommentType
    {
        IGNORE,
        IF,
        LIF, // ~IF
        ATTR_ADD,
        ATTR_DEL
    }

    partial class Preprocessor
    {
        public CommentType GetNodeType(XNode node)
        {
            if (node.NodeType == System.Xml.XmlNodeType.Comment)
            {
                XComment comm = node as XComment;
                string str = comm.Value;
                if (Directives.IsDirectiveIF(str))
                    return CommentType.IF;
                if (Directives.IsDirectiveLIF(str))
                    return CommentType.LIF;
                if (Directives.IsDirectiveATTR_ADD(str))
                    return CommentType.ATTR_ADD;
                if (Directives.IsDirectiveATTR_DEL(str))
                    return CommentType.ATTR_DEL;
                return CommentType.IGNORE;
            }
            else
            {
                return CommentType.IGNORE;
            }
        }
    }
}
