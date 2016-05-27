using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
namespace api_auth
{
    public class Api_auth
    {
        //HMACSHA1
        //create an md5 of the content
        //format the date

        private string access_id;
        private string secret;

        public Api_auth ( string access_id , string secret ) {
            this.access_id = access_id;
            this.secret = secret;
        }


        /// <summary>
        /// Generates the hmac of an api request
        /// </summary>
        /// <param name="headers"> a Dictionary of headers</param>
        /// <param name="path"> the path of this api </param>
        /// <param name="content"> the content for this HMAC</param>
        public Dictionary<String, String> sign(Dictionary<String, String> headers, String path, String content = "")
        {

            ApiAuthResponse authRequest = sign(headers["content-type"], null, path, System.Text.Encoding.ASCII.GetBytes(content));

            headers["Content-MD5"] = authRequest.contentMD5;
            headers["DATE"] = authRequest.date;
            headers["Authorization"] = authRequest.authoirisation;
         //   headers["debugAuth"] = authRequest._debugAuth;

            return headers;
        }

        public WebRequest sign(WebRequest request, String path , String content)
        {

            ApiAuthResponse authRequest = sign(request.ContentType, request.Headers.Get("date"), path, System.Text.Encoding.ASCII.GetBytes(content));

            request.Headers.Add( "Content-MD5",authRequest.contentMD5);
            //request.Headers.Add( "DATE",authRequest.date);
            request.Headers.Add( "Authorization",authRequest.authoirisation);

          //  request.Headers.Add("debugAuth",  authRequest._debugAuth );

            return request;
        }


        public ApiAuthResponse sign(String contentType, String date, String path, Byte[] content)
        {
            

            //generate a hash of the content
            MD5 content_hash_md5 = MD5.Create();
            content_hash_md5.ComputeHash(content);
            String content_hash = Convert.ToBase64String(content_hash_md5.Hash);

            if (!String.IsNullOrEmpty( date ))
            {
                DateTime _date = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                date = _date.ToString("R");
            }


            String canonical_string = contentType + "," + content_hash + "," + path + "," + date;

            byte[] canonical_bytes = System.Text.Encoding.ASCII.GetBytes(canonical_string);

            HMACSHA1 hmacsha1 = new HMACSHA1(System.Text.Encoding.ASCII.GetBytes(this.secret));
            hmacsha1.ComputeHash(canonical_bytes);
            String hmac = Convert.ToBase64String(hmacsha1.Hash); //BitConverter.ToString(hmacsha1.Hash).Replace("-", String.Empty).ToLower();

            ApiAuthResponse response = new ApiAuthResponse();

            response.contentMD5 = content_hash;
            response.date = date;
            response.authoirisation = "APIAuth " + this.access_id + ":" + hmac;

            //response._debugAuth = canonical_string;



            return response;
        }
        
    }

    public class ApiAuthResponse
    {
        public String contentMD5;
        public String date;
        public String authoirisation;
        public String _debugAuth;

    }
}
