using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace wwHTTP.Tests
{
    /// <summary>
    /// https://www.west-wind.com/presentations/dotnetwebrequest/dotnetwebrequest.htm
    /// https://www.west-wind.com/presentations/dotnetWebRequest/dotnetWebRequest.zip
    /// https://docs.google.com/document/d/10RF1ySgGdwD_BSz58ZchmHM3wEAs93b8gw87jZvP0-g/
    /// https://www.codewrecks.com/post/old/2013/05/unit-test-class-that-makes-use-of-httpwebrequest-thanks-to-visual-studio-fakes-library/
    /// </summary>
    [TestClass]
    public class SmartSearchTest
    {
        /// <summary>
        /// DocRepChunkVector collection
        /// </summary>
        [TestMethod]
        public void DocRepChunkVector_NoFilter_Returns_Success()
        {
            string responseResult = string.Empty;
            HttpWebResponse oResponse = null;
            HttpWebRequest oRequest = null;

            //string fullUrl = "http://localhost:8989/solr/#/DocRepChunkVector/query?q=iAssetID:(271658%201925926)&q.op=OR&indent=true&useParams=";
            //=>
            //string fullUrl = "http://localhost:8989/solr/DocRepChunkVector/select?q=iAssetID:(271658%201925926)&q.op=OR&indent=true&useParams=";

            try
            {
                string fullUrl = "http://localhost:8989/solr/DocRepChunkVector/select?q=iAssetID:(271658%201925926)&q.op=OR&indent=true&useParams=";
                oRequest = (HttpWebRequest)WebRequest.Create(fullUrl);
                Debug.WriteLine($"Solar query {fullUrl}.");

                oRequest.ContentType = "application/x-www-form-urlencoded";
                oRequest.Credentials = CredentialCache.DefaultCredentials;
                oRequest.Method = "GET";
                oRequest.KeepAlive = true;

                using (oResponse = (HttpWebResponse)oRequest.GetResponse())
                {
                    using (Stream dataStream = oResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            responseResult = reader.ReadToEnd();
                        }
                    }

                    if (oResponse.StatusCode == HttpStatusCode.OK)
                    {
                        Debug.WriteLine(responseResult);
                    }
                }
            }
            catch (WebException ex)
            {
                Debug.WriteLine(ex);
            }

            Assert.AreNotEqual(null, oResponse);
            Assert.AreNotEqual(string.Empty, responseResult);
        }

        /// <summary>
        /// Simple retrieval of Web data over HTTP.
        /// </summary>
        [TestMethod]
        public void Getting_Data_Returns_Success()
        {
            string lcUrl = "http://www.west-wind.com/TestPage.wwd";

            // *** Establish the request
            HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(lcUrl);

            // *** Set properties
            loHttp.Timeout = 10000; // 10 secs
            loHttp.UserAgent = "Code Sample Web Client";

            // *** Retrieve request info headers
            HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();

            Encoding enc = Encoding.GetEncoding(1252); // Windows default Code Page

            StreamReader loResponseStream = new StreamReader(loWebResponse.GetResponseStream(), enc);

            string lcHtml = loResponseStream.ReadToEnd();

            Debug.WriteLine(lcHtml);

            loWebResponse.Close();

            loResponseStream.Close();
        }

        /// <summary>
        /// POSTing data to the Web Server
        /// </summary>
        [TestMethod]
        public void Posting_Data_Returns_Success()
        {
            string lcUrl = "http://www.west-wind.com/testpage.wwd";

            HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(lcUrl);

            // *** Send any POST data
            string lcPostData = "Name=" + HttpUtility.UrlEncode("Rick Strahl") + "&Company=" + HttpUtility.UrlEncode("West Wind ");

            loHttp.Method = "POST";
            byte[] lbPostBuffer = Encoding.GetEncoding(1252).GetBytes(lcPostData);

            loHttp.ContentLength = lbPostBuffer.Length;

            Stream loPostData = loHttp.GetRequestStream();

            loPostData.Write(lbPostBuffer, 0, lbPostBuffer.Length);

            loPostData.Close();

            HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();

            Encoding enc = Encoding.GetEncoding(1252);

            StreamReader loResponseStream = new StreamReader(loWebResponse.GetResponseStream(), enc);

            string lcHtml = loResponseStream.ReadToEnd();

            Debug.WriteLine(lcHtml);

            loWebResponse.Close();

            loResponseStream.Close();
        }
    }
}