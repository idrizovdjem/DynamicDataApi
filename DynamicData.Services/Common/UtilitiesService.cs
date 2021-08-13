using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using DynamicData.Services.Common.Contracts;

namespace DynamicData.Services.Common
{
    public class UtilitiesService : IUtilitiesService
    {
        private const byte BEARER_LENGTH = 7;

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

            var rawToken = httpContext.Request.Headers["authorization"].ToString();
            if (rawToken == "Bearer")
            {
                return null;
            }

            var accessToken = rawToken.Substring(BEARER_LENGTH);
            return accessToken;
        }
    }
}
