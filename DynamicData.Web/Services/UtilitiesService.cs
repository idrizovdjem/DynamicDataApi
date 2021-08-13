using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

namespace DynamicData.Web.Services
{
    public class UtilitiesService : IUtilitiesService
    {
        public string HashPassword(string password)
        {
            using SHA512 sha512Hash = SHA512.Create();
            byte[] sourceBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
            string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            return hash;
        }

        public List<string> GetModelStateErorrs(ModelStateDictionary modelState)
        {
            // get all validation errors from ModelState and return the in list of strings

            var modelErrors = new List<string>();
            foreach (var currentModelState in modelState.Values)
            {
                foreach (var modelError in currentModelState.Errors)
                {
                    modelErrors.Add(modelError.ErrorMessage);
                }
            }
            return modelErrors;
        }

        public string GetAccessTokenHeader(HttpContext httpContext)
        {
            // check if there is authorization header
            if (httpContext.Request.Headers.ContainsKey("authorization") == false)
            {
                return null;
            }

            // get the header value
            var rawToken = httpContext.Request.Headers["authorization"].ToString();

            if (rawToken == "Bearer")
            {
                return null;
            }

            // remove "Bearer " from the token
            var accessToken = rawToken.Substring(7);
            return accessToken;
        }
    }
}
