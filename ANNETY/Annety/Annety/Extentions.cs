using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Annety
{
    public partial class Categories
    {
       
        public string  LongName
        {
            get
            {
                string girlOrChild = "";
                if (this.ParentCategory)
                    girlOrChild = "girls";
                else
                    girlOrChild = "boys";

                return this.Desc+" "+girlOrChild  ;

            }
         }

    }
    //public partial class Users
    //{
 
    //    public string DisplayPassword
    //    {
    //      //  get { return Password .decript(); }
    //        set { Password  = value.להצפין(); }
    //    }

    //}


}