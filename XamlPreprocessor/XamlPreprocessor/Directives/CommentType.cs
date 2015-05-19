using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamlPreprocessor
{
    /// <summary>
    /// Type de directive préprocesseur que peut avoir un noeud XML de commentaire.
    /// La valeur IGNORE est également utilisée pour ignorer les noeuds non commentaires.
    /// </summary>
    public enum CommentType
    {
        IGNORE,
        IF,
        LIF, // ~IF
        ATTR_ADD,
        ATTR_DEL
    }
}
