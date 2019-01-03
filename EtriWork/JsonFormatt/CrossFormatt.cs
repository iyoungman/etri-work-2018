using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtriWork
{
    class CrossFormatt
    {
        public class Answer
        {
            public string text { get; set; }
            public string text_en { get; set; }
            public string text_tagged { get; set; }
            public string text_syn { get; set; }
            public int answer_start { get; set; }
            public int answer_end { get; set; }
        }

        public class Qa
        {
            public string id { get; set; }
            public bool confuseQt1 { get; set; }
            public bool confuseQf1 { get; set; }
            public bool confuseLat1 { get; set; }
            public bool confuseSat1 { get; set; }

            public string question { get; set; }
            public string question_en { get; set; }
            public string question_tagged1 { get; set; }
            public string questionType1 { get; set; }
            public string questionFocus1 { get; set; }
            public string questionSAT1 { get; set; }
            public string questionLAT1 { get; set; }

/*----------------------------------------------------------*/
            public bool confuseQt2 { get; set; }
            public bool confuseQf2 { get; set; }
            public bool confuseLat2 { get; set; }
            public bool confuseSat2 { get; set; }          
            public string question_tagged2 { get; set; }
            public string questionType2 { get; set; }
            public string questionFocus2 { get; set; }
            public string questionSAT2 { get; set; }
            public string questionLAT2 { get; set; }

            
/*----------------------------------------------------------*/

            public bool confuseQt3 { get; set; }
            public bool confuseQf3 { get; set; }
            public bool confuseLat3 { get; set; }
            public bool confuseSat3 { get; set; }
            public string question_tagged3 { get; set; }
            public string questionType3 { get; set; }
            public string questionFocus3 { get; set; }
            public string questionSAT3 { get; set; }
            public string questionLAT3 { get; set; }



            public List<Answer> answers { get; set; }


            internal void add(Answer j)
            {
                throw new NotImplementedException();
            }
        }

        public class Paragraph
        {
            public string context { get; set; }
            public string context_en { get; set; }
            public string context_tagged { get; set; }
            public List<Qa> qas { get; set; }
        }

        public class Datum
        {
            public string title { get; set; }
            public List<Paragraph> paragraphs { get; set; }
        }

        public class RootObject
        {
            public string version { get; set; }
            public string creator { get; set; }
            public int progress { get; set; }
            public string formatt { get; set; }
            public double time { get; set; }
            public bool check { get; set; }//추가
            public string firstfile { get; set; }//추가
            public string secondfile { get; set; }//추가
            public List<Datum> data { get; set; }
        }


    }
}
