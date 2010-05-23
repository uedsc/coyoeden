using System;
using System.Collections.Generic;
using System.Text;

namespace Cynthia.Business
{
    public class GroupThreadMovedArgs
    {
        private int forumID;
        private int originalGroupID;

        public int GroupId
        {
            get { return forumID; }
            set { forumID = value; }
        }

        public int OriginalGroupId
        {
            get { return originalGroupID; }
            set { originalGroupID = value; }
        }

    }
}
