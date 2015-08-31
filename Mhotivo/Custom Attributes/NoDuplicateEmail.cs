using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using Mhotivo.Implement.Context;
using Mhotivo.Models;

namespace Mhotivo.Custom_Attributes
{
 //    [AttributeUsage(AttributeTargets.All)]
    public class NoDuplicateEmail : ValidationAttribute
    {


        
         protected override ValidationResult IsValid(object value, ValidationContext validationContext)
         {
              var context = new MhotivoContext();

             var email = validationContext.ObjectInstance.GetType().GetProperties(
                 ).FirstOrDefault(prop => IsDefined(prop, typeof(NoDuplicateEmail)));

             var emailValue = (string)email.GetValue(validationContext.ObjectInstance);

             if(context.Users.FirstOrDefault(x => x.Email == emailValue)!=null)
                 return new ValidationResult("FUCK!");
        
             return ValidationResult.Success;

         } 
        
    }
}