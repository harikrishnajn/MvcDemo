//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcDemo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChatHistory
    {
        public int ID { get; set; }
        public string WorkItemID { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Messages { get; set; }
        public string CreatedDate { get; set; }
    }
}
