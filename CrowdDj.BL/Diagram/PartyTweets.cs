//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CrowdDj.BL.Diagram
{
    using System;
    using System.Collections.Generic;
    
    public partial class PartyTweets
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public byte[] RowVersion { get; set; }
        public int PartyGuest_Id { get; set; }
    
        public virtual PartyGuests PartyGuests { get; set; }
    }
}