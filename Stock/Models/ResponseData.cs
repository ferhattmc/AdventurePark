using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stock.Models
{
    public class ResponseData
    {
        public ResponseStatu statu { get; set; }
        public string message { get; set; }
        public Object data { get; set; }

        public static ResponseData Success(object obj)
        {
            ResponseData result = new ResponseData();
            result.statu = ResponseStatu.SUCCESS;
            result.message = null;
            result.data = obj;
            return result;
        }
        public static ResponseData Success(object obj, string msg)
        {
            ResponseData result = new ResponseData();
            result.statu = ResponseStatu.SUCCESS;
            result.message = msg;
            result.data = obj;
            return result;
        }
        public static ResponseData Error(string msg)
        {
            ResponseData result = new ResponseData();
            result.statu = ResponseStatu.ERROR;
            result.message = msg;
            result.data = null;
            return result;
        }
        public static ResponseData Error()
        {
            ResponseData result = new ResponseData();
            result.statu = ResponseStatu.ERROR;
            result.data = null;
            result.message = "An error occured! Please try again later.";
            return result;
        }
        public static ResponseData Message(string msg)
        {
            ResponseData result = new ResponseData();
            result.data = null;
            result.statu = ResponseStatu.MESSAGE;
            result.message = msg;
            return result;
        }
    }

    public enum ResponseStatu
    {
        ERROR = 0,
        SUCCESS = 1,
        MESSAGE = 2
    }

}