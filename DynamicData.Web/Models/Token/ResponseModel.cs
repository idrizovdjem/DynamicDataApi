using System.Collections.Generic;

namespace DynamicData.Web.Models.Token
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            this.Successfull = true;
            this.StatusCode = 200;
            this.ErrorMessages = new List<string>();
        }

        public bool Successfull { get; set; }

        public int StatusCode { get; set; }

        public List<string> ErrorMessages { get; set; }

        public object Data { get; set; }

        public void AddErrorMessage(string message)
        {
            this.Successfull = false;
            this.ErrorMessages.Add(message);
        }
    }
}
