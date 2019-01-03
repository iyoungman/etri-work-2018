using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace EtriWork
{
    class EtriOpenApi
    {
        string question;
        string answer;
        
        string sat;
        string lat;
        string questionType;
        string questionFocus;

        string openApiURL = "http://aiopen.etri.re.kr:8000/WiseQAnal"; //질문 분석
        string openApiURL2 = "http://aiopen.etri.re.kr:8000/WiseNLU"; //정답 분석

        //string accessKey = "fab12f65-c4d4-46d0-b450-36a3bdbe37ff";
        //string accessKey2 = "fab12f65-c4d4-46d0-b450-36a3bdbe37ff"; //영준 1
        //string accessKey = "57d4f5e7-d6a4-4156-8ea1-9cea98ba157c";//춘소 2
        //string accessKey2 = "57d4f5e7-d6a4-4156-8ea1-9cea98ba157c";//춘소 2

        //string accessKey = "14a43b84-6f4c-4e01-9206-e132263087ad";//재연 3
        //string accessKey2 = "14a43b84-6f4c-4e01-9206-e132263087ad";//재연 3
        //string accessKey = "14a43b84-6f4c-4e01-9206-e132263087ad";//재연 3
          string accessKey = "6821792d-7187-48a9-8a53-571809001556";//재호 4
          string accessKey2 = "6821792d-7187-48a9-8a53-571809001556";//재호 4
        //string accessKey = "2e9b510c-69e7-4bd7-a1fc-7efd0e9d85fd";//유진
        //string accessKey2 = "2e9b510c-69e7-4bd7-a1fc-7efd0e9d85fd";//유진
        //string accessKey2 = "57d4f5e7-d6a4-4156-8ea1-9cea98ba157c";//춘소-현재 배포되어있는것


        Dictionary<string, object> request = new Dictionary<string, object>();
        Dictionary<string, string> argument = new Dictionary<string, string>();


        ArrayList text_List;
        ArrayList type_List;

        public EtriOpenApi()
        {
            text_List = new ArrayList();
            type_List = new ArrayList();
        }

        //public EtriOpenApi(string _question)
        //{
        //    question = _question;
        //}

        public void setQuestion(string _question)
        {
            question = _question;
        }

        public string getQuestionFocus()
        {
            return this.questionFocus;
        }

        public string getSat()
        {
            return this.sat;
        }

        public string getLat()
        {
            return this.lat;
        }

        public string getQustionType()
        {
            return this.questionType;
        }



        public void setAnswer(string _answer)
        {
            answer = _answer;
        }


        public ArrayList getTextList()
        {
            return text_List;
        }

        public ArrayList getTypeList()
        {
            return type_List;
        }









       
        public void useApi()
        {
            #region 질문분석 Api

            argument.Add("text", question);
            request.Add("access_key", accessKey);
            request.Add("argument", argument);

            string responBody = null;

            try
            {

                var wq = (HttpWebRequest)WebRequest.Create(openApiURL);
                wq.Method = "POST";
                wq.ContentType = "Application/json; charset=utf-8";

                //네트워크 상으로 데이터를 보내기 위해서 직렬화
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                string json = serializer.Serialize((object)request);

                //보내기
                var sendstream = new StreamWriter(wq.GetRequestStream());
                sendstream.Write(json);
                sendstream.Flush();
                sendstream.Close();

                //전송 응답
                var response = (HttpWebResponse)wq.GetResponse();
                Stream resppoststream = response.GetResponseStream();
                StreamReader readerPost = new StreamReader(resppoststream, Encoding.UTF8);

                ////응답 코드
                //responseCode = (int)response.StatusCode;

                //응답 Body
                responBody = readerPost.ReadToEnd();

                //연결닫기
                readerPost.Close();
                resppoststream.Close();
                response.Close();

                dynamic myObject = JsonConvert.DeserializeObject<dynamic>(responBody);

                decimal Amount = Convert.ToDecimal(myObject.Amount);
                string Message = myObject.Message;

                etriOpenApiParser(responBody.ToString());

            
            }

            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }

            #endregion
        }

        public void etriOpenApiParser(string text)
        {
            #region 질문분석 파싱
            //text.Replace(",", " ");

            JObject obj = JObject.Parse(text);//jobject 형태로 받는다.
            JObject obj2 = JObject.Parse(obj["return_object"]["orgQInfo"]["orgQUnit"].ToString());//jobject 형태로 받는다.
            
            JArray array = JArray.Parse(obj2["vQFs"].ToString());
            JArray array2 = JArray.Parse(obj2["vSATs"].ToString());
            JArray array3 = JArray.Parse(obj2["vLATs"].ToString());

            questionType = obj["return_object"]["QClassification"]["ansQType"]["strQType4Chg"].ToString();//jobject 형태로 받는다.


            //test = obj["return_object"]["orgQInfo"]["orgQUnit"].ToString();
            //JArray array = JArray.Parse(obj["return_object"]["orgQInfo"]["orgQUnit"]["vQTs"].ToString());

           
            //    ////questionFocus = obj["return_object"]["orgQInfo"]["orgQUnit"]["vQts"][0]["strQTClue"].ToString();
            foreach (JObject itemObj in array)
            {

                questionFocus = itemObj["strQF"].ToString();
                break;
         
            }

            foreach (JObject itemObj2 in array2)
            {
               
                sat = itemObj2["strSAT"].ToString();
                break;

            }

            foreach (JObject itemObj2 in array3)
            {

                lat = itemObj2["strLAT"].ToString();
                break;

            }

             #endregion
        }



        public void useApi2()
        {
            #region 정답분석 Api

            argument.Add("analysis_code", "ner");
            argument.Add("text", answer);
            request.Add("access_key", accessKey2);
            request.Add("argument", argument);

            string responBody = null;

            try
            {

                var wq = (HttpWebRequest)WebRequest.Create(openApiURL2);
                wq.Method = "POST";
                wq.ContentType = "Application/json; charset=utf-8";

                //네트워크 상으로 데이터를 보내기 위해서 직렬화
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                string json = serializer.Serialize((object)request);

                //보내기
                var sendstream = new StreamWriter(wq.GetRequestStream());
                sendstream.Write(json);
                sendstream.Flush();
                sendstream.Close();

                //전송 응답
                var response = (HttpWebResponse)wq.GetResponse();
                Stream resppoststream = response.GetResponseStream();
                StreamReader readerPost = new StreamReader(resppoststream, Encoding.UTF8);

                ////응답 코드
                //responseCode = (int)response.StatusCode;

                //응답 Body
                responBody = readerPost.ReadToEnd();

                //연결닫기
                readerPost.Close();
                resppoststream.Close();
                response.Close();

                dynamic myObject = JsonConvert.DeserializeObject<dynamic>(responBody);

                decimal Amount = Convert.ToDecimal(myObject.Amount);
                string Message = myObject.Message;

                etriOpenApiParser2(responBody.ToString());


            }

            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }

            #endregion
        }


        public void etriOpenApiParser2(string answer)
        {
            #region 정답분석 파싱

            text_List.Clear();
            type_List.Clear();

            JObject obj = JObject.Parse(answer);//jobject 형태로 받는다.
            JObject obj2 = JObject.Parse(obj["return_object"].ToString());//jobject 형태로 받는다.
            JArray array = JArray.Parse(obj2["sentence"].ToString());

            string text;
            string type;

            foreach (JObject itemObj in array)
            {
                JArray ne = JArray.Parse(itemObj["NE"].ToString());
                
                 foreach (JObject itemObj2 in ne)
                 {
                     text =  itemObj2["text"].ToString();
                     type = itemObj2["type"].ToString();

                     text_List.Add(text.ToString());
                     type_List.Add(type.ToString());
                 }
            }

            #endregion
        }   

    }
}
