using System;
using System.Collections.Generic;
using System.Text;

namespace Cynthia.Business
{
    

    public interface IIndexableContent
    {

        event ContentChangedEventHandler ContentChanged;
        

    }
}
