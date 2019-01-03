using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;


namespace EtriWork
{
    class WorkFormatt
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
            public bool confuseQt { get; set; }
            public bool confuseQf { get; set; }
            public bool confuseLat { get; set; }
            public bool confuseSat { get; set; }
            public double time { get; set; }
            
            public string question { get; set; }
            public string question_en { get; set; }
            public string question_tagged { get; set; }
            public string questionType { get; set; }
            public string questionFocus { get; set; }
            public string questionSAT { get; set; }
            public string questionLAT { get; set; }
            public bool etriQtCheck { get; set; }//추가
            public bool etriQfCheck { get; set; }//추가
            public bool etriLatCheck { get; set; }//추가
            public bool etriSatCheck { get; set; }//추가
            public string etriQt { get; set; }//추가
            public string etriQf { get; set; }//추가
            public string etriLat { get; set; }//추가
            public string etriSat { get; set; }//추가
            public bool checkIndividual { get; set; }//추가
            

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
            public List<Datum> data { get; set; }
        }


       
    }
}
