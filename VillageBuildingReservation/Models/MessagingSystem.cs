using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VillageBuildingReservation.Models
{
    public class MessagingSystem
    {
        public string Message { set; get; }
        public string MessageType { get; set; }
        //Creates the HTML string.
        //This outputs the div in HTML with the current message formatted. 
        public static string GenerateMessage(string Message = "", string Type = "success")
        {
            //Div Tag
            var divTag = new TagBuilder("div");
            divTag.AddCssClass("alert alert-dismissible show alert-" + Type.ToString());
            divTag.Attributes.Add("role", "alert");
            divTag.InnerHtml += Message + "<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden ='true'> &times;</span></button>";
            return divTag.ToString();
        }
        public static MessagingSystem AddMessage(string message,string type)
        {
            MessagingSystem m = new MessagingSystem();
            m.Message = message;
            m.MessageType = type;
            return m;
        }
    }
    //The bootstrap alert types.
    //public enum MessageType
    //{
    //    success,
    //    info,
    //    warning,
    //    danger
    //}
}