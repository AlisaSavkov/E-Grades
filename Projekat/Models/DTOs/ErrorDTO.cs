using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models.DTOs
{
    
    public class ErrorDTO
    {
       
        public ErrorDTO(string exceptionMessage)

        {

            MessageDetails = exceptionMessage;
            
        }

        public string Message = "An error has occured.";
        public string MessageDetails { get; set; }
        

        public string WriteM()
        {
            return Message + " " + MessageDetails;
        }
    }

}