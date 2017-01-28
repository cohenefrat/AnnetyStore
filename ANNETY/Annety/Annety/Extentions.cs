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
                    girlOrChild = "Boys";
                else
                    girlOrChild = "Girls";

                return girlOrChild+" "+this.Desc  ;

            }
         }

    }
    //public partial class Users
    //{

        //public string DisplayPassword
        //{
        //    get { return string.Empty; }
        //    set
        //    {
        //        Password = value.HashPass();
        //    }
        //}

        //}
        //public static MySessionObject GetMySessionObject(this HttpContext current)
        //{
        //    return current != null ? (MySessionObject)current.Session["__MySessionObject"] : null;
        //}


    }