using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net;
using System.Xml.Serialization;
using System.IO;

namespace Remwave.Nikotalkie
{
    class NTransport
    {
        Configuration mUserConfiguration;        

        public NTransport(Configuration configuration)
        {
            mUserConfiguration = configuration;
        }
        public static String BulildWebRequestQuery(String method, Hashtable parameters)
        {
            String query = "?" + "method=" + method;

            ICollection keys = parameters.Keys;

            foreach (string key in keys)
            {
                query += "&" + key + "=" + parameters[key].ToString();
            }
            return query;
        }
        public NResultSend SendMessage(NMessage message)
        {
            NResultSend resultSend = new NResultSend();
            resultSend.Success = false;
            //$username, $password, $recipients, $compression, $bytes, $type, $format
            try
            {

                String recipients = "";
                foreach (String recipient in message.Header.To)
                {
                    recipients += recipient + ";";
                }
                //XMLSendMessage($username, $password, $recipients, $compression, $bytes, $type, $format)
                Hashtable parameters = new Hashtable();
                parameters.Add("username", mUserConfiguration.Username);
                parameters.Add("password", mUserConfiguration.Password);
                parameters.Add("recipients", recipients);
                parameters.Add("compression", "none");
                parameters.Add("bytes", 0);
                parameters.Add("type", "XML");
                parameters.Add("format", "amr");

                
                try
                {
                    String boundary = Guid.NewGuid().ToString();
                    WebRequest requestSendMessage = WebRequest.Create(mUserConfiguration.Url + BulildWebRequestQuery("XMLSendMessage", parameters));
                    
                  

                    requestSendMessage.ContentType = "multipart/form-data; boundary=" + boundary;
                    requestSendMessage.Method = "POST";


                    //encode header
                    String postHeader="";
                    postHeader += "\r\n" + @"--" + boundary + "\r\n";
                    postHeader += "Content-Disposition: form-data; name=\"uploadFile1\"; filename=\"uploadFile1\"\r\n";
                    postHeader += "Content-Type: application/octet-stream\r\n\r\n";

                    byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);
                    byte[] boundaryBytes = Encoding.UTF8.GetBytes("\r\n" + @"--" + boundary + @"--" + "\r\n");
                    //read in the file as a stream
                    long contentLength = postHeaderBytes.Length + message.Body.AttachementBodys[0].Data.Length + boundaryBytes.Length;

                    requestSendMessage.ContentLength = contentLength;

                    Stream requestStream = requestSendMessage.GetRequestStream();
                    requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                    requestStream.Write(message.Body.AttachementBodys[0].Data, 0, message.Body.AttachementBodys[0].Data.Length);
                    requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    //requestStream.Close();


                    HttpWebResponse responseSendMessage = (HttpWebResponse)requestSendMessage.GetResponse();
                    Stream dataStream = responseSendMessage.GetResponseStream();
                    XmlSerializer des = new XmlSerializer(typeof(NResultSend));
                    resultSend = (NResultSend)des.Deserialize(new System.Xml.XmlTextReader(dataStream));
                    dataStream.Close();

                }
                catch (Exception ex)
                {
#if(DEBUG)
                    throw;
#endif
                }


            }
            catch (Exception)
            {
#if (DEBUG)
                throw;
#endif
            }
            return resultSend;
        }

        public List<NMessageHeader> ReciveHeaders()
        {
            List<NMessageHeader> messageHeadersList = null;
            if (mUserConfiguration == null) return messageHeadersList;
            //Authorize(username, password, lastmsg,type) type=XML
            Hashtable parameters = new Hashtable();
            parameters.Add("username", mUserConfiguration.Username);
            parameters.Add("password", mUserConfiguration.Password);
            parameters.Add("lastmsg", mUserConfiguration.LastMessageID);
            parameters.Add("type", "XML");
            
            try
            {
                WebRequest requestGetMessageList = WebRequest.Create(mUserConfiguration.Url + BulildWebRequestQuery("XMLGetMessageList", parameters));
                HttpWebResponse responseRequestAuthorize = (HttpWebResponse)requestGetMessageList.GetResponse();
                Stream dataStream = responseRequestAuthorize.GetResponseStream();
                XmlSerializer des = new XmlSerializer(typeof(List<NMessageHeader>));
                messageHeadersList = (List<NMessageHeader>)des.Deserialize(new System.Xml.XmlTextReader(dataStream));
                dataStream.Close();

            }
            catch (Exception ex)
            {
#if(DEBUG)
                throw;
#endif
            }

            
            return messageHeadersList;
        }

        public NMessage ReciveMessage(NMessageHeader messageHeader)
        {
            NMessage message = null;
            if (mUserConfiguration == null) return message;
                //Authorize(username, password, lastmsg,type) type=XML
                Hashtable parameters = new Hashtable();
                parameters.Add("username", mUserConfiguration.Username);
                parameters.Add("password", mUserConfiguration.Password);
                parameters.Add("messageid", messageHeader.MsgID);
                parameters.Add("type", "XML");
                parameters.Add("format", "amr");

                try
                {
                    List<NMessageBody> messageBodysList = new List<NMessageBody>();

                    WebRequest requestGetMessage = WebRequest.Create(mUserConfiguration.Url + BulildWebRequestQuery("XMLGetMessage", parameters));
                    HttpWebResponse responseGetMessage = (HttpWebResponse)requestGetMessage.GetResponse();
                    Stream dataStream = responseGetMessage.GetResponseStream();

                    //String DATA = new StreamReader(dataStream).ReadToEnd();
                    XmlSerializer des = new XmlSerializer(typeof(NMessageBody));
                    NMessageBody messageBody = (NMessageBody)des.Deserialize(new System.Xml.XmlTextReader(dataStream));
                    dataStream.Close();
                    message = new NMessage(messageHeader, messageBody);
                    
                }
                catch (Exception ex)
                {
#if(DEBUG)
                    throw;
#endif
                }
                return message;
            }
    }
}
