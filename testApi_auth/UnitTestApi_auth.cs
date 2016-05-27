using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using api_auth;
using System.Collections.Generic;

namespace testApi_auth
{
    [TestClass]
    public class UnitTestApi_auth
    {
        [TestMethod]
        public void TestAath()
        {
            Api_auth auth = new Api_auth("ID", "SECRET");
            Dictionary<string,string> options = new Dictionary<string,string>();
            options.Add( "content-type" , "application/json" );

            options = auth.sign(options, "/", "BLA BLA BLA");
            Console.WriteLine(options["DATE"]);
            Console.WriteLine(options["Authorization"]);
            Console.WriteLine(options["Content-MD5"]);
            Assert.AreEqual(options["DATE"], "TEST");

            Assert.AreEqual(options["Authorization"], "TEST");
            
        }
    }
}
