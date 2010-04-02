/// Author:					Joe Audette
/// Created:				2006-08-28
/// Last Modified:			2010-02-08
///		
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.		

using System.Web.UI.WebControls;
using log4net;

namespace Cynthia.Web.UI
{
   
    public class CTreeView : TreeView
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CTreeView));

        public static void ExpandToValuePath(TreeView treeView, string valuePath)
        {
            if (treeView == null) return;
            if (valuePath == null) return;

            if (!valuePath.Contains(treeView.PathSeparator.ToString()))
            {
                ExpandValuePath(treeView, valuePath);
            }
            else
            {
                string[] pathSegments
                            = valuePath.Split(new char[] { '|' });

                string pathToExpand = pathSegments[0];
                ExpandValuePath(treeView, pathToExpand);
                for (int i = 1; i < pathSegments.Length; i++)
                {
                    pathToExpand = pathToExpand + treeView.PathSeparator + pathSegments[i];
                    ExpandValuePath(treeView, pathToExpand);
                }
            }

        }

        private static void ExpandValuePath(TreeView treeView, string valuePath)
        {
            if (treeView == null) return;
            if (valuePath == null) return;

            if (valuePath.Length > 0)
            {
                TreeNode treeNode;
                treeNode = treeView.FindNode(valuePath);

                if (treeNode != null)
                {
                    if (
                        (treeNode.Expanded == null)
                        || (treeNode.Expanded.Equals(false))
                        )
                    {
                        treeNode.Expand();
                        log.Debug("Expanded treeNode found for value path " + valuePath);
                    }
                }
                else
                {
                    log.Debug(" treeNode was null for " + valuePath);
                }
            }

        }

        protected override TreeNode CreateNode()
        {
            //return base.CreateNode();
            return new CTreeNode();
        } 

    }
}
