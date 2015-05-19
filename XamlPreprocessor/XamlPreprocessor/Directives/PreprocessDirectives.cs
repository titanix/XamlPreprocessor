using System.Xml;
using System.Xml.Linq;

namespace XamlPreprocessor
{
    partial class Preprocessor
    {
        public CommentType GetNodeType(XNode node)
        {
            if (node.NodeType == XmlNodeType.Comment)
            {
                XComment commentNode = node as XComment;
                string commentContent = commentNode.Value;
                if (Directives.IsDirectiveIF(commentContent))
                    return CommentType.IF;
                if (Directives.IsDirectiveLIF(commentContent))
                    return CommentType.LIF;
                if (Directives.IsDirectiveATTR_ADD(commentContent))
                    return CommentType.ATTR_ADD;
                if (Directives.IsDirectiveATTR_DEL(commentContent))
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
