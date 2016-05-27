# API_AUTH

An API AUTH client library for .net

API AUTH is a way to hmac sign requests to REST API's

Usage:

```C#

string id = "RYAN";
string secret = "PASSWORD";
string url = @"http://www.test.com/api/content";
Api_auth auth = new Api_auth(id, secret);
WebHeaderCollection headers = this.request.Headers;
this.request.ContentType = "Application/Json";
this.request.Date = DateTime.Now;
this.request.Method = "POST";

byte[] byteArray = Encoding.UTF8.GetBytes(content);
this.request.ContentLength = byteArray.Length;
this.request = (HttpWebRequest)auth.sign(this.request, url, content);

```
