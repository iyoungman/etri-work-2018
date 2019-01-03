using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace EtriWork
{
    public partial class StatusForm : Form
    {
        #region 변수
        static List<double> Time_List;

        static List<bool> EtriQtCheck_List;
        static List<bool> EtriQfCheck_List;
        static List<bool> EtriLatCheck_List;
        static List<bool> EtriSatCheck_List;

        static ArrayList Check1_QuestionType;
        static ArrayList Check1_QuestionFocus;
        static ArrayList Check1_QuestionLat;
        public static ArrayList Check1_QuestionSat;

        static ArrayList CheckWorker1_QuestionType;
        static ArrayList CheckWorker1_QuestionFocus;
        static ArrayList CheckWorker1_QuestionLat;
        static ArrayList CheckWorker1_QuestionSat;

        static ArrayList CheckWorker2_QuestionType;
        static ArrayList CheckWorker2_QuestionFocus;
        static ArrayList CheckWorker2_QuestionLat;
        static ArrayList CheckWorker2_QuestionSat;

        static ArrayList CheckAdmin_QuestionType;
        static ArrayList CheckAdmin_QuestionFocus;
        static ArrayList CheckAdmin_QuestionLat;
        static ArrayList CheckAdmin_QuestionSat;

        public static List<int> FilesQuestionCount; //파일별 문제수
        public static List<string> FilesTextList;
        public static int startNum;
        public static int endNum;

        public static List<int> FilesCrossQuestionCount; //파일별 문제수
        public static List<string> FilesCrossTextList;
        public static int startWrongNum;
        public static int endWrongNum;

        private List<double> a;
        private List<double> b;
        private List<double> c;
        private List<double> d;

        static List<bool> CrossCheck_List1;
        static List<bool> CrossCheck_List2;
        static List<bool> CrossCheck_List3;
        static List<bool> CrossCheck_List4;

        int sumQtCount;
        int sumQfCount;
        int sumLatCount;
        int sumSatCount;

        static List<int> wrongQtCount1;
        static List<int> wrongQfCount1;
        static List<int> wrongLatCount1;
        static List<int> wrongSatCount1;

        static List<int> wrongQtCount2;
        static List<int> wrongQfCount2;
        static List<int> wrongLatCount2;
        static List<int> wrongSatCount2;

        StatusSATDto dto = new StatusSATDto();
        #endregion

        public StatusForm()
        {
            InitializeComponent();
        }

        private void initFile1()
        {
            #region initFile1
            FilesTextList = new List<string>();
            FilesQuestionCount = new List<int>();

            a = new List<double>();
            b = new List<double>();
            c = new List<double>();
            d = new List<double>();

            Check1_QuestionType = new ArrayList();
            Check1_QuestionFocus = new ArrayList();
            Check1_QuestionLat = new ArrayList();
            Check1_QuestionSat = new ArrayList();

            EtriQtCheck_List = new List<bool>();
            EtriQfCheck_List = new List<bool>();
            EtriLatCheck_List = new List<bool>();
            EtriSatCheck_List = new List<bool>();

            Time_List = new List<double>();

            #endregion
        }

        private void initFile2()
        {
            #region initFile2
            wrongQtCount1 = new List<int>();
            wrongQfCount1 = new List<int>();
            wrongLatCount1 = new List<int>();
            wrongSatCount1 = new List<int>();

            wrongQtCount2 = new List<int>();
            wrongQfCount2 = new List<int>();
            wrongLatCount2 = new List<int>();
            wrongSatCount2 = new List<int>();

            FilesCrossTextList = new List<string>();
            FilesCrossQuestionCount = new List<int>();

            a = new List<double>();
            b = new List<double>();
            c = new List<double>();
            d = new List<double>();

            CrossCheck_List1 = new List<bool>();
            CrossCheck_List2 = new List<bool>();
            CrossCheck_List3 = new List<bool>();
            CrossCheck_List4 = new List<bool>();

            CheckWorker1_QuestionType = new ArrayList();
            CheckWorker1_QuestionFocus = new ArrayList();
            CheckWorker1_QuestionLat = new ArrayList();
            CheckWorker1_QuestionSat = new ArrayList();

            CheckWorker2_QuestionType = new ArrayList();
            CheckWorker2_QuestionFocus = new ArrayList();
            CheckWorker2_QuestionLat = new ArrayList();
            CheckWorker2_QuestionSat = new ArrayList();

            CheckAdmin_QuestionType = new ArrayList();
            CheckAdmin_QuestionFocus = new ArrayList();
            CheckAdmin_QuestionLat = new ArrayList();
            CheckAdmin_QuestionSat = new ArrayList();

            #endregion
        }

        private void originalReadParser(string text, int fileStart, int fileEnd)
        {
            #region Original파일 불러오기

            JObject obj = JObject.Parse(text);
            JArray array = JArray.Parse(obj["data"].ToString());

            string c;
            c = obj["formatt"].ToString();

            string b;
            string d, e, f, g;

            double time;
            bool aaa, bbb, ccc, ddd;

            int count = 0;
            int fileCount = 0;

            try
            {

                foreach (JObject itemObj in array)
                {
                    JArray ooo = JArray.Parse(itemObj["paragraphs"].ToString());

                    foreach (JObject itemObj2 in ooo)
                    {
                        JArray iii = JArray.Parse(itemObj2["qas"].ToString());

                        foreach (JObject itemObj3 in iii)
                        {
                            count++;
                            b = itemObj3["question"].ToString();

                            ////////////
                            d = itemObj3["questionType"].ToString();
                            e = itemObj3["questionFocus"].ToString();
                            f = itemObj3["questionLAT"].ToString();
                            g = itemObj3["questionSAT"].ToString();

                            aaa = Convert.ToBoolean(itemObj3["etriQtCheck"]);
                            bbb = Convert.ToBoolean(itemObj3["etriQfCheck"]);
                            ccc = Convert.ToBoolean(itemObj3["etriLatCheck"]);
                            ddd = Convert.ToBoolean(itemObj3["etriSatCheck"]);

                            time = Convert.ToDouble(itemObj3["time"]);

                            if (fileStart <= count && count <= fileEnd)
                            {
                                fileCount++;
                                Check1_QuestionType.Add(d);
                                Check1_QuestionFocus.Add(e);
                                Check1_QuestionLat.Add(f);
                                Check1_QuestionSat.Add(g);

                                EtriQtCheck_List.Add(aaa);
                                EtriQfCheck_List.Add(bbb);
                                EtriLatCheck_List.Add(ccc);
                                EtriSatCheck_List.Add(ddd);

                                Time_List.Add(time);
                            }

                            JArray yyy = JArray.Parse(itemObj3["answers"].ToString());

                            foreach (JObject itemObj4 in yyy)
                            {


                            }

                        }
                    }

                }

            }
            catch (Exception exception)
            {
                throw exception;
            }

            //파일 개수 목록
            FilesQuestionCount.Add(fileCount);

            //for (int i = 0; i < FilesQuestionCount.Count; i++)
            //    MessageBox.Show("개수는" + FilesQuestionCount[i].ToString());

            #endregion
        }

        private void resetOriginal_TextBox()
        {
            #region Original파일 불러올때 Reset
            TextBox_파일이름.Text = "";
            TextBox_시간.Text = "";
            TextBox_전체평균시간.Text = "";
            TextBox_시간표준편차.Text = "";
            TextBox_총개수.Text = "";
            TextBox_질문시작.Text = "";
            TextBox_질문끝.Text = "";
            TextBox_유형전체평균.Text = "";
            TextBox_Etri질문유형.Text = "";
            TextBox_Etri질문유형비율.Text = "";
            TextBox_Etri질문초점.Text = "";
            TextBox_Etri질문초점비율.Text = "";
            TextBox_EtriLAT.Text = "";
            TextBox_EtriLAT비율.Text = "";
            TextBox_EtriSAT.Text = "";
            TextBox_단답형.Text = "";
            TextBox_단답형비율.Text = "";
            TextBox_나열형.Text = "";
            TextBox_나열형비율.Text = "";
            TextBox_정의.Text = "";
            TextBox_정의비율.Text = "";
            TextBox_이유.Text = "";
            TextBox_이유비율.Text = "";
            TextBox_방법.Text = "";
            TextBox_방법비율.Text = "";
            TextBox_목적.Text = "";
            TextBox_목적비율.Text = "";
            TextBox_조건.Text = "";
            TextBox_조건비율.Text = "";
            TextBox_기타.Text = "";
            TextBox_기타비율.Text = "";
            TextBox_서술형.Text = "";
            TextBox_서술형비율.Text = "";
            TextBox_의미.Text = "";
            TextBox_의미비율.Text = "";
            #endregion
        }


        private void EtriApiCheck_Btn_Click(object sender, EventArgs e)
        {
            #region WorkJson관련 정보 버튼
            try
            {
                startNum = 0;
                endNum = 0;

                if (workFile_comboBox.Text == "")
                {
                    MessageBox.Show("파일을 선택해주세요");
                    return;
                }

                int index = workFile_comboBox.SelectedIndex;

                if (index == 0)//전체 선택
                {
                    startNum = 1;
                    endNum = Check1_QuestionType.Count;
                }
                else //개별 선택
                {
                    if (index == 1) startNum = 0;//첫번째 선택
                    else//두번째 이후로 선택
                    {
                        for (int i = 0; i < index - 1; i++)
                        {
                            startNum = startNum + FilesQuestionCount[i];
                        }
                    }
                    endNum = startNum + FilesQuestionCount[index - 1];
                    startNum = startNum + 1;
                }

                TextBox_선택개수.Text = (endNum - startNum + 1).ToString();

                //평균시간
                if (TextBox_전체평균시간.Text == "")
                {
                    //시간 표준편차세팅
                    setStandardDeviationAverageTime(setAllAverageTime());
                }
                setAllAverageTime(startNum, endNum);//개별

                //EtriApi 세팅
                bool all = true;
                calcurateEtriApi(1, Check1_QuestionType.Count, all);//전체평균세팅
                all = false;
                calcurateEtriApi(startNum, endNum, all);//개별평균세팅
                calcurateStandardDeviationEtriApi();//표준편차 세팅

                //QuestionType세팅
                setQuestionType(startNum, endNum);

            }
            catch
            {
                MessageBox.Show("오류 발생");
            }

            #endregion
        }

        private void setStandardDeviationAverageTime(double averageTime)
        {
            #region 시간 표준편차 셋팅
            int start = 1;
            int end = 0;

            List<double> averageTime_List = new List<double>();
            averageTime_List.Clear();

            for (int j = 0; j < FilesQuestionCount.Count; j++)
            {
                end = start + FilesQuestionCount[j] - 1;
                double time = 0;

                int fileCount = end - start + 1;

                for (int i = start - 1; i < end; i++)
                {
                    time = time + Time_List[i];
                    //한 파일당 개수를 나눈다.
                }

                time = (time / fileCount);
                time = Math.Round(time, 2);
                //MessageBox.Show(time.ToString());

                averageTime_List.Add(time);

                start = end + 1;
            }

            TextBox_시간표준편차.Text = Math.Round(calcurateEtriApiStandardDeviation(averageTime_List, averageTime), 2).ToString();
            #endregion
        }

        private void setQuestionType(int start, int end)
        {
            #region 질문유형 세팅
            double sumCount = end - start + 1;

            int 단답형 = 0; int 나열형 = 0; int 서술형 = 0; int 정의 = 0; int 이유 = 0; int 방법 = 0; int 목적 = 0; int 조건 = 0; int 기타 = 0; int 의미 = 0;

            for (int i = start - 1; i < end; i++)
            {
                switch (Check1_QuestionType[i].ToString())
                {
                    case "단답형":
                        단답형++;
                        break;
                    case "나열형":
                        나열형++;
                        break;
                    case "서술형-정의":
                        정의++;
                        서술형++;
                        break;
                    case "서술형-이유":
                        이유++;
                        서술형++;
                        break;
                    case "서술형-방법":
                        방법++;
                        서술형++;
                        break;
                    case "서술형-목적":
                        목적++;
                        서술형++;
                        break;
                    case "서술형-조건":
                        조건++;
                        서술형++;
                        break;
                    case "서술형-기타":
                        기타++;
                        서술형++;
                        break;
                    case "서술형-의미":
                        의미++;
                        서술형++;
                        break;
                }
            }

            TextBox_단답형.Text = 단답형.ToString() + " / " + sumCount.ToString();
            TextBox_나열형.Text = 나열형.ToString() + " / " + sumCount.ToString();
            TextBox_서술형.Text = 서술형.ToString() + " / " + sumCount.ToString();
            TextBox_정의.Text = 정의.ToString() + " / " + sumCount.ToString();
            TextBox_이유.Text = 이유.ToString() + " / " + sumCount.ToString();
            TextBox_방법.Text = 방법.ToString() + " / " + sumCount.ToString();
            TextBox_목적.Text = 목적.ToString() + " / " + sumCount.ToString();
            TextBox_조건.Text = 조건.ToString() + " / " + sumCount.ToString();
            TextBox_기타.Text = 기타.ToString() + " / " + sumCount.ToString();
            TextBox_의미.Text = 의미.ToString() + " / " + sumCount.ToString();

            TextBox_단답형비율.Text = (단답형 / sumCount * 100).ToString("N2") + "%";
            TextBox_나열형비율.Text = (나열형 / sumCount * 100).ToString("N2") + "%";
            TextBox_서술형비율.Text = (서술형 / sumCount * 100).ToString("N2") + "%";
            TextBox_정의비율.Text = (정의 / sumCount * 100).ToString("N2") + "%";
            TextBox_이유비율.Text = (이유 / sumCount * 100).ToString("N2") + "%";
            TextBox_방법비율.Text = (방법 / sumCount * 100).ToString("N2") + "%";
            TextBox_목적비율.Text = (목적 / sumCount * 100).ToString("N2") + "%";
            TextBox_조건비율.Text = (조건 / sumCount * 100).ToString("N2") + "%";
            TextBox_기타비율.Text = (기타 / sumCount * 100).ToString("N2") + "%";
            TextBox_의미비율.Text = (의미 / sumCount * 100).ToString("N2") + "%";
            #endregion
        }

        private double setAllAverageTime()
        {
            #region 전체 평균시간 세팅
            double time = 0;

            for (int i = 0; i < Check1_QuestionType.Count; i++)
            {
                time = time + Convert.ToDouble(Time_List[i]);
            }

            time = (time / Check1_QuestionType.Count);
            time = Math.Round(time, 2);//소수점 2자리 반올림
            TextBox_전체평균시간.Text = time.ToString();

            return time;
            #endregion
        }

        private void setAllAverageTime(int start, int end)
        {
            #region 개별 평균시간 세팅
            double time = 0;

            for (int i = start - 1; i < end; i++)
            {
                time = time + Convert.ToDouble(Time_List[i]);
            }

            time = (time / (end - start + 1));
            time = Math.Round(time, 2);//소수점 2자리 반올림
            TextBox_시간.Text = time.ToString();
            #endregion
        }

        private void calcurateEtriApi(int start, int end, bool allCheck)
        {
            #region EtriOpenApi사용 계산
            int qtCount = 0; int qfCount = 0; int latCount = 0; int satCount = 0;

            double sumCount = end - start + 1;

            for (int i = start - 1; i < end; i++)
            {
                if (Convert.ToBoolean(EtriQtCheck_List[i]))
                {
                    qtCount++;
                }
                if (Convert.ToBoolean(EtriQfCheck_List[i]))
                {
                    qfCount++;
                }
                if (Convert.ToBoolean(EtriLatCheck_List[i]))
                {
                    latCount++;
                }
                if (Convert.ToBoolean(EtriSatCheck_List[i]))
                {
                    satCount++;
                }
            }

            if (allCheck == true)//전체이면
            {
                TextBox_유형전체평균.Text = (qtCount / sumCount * 100).ToString("N2") + "%";
                TextBox_초점전체평균.Text = (qfCount / sumCount * 100).ToString("N2") + "%";
                TextBox_LAT전체평균.Text = (latCount / sumCount * 100).ToString("N2") + "%";
                TextBox_SAT전체평균.Text = (satCount / sumCount * 100).ToString("N2") + "%";
            }
            else//개별평균
            {
                TextBox_Etri질문유형.Text = qtCount.ToString() + " / " + sumCount.ToString();
                TextBox_Etri질문초점.Text = qfCount.ToString() + " / " + sumCount.ToString();
                TextBox_EtriLAT.Text = latCount.ToString() + " / " + sumCount.ToString();
                TextBox_EtriSAT.Text = satCount.ToString() + " / " + sumCount.ToString();

                TextBox_Etri질문유형비율.Text = (qtCount / sumCount * 100).ToString("N2") + "%";
                TextBox_Etri질문초점비율.Text = (qfCount / sumCount * 100).ToString("N2") + "%";
                TextBox_EtriLAT비율.Text = (latCount / sumCount * 100).ToString("N2") + "%";
                TextBox_EtriSAT비율.Text = (satCount / sumCount * 100).ToString("N2") + "%";
            }
            #endregion
        }

        private void calcurateStandardDeviationEtriApi()
        {
            #region EtriOpenApi 표준편차 계산
            int start = 1;
            int end = 0;

            a.Clear(); b.Clear(); c.Clear(); d.Clear();

            for (int j = 0; j < FilesQuestionCount.Count; j++)
            {
                end = start + FilesQuestionCount[j] - 1;
                int qtCount = 0; int qfCount = 0; int latCount = 0; int satCount = 0;
                double sumCount = end - start + 1;

                for (int i = start - 1; i < end; i++)
                {
                    if (Convert.ToBoolean(EtriQtCheck_List[i]))
                    {
                        qtCount++;
                    }
                    if (Convert.ToBoolean(EtriQfCheck_List[i]))
                    {
                        qfCount++;
                    }
                    if (Convert.ToBoolean(EtriLatCheck_List[i]))
                    {
                        latCount++;
                    }
                    if (Convert.ToBoolean(EtriSatCheck_List[i]))
                    {
                        satCount++;
                    }
                }

                a.Add(qtCount / sumCount * 100);
                b.Add(qfCount / sumCount * 100);
                c.Add(latCount / sumCount * 100);
                d.Add(satCount / sumCount * 100);

                start = end + 1;
            }

            string set1 = TextBox_유형전체평균.Text.Replace("%", "");
            double set1_1 = Convert.ToDouble(set1);
            TextBox_유형표준편차.Text = calcurateEtriApiStandardDeviation(a, set1_1).ToString("N2") + "%";

            set1 = TextBox_초점전체평균.Text.Replace("%", "");
            set1_1 = Convert.ToDouble(set1);
            TextBox_초점표준편차.Text = calcurateEtriApiStandardDeviation(b, set1_1).ToString("N2") + "%";

            set1 = TextBox_LAT전체평균.Text.Replace("%", "");
            set1_1 = Convert.ToDouble(set1);
            TextBox_LAT표준편차.Text = calcurateEtriApiStandardDeviation(c, set1_1).ToString("N2") + "%";

            set1 = TextBox_SAT전체평균.Text.Replace("%", "");
            set1_1 = Convert.ToDouble(set1);
            TextBox_SAT표준편차.Text = calcurateEtriApiStandardDeviation(d, set1_1).ToString("N2") + "%";
            #endregion
        }


        public double calcurateEtriApiStandardDeviation(List<double> valueList, double average)
        {
            #region 표준편차 구하는 식
            if (average == 0)
            {
                return 0;
            }
            else
            {
                double variance = 0d;

                foreach (double value in valueList)
                {
                    variance += Math.Pow(value - average, 2);
                }
                return Math.Sqrt(variance / valueList.Count);
            }
            #endregion
        }

        private string convertTime(double time)
        {
            #region 시간을 시,분,초로 변환
            double hour = Math.Round(time / 3600);
            double minute = Math.Round((time % 3600) / 60);
            double sec = Math.Round(time % 60);

            string result = hour.ToString() + "시간" + minute.ToString() + "분" + sec.ToString() + "초";
            return result;
            #endregion
        }

        private void Compare_Btn_Click(object sender, EventArgs e)
        {
            #region Compare파일 불러오기
            List<string> filePathList = new List<string>();

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt"; ;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "CrossJson파일 선택";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                resetCompare_TextBox();

                filePathList.AddRange(openFileDialog.FileNames);
                convertCrossFiles(filePathList);
            }
            #endregion
        }

        private void convertCrossFiles(IList<string> filePaths)
        {
            #region CrossFile리스트
            initFile2();
            compareFile_comboBox.Items.Clear();
            compareFile_comboBox.Items.Add("전체");//전체파일

            foreach (var item in filePaths)
            {
                int index = item.LastIndexOf(@"\");
                string fileName = item.Substring(index + 1, item.Length - index - 1);
                //fileName에서 빼올것: start & end
                int start = getStartNum(fileName);
                int end = getEndNum(fileName);

                compareFile_comboBox.Items.Add(fileName);
                convertCross(item, start, end);
            }

            compareFile_comboBox.Text = "전체";
            TextBox_크로스총개수.Text = CheckAdmin_QuestionType.Count.ToString();
            #endregion
        }

        private void convertCross(string filepath, int fileStart, int fileEnd)
        {
            #region Cross파일 파싱
            FileStream fs_read = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader stream_read = new StreamReader(fs_read, System.Text.Encoding.UTF8);

            String mod = stream_read.ReadToEnd();
            FilesCrossTextList.Add(mod);

            stream_read.Close();
            fs_read.Close();

            compareReadParser(mod, fileStart, fileEnd);
            #endregion
        }

        private void compareReadParser(string text, int fileStart, int fileEnd)
        {
            #region Cross파일 읽기
            JObject obj = JObject.Parse(text);
            JArray array = JArray.Parse(obj["data"].ToString());

            string c;
            c = obj["formatt"].ToString();

            if (c != "Cross")
            {
                MessageBox.Show("CrossJson파일 형식이 맞지 않습니다.");
                return;
            }

            string b;
            int count = 0;
            int fileCount = 0;
            string worker1T, worker1F, worker1L, worker1S;
            string worker2T, worker2F, worker2L, worker2S;
            string adminT, adminF, adminL, adminS;

            foreach (JObject itemObj in array)
            {
                JArray ooo = JArray.Parse(itemObj["paragraphs"].ToString());

                foreach (JObject itemObj2 in ooo)
                {
                    JArray iii = JArray.Parse(itemObj2["qas"].ToString());

                    foreach (JObject itemObj3 in iii)
                    {
                        b = itemObj3["question"].ToString();
                        count++;

                        worker1T = itemObj3["questionType1"].ToString();
                        worker1F = itemObj3["questionFocus1"].ToString();
                        worker1L = itemObj3["questionLAT1"].ToString();
                        worker1S = itemObj3["questionSAT1"].ToString();
                        worker2T = itemObj3["questionType2"].ToString();
                        worker2F = itemObj3["questionFocus2"].ToString();
                        worker2L = itemObj3["questionLAT2"].ToString();
                        worker2S = itemObj3["questionSAT2"].ToString();
                        adminT = itemObj3["questionType3"].ToString();
                        adminF = itemObj3["questionFocus3"].ToString();
                        adminL = itemObj3["questionLAT3"].ToString();
                        adminS = itemObj3["questionSAT3"].ToString();

                        if (fileStart <= count && count <= fileEnd)
                        {
                            fileCount++;
                            CheckWorker1_QuestionType.Add(worker1T);
                            CheckWorker1_QuestionFocus.Add(worker1F);
                            CheckWorker1_QuestionLat.Add(worker1L);
                            CheckWorker1_QuestionSat.Add(worker1S);

                            CheckWorker2_QuestionType.Add(worker2T);
                            CheckWorker2_QuestionFocus.Add(worker2F);
                            CheckWorker2_QuestionLat.Add(worker2L);
                            CheckWorker2_QuestionSat.Add(worker2S);

                            CheckAdmin_QuestionType.Add(adminT);
                            CheckAdmin_QuestionFocus.Add(adminF);
                            CheckAdmin_QuestionLat.Add(adminL);
                            CheckAdmin_QuestionSat.Add(adminS);
                        }

                        JArray yyy = JArray.Parse(itemObj3["answers"].ToString());

                        foreach (JObject itemObj4 in yyy)
                        {

                        }

                    }
                }

            }

            FilesCrossQuestionCount.Add(fileCount);

            //for (int i = 0; i < FilesCrossQuestionCount.Count; i++)
            //    MessageBox.Show("크로스개 수는" + FilesCrossQuestionCount[i].ToString() + " wdw" + CheckWorker1_QuestionType.Count.ToString());


            #endregion
        }

        private void resetCompare_TextBox()
        {
            #region Cross파일 불러올때 Reset
            TextBox_비교파일.Text = "";

            TextBox_Wrong작업자질문유형비율.Text = "";
            TextBox_Wrong작업자질문초점비율.Text = "";
            TextBox_Wrong작업자LAT비율.Text = "";
            TextBox_Wrong작업자SAT비율.Text = "";

            TextBox_Wrong작업관리자SAT비율.Text = "";
            TextBox_Wrong작업관리자LAT비율.Text = "";
            TextBox_Wrong작업관리자질문유형비율.Text = "";
            TextBox_Wrong작업관리자질문초점비율.Text = "";

            TextBox_Wrong작업관리자2SAT비율.Text = "";
            TextBox_Wrong작업관리자2LAT비율.Text = "";
            TextBox_Wrong작업관리자2질문유형비율.Text = "";
            TextBox_Wrong작업관리자2질문초점비율.Text = "";

            TextBox_Wrong작업자질문유형전체평균.Text = "";
            TextBox_Wrong작업자질문초점전체평균.Text = "";
            TextBox_Wrong작업자LAT전체평균.Text = "";
            TextBox_Wrong작업자SAT전체평균.Text = "";

            TextBox_Wrong작업관리자질문유형전체평균.Text = "";
            TextBox_Wrong작업관리자질문초점전체평균.Text = "";
            TextBox_Wrong작업관리자LAT전체평균.Text = "";
            TextBox_Wrong작업관리자SAT전체평균.Text = "";

            TextBox_Wrong작업자질문유형표준편차.Text = "";
            TextBox_Wrong작업자질문초점표준편차.Text = "";
            TextBox_Wrong작업자LAT표준편차.Text = "";
            TextBox_Wrong작업자SAT표준편차.Text = "";

            TextBox_Wrong작업관리자질문유형표준편차.Text = "";
            TextBox_Wrong작업관리자질문초점표준편차.Text = "";
            TextBox_Wrong작업관리자LAT표준편차.Text = "";
            TextBox_Wrong작업관리자SAT표준편차.Text = "";

            #endregion
        }

        private void WrongCheck_Btn_Click(object sender, EventArgs e)
        {
            #region 오답체크 버튼
            try
            {
                sumQtCount = 0;
                sumQfCount = 0;
                sumLatCount = 0;
                sumSatCount = 0;
                startWrongNum = 0;
                endWrongNum = 0;

                if (compareFile_comboBox.Text == "")
                {
                    MessageBox.Show("파일을 선택해주세요");
                    return;
                }

                int index = compareFile_comboBox.SelectedIndex;

                if (index == 0)//전체 선택
                {
                    startWrongNum = 1;
                    endWrongNum = CheckWorker1_QuestionType.Count;
                    resetIndividualTextBox();
                }
                else //개별 선택
                {
                    if (index == 1) startWrongNum = 0;//첫번째 선택
                    else//두번째 이후로 선택
                    {
                        for (int i = 0; i < index - 1; i++)
                        {
                            startWrongNum = startWrongNum + FilesCrossQuestionCount[i];
                        }
                    }
                    endWrongNum = startWrongNum + FilesCrossQuestionCount[index - 1];
                    startWrongNum = startWrongNum + 1;
                }

                TextBox_크로스선택개수.Text = (endWrongNum - startWrongNum + 1).ToString();

                if (TextBox_Wrong작업자질문유형전체평균.Text == "")
                {
                    checkWrongResult();//전체 평균 셋팅
                    setStandardDeviationWorkerWrong();//전체 표준편차 셋팅
                }

                if (TextBox_Wrong작업관리자질문유형전체평균.Text == "")
                {
                    checkAdminWrongResult(true);//전체 평균 셋팅
                    checkAdminWrongResult(false);
                    //전체 표준편차 셋팅
                    setStandardDeviationAdminWrong();
                }

                //작업자간 개별 세팅
                if (index >= 1)
                    setWrongAverage(startWrongNum, endWrongNum);

                //작업자-관리자간 개별 세팅
                if (index >= 1)
                    setWrongAdminAverage(startWrongNum, endWrongNum, index - 1);
                else
                {
                    //TextBox_Wrong작업관리자질문유형비율.Text = TextBox_Wrong작업관리자질문유형전체평균.Text;
                    //TextBox_Wrong작업관리자질문초점비율.Text = TextBox_Wrong작업관리자질문초점전체평균.Text;
                    //TextBox_Wrong작업관리자LAT비율.Text = TextBox_Wrong작업관리자LAT전체평균.Text;
                    //TextBox_Wrong작업관리자SAT비율.Text = TextBox_Wrong작업관리자SAT전체평균.Text;
                }

            }
            catch
            {
                MessageBox.Show("올바른 셋팅을 해주세요");
            }

            #endregion
        }

        public void resetIndividualTextBox()
        {
            #region 전체 일때 파일 평균 TextBox Reset
            TextBox_Wrong작업자SAT비율.Text = "";
            TextBox_Wrong작업자LAT비율.Text = "";
            TextBox_Wrong작업자질문초점비율.Text = "";
            TextBox_Wrong작업자질문유형비율.Text = "";

            TextBox_Wrong작업관리자SAT비율.Text = "";
            TextBox_Wrong작업관리자LAT비율.Text = "";
            TextBox_Wrong작업관리자질문유형비율.Text = "";
            TextBox_Wrong작업관리자질문초점비율.Text = "";

            TextBox_Wrong작업관리자2SAT비율.Text = "";
            TextBox_Wrong작업관리자2LAT비율.Text = "";
            TextBox_Wrong작업관리자2질문유형비율.Text = "";
            TextBox_Wrong작업관리자2질문초점비율.Text = "";
            #endregion
        }

        public void setWrongAdminAverage(int start, int end, int index)
        {
            #region 작업자-관리자간 개별평균
            double sumCount = end - start + 1;

            TextBox_Wrong작업관리자질문유형비율.Text = Math.Round((wrongQtCount1[index] / sumCount * 100), 2).ToString() + "%";
            TextBox_Wrong작업관리자질문초점비율.Text = Math.Round((wrongQfCount1[index] / sumCount * 100), 2).ToString() + "%";
            TextBox_Wrong작업관리자LAT비율.Text = Math.Round((wrongLatCount1[index] / sumCount * 100), 2).ToString() + "%";
            TextBox_Wrong작업관리자SAT비율.Text = Math.Round((wrongSatCount1[index] / sumCount * 100), 2).ToString() + "%";

            TextBox_Wrong작업관리자2질문유형비율.Text = Math.Round((wrongQtCount2[index] / sumCount * 100), 2).ToString() + "%";
            TextBox_Wrong작업관리자2질문초점비율.Text = Math.Round((wrongQfCount2[index] / sumCount * 100), 2).ToString() + "%";
            TextBox_Wrong작업관리자2LAT비율.Text = Math.Round((wrongLatCount2[index] / sumCount * 100), 2).ToString() + "%";
            TextBox_Wrong작업관리자2SAT비율.Text = Math.Round((wrongSatCount2[index] / sumCount * 100), 2).ToString() + "%";
            #endregion
        }

        public void setWrongAverage(int start, int end)
        {
            #region 작업자간 개별평균
            int qtCount = 0; int qfCount = 0; int latCount = 0; int satCount = 0;
            double sumCount = end - start + 1;

            for (int i = start - 1; i < end; i++)
            {
                if (!Convert.ToBoolean(CrossCheck_List1[i]))
                {
                    qtCount++;
                }
                if (!Convert.ToBoolean(CrossCheck_List2[i]))
                {
                    qfCount++;
                }
                if (!Convert.ToBoolean(CrossCheck_List3[i]))
                {
                    latCount++;
                }
                if (!Convert.ToBoolean(CrossCheck_List4[i]))
                {
                    satCount++;
                }
            }

            TextBox_Wrong작업자질문유형비율.Text = Math.Round((qtCount / sumCount * 100), 2).ToString() + "%";
            TextBox_Wrong작업자질문초점비율.Text = Math.Round((qfCount / sumCount * 100), 2).ToString() + "%";
            TextBox_Wrong작업자LAT비율.Text = Math.Round((latCount / sumCount * 100), 2).ToString() + "%";
            TextBox_Wrong작업자SAT비율.Text = Math.Round((satCount / sumCount * 100), 2).ToString() + "%";
            #endregion
        }

        public void setStandardDeviationWorkerWrong()
        {
            #region 작업자간 표준편차
            int start = 1;
            int end = 0;

            a.Clear(); b.Clear(); c.Clear(); d.Clear();

            for (int j = 0; j < FilesCrossQuestionCount.Count; j++)
            {
                end = start + FilesCrossQuestionCount[j] - 1;
                int qtCount = 0; int qfCount = 0; int latCount = 0; int satCount = 0;
                double sumCount = end - start + 1;

                for (int i = start - 1; i < end; i++)
                {
                    if (!Convert.ToBoolean(CrossCheck_List1[i]))
                    {
                        qtCount++;
                    }
                    if (!Convert.ToBoolean(CrossCheck_List2[i]))
                    {
                        qfCount++;
                    }
                    if (!Convert.ToBoolean(CrossCheck_List3[i]))
                    {
                        latCount++;
                    }
                    if (!Convert.ToBoolean(CrossCheck_List4[i]))
                    {
                        satCount++;
                    }
                }

                a.Add(qtCount / sumCount * 100);
                b.Add(qfCount / sumCount * 100);
                c.Add(latCount / sumCount * 100);
                d.Add(satCount / sumCount * 100);

                start = end + 1;
            }

            string set1 = TextBox_Wrong작업자질문유형전체평균.Text.Replace("%", "");
            double set1_1 = Convert.ToDouble(set1);
            TextBox_Wrong작업자질문유형표준편차.Text = calcurateEtriApiStandardDeviation(a, set1_1).ToString("N2") + "%";

            set1 = TextBox_Wrong작업자질문초점전체평균.Text.Replace("%", "");
            set1_1 = Convert.ToDouble(set1);
            TextBox_Wrong작업자질문초점표준편차.Text = calcurateEtriApiStandardDeviation(b, set1_1).ToString("N2") + "%";

            set1 = TextBox_Wrong작업자LAT전체평균.Text.Replace("%", "");
            set1_1 = Convert.ToDouble(set1);
            TextBox_Wrong작업자LAT표준편차.Text = calcurateEtriApiStandardDeviation(c, set1_1).ToString("N2") + "%";

            set1 = TextBox_Wrong작업자SAT전체평균.Text.Replace("%", "");
            set1_1 = Convert.ToDouble(set1);
            TextBox_Wrong작업자SAT표준편차.Text = calcurateEtriApiStandardDeviation(d, set1_1).ToString("N2") + "%";
            #endregion
        }


        public void setStandardDeviationAdminWrong()
        {
            #region 작업자-관리자간 표준편차
            a.Clear(); b.Clear(); c.Clear(); d.Clear();

            for (int j = 0; j < FilesCrossQuestionCount.Count; j++)
            {
                a.Add((wrongQtCount1[j] + wrongQtCount2[j]) / Convert.ToDouble(2 * FilesCrossQuestionCount[j]) * 100);
                b.Add((wrongQfCount1[j] + wrongQfCount2[j]) / Convert.ToDouble(2 * FilesCrossQuestionCount[j]) * 100);
                c.Add((wrongLatCount1[j] + wrongLatCount2[j]) / Convert.ToDouble(2 * FilesCrossQuestionCount[j]) * 100);
                d.Add((wrongSatCount1[j] + wrongSatCount2[j]) / Convert.ToDouble(2 * FilesCrossQuestionCount[j]) * 100);
            }

            string set1 = TextBox_Wrong작업관리자질문유형전체평균.Text.Replace("%", "");
            double set1_1 = Convert.ToDouble(set1);
            TextBox_Wrong작업관리자질문유형표준편차.Text = calcurateEtriApiStandardDeviation(a, set1_1).ToString("N2") + "%";

            set1 = TextBox_Wrong작업관리자질문초점전체평균.Text.Replace("%", "");
            set1_1 = Convert.ToDouble(set1);
            TextBox_Wrong작업관리자질문초점표준편차.Text = calcurateEtriApiStandardDeviation(b, set1_1).ToString("N2") + "%";

            set1 = TextBox_Wrong작업관리자LAT전체평균.Text.Replace("%", "");
            set1_1 = Convert.ToDouble(set1);
            TextBox_Wrong작업관리자LAT표준편차.Text = calcurateEtriApiStandardDeviation(c, set1_1).ToString("N2") + "%";

            set1 = TextBox_Wrong작업관리자SAT전체평균.Text.Replace("%", "");
            set1_1 = Convert.ToDouble(set1);
            TextBox_Wrong작업관리자SAT표준편차.Text = calcurateEtriApiStandardDeviation(d, set1_1).ToString("N2") + "%";
            #endregion
        }

        public void checkAdminWrongResult(Boolean check)
        {
            #region 작업자-관리자간 전체평균
            try
            {
                int qtCount = 0;
                int qfCount = 0;
                int latCount = 0;
                int satCount = 0;

                double sumCount = 0;
                ArrayList arr1, arr2, arr3, arr4;

                if (check)
                {
                    arr1 = CheckWorker1_QuestionType;
                    arr2 = CheckWorker1_QuestionFocus;
                    arr3 = CheckWorker1_QuestionLat;
                    arr4 = CheckWorker1_QuestionSat;
                }
                else
                {
                    arr1 = CheckWorker2_QuestionType;
                    arr2 = CheckWorker2_QuestionFocus;
                    arr3 = CheckWorker2_QuestionLat;
                    arr4 = CheckWorker2_QuestionSat;
                }

                for (int i = 0; i < FilesCrossQuestionCount.Count; i++)
                {
                    sumCount = sumCount + FilesCrossQuestionCount[i];
                }

                sumCount = sumCount * 2;//작업자1, 작업자2 두명

                //start end 잡기
                int start = 1;
                int end = 0;

                for (int j = 0; j < FilesCrossQuestionCount.Count; j++)
                {
                    end = start + FilesCrossQuestionCount[j] - 1;

                    for (int i = start - 1; i < end; i++)//0부터 1500까지만 반복
                    {
                        if (arr1[i].ToString() != CheckAdmin_QuestionType[i].ToString())
                        {
                            qtCount++;
                        }
                        if (arr2[i].ToString() != CheckAdmin_QuestionFocus[i].ToString())
                        {
                            qfCount++;
                        }
                        if (arr3[i].ToString() != CheckAdmin_QuestionLat[i].ToString())
                        {
                            latCount++;
                        }
                        if (arr4[i].ToString() != CheckAdmin_QuestionSat[i].ToString())
                        {
                            satCount++;
                        }
                    }

                    start = end + 1;
                    sumQtCount = sumQtCount + qtCount;
                    if (check)
                    {
                        wrongQtCount1.Add(qtCount);
                    }
                    else
                    {
                        wrongQtCount2.Add(qtCount);
                    }
                    qtCount = 0;

                    sumQfCount = sumQfCount + qfCount;
                    if (check)
                        wrongQfCount1.Add(qfCount);
                    else
                        wrongQfCount2.Add(qfCount);
                    qfCount = 0;

                    sumLatCount = sumLatCount + latCount;
                    if (check)
                        wrongLatCount1.Add(latCount);
                    else
                        wrongLatCount2.Add(latCount);
                    latCount = 0;

                    sumSatCount = sumSatCount + satCount;
                    if (check)
                        wrongSatCount1.Add(satCount);
                    else
                        wrongSatCount2.Add(satCount);
                    satCount = 0;

                }
                TextBox_Wrong작업관리자질문유형전체평균.Text = (sumQtCount / sumCount * 100).ToString("N2") + "%";
                TextBox_Wrong작업관리자질문초점전체평균.Text = (sumQfCount / sumCount * 100).ToString("N2") + "%";
                TextBox_Wrong작업관리자LAT전체평균.Text = (sumLatCount / sumCount * 100).ToString("N2") + "%";
                TextBox_Wrong작업관리자SAT전체평균.Text = (sumSatCount / sumCount * 100).ToString("N2") + "%";
            }
            catch
            {
                MessageBox.Show("오답비교가 불안정합니다.");
            }
            #endregion
        }

        public void checkWrongResult()
        {
            #region 작업자간 전체평균
            try
            {
                int qtCount = 0;
                int qfCount = 0;
                int latCount = 0;
                int satCount = 0;

                double sumCount = 0;

                for (int i = 0; i < FilesCrossQuestionCount.Count; i++)
                {
                    sumCount = sumCount + FilesCrossQuestionCount[i];
                }


                for (int i = 0; i < sumCount; i++)
                {
                    if (CheckWorker1_QuestionType[i].ToString() != CheckWorker2_QuestionType[i].ToString())
                    {
                        qtCount++;
                        CrossCheck_List1.Add(false);
                    }
                    else
                    {
                        CrossCheck_List1.Add(true);
                    }
                    ///////////////////////
                    if (CheckWorker1_QuestionFocus[i].ToString() != CheckWorker2_QuestionFocus[i].ToString())
                    {
                        qfCount++;
                        CrossCheck_List2.Add(false);
                    }
                    else
                    {
                        CrossCheck_List2.Add(true);
                    }
                    ///////////////////////
                    if (CheckWorker1_QuestionLat[i].ToString() != CheckWorker2_QuestionLat[i].ToString())
                    {
                        latCount++;
                        CrossCheck_List3.Add(false);
                    }
                    else
                    {
                        CrossCheck_List3.Add(true);
                    }
                    ////////////////////////
                    if (CheckWorker1_QuestionSat[i].ToString() != CheckWorker2_QuestionSat[i].ToString())
                    {
                        satCount++;
                        CrossCheck_List4.Add(false);
                    }
                    else
                    {
                        CrossCheck_List4.Add(true);
                    }
                }
                TextBox_Wrong작업자질문유형전체평균.Text = (qtCount / sumCount * 100).ToString("N2") + "%";
                TextBox_Wrong작업자질문초점전체평균.Text = (qfCount / sumCount * 100).ToString("N2") + "%";
                TextBox_Wrong작업자LAT전체평균.Text = (latCount / sumCount * 100).ToString("N2") + "%";
                TextBox_Wrong작업자SAT전체평균.Text = (satCount / sumCount * 100).ToString("N2") + "%";
            }
            catch
            {
                MessageBox.Show("오답비교가 불안정합니다.");
            }
            #endregion
        }

        private void Work_Btn_Click(object sender, EventArgs e)
        {
            #region Work파일 불러오기
            List<string> filePathList = new List<string>();

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt"; ;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;//여러 파일 선택
            openFileDialog.Title = "WorkJson파일 선택";


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                resetOriginal_TextBox();

                filePathList.AddRange(openFileDialog.FileNames);
                convertFiles(filePathList);
            }
            #endregion
        }

        private void convertFiles(IList<string> filePaths)
        {
            #region Work파일 리스트
            initFile1();
            workFile_comboBox.Items.Clear();
            workFile_comboBox.Items.Add("전체");//전체파일

            string fileName = null;

            foreach (var item in filePaths)
            {
                try
                {
                    int index = item.LastIndexOf(@"\");
                    fileName = item.Substring(index + 1, item.Length - index - 1);
                    //fileName에서 빼올것: start & end
                    int start = getStartNum(fileName);
                    int end = getEndNum(fileName);

                    workFile_comboBox.Items.Add(fileName);
                    convert(item, start, end);
                }
                catch
                {
                    MessageBox.Show(fileName);
                }
            }

            workFile_comboBox.Text = "전체";
            TextBox_총개수.Text = Check1_QuestionType.Count.ToString();
            #endregion
        }

        private int getStartNum(string fileName)
        {
            #region 파일 시작번호 가져오기
            int index = fileName.IndexOf('(');
            fileName = fileName.Substring(index, fileName.Length - index);

            index = fileName.IndexOf('(');
            int index2 = fileName.IndexOf('-');

            string start = fileName.Substring(index + 1, index2 - index - 1);
            int startNum = Convert.ToInt32(start);

            return startNum;
            #endregion
        }

        private int getEndNum(string fileName)
        {
            #region 파일 끝번호 가져오기
            int index = fileName.IndexOf('(');
            fileName = fileName.Substring(index, fileName.Length - index);

            index = fileName.IndexOf(')');
            int index2 = fileName.IndexOf('-');

            string end = fileName.Substring(index2 + 1, index - index2 - 1);
            int endNum = Convert.ToInt32(end);

            return endNum;
            #endregion
        }

        private void convert(string filepath, int fileStart, int fileEnd)
        {
            #region Work파일 파싱
            FileStream fs_read = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader stream_read = new StreamReader(fs_read, System.Text.Encoding.UTF8);

            String mod = stream_read.ReadToEnd();
            FilesTextList.Add(mod);

            stream_read.Close();
            fs_read.Close();

            try
            {
                originalReadParser(mod, fileStart, fileEnd);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                MessageBox.Show(filepath);

            }
            #endregion
        }

        private void moveSatStatusForm_Click(object sender, EventArgs e)
        {
            #region Sat통계로 이동
            if (startNum == 0 || endNum == 0)
            {
                MessageBox.Show("통계 범위를 선택해주세요");
            }
            else
            {
                SATStatsForm sForm = new SATStatsForm();
                dto = sForm.Getstatus();
                sForm.ShowDialog();
            }
            #endregion
        }

        private void CompareSave_Btn_Click(object sender, EventArgs e)
        {
            writeToExcel();
        }

        private void writeToExcel()
        {

        }

        private void WorkSave_Btn_Click(object sender, EventArgs e)
        {
            #region 통계 결과 저장
            StatusETRUDto etriDto = new StatusETRUDto();
            StatusSeosulDto seosulDto = new StatusSeosulDto();
            StatusQTDto qtDto = new StatusQTDto();
            StatusSATDto satDto = new StatusSATDto();

            etriDto.totalCnt = TextBox_총개수.Text;
            etriDto.timeAve = TextBox_전체평균시간.Text;
            etriDto.timeDev = TextBox_시간표준편차.Text;
            etriDto.timeCnt = TextBox_시간.Text;
            etriDto.etriQTAve = TextBox_유형전체평균.Text;
            etriDto.etriQTDev = TextBox_유형표준편차.Text;
            etriDto.etriQTCnt = TextBox_Etri질문유형.Text;
            etriDto.etriQFAve = TextBox_초점전체평균.Text;
            etriDto.etriQFDev = TextBox_초점표준편차.Text;
            etriDto.etriQFCnt = TextBox_Etri질문초점.Text;
            etriDto.etriLATAve = TextBox_LAT전체평균.Text;
            etriDto.etriLATDev = TextBox_LAT표준편차.Text;
            etriDto.etriLATCnt = TextBox_EtriLAT.Text;
            etriDto.etriSATAve = TextBox_SAT전체평균.Text;
            etriDto.etriSATDev = TextBox_SAT표준편차.Text;
            etriDto.etriSATCnt = TextBox_EtriSAT.Text;
            
            qtDto.dandabRto = TextBox_단답형비율.Text;
            qtDto.dandabCnt = TextBox_단답형.Text;
            qtDto.nayulRto = TextBox_나열형비율.Text;
            qtDto.nayulCnt = TextBox_나열형.Text;
            qtDto.seosulRto = TextBox_서술형비율.Text;
            qtDto.seosulCnt = TextBox_서술형.Text;

            seosulDto.defineRto = TextBox_정의비율.Text;
            seosulDto.defineCnt = TextBox_정의.Text;
            seosulDto.reasonRto = TextBox_이유비율.Text;
            seosulDto.reasonCnt = TextBox_이유.Text;
            seosulDto.wayRto = TextBox_방법비율.Text;
            seosulDto.wayCnt = TextBox_방법.Text;
            seosulDto.purposeRto = TextBox_목적비율.Text;
            seosulDto.purposeCnt = TextBox_목적.Text;
            seosulDto.conditionRto = TextBox_조건비율.Text;
            seosulDto.conditionCnt = TextBox_조건.Text;
            seosulDto.etcRto = TextBox_기타비율.Text;
            seosulDto.etcCnt = TextBox_기타.Text;
            seosulDto.meanRto = TextBox_의미비율.Text;
            seosulDto.meanCnt = TextBox_의미.Text;

            satDto = dto;


            if (workFile_comboBox.Items.Count != 0)
            {
                StatusSaveToExcel saveToExcel = new StatusSaveToExcel(etriDto, qtDto, satDto, seosulDto);
                MessageBox.Show(saveToExcel.saveFile());
            }
            else
            {
                MessageBox.Show("선택된 파일이 없습니다.");
            }
            #endregion
        }



    }
}
