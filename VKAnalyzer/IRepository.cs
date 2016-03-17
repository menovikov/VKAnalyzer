using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VKAnalyzer
{
    public interface IRepository
    {
        //IRepository Instance { get; set; }
        string AccessToken { get; set; }
        string LoggedInUserID { get; set; }

        string AppID { get; set; }
        string Scope { get; set; }
        string RequestedUserID { get; set; }
        bool SignedIn { get; set; }
        
    }
}
