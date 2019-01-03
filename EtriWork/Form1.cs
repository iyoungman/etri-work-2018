using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using JSON_ExcelDirectionalConverter;

namespace EtriWork
{
    public partial class Form1 : Form
    {
        #region WorkPanel변수

        ArrayList Work_Etri_list;//전체 텍스트를 담고있는 list
        ArrayList Question_LIst;//전체 질문 리스트
        ArrayList Answer_List;//전체 답변 리스트
        ArrayList QuestionType_List;//전체 질문 유형 리스트
        ArrayList QuestionFocus_List;//전체 질문 초점 리스트
        ArrayList QuestionLat_List;//전체 LAT 리스트
        ArrayList QuestionSat_List;//전체 SAT 리스트
        ArrayList QuestionTagged_List;//전체 Tagged 리스트
        ArrayList Context_List;//전체 Context 리스트

        List<bool> ConfuseQt_List;
        List<bool> ConfuseQf_List;
        List<bool> ConfuseLat_List;
        List<bool> ConfuseSat_List;
        List<bool> EtriQtCheck_List;
        List<bool> EtriQfCheck_List;
        List<bool> EtriLatCheck_List;
        List<bool> EtriSatCheck_List;
        List<bool> CheckIndividual_List;
        List<double> Time_List;

        ArrayList EtriQuestion_List;//Etri질문 리스트
        ArrayList EtriQuestionType_List;//Etri 질문 유형 리스트
        ArrayList EtriQuestionFocus_List;//Etri 질문 초점 리스트
        ArrayList EtriQuestionLat_List;//Etri LAT 리스트
        ArrayList EtriQuestionSat_List;//Etri SAT 리스트

        String path;//파일 경로
        String workText;//전체 텍스트
        int questionCount;//총 질문 개수
        
        EtriOpenApi obj_EtriOpenApi;//EtriOpenApi객체

        Dictionary<string, string> Dic_All;
        Dictionary<string, string> Dic_Person;
        Dictionary<string, string> Dic_Location;
        Dictionary<string, string> Dic_Organization;
        Dictionary<string, string> Dic_Artifacts;
        Dictionary<string, string> Dic_Date;
        Dictionary<string, string> Dic_Time;
        Dictionary<string, string> Dic_Civilization;
        Dictionary<string, string> Dic_Animal;
        Dictionary<string, string> Dic_Plant;
        Dictionary<string, string> Dic_Quantity;
        Dictionary<string, string> Dic_StudyField;
        Dictionary<string, string> Dic_Theory;
        Dictionary<string, string> Dic_Event;
        Dictionary<string, string> Dic_Material;
        Dictionary<string, string> Dic_Term;
        Dictionary<string, string> Dic_Etc;
        Dictionary<string, string> Dic_Help;

        int currentReadQuestion;//json에 읽을-현재까지 작업한 목록
        int currentWriteQuestion;//json에 저장-현재까지 작업한 목록

        //Text Highlihgt
        int qtStartIndex;
        int latStartIndex;

        //중복질문 제거를 위한 변수
        //List<bool> checkOverlap_List;
        //List<bool> checkContextOverlap_List;
        int overlabCount = 0;
        List<int> tmp2;
        #endregion


        #region CrossPanel변수
        
        string crossText;

        ArrayList Check1_Question;
        ArrayList Check2_Question;

        ArrayList Check1_Question_Answer;
        ArrayList Check2_Question_Answer;

        ArrayList Check1_Question_Tagged;
        ArrayList Check2_Question_Tagged;
        ArrayList CheckEnd_Question_Tagged;

        ArrayList Check1_QuestionType;
        ArrayList Check2_QuestionType;
        ArrayList CheckEnd_QuestionType;
        List<bool> Check1_ConfuseQt;
        List<bool> Check2_ConfuseQt;
        List<bool> CheckEnd_ConfuseQt;

        ArrayList Check1_QuestionFocus;
        ArrayList Check2_QuestionFocus;
        ArrayList CheckEnd_QuestionFocus;
        List<bool> Check1_ConfuseQf;
        List<bool> Check2_ConfuseQf;
        List<bool> CheckEnd_ConfuseQf;

        ArrayList Check1_QuestionLat;
        ArrayList Check2_QuestionLat;
        ArrayList CheckEnd_QuestionLat;
        List<bool> Check1_ConfuseLat;
        List<bool> Check2_ConfuseLat;
        List<bool> CheckEnd_ConfuseLat;

        ArrayList Check1_QuestionSat;
        ArrayList Check2_QuestionSat;
        ArrayList CheckEnd_QuestionSat;
        List<bool> Check1_ConfuseSat;
        List<bool> Check2_ConfuseSat;
        List<bool> CheckEnd_ConfuseSat;


        String cross1_path;
        String cross2_path;
        String crossFinal_path;
        int cross_index = 0;

        String firstFileName;
        String secondFileName;
       
        #endregion


        public Form1()
        {
            #region Form1초기화
            InitializeComponent();
            initDictionary();
            obj_EtriOpenApi = new EtriOpenApi();
            work_Panel.Enabled = false;
            initList();
            descriptive_listBox.Visible = false;
            initFileInput();
            작업모드ToolStripMenuItem.Checked = true;
            work_Panel.Visible = true;
            cross_Panel.Visible = false;
            etriQt_textBox.Enabled = false;
            etriQf_textBox.Enabled = false;
            etriLat_textBox.Enabled = false;
            etriSat_textBox.Enabled = false;
            #endregion
        }

        private void initFileInput()
        {
            #region SAT도움말 파일 읽기

            Dic_Help = new Dictionary<string, string>();

            /*****************************************************************/

            string path = @".\help\PERSON\PS_NAME.txt";
            string PS_NAME = File.ReadAllText(path, Encoding.Default);

            Dic_Help.Add("사람,신화,소설/게임,가수 등의 이름", PS_NAME);

            /*****************************************************************/
            path = @".\help\LOCATION\LC_OTHERS.txt";
            string LC_OTHERS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\LOCATION\LC_SPACE.txt";
            string LC_SPACE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\LOCATION\LC_TOUR.txt";
            string LC_TOUR = File.ReadAllText(path, Encoding.Default);

            path = @".\help\LOCATION\LCG_BAY.txt";
            string LCG_BAY = File.ReadAllText(path, Encoding.Default);

            path = @".\help\LOCATION\LCG_CONTINENT.txt";
            string LCG_CONTINENT = File.ReadAllText(path, Encoding.Default);

            path = @".\help\LOCATION\LCG_ISLAND.txt";
            string LCG_ISLAND = File.ReadAllText(path, Encoding.Default);

            path = @".\help\LOCATION\LCG_MOUNTAIN.txt";
            string LCG_MOUNTAIN = File.ReadAllText(path, Encoding.Default);

            path = @".\help\LOCATION\LCG_OCEAN.txt";
            string LCG_OCEAN = File.ReadAllText(path, Encoding.Default);

            path = @".\help\LOCATION\LCG_RIVER.txt";
            string LCG_RIVER = File.ReadAllText(path, Encoding.Default);

            path = @".\help\LOCATION\LCP_CAPITALCITY.txt";
            string LCP_CAPITALCITY = File.ReadAllText(path, Encoding.Default);

            path = @".\help\LOCATION\LCP_CITY.txt";
            string LCP_CITY = File.ReadAllText(path, Encoding.Default);

            path = @".\help\LOCATION\LCP_COUNTRY.txt";
            string LCP_COUNTRY = File.ReadAllText(path, Encoding.Default);

            path = @".\help\LOCATION\LCP_COUNTY.txt";
            string LCP_COUNTY = File.ReadAllText(path, Encoding.Default);

            path = @".\help\LOCATION\LCP_PROVINCE.txt";
            string LCP_PROVINCE = File.ReadAllText(path, Encoding.Default);

            Dic_Help.Add("LC_OTHERS", LC_OTHERS);
            Dic_Help.Add("국가명", LCP_COUNTRY);
            Dic_Help.Add("도, 주 지역명", LCP_PROVINCE);
            Dic_Help.Add("군, 면, 읍, 리, 동 등과 같은 세부 행정구역명, ~마을", LCP_COUNTY);
            Dic_Help.Add("도시명", LCP_CITY);
            Dic_Help.Add("수도명", LCP_CAPITALCITY);
            Dic_Help.Add("강, 호수, 연못", LCG_RIVER);
            Dic_Help.Add("해양/바다 명칭", LCG_OCEAN);
            Dic_Help.Add("반도/만의 명칭", LCG_BAY);
            Dic_Help.Add("산맥/산의 명칭", LCG_MOUNTAIN);
            Dic_Help.Add("섬/제도 명칭", LCG_ISLAND);
            Dic_Help.Add("대륙 명칭(아시아/아프리카)", LCG_CONTINENT);
            Dic_Help.Add("관광 명소", LC_TOUR);
            Dic_Help.Add("천체 명칭, 항성, 행성, 위성, 유성, 별자리 명칭", LC_SPACE);

            /*****************************************************************/

            path = @".\help\ORGANIZATION\OG_OTHERS.txt";
            string OG_OTHERS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ORGANIZATION\OGG_ART.txt";
            string OGG_ART = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ORGANIZATION\OGG_ECONOMY.txt";
            string OGG_ECONOMY = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ORGANIZATION\OGG_EDUCATION.txt";
            string OGG_EDUCATION = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ORGANIZATION\OGG_FOOD.txt";
            string OGG_FOOD = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ORGANIZATION\OGG_HOTEL.txt";
            string OGG_HOTEL = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ORGANIZATION\OGG_LAW.txt";
            string OGG_LAW = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ORGANIZATION\OGG_LIBRARY.txt";
            string OGG_LIBRARY = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ORGANIZATION\OGG_MEDIA.txt";
            string OGG_MEDIA = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ORGANIZATION\OGG_MEDICINE.txt";
            string OGG_MEDICINE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ORGANIZATION\OGG_MILITARY.txt";
            string OGG_MILITARY = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ORGANIZATION\OGG_POLITICS.txt";
            string OGG_POLITICS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ORGANIZATION\OGG_RELIGION.txt";
            string OGG_RELIGION = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ORGANIZATION\OGG_SCIENCE.txt";
            string OGG_SCIENCE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ORGANIZATION\OGG_SPORTS.txt";
            string OGG_SPORTS = File.ReadAllText(path, Encoding.Default);

            Dic_Help.Add("OG_OTHERS", OG_OTHERS);
            Dic_Help.Add("경제 관련 기관/단체/기업", OGG_ECONOMY);
            Dic_Help.Add("교육 기관/단체, 교육관련 기관", OGG_EDUCATION);
            Dic_Help.Add("국방기관", OGG_MILITARY);
            Dic_Help.Add("미디어 기관/단체, 방송관련 기관/기업", OGG_MEDIA);
            Dic_Help.Add("스포츠 관련 단체, 스포츠 팀", OGG_SPORTS);
            Dic_Help.Add("예술 기관/단체", OGG_ART);
            Dic_Help.Add("의학/의료 기관/단체", OGG_MEDICINE);
            Dic_Help.Add("종교 기관", OGG_RELIGION);
            Dic_Help.Add("과학 기관", OGG_SCIENCE);
            Dic_Help.Add("도서관 및 도서관 관련 기관/단체", OGG_LIBRARY);
            Dic_Help.Add("법률기관", OGG_LAW);
            Dic_Help.Add("정부/행정 기관, 공공기관, 정치기관", OGG_POLITICS);
            Dic_Help.Add("식당 이름", OGG_FOOD);
            Dic_Help.Add("호텔, 모텔, 민박, 펜션, 콘도, 리조트", OGG_HOTEL);
            /*****************************************************************/

            path = @".\help\ARTIFACTS\AF_BUILDING.txt";
            string AF_BUILDING = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ARTIFACTS\AF_CULTURAL_ASSET.txt";
            string AF_CULTURAL_ASSET = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ARTIFACTS\AF_MUSICAL_INSTRUMENT.txt";
            string AF_MUSICAL_INSTRUMENT = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ARTIFACTS\AF_ROAD.txt";
            string AF_ROAD = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ARTIFACTS\AF_TRANSPORT.txt";
            string AF_TRANSPORT = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ARTIFACTS\AF_WARES.txt";
            string AF_WARES = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ARTIFACTS\AF_WEAPON.txt";
            string AF_WEAPON = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ARTIFACTS\AF_WORKS.txt";
            string AF_WORKS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ARTIFACTS\AFW_ART_CRAFT.txt";
            string AFW_ART_CRAFT = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ARTIFACTS\AFW_DOCUMENT.txt";
            string AFW_DOCUMENT = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ARTIFACTS\AFW_MUSIC.txt";
            string AFW_MUSIC = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ARTIFACTS\AFW_PERFORMANCE.txt";
            string AFW_PERFORMANCE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ARTIFACTS\AFW_VIDEO.txt";
            string AFW_VIDEO = File.ReadAllText(path, Encoding.Default);

            Dic_Help.Add("문화재 명칭", AF_CULTURAL_ASSET);
            Dic_Help.Add("건축물/토목 건설물, 운동장이름, 아파트, 다리, 등대", AF_BUILDING);
            Dic_Help.Add("악기 명칭", AF_MUSICAL_INSTRUMENT);
            Dic_Help.Add("도로/철로 명칭", AF_ROAD);
            Dic_Help.Add("무기 명칭", AF_WEAPON);
            Dic_Help.Add("교통수단/자동차/선박 모델 및 유형, 운송수단, 놀이기구", AF_TRANSPORT);
            Dic_Help.Add("작품명", AF_WORKS);
            Dic_Help.Add("도서/서적 작품명", AFW_DOCUMENT);
            Dic_Help.Add("춤/무용/연극/가극 작품명 및 춤 종류", AFW_PERFORMANCE);
            Dic_Help.Add("영화 작품명/TV 프로그램 이름", AFW_VIDEO);
            Dic_Help.Add("미술 작품명", AFW_ART_CRAFT);
            Dic_Help.Add("음악 작품명", AFW_MUSIC);
            Dic_Help.Add("상품/제품 이름", AF_WARES);
            /*****************************************************************/

            path = @".\help\DATE\DT_DAY.txt";
            string DT_DAY = File.ReadAllText(path, Encoding.Default);

            path = @".\help\DATE\DT_DURATION.txt";
            string DT_DURATION = File.ReadAllText(path, Encoding.Default);

            path = @".\help\DATE\DT_DYNASTY.txt";
            string DT_DYNASTY = File.ReadAllText(path, Encoding.Default);

            path = @".\help\DATE\DT_GEOAGE.txt";
            string DT_GEOAGE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\DATE\DT_MONTH.txt";
            string DT_MONTH = File.ReadAllText(path, Encoding.Default);

            path = @".\help\DATE\DT_OTHERS.txt";
            string DT_OTHERS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\DATE\DT_SEASON.txt";
            string DT_SEASON = File.ReadAllText(path, Encoding.Default);

            path = @".\help\DATE\DT_YEAR.txt";
            string DT_YEAR = File.ReadAllText(path, Encoding.Default);

            Dic_Help.Add("DT_OTHERS", DT_OTHERS);
            Dic_Help.Add("기간(날짜)", DT_DURATION);
            Dic_Help.Add("날짜, 절기", DT_DAY);
            Dic_Help.Add("달", DT_MONTH);
            Dic_Help.Add("년", DT_YEAR);
            Dic_Help.Add("계절", DT_SEASON);
            Dic_Help.Add("지질 시대", DT_GEOAGE);
            Dic_Help.Add("왕조 시대", DT_DYNASTY);
            /*****************************************************************/

            path = @".\help\CIVILIZATION\CV_BUILDING_TYPE.txt";
            string CV_BUILDING_TYPE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_CLOTHING.txt";
            string CV_CLOTHING = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_CURRENCY.txt";
            string CV_CURRENCY = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_DRINK.txt";
            string CV_DRINK = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_FOOD.txt";
            string CV_FOOD = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_FOOD_STYLE.txt";
            string CV_FOOD_STYLE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_FUNDS.txt";
            string CV_FUNDS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_LANGUAGE.txt";
            string CV_LANGUAGE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_LAW.txt";
            string CV_LAW = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_NAME.txt";
            string CV_NAME = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_OCCUPATION.txt";
            string CV_OCCUPATION = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_POLICY.txt";
            string CV_POLICY = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_POSITION.txt";
            string CV_POSITION = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_PRIZE.txt";
            string CV_PRIZE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_RELATION.txt";
            string CV_RELATION = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_SPORTS.txt";
            string CV_SPORTS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_SPORTS_INST.txt";
            string CV_SPORTS_INST = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_TAX.txt";
            string CV_TAX = File.ReadAllText(path, Encoding.Default);

            path = @".\help\CIVILIZATION\CV_TRIBE.txt";
            string CV_TRIBE = File.ReadAllText(path, Encoding.Default);

            Dic_Help.Add("문명/문화 명칭", CV_NAME);
            Dic_Help.Add("민족/종족 명칭 또한 국가를 구성하는 국민을 지칭", CV_TRIBE);
            Dic_Help.Add("스포츠/레포츠/레져 명칭", CV_SPORTS);
            Dic_Help.Add("스포츠 용품/도구", CV_SPORTS_INST);
            Dic_Help.Add("제도/정책 명칭", CV_POLICY);
            Dic_Help.Add("조세 명칭", CV_TAX);
            Dic_Help.Add("연금, 기금, 자금 명칭", CV_FUNDS);
            Dic_Help.Add("언어 명칭", CV_LANGUAGE);
            Dic_Help.Add("건축양식 명칭", CV_BUILDING_TYPE);
            Dic_Help.Add("음식/곡물 명칭", CV_FOOD);
            Dic_Help.Add("음료수, 술 명칭", CV_DRINK);
            Dic_Help.Add("의복/섬유 명칭", CV_CLOTHING);
            Dic_Help.Add("직위 명칭", CV_POSITION);
            Dic_Help.Add("인간 관계 명칭", CV_RELATION);
            Dic_Help.Add("직업 명칭", CV_OCCUPATION);
            Dic_Help.Add("통화 명칭", CV_CURRENCY);
            Dic_Help.Add("상과 훈장", CV_PRIZE);
            Dic_Help.Add("법/법률 명칭", CV_LAW);
            Dic_Help.Add("음식 종류 (한식, 일식, 양식 등)", CV_FOOD_STYLE);

            /*****************************************************************/


            path = @".\help\ANIMAL\AM_AMPHIBIA.txt";
            string AM_AMPHIBIA = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ANIMAL\AM_BIRD.txt";
            string AM_BIRD = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ANIMAL\AM_FISH.txt";
            string AM_FISH = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ANIMAL\AM_INSECT.txt";
            string AM_INSECT = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ANIMAL\AM_MAMMALIA.txt";
            string AM_MAMMALIA = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ANIMAL\AM_OTHERS.txt";
            string AM_OTHERS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ANIMAL\AM_PART.txt";
            string AM_PART = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ANIMAL\AM_REPTILIA.txt";
            string AM_REPTILIA = File.ReadAllText(path, Encoding.Default);

            path = @".\help\ANIMAL\AM_TYPE.txt";
            string AM_TYPE = File.ReadAllText(path, Encoding.Default);

            Dic_Help.Add("AM_OTHERS", AM_OTHERS);
            Dic_Help.Add("곤충", AM_INSECT);
            Dic_Help.Add("새", AM_BIRD);
            Dic_Help.Add("어류", AM_FISH);
            Dic_Help.Add("포유류", AM_MAMMALIA);
            Dic_Help.Add("양서류", AM_AMPHIBIA);
            Dic_Help.Add("파충류", AM_REPTILIA);
            Dic_Help.Add("동물 분류 명칭", AM_TYPE);
            Dic_Help.Add("동물의 한 부분/부위", AM_PART);

            /*****************************************************************/
            path = @".\help\PLANT\PT_FLOWER.txt";
            string PT_FLOWER = File.ReadAllText(path, Encoding.Default);

            path = @".\help\PLANT\PT_FRUIT.txt";
            string PT_FRUIT = File.ReadAllText(path, Encoding.Default);

            path = @".\help\PLANT\PT_GRASS.txt";
            string PT_GRASS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\PLANT\PT_OTHERS.txt";
            string PT_OTHERS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\PLANT\PT_PART.txt";
            string PT_PART = File.ReadAllText(path, Encoding.Default);

            path = @".\help\PLANT\PT_TREE.txt";
            string PT_TREE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\PLANT\PT_TYPE.txt";
            string PT_TYPE = File.ReadAllText(path, Encoding.Default);

            Dic_Help.Add("PT_OTHERS", PT_OTHERS);
            Dic_Help.Add("과일 이름", PT_FRUIT);
            Dic_Help.Add("꽃", PT_FLOWER);
            Dic_Help.Add("나무", PT_TREE);
            Dic_Help.Add("풀", PT_GRASS);
            Dic_Help.Add("식물 유형 명칭", PT_TYPE);
            Dic_Help.Add("식물의 한 부분에 대한 명칭", PT_PART);

            /*****************************************************************/
            path = @".\help\TIME\TI_DURATION.txt";
            string TI_DURATION = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TIME\TI_HOUR.txt";
            string TI_HOUR = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TIME\TI_MINUTE.txt";
            string TI_MINUTE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TIME\TI_OTHERS.txt";
            string TI_OTHERS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TIME\TI_SECOND.txt";
            string TI_SECOND = File.ReadAllText(path, Encoding.Default);

            Dic_Help.Add("TI_OTHERS", TI_OTHERS);
            Dic_Help.Add("기간(시간)", TI_DURATION);
            Dic_Help.Add("시각", TI_HOUR);
            Dic_Help.Add("분", TI_MINUTE);
            Dic_Help.Add("초", TI_SECOND);

            /*****************************************************************/
            path = @".\help\QUANTITY\QT_AGE.txt";
            string QT_AGE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_ALBUM.txt";
            string QT_ALBUM = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_CHANNEL.txt";
            string QT_CHANNEL = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_COUNT.txt";
            string QT_COUNT = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_LENGTH.txt";
            string QT_LENGTH = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_MAN_COUNT.txt";
            string QT_MAN_COUNT = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_ORDER.txt";
            string QT_ORDER = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_OTHERS.txt";
            string QT_OTHERS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_PERCENTAGE.txt";
            string QT_PERCENTAGE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_PHONE.txt";
            string QT_PHONE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_PRICE.txt";
            string QT_PRICE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_SIZE.txt";
            string QT_SIZE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_SPEED.txt";
            string QT_SPEED = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_SPORTS.txt";
            string QT_SPORTS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_TEMPERATURE.txt";
            string QT_TEMPERATURE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_VOLUME.txt";
            string QT_VOLUME = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_WEIGHT.txt";
            string QT_WEIGHT = File.ReadAllText(path, Encoding.Default);

            path = @".\help\QUANTITY\QT_ZIPCODE.txt";
            string QT_ZIPCODE = File.ReadAllText(path, Encoding.Default);

            Dic_Help.Add("QT_OTHERS", QT_OTHERS);
            Dic_Help.Add("나이", QT_AGE);
            Dic_Help.Add("크기/넓이", QT_SIZE);
            Dic_Help.Add("길이/거리/높이", QT_LENGTH);
            Dic_Help.Add("개수, 빈도", QT_COUNT);
            Dic_Help.Add("인원수", QT_MAN_COUNT);
            Dic_Help.Add("무게", QT_WEIGHT);
            Dic_Help.Add("백분율,  비율, 농도", QT_PERCENTAGE);
            Dic_Help.Add("속도", QT_SPEED);
            Dic_Help.Add("온도", QT_TEMPERATURE);
            Dic_Help.Add("부피", QT_VOLUME);
            Dic_Help.Add("순서,  순차적 표현", QT_ORDER);
            Dic_Help.Add("금액", QT_PRICE);
            Dic_Help.Add("전화번호", QT_PHONE);
            Dic_Help.Add("스포츠 관련 수량 표현 (점수 등)", QT_SPORTS);
            Dic_Help.Add("TV 채널번호", QT_CHANNEL);
            Dic_Help.Add("앨범 관련 수량 표현 (1집, 2집 등)", QT_ALBUM);
            Dic_Help.Add("우편번호", QT_ZIPCODE);


            /*****************************************************************/

            path = @".\help\THEORY\TR_ART.txt";
            string TR_ART = File.ReadAllText(path, Encoding.Default);

            path = @".\help\THEORY\TR_MEDICINE.txt";
            string TR_MEDICINE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\THEORY\TR_OTHERS.txt";
            string TR_OTHERS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\THEORY\TR_PHILOSOPHY.txt";
            string TR_PHILOSOPHY = File.ReadAllText(path, Encoding.Default);

            path = @".\help\THEORY\TR_SCIENCE.txt";
            string TR_SCIENCE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\THEORY\TR_SOCIAL_SCIENCE.txt";
            string TR_SOCIAL_SCIENCE = File.ReadAllText(path, Encoding.Default);


            Dic_Help.Add("TR_OTHERS", TR_OTHERS);
            Dic_Help.Add("과학 관련 기술/이론/법칙/방식/양식", TR_SCIENCE);
            Dic_Help.Add("사회과학 이론/법칙/방법", TR_SOCIAL_SCIENCE);
            Dic_Help.Add("예술관련 이론/법칙/방식", TR_ART);
            Dic_Help.Add("철학 이론/사상", TR_PHILOSOPHY);
            Dic_Help.Add("의학 요법/처방, 의학 진단법", TR_MEDICINE);


            /*****************************************************************/
            path = @".\help\STUDY_FIELD\FD_ART.txt";
            string FD_ART = File.ReadAllText(path, Encoding.Default);

            path = @".\help\STUDY_FIELD\FD_MEDICINE.txt";
            string FD_MEDICINE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\STUDY_FIELD\FD_OTHERS.txt";
            string FD_OTHERS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\STUDY_FIELD\FD_PHILOSOPHY.txt";
            string FD_PHILOSOPHY = File.ReadAllText(path, Encoding.Default);

            path = @".\help\STUDY_FIELD\FD_SCIENCE.txt";
            string FD_SCIENCE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\STUDY_FIELD\FD_SOCIAL_SCIENCE.txt";
            string FD_SOCIAL_SCIENCE = File.ReadAllText(path, Encoding.Default);


            Dic_Help.Add("FD_OTHERS", FD_OTHERS);
            Dic_Help.Add("과학관련 학문 분야 및 과학 학파", FD_SCIENCE);
            Dic_Help.Add("사회과학 관련 학문 분야 및 학파", FD_SOCIAL_SCIENCE);
            Dic_Help.Add("의학관련 학문 분야 및 학파", FD_MEDICINE);
            Dic_Help.Add("예술관련 학문분야 및 학파(유파)", FD_ART);
            Dic_Help.Add("철학관련 학문분야 및 학파(유파)", FD_PHILOSOPHY);

            /*****************************************************************/
            path = @".\help\EVENT\EV_ACTIVITY.txt";
            string EV_ACTIVITY = File.ReadAllText(path, Encoding.Default);

            path = @".\help\EVENT\EV_FESTIVAL.txt";
            string EV_FESTIVAL = File.ReadAllText(path, Encoding.Default);

            path = @".\help\EVENT\EV_OTHERS.txt";
            string EV_OTHERS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\EVENT\EV_SPORTS.txt";
            string EV_SPORTS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\EVENT\EV_WAR_REVOLUTION.txt";
            string EV_WAR_REVOLUTION = File.ReadAllText(path, Encoding.Default);

            Dic_Help.Add("EV_OTHERS", EV_OTHERS);
            Dic_Help.Add("사회운동  및 선언", EV_ACTIVITY);
            Dic_Help.Add("전쟁/혁명", EV_WAR_REVOLUTION);
            Dic_Help.Add("스포츠/레저  관련 행사", EV_SPORTS);
            Dic_Help.Add("축제 명칭", EV_FESTIVAL);

            /*****************************************************************/
            path = @".\help\MATERIAL\MT_CHEMICAL.txt";
            string MT_CHEMICAL = File.ReadAllText(path, Encoding.Default);

            path = @".\help\MATERIAL\MT_ELEMENT.txt";
            string MT_ELEMENT = File.ReadAllText(path, Encoding.Default);

            path = @".\help\MATERIAL\MT_METAL.txt";
            string MT_METAL = File.ReadAllText(path, Encoding.Default);

            path = @".\help\MATERIAL\MT_ROCK.txt";
            string MT_ROCK = File.ReadAllText(path, Encoding.Default);


            Dic_Help.Add("원소명", MT_ELEMENT);
            Dic_Help.Add("금속물", MT_METAL);
            Dic_Help.Add("암석", MT_ROCK);
            Dic_Help.Add("화학물질", MT_CHEMICAL);

            /*****************************************************************/

            path = @".\help\TERM\TM_CELL_TISSUE.txt";
            string TM_CELL_TISSUE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TERM\TM_CLIMATE.txt";
            string TM_CLIMATE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TERM\TM_COLOR.txt";
            string TM_COLOR = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TERM\TM_DIRECTION.txt";
            string TM_DIRECTION = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TERM\TM_SHAPE.txt";
            string TM_SHAPE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TERM\TM_SPORTS.txt";
            string TM_SPORTS = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TERM\TMI_EMAIL.txt";
            string TMI_EMAIL = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TERM\TMI_HW.txt";
            string TMI_HW = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TERM\TMI_MODEL.txt";
            string TMI_MODEL = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TERM\TMI_PROJECT.txt";
            string TMI_PROJECT = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TERM\TMI_SERVICE.txt";
            string TMI_SERVICE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TERM\TMI_SITE.txt";
            string TMI_SITE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TERM\TMI_SW.txt";
            string TMI_SW = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TERM\TMIG_GENRE.txt";
            string TMIG_GENRE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TERM\TMM_DISEASE.txt";
            string TMM_DISEASE = File.ReadAllText(path, Encoding.Default);

            path = @".\help\TERM\TMM_DRUG.txt";
            string TMM_DRUG = File.ReadAllText(path, Encoding.Default);

            Dic_Help.Add("색", TM_COLOR);
            Dic_Help.Add("방향", TM_DIRECTION);
            Dic_Help.Add("기후지역 명칭", TM_CLIMATE);
            Dic_Help.Add("모양/형태", TM_SHAPE);
            Dic_Help.Add("세포/조직, 세포 명칭과 생물의 조직 및 기관에 대한 명칭", TM_CELL_TISSUE);
            Dic_Help.Add("증상/증세/질병", TMM_DISEASE);
            Dic_Help.Add("약/약품명", TMM_DRUG);
            Dic_Help.Add("하드웨어 용어", TMI_HW);
            Dic_Help.Add("소프트웨어 용어", TMI_SW);
            Dic_Help.Add("url 주소", TMI_SITE);
            Dic_Help.Add("이메일 주소", TMI_EMAIL);
            Dic_Help.Add("각종 제품 등의 모델명 (LM1000-2CX 등)", TMI_MODEL);
            Dic_Help.Add("IT 서비스 용어 (와이브로 서비스, DMB 서비스)", TMI_SERVICE);
            Dic_Help.Add("프로젝트 명칭", TMI_PROJECT);
            Dic_Help.Add("게임장르", TMIG_GENRE);
            Dic_Help.Add("스포츠/레저 용어 (기술/규칙 명칭)", TM_SPORTS);

            /*****************************************************************/

            Dic_Help.Add("기타", "- ETRI 개체명 가이드라인에 포함되지 않은 유형");

            /*****************************************************************/

            #endregion
        }

        private void initDictionary()
        {
            #region SAT Dictionary

            //**************************************전체*****************************************//

            //All
            Dic_All = new Dictionary<string, string>();
            Dic_All.Add("사람,신화,소설/게임,가수 등의 이름", "PS_NAME");
            Dic_All.Add("LC_OTHERS", "LC_OTHERS");
            Dic_All.Add("국가명", "LCP_COUNTRY");
            Dic_All.Add("도, 주 지역명", "LCP_PROVINCE");
            Dic_All.Add("군, 면, 읍, 리, 동 등과 같은 세부 행정구역명, ~마을", "LCP_COUNTY");
            Dic_All.Add("도시명", "LCP_CITY");
            Dic_All.Add("수도명", "LCP_CAPITALCITY");
            Dic_All.Add("강, 호수, 연못", "LCG_RIVER");
            Dic_All.Add("해양/바다 명칭", "LCG_OCEAN");
            Dic_All.Add("반도/만의 명칭", "LCG_BAY");
            Dic_All.Add("산맥/산의 명칭", "LCG_MOUNTAIN");
            Dic_All.Add("섬/제도 명칭", "LCG_ISLAND");
            Dic_All.Add("대륙 명칭(아시아/아프리카)", "LCG_CONTINENT");
            Dic_All.Add("관광 명소", "LC_TOUR");
            Dic_All.Add("천체 명칭, 항성, 행성, 위성, 유성, 별자리 명칭", "LC_SPACE");
            Dic_All.Add("OG_OTHERS", "OG_OTHERS");
            Dic_All.Add("경제 관련 기관/단체/기업", "OGG_ECONOMY");
            Dic_All.Add("교육 기관/단체, 교육관련 기관", "OGG_EDUCATION");
            Dic_All.Add("국방기관", "OGG_MILITARY");
            Dic_All.Add("미디어 기관/단체, 방송관련 기관/기업", "OGG_MEDIA");
            Dic_All.Add("스포츠 관련 단체, 스포츠 팀", "OGG_SPORTS");
            Dic_All.Add("예술 기관/단체", "OGG_ART");
            Dic_All.Add("의학/의료 기관/단체", "OGG_MEDICINE");
            Dic_All.Add("종교 기관", "OGG_RELIGION");
            Dic_All.Add("과학 기관", "OGG_SCIENCE");
            Dic_All.Add("도서관 및 도서관 관련 기관/단체", "OGG_LIBRARY");
            Dic_All.Add("법률기관", "OGG_LAW");
            Dic_All.Add("정부/행정 기관, 공공기관, 정치기관", "OGG_POLITICS");
            Dic_All.Add("식당 이름", "OGG_FOOD");
            Dic_All.Add("호텔, 모텔, 민박, 펜션, 콘도, 리조트", "OGG_HOTEL");
            Dic_All.Add("문화재 명칭", "AF_CULTURAL_ASSET");
            Dic_All.Add("건축물/토목 건설물, 운동장이름, 아파트, 다리, 등대", "AF_BUILDING");
            Dic_All.Add("악기 명칭", "AF_MUSICAL_INSTRUMENT");
            Dic_All.Add("도로/철로 명칭", "AF_ROAD");
            Dic_All.Add("무기 명칭", "AF_WEAPON");
            Dic_All.Add("교통수단/자동차/선박 모델 및 유형, 운송수단, 놀이기구", "AF_TRANSPORT");
            Dic_All.Add("작품명", "AF_WORKS");
            Dic_All.Add("도서/서적 작품명", "AFW_DOCUMENT");
            Dic_All.Add("춤/무용/연극/가극 작품명 및 춤 종류", "AFW_PERFORMANCE");
            Dic_All.Add("영화 작품명/TV 프로그램 이름", "AFW_VIDEO");
            Dic_All.Add("미술 작품명", "AFW_ART_CRAFT");
            Dic_All.Add("음악 작품명", "AFW_MUSIC");
            Dic_All.Add("상품/제품 이름", "AF_WARES");
            Dic_All.Add("DT_OTHERS", "DT_OTHERS");
            Dic_All.Add("기간(날짜)", "DT_DURATION");
            Dic_All.Add("날짜, 절기", "DT_DAY");
            Dic_All.Add("달", "DT_MONTH");
            Dic_All.Add("년", "DT_YEAR");
            Dic_All.Add("계절", "DT_SEASON");
            Dic_All.Add("지질 시대", "DT_GEOAGE");
            Dic_All.Add("왕조 시대", "DT_DYNASTY");
            Dic_All.Add("TI_OTHERS", "TI_OTHERS");
            Dic_All.Add("기간(시간)", "TI_DURATION");
            Dic_All.Add("시각", "TI_HOUR");
            Dic_All.Add("분", "TI_MINUTE");
            Dic_All.Add("초", "TI_SECOND");
            Dic_All.Add("문명/문화 명칭", "CV_NAME");
            Dic_All.Add("민족/종족 명칭 또한 국가를 구성하는 국민을 지칭", "CV_TRIBE");
            Dic_All.Add("스포츠/레포츠/레져 명칭", "CV_SPORTS");
            Dic_All.Add("스포츠 용품/도구", "CV_SPORTS_INST");
            Dic_All.Add("제도/정책 명칭", "CV_POLICY");
            Dic_All.Add("조세 명칭", "CV_TAX");
            Dic_All.Add("연금, 기금, 자금 명칭", "CV_FUNDS");
            Dic_All.Add("언어 명칭", "CV_LANGUAGE");
            Dic_All.Add("건축양식 명칭", "CV_BUILDING_TYPE");
            Dic_All.Add("음식/곡물 명칭", "CV_FOOD");
            Dic_All.Add("음료수, 술 명칭", "CV_DRINK");
            Dic_All.Add("의복/섬유 명칭", "CV_CLOTHING");
            Dic_All.Add("직위 명칭", "CV_POSITION");
            Dic_All.Add("인간 관계 명칭", "CV_RELATION");
            Dic_All.Add("직업 명칭", "CV_OCCUPATION");
            Dic_All.Add("통화 명칭", "CV_CURRENCY");
            Dic_All.Add("상과 훈장", "CV_PRIZE");
            Dic_All.Add("법/법률 명칭", "CV_LAW");
            Dic_All.Add("음식 종류 (한식, 일식, 양식 등)", "CV_FOOD_STYLE");
            Dic_All.Add("AM_OTHERS", "AM_OTHERS");
            Dic_All.Add("곤충", "AM_INSECT");
            Dic_All.Add("새", "AM_BIRD");
            Dic_All.Add("어류", "AM_FISH");
            Dic_All.Add("포유류", "AM_MAMMALIA");
            Dic_All.Add("양서류", "AM_AMPHIBIA");
            Dic_All.Add("파충류", "AM_REPTILIA");
            Dic_All.Add("동물 분류 명칭", "AM_TYPE");
            Dic_All.Add("동물의 한 부분/부위", "AM_PART");
            Dic_All.Add("PT_OTHERS", "PT_OTHERS");
            Dic_All.Add("과일 이름", "PT_FRUIT");
            Dic_All.Add("꽃", "PT_FLOWER");
            Dic_All.Add("나무", "PT_TREE");
            Dic_All.Add("풀", "PT_GRASS");
            Dic_All.Add("식물 유형 명칭", "PT_TYPE");
            Dic_All.Add("식물의 한 부분에 대한 명칭", "PT_PART");
            Dic_All.Add("QT_OTHERS", "QT_OTHERS");
            Dic_All.Add("나이", "QT_AGE");
            Dic_All.Add("크기/넓이", "QT_SIZE");
            Dic_All.Add("길이/거리/높이", "QT_LENGTH");
            Dic_All.Add("개수, 빈도", "QT_COUNT");
            Dic_All.Add("인원수", "QT_MAN_COUNT");
            Dic_All.Add("무게", "QT_WEIGHT");
            Dic_All.Add("백분율,  비율, 농도", "QT_PERCENTAGE");
            Dic_All.Add("속도", "QT_SPEED");
            Dic_All.Add("온도", "QT_TEMPERATURE");
            Dic_All.Add("부피", "QT_VOLUME");
            Dic_All.Add("순서,  순차적 표현", "QT_ORDER");
            Dic_All.Add("금액", "QT_PRICE");
            Dic_All.Add("전화번호", "QT_PHONE");
            Dic_All.Add("스포츠 관련 수량 표현 (점수 등)", "QT_SPORTS");
            Dic_All.Add("TV 채널번호", "QT_CHANNEL");
            Dic_All.Add("앨범 관련 수량 표현 (1집, 2집 등)", "QT_ALBUM");
            Dic_All.Add("우편번호", "QT_ZIPCODE");
            Dic_All.Add("FD_OTHERS", "FD_OTHERS");
            Dic_All.Add("과학관련 학문 분야 및 과학 학파", "FD_SCIENCE");
            Dic_All.Add("사회과학 관련 학문 분야 및 학파", "FD_SOCIAL_SCIENCE");
            Dic_All.Add("의학관련 학문 분야 및 학파", "FD_MEDICINE");
            Dic_All.Add("예술관련 학문분야 및 학파(유파)", "FD_ART");
            Dic_All.Add("철학관련 학문분야 및 학파(유파)", "FD_PHILOSOPHY");
            Dic_All.Add("TR_OTHERS", "TR_OTHERS");
            Dic_All.Add("과학 관련 기술/이론/법칙/방식/양식", "TR_SCIENCE");
            Dic_All.Add("사회과학 이론/법칙/방법", "TR_SOCIAL_SCIENCE");
            Dic_All.Add("예술관련 이론/법칙/방식", "TR_ART");
            Dic_All.Add("철학 이론/사상", "TR_PHILOSOPHY");
            Dic_All.Add("의학 요법/처방, 의학 진단법", "TR_MEDICINE");
            Dic_All.Add("EV_OTHERS", "EV_OTHERS");
            Dic_All.Add("사회운동  및 선언", "EV_ACTIVITY");
            Dic_All.Add("전쟁/혁명", "EV_WAR_REVOLUTION");
            Dic_All.Add("스포츠/레저  관련 행사", "EV_SPORTS");
            Dic_All.Add("축제 명칭", "EV_FESTIVAL");
            Dic_All.Add("원소명", "MT_ELEMENT");
            Dic_All.Add("금속물", "MT_METAL");
            Dic_All.Add("암석", "MT_ROCK");
            Dic_All.Add("화학물질", "MT_CHEMICAL");
            Dic_All.Add("색", "TM_COLOR");
            Dic_All.Add("방향", "TM_DIRECTION");
            Dic_All.Add("기후지역 명칭", "TM_CLIMATE");
            Dic_All.Add("모양/형태", "TM_SHAPE");
            Dic_All.Add("세포/조직, 세포 명칭과 생물의 조직 및 기관에 대한 명칭", "TM_CELL_TISSUE");
            Dic_All.Add("증상/증세/질병", "TMM_DISEASE");
            Dic_All.Add("약/약품명", "TMM_DRUG");
            Dic_All.Add("하드웨어 용어", "TMI_HW");
            Dic_All.Add("소프트웨어 용어", "TMI_SW");
            Dic_All.Add("url 주소", "TMI_SITE");
            Dic_All.Add("이메일 주소", "TMI_EMAIL");
            Dic_All.Add("각종 제품 등의 모델명 (LM1000-2CX 등)", "TMI_MODEL");
            Dic_All.Add("IT 서비스 용어 (와이브로 서비스, DMB 서비스)", "TMI_SERVICE");
            Dic_All.Add("프로젝트 명칭", "TMI_PROJECT");
            Dic_All.Add("게임장르", "TMIG_GENRE");
            Dic_All.Add("스포츠/레저 용어 (기술/규칙 명칭)", "TM_SPORTS");
            Dic_All.Add("기타", "ETC");



            //**************************************개별*****************************************//

            //1
            Dic_Person = new Dictionary<string, string>();
            Dic_Person.Add("사람,신화,소설/게임,가수 등의 이름", "PS_NAME");

            //2
            Dic_Location = new Dictionary<string, string>();
            Dic_Location.Add("LC_OTHERS", "LC_OTHERS");
            Dic_Location.Add("국가명", "LCP_COUNTRY");
            Dic_Location.Add("도, 주 지역명", "LCP_PROVINCE");
            Dic_Location.Add("군, 면, 읍, 리, 동 등과 같은 세부 행정구역명, ~마을", "LCP_COUNTY");
            Dic_Location.Add("도시명", "LCP_CITY");
            Dic_Location.Add("수도명", "LCP_CAPITALCITY");
            Dic_Location.Add("강, 호수, 연못", "LCG_RIVER");
            Dic_Location.Add("해양/바다 명칭", "LCG_OCEAN");
            Dic_Location.Add("반도/만의 명칭", "LCG_BAY");
            Dic_Location.Add("산맥/산의 명칭", "LCG_MOUNTAIN");
            Dic_Location.Add("섬/제도 명칭", "LCG_ISLAND");
            Dic_Location.Add("대륙 명칭(아시아/아프리카)", "LCG_CONTINENT");
            Dic_Location.Add("관광 명소", "LC_TOUR");
            Dic_Location.Add("천체 명칭, 항성, 행성, 위성, 유성, 별자리 명칭", "LC_SPACE");

            //3
            Dic_Organization = new Dictionary<string, string>();
            Dic_Organization.Add("OG_OTHERS", "OG_OTHERS");
            Dic_Organization.Add("경제 관련 기관/단체/기업", "OGG_ECONOMY");
            Dic_Organization.Add("교육 기관/단체, 교육관련 기관", "OGG_EDUCATION");
            Dic_Organization.Add("국방기관", "OGG_MILITARY");
            Dic_Organization.Add("미디어 기관/단체, 방송관련 기관/기업", "OGG_MEDIA");
            Dic_Organization.Add("스포츠 관련 단체, 스포츠 팀", "OGG_SPORTS");
            Dic_Organization.Add("예술 기관/단체", "OGG_ART");
            Dic_Organization.Add("의학/의료 기관/단체", "OGG_MEDICINE");
            Dic_Organization.Add("종교 기관", "OGG_RELIGION");
            Dic_Organization.Add("과학 기관", "OGG_SCIENCE");
            Dic_Organization.Add("도서관 및 도서관 관련 기관/단체", "OGG_LIBRARY");
            Dic_Organization.Add("법률기관", "OGG_LAW");
            Dic_Organization.Add("정부/행정 기관, 공공기관, 정치기관", "OGG_POLITICS");
            Dic_Organization.Add("식당 이름", "OGG_FOOD");
            Dic_Organization.Add("호텔, 모텔, 민박, 펜션, 콘도, 리조트", "OGG_HOTEL");

            //4
            Dic_Artifacts = new Dictionary<string, string>();
            Dic_Artifacts.Add("문화재 명칭", "AF_CULTURAL_ASSET");
            Dic_Artifacts.Add("건축물/토목 건설물, 운동장이름, 아파트, 다리, 등대", "AF_BUILDING");
            Dic_Artifacts.Add("악기 명칭", "AF_MUSICAL_INSTRUMENT");
            Dic_Artifacts.Add("도로/철로 명칭", "AF_ROAD");
            Dic_Artifacts.Add("무기 명칭", "AF_WEAPON");
            Dic_Artifacts.Add("교통수단/자동차/선박 모델 및 유형, 운송수단, 놀이기구", "AF_TRANSPORT");
            Dic_Artifacts.Add("작품명", "AF_WORKS");
            Dic_Artifacts.Add("도서/서적 작품명", "AFW_DOCUMENT");
            Dic_Artifacts.Add("춤/무용/연극/가극 작품명 및 춤 종류", "AFW_PERFORMANCE");
            Dic_Artifacts.Add("영화 작품명/TV 프로그램 이름", "AFW_VIDEO");
            Dic_Artifacts.Add("미술 작품명", "AFW_ART_CRAFT");
            Dic_Artifacts.Add("음악 작품명", "AFW_MUSIC");
            Dic_Artifacts.Add("상품/제품 이름", "AF_WARES");

            //5
            Dic_Date = new Dictionary<string, string>();
            Dic_Date.Add("DT_OTHERS", "DT_OTHERS");
            Dic_Date.Add("기간(날짜)", "DT_DURATION");
            Dic_Date.Add("날짜, 절기", "DT_DAY");
            Dic_Date.Add("달", "DT_MONTH");
            Dic_Date.Add("년", "DT_YEAR");
            Dic_Date.Add("계절", "DT_SEASON");
            Dic_Date.Add("지질 시대", "DT_GEOAGE");
            Dic_Date.Add("왕조 시대", "DT_DYNASTY");

            //6
            Dic_Time = new Dictionary<string, string>();
            Dic_Time.Add("TI_OTHERS", "TI_OTHERS");
            Dic_Time.Add("기간(시간)", "TI_DURATION");
            Dic_Time.Add("시각", "TI_HOUR");
            Dic_Time.Add("분", "TI_MINUTE");
            Dic_Time.Add("초", "TI_SECOND");

            //7
            Dic_Civilization = new Dictionary<string, string>();
            Dic_Civilization.Add("문명/문화 명칭", "CV_NAME");
            Dic_Civilization.Add("민족/종족 명칭 또한 국가를 구성하는 국민을 지칭", "CV_TRIBE");
            Dic_Civilization.Add("스포츠/레포츠/레져 명칭", "CV_SPORTS");
            Dic_Civilization.Add("스포츠 용품/도구", "CV_SPORTS_INST");
            Dic_Civilization.Add("제도/정책 명칭", "CV_POLICY");
            Dic_Civilization.Add("조세 명칭", "CV_TAX");
            Dic_Civilization.Add("연금, 기금, 자금 명칭", "CV_FUNDS");
            Dic_Civilization.Add("언어 명칭", "CV_LANGUAGE");
            Dic_Civilization.Add("건축양식 명칭", "CV_BUILDING_TYPE");
            Dic_Civilization.Add("음식/곡물 명칭", "CV_FOOD");
            Dic_Civilization.Add("음료수, 술 명칭", "CV_DRINK");
            Dic_Civilization.Add("의복/섬유 명칭", "CV_CLOTHING");
            Dic_Civilization.Add("직위 명칭", "CV_POSITION");
            Dic_Civilization.Add("인간 관계 명칭", "CV_RELATION");
            Dic_Civilization.Add("직업 명칭", "CV_OCCUPATION");
            Dic_Civilization.Add("통화 명칭", "CV_CURRENCY");
            Dic_Civilization.Add("상과 훈장", "CV_PRIZE");
            Dic_Civilization.Add("법/법률 명칭", "CV_LAW");
            Dic_Civilization.Add("음식 종류 (한식, 일식, 양식 등)", "CV_FOOD_STYLE");

            //8
            Dic_Animal = new Dictionary<string, string>();
            Dic_Animal.Add("AM_OTHERS", "AM_OTHERS");
            Dic_Animal.Add("곤충", "AM_INSECT");
            Dic_Animal.Add("새", "AM_BIRD");
            Dic_Animal.Add("어류", "AM_FISH");
            Dic_Animal.Add("포유류", "AM_MAMMALIA");
            Dic_Animal.Add("양서류", "AM_AMPHIBIA");
            Dic_Animal.Add("파충류", "AM_REPTILIA");
            Dic_Animal.Add("동물 분류 명칭", "AM_TYPE");
            Dic_Animal.Add("동물의 한 부분/부위", "AM_PART");

            //9
            Dic_Plant = new Dictionary<string, string>();
            Dic_Plant.Add("PT_OTHERS", "PT_OTHERS");
            Dic_Plant.Add("과일 이름", "PT_FRUIT");
            Dic_Plant.Add("꽃", "PT_FLOWER");
            Dic_Plant.Add("나무", "PT_TREE");
            Dic_Plant.Add("풀", "PT_GRASS");
            Dic_Plant.Add("식물 유형 명칭", "PT_TYPE");
            Dic_Plant.Add("식물의 한 부분에 대한 명칭", "PT_PART");

            //10
            Dic_Quantity = new Dictionary<string, string>();
            Dic_Quantity.Add("QT_OTHERS", "QT_OTHERS");
            Dic_Quantity.Add("나이", "QT_AGE");
            Dic_Quantity.Add("크기/넓이", "QT_SIZE");
            Dic_Quantity.Add("길이/거리/높이", "QT_LENGTH");
            Dic_Quantity.Add("개수, 빈도", "QT_COUNT");
            Dic_Quantity.Add("인원수", "QT_MAN_COUNT");
            Dic_Quantity.Add("무게", "QT_WEIGHT");
            Dic_Quantity.Add("백분율,  비율, 농도", "QT_PERCENTAGE");
            Dic_Quantity.Add("속도", "QT_SPEED");
            Dic_Quantity.Add("온도", "QT_TEMPERATURE");
            Dic_Quantity.Add("부피", "QT_VOLUME");
            Dic_Quantity.Add("순서,  순차적 표현", "QT_ORDER");
            Dic_Quantity.Add("금액", "QT_PRICE");
            Dic_Quantity.Add("전화번호", "QT_PHONE");
            Dic_Quantity.Add("스포츠 관련 수량 표현 (점수 등)", "QT_SPORTS");
            Dic_Quantity.Add("TV 채널번호", "QT_CHANNEL");
            Dic_Quantity.Add("앨범 관련 수량 표현 (1집, 2집 등)", "QT_ALBUM");
            Dic_Quantity.Add("우편번호", "QT_ZIPCODE");

            //11
            Dic_StudyField = new Dictionary<string, string>();
            Dic_StudyField.Add("FD_OTHERS", "FD_OTHERS");
            Dic_StudyField.Add("과학관련 학문 분야 및 과학 학파", "FD_SCIENCE");
            Dic_StudyField.Add("사회과학 관련 학문 분야 및 학파", "FD_SOCIAL_SCIENCE");
            Dic_StudyField.Add("의학관련 학문 분야 및 학파", "FD_MEDICINE");
            Dic_StudyField.Add("예술관련 학문분야 및 학파(유파)", "FD_ART");
            Dic_StudyField.Add("철학관련 학문분야 및 학파(유파)", "FD_PHILOSOPHY");

            //12
            Dic_Theory = new Dictionary<string, string>();
            Dic_Theory.Add("TR_OTHERS", "TR_OTHERS");
            Dic_Theory.Add("과학 관련 기술/이론/법칙/방식/양식", "TR_SCIENCE");
            Dic_Theory.Add("사회과학 이론/법칙/방법", "TR_SOCIAL_SCIENCE");
            Dic_Theory.Add("예술관련 이론/법칙/방식", "TR_ART");
            Dic_Theory.Add("철학 이론/사상", "TR_PHILOSOPHY");
            Dic_Theory.Add("의학 요법/처방, 의학 진단법", "TR_MEDICINE");

            //13
            Dic_Event = new Dictionary<string, string>();
            Dic_Event.Add("EV_OTHERS", "EV_OTHERS");
            Dic_Event.Add("사회운동  및 선언", "EV_ACTIVITY");
            Dic_Event.Add("전쟁/혁명", "EV_WAR_REVOLUTION");
            Dic_Event.Add("스포츠/레저  관련 행사", "EV_SPORTS");
            Dic_Event.Add("축제 명칭", "EV_FESTIVAL");

            //14
            Dic_Material = new Dictionary<string, string>();
            Dic_Material.Add("원소명", "MT_ELEMENT");
            Dic_Material.Add("금속물", "MT_METAL");
            Dic_Material.Add("암석", "MT_ROCK");
            Dic_Material.Add("화학물질", "MT_CHEMICAL");

            //15
            Dic_Term = new Dictionary<string, string>();
            Dic_Term.Add("색", "TM_COLOR");
            Dic_Term.Add("방향", "TM_DIRECTION");
            Dic_Term.Add("기후지역 명칭", "TM_CLIMATE");
            Dic_Term.Add("모양/형태", "TM_SHAPE");
            Dic_Term.Add("세포/조직, 세포 명칭과 생물의 조직 및 기관에 대한 명칭", "TM_CELL_TISSUE");
            Dic_Term.Add("증상/증세/질병", "TMM_DISEASE");
            Dic_Term.Add("약/약품명", "TMM_DRUG");
            Dic_Term.Add("하드웨어 용어", "TMI_HW");
            Dic_Term.Add("소프트웨어 용어", "TMI_SW");
            Dic_Term.Add("url 주소", "TMI_SITE");
            Dic_Term.Add("이메일 주소", "TMI_EMAIL");
            Dic_Term.Add("각종 제품 등의 모델명 (LM1000-2CX 등)", "TMI_MODEL");
            Dic_Term.Add("IT 서비스 용어 (와이브로 서비스, DMB 서비스)", "TMI_SERVICE");
            Dic_Term.Add("프로젝트 명칭", "TMI_PROJECT");
            Dic_Term.Add("게임장르", "TMIG_GENRE");
            Dic_Term.Add("스포츠/레저 용어 (기술/규칙 명칭)", "TM_SPORTS");

            //16
            Dic_Etc = new Dictionary<string, string>();
            Dic_Etc.Add("기타", "ETC");

            #endregion
        }

        private void initList()
        {
            #region List셋팅
            Work_Etri_list = new ArrayList();
            Question_LIst = new ArrayList();
            Answer_List = new ArrayList();
            QuestionType_List = new ArrayList();
            QuestionFocus_List = new ArrayList();
            QuestionLat_List = new ArrayList();
            QuestionSat_List = new ArrayList();
            QuestionTagged_List = new ArrayList();
            ConfuseQt_List = new List<bool>();
            ConfuseQf_List = new List<bool>();
            ConfuseLat_List = new List<bool>();
            ConfuseSat_List = new List<bool>();
            CheckIndividual_List = new List<bool>();
            Time_List = new List<double>();
            Context_List = new ArrayList();


            //Etri
            EtriQuestion_List = new ArrayList();
            EtriQuestionType_List = new ArrayList();
            EtriQuestionFocus_List = new ArrayList();
            EtriQuestionLat_List = new ArrayList();
            EtriQuestionSat_List = new ArrayList();

            EtriQtCheck_List = new List<bool>();
            EtriQfCheck_List = new List<bool>();
            EtriLatCheck_List = new List<bool>();
            EtriSatCheck_List = new List<bool>();
            #endregion
        }


        private void 불러오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region 파일 불러오기
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt"; ;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                work_Panel.Enabled = true;
                작업모드ToolStripMenuItem.Checked = true;
                크로스체크모드ToolStripMenuItem.Checked = false;
                work_Panel.Visible = true;
                cross_Panel.Visible = false;

                allData_ListBox.Items.Clear();
                path = openFileDialog.FileName;
                FileStream fs_read = new FileStream(path, FileMode.Open, FileAccess.Read);
                StreamReader stream_read = new StreamReader(fs_read, System.Text.Encoding.UTF8);

                String mod = stream_read.ReadToEnd();
                workText = mod;//////////////

                stream_read.Close();
                fs_read.Close();

                try
                {
                    workReadParser(mod);
                    label12.Text = questionCount.ToString();
                   
                    //현재 체크
                    allData_ListBox.SelectedIndex = currentReadQuestion;
                    //allData_ListBox.SelectedIndex = 0;
                    allData_ListBox.Select();

                }
                catch
                {

                }
            }

            #endregion
        }

        private void squadCosmosJsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region SquadJson->CosmosJson 변환버튼 클릭

            string conversionPath;
            string conversionText;//변환되어 우리json에 저장될 변수

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt"; ;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                conversionPath = openFileDialog.FileName;
                FileStream fs_read = new FileStream(conversionPath, FileMode.Open, FileAccess.Read);
                StreamReader stream_read = new StreamReader(fs_read, System.Text.Encoding.UTF8);


                String mod = stream_read.ReadToEnd();


                stream_read.Close();
                fs_read.Close();

                try
                {
                    conversionText = conversionWriteParser(mod);

                    //쓰기는 새 파일에
                    SaveFileDialog openFileDialog_final = new SaveFileDialog();
                    openFileDialog_final.Title = "새로 저장";
                    openFileDialog_final.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt"; ;
                    openFileDialog_final.RestoreDirectory = true;
                    openFileDialog_final.InitialDirectory = @"C:\";

                    if (openFileDialog_final.ShowDialog() == DialogResult.OK)
                    {
                        //string conversionPath_final = openFileDialog_final.FileName;

                        FileStream filestream = new FileStream(openFileDialog_final.FileName, FileMode.Create, FileAccess.Write);
                        StreamWriter stream_write = new StreamWriter(filestream, Encoding.UTF8);//true:이어쓰기 false:덮어쓰기
                        stream_write.Write(conversionText);
                        stream_write.Close();
                    }
                    MessageBox.Show("변환 후 저장 성공");
                }
                catch
                {
                    MessageBox.Show("변환 후 저장 실패");
                }

            }

            #endregion
        }

        private void workReadParser(string text)
        {
            #region WorkJson파싱

            Question_LIst.Clear();
            Answer_List.Clear();

            Context_List.Clear();

            QuestionType_List.Clear();
            QuestionFocus_List.Clear();
            QuestionLat_List.Clear();
            QuestionSat_List.Clear();
            QuestionTagged_List.Clear();

            ConfuseQt_List.Clear();
            ConfuseQf_List.Clear();
            ConfuseLat_List.Clear();
            ConfuseSat_List.Clear();
            CheckIndividual_List.Clear();

            EtriQtCheck_List.Clear();
            EtriQfCheck_List.Clear();
            EtriLatCheck_List.Clear();
            EtriSatCheck_List.Clear();

            Time_List.Clear();

            JObject obj = JObject.Parse(text);     
            JArray array = JArray.Parse(obj["data"].ToString());
            currentReadQuestion = Convert.ToInt32(obj["progress"]);
        
            string c;
            try
            {
                c = obj["formatt"].ToString();
            }
            catch
            {
                MessageBox.Show("Json파일 형식이 맞지 않습니다.");
                return;
            }

            string b;
            string a;

            string d, e, f, g, h;

            bool aa, bb, cc, dd;
            bool aaa, bbb, ccc, ddd;
            bool eee;

            string context;
            string id;

            double ttt;

            foreach (JObject itemObj in array)
            {
                JArray ooo = JArray.Parse(itemObj["paragraphs"].ToString());

                foreach (JObject itemObj2 in ooo)
                {
                    context = itemObj2["context"].ToString();
                   
                    JArray iii = JArray.Parse(itemObj2["qas"].ToString());

                    foreach (JObject itemObj3 in iii)
                    {
                        b = itemObj3["question"].ToString();

                        id = itemObj3["id"].ToString();

                        d = itemObj3["questionType"].ToString();
                        e = itemObj3["questionFocus"].ToString();
                        f = itemObj3["questionSAT"].ToString();
                        g = itemObj3["questionLAT"].ToString();
                        h = itemObj3["question_tagged"].ToString();

                        aa = Convert.ToBoolean(itemObj3["confuseQt"]);
                        bb = Convert.ToBoolean(itemObj3["confuseQf"]);
                        cc = Convert.ToBoolean(itemObj3["confuseLat"]);
                        dd = Convert.ToBoolean(itemObj3["confuseSat"]);

                        aaa = Convert.ToBoolean(itemObj3["etriQtCheck"]);
                        bbb = Convert.ToBoolean(itemObj3["etriQfCheck"]);
                        ccc = Convert.ToBoolean(itemObj3["etriLatCheck"]);
                        ddd = Convert.ToBoolean(itemObj3["etriSatCheck"]);

                        ttt = Convert.ToDouble(itemObj3["time"]);

                        eee = Convert.ToBoolean(itemObj3["checkIndividual"]);

                        Question_LIst.Add(b);

                        QuestionType_List.Add(d);
                        QuestionFocus_List.Add(e);
                        QuestionLat_List.Add(g);
                        QuestionSat_List.Add(f);
                        QuestionTagged_List.Add(h);

                        ConfuseQt_List.Add(aa);
                        ConfuseQf_List.Add(bb);
                        ConfuseLat_List.Add(cc);
                        ConfuseSat_List.Add(dd);

                        EtriQtCheck_List.Add(aaa);
                        EtriQfCheck_List.Add(bbb);
                        EtriLatCheck_List.Add(ccc);
                        EtriSatCheck_List.Add(ddd);
                        CheckIndividual_List.Add(eee);
                    
                        Time_List.Add(ttt);

                        JArray yyy = JArray.Parse(itemObj3["answers"].ToString());

                        foreach (JObject itemObj4 in yyy)
                        {
                            a = itemObj4["text"].ToString();

                            Answer_List.Add(a);

                            allData_ListBox.Items.Add(b.ToString() + " --- " + a.ToString());

                        }
                    }
                }
            }

            //총 질문 개수 set
            questionCount = allData_ListBox.Items.Count;

            #endregion
        }

        private string workWriteParser(string text)
        {
            #region WorkJson저장

            JObject obj = JObject.Parse(text);
            JArray array = JArray.Parse(obj["data"].ToString());

            WorkFormatt.RootObject r = new WorkFormatt.RootObject();
            WorkFormatt.Datum d;
            r.data = new List<WorkFormatt.Datum>();

            WorkFormatt.Paragraph p;///////////////
            WorkFormatt.Qa q;
            WorkFormatt.Answer j;
         
            int i = 0;

            r.version = obj["version"].ToString();
            r.creator = obj["creator"].ToString();
            r.formatt = "Work";

            currentWriteQuestion = allData_ListBox.SelectedIndex;
            r.progress = currentWriteQuestion;

            foreach (JObject itemObj in array)
            {
                d = new WorkFormatt.Datum();
                d.paragraphs = new List<EtriWork.WorkFormatt.Paragraph>();

                d.title = itemObj["title"].ToString();

                r.data.Add(d);

                JArray ooo = JArray.Parse(itemObj["paragraphs"].ToString());


                foreach (JObject itemObj2 in ooo)
                {
                    p = new WorkFormatt.Paragraph();
                    p.qas = new List<EtriWork.WorkFormatt.Qa>();

                    p.context = itemObj2["context"].ToString();
                    p.context_en = itemObj2["context_en"].ToString();
                    p.context_tagged = itemObj2["context_tagged"].ToString();

                    d.paragraphs.Add(p);

                    JArray iii = JArray.Parse(itemObj2["qas"].ToString());

                    foreach (JObject itemObj3 in iii)
                    {
                        q = new WorkFormatt.Qa();
                        q.answers = new List<EtriWork.WorkFormatt.Answer>();

                        q.questionType = QuestionType_List[i].ToString();
                        q.questionFocus = QuestionFocus_List[i].ToString();
                        q.questionSAT = QuestionSat_List[i].ToString();
                        q.questionLAT = QuestionLat_List[i].ToString();
                        q.question_tagged = QuestionTagged_List[i].ToString();

                        q.confuseQt = Convert.ToBoolean(ConfuseQt_List[i]);
                        q.confuseQf = Convert.ToBoolean(ConfuseQf_List[i]);
                        q.confuseLat = Convert.ToBoolean(ConfuseLat_List[i]);
                        q.confuseSat = Convert.ToBoolean(ConfuseSat_List[i]);

                        q.etriQtCheck = Convert.ToBoolean(EtriQtCheck_List[i]);
                        q.etriQfCheck = Convert.ToBoolean(EtriQfCheck_List[i]);
                        q.etriLatCheck = Convert.ToBoolean(EtriLatCheck_List[i]);
                        q.etriSatCheck = Convert.ToBoolean(EtriSatCheck_List[i]);

               
                        try
                        {
                            q.etriQt = itemObj3["etriQt"].ToString();
                            q.etriQf = itemObj3["etriQf"].ToString();
                            q.etriLat = itemObj3["etriLat"].ToString();
                            q.etriSat = itemObj3["etriSat"].ToString();
                        }
                        catch
                        {

                        }
                      
                        q.checkIndividual = Convert.ToBoolean(CheckIndividual_List[i]);

                        q.time = Convert.ToDouble(Time_List[i]);

                        q.id = itemObj3["id"].ToString();
                        q.question = itemObj3["question"].ToString();
                        q.question_en = itemObj3["question_en"].ToString();
                        p.qas.Add(q);

                        JArray yyy = JArray.Parse(itemObj3["answers"].ToString());

                        foreach (JObject itemObj4 in yyy)
                        {
                            j = new WorkFormatt.Answer();
                            j.text = itemObj4["text"].ToString();
                            j.text_en = itemObj4["text_en"].ToString();
                            j.text_tagged = itemObj4["text_tagged"].ToString();
                            j.text_syn = itemObj4["text_syn"].ToString();
                            j.answer_start = Convert.ToInt32(itemObj4["answer_start"]);
                            j.answer_end = Convert.ToInt32(itemObj4["answer_end"]);

                            q.answers.Add(j);

                        }

                        i++;
                    }
                }
            }

            string json = JsonConvert.SerializeObject(r, Formatting.Indented);

            workText = json;

            return json;

            #endregion
        }

        private string conversionWriteParser(string text)
        {
            #region SquadJson->WorkJson 변환

            JObject obj = JObject.Parse(text);
            JArray array = JArray.Parse(obj["data"].ToString());

            WorkFormatt.RootObject r = new WorkFormatt.RootObject();
            WorkFormatt.Datum d;
            r.data = new List<WorkFormatt.Datum>();

            WorkFormatt.Paragraph p;
            WorkFormatt.Qa q;
            WorkFormatt.Answer j;
          
            r.version = obj["version"].ToString();
            r.creator = obj["creator"].ToString();
            r.formatt = "Work";

            foreach (JObject itemObj in array)
            {
                d = new WorkFormatt.Datum();
                d.paragraphs = new List<EtriWork.WorkFormatt.Paragraph>();

                d.title = itemObj["title"].ToString();

                r.data.Add(d);

                JArray ooo = JArray.Parse(itemObj["paragraphs"].ToString());


                foreach (JObject itemObj2 in ooo)
                {
                    p = new WorkFormatt.Paragraph();
                    p.qas = new List<EtriWork.WorkFormatt.Qa>();

                    p.context = itemObj2["context_original"].ToString();
                
                    d.paragraphs.Add(p);

                    JArray iii = JArray.Parse(itemObj2["qas"].ToString());

                    foreach (JObject itemObj3 in iii)
                    {
                        q = new WorkFormatt.Qa();
                        q.answers = new List<EtriWork.WorkFormatt.Answer>();
                        q.id = itemObj3["id"].ToString();
                        q.question = itemObj3["question_original"].ToString();
                      
                        p.qas.Add(q);

                        JArray yyy = JArray.Parse(itemObj3["answers"].ToString());

                        foreach (JObject itemObj4 in yyy)
                        {
                            j = new WorkFormatt.Answer();
                            j.text = itemObj4["text_original"].ToString();
                            j.answer_start = Convert.ToInt32(itemObj4["answer_start"]);
                            j.answer_end = Convert.ToInt32(itemObj4["answer_end"]);

                            q.answers.Add(j);


                        }
                    }
                }
            }

            string json = JsonConvert.SerializeObject(r, Formatting.Indented);

            return json;

            #endregion
        }

        private void allData_ListBox_SelectedChanged(object sender, EventArgs e)
        {
            #region allData_SelectedChanged Event

            string question = Question_LIst[allData_ListBox.SelectedIndex].ToString();
            string question_tagged = QuestionTagged_List[allData_ListBox.SelectedIndex].ToString();
            string answer = Answer_List[allData_ListBox.SelectedIndex].ToString();
            string questionType = QuestionType_List[allData_ListBox.SelectedIndex].ToString();
            string questionFocus = QuestionFocus_List[allData_ListBox.SelectedIndex].ToString();
            string questionLat = QuestionLat_List[allData_ListBox.SelectedIndex].ToString();
            string questionSat = QuestionSat_List[allData_ListBox.SelectedIndex].ToString();

            if (ConfuseQt_List[allData_ListBox.SelectedIndex] == true)
                questionTypeCheckBox.Checked = true;
            else
                questionTypeCheckBox.Checked = false;

            if (ConfuseQf_List[allData_ListBox.SelectedIndex] == true)
                questionFocusCheckBox.Checked = true;
            else
                questionFocusCheckBox.Checked = false;


            if (ConfuseLat_List[allData_ListBox.SelectedIndex] == true)
                questionLatCheckBox.Checked = true;
            else
                questionLatCheckBox.Checked = false;


            if (ConfuseSat_List[allData_ListBox.SelectedIndex] == true)
                questionSatCheckBox.Checked = true;
            else
                questionSatCheckBox.Checked = false;

            //set
            qtStartIndex = 0;
            latStartIndex = 0;

            //선택된 질문,선택된 답
            selectedAnswer_textBox.Text = answer;

            if (question_tagged == "")
                selectedQuestion_TextBox.Text = question;
            else
                selectedQuestion_TextBox.Text = question_tagged;

            ourSubmitQt_textBox.Text = questionType;
            ourSubmitQf_textBox.Text = questionFocus;
            ourSubmitLat_textBox.Text = questionLat;
            ourSubmitSat_textBox.Text = questionSat;

            etriQf_textBox.Text = "";
            etriSat_textBox.Text = "";
            etriQt_textBox.Text = "";
            etriLat_textBox.Text = "";

            selectedAnswer_textBox.ForeColor = Color.Black;

            try
            {
                bool redAnswer = false;
                bool blueAnswer = false;


                //etriOpenApi
                obj_EtriOpenApi = new EtriOpenApi();
                obj_EtriOpenApi.setAnswer(answer);
                obj_EtriOpenApi.useApi2();

                ArrayList arr = new ArrayList();
                arr = obj_EtriOpenApi.getTextList();

                ArrayList arr2 = new ArrayList();
                arr2 = obj_EtriOpenApi.getTypeList();

                //모두 인식
                selectedAnswer_textBox.AppendText("\r\n");
                selectedAnswer_textBox.AppendText("--------------------------------" + "\r\n");

                if (checkError(arr, answer) == true)//일부만 인식하는 경우
                {
                    redAnswer = true;

                    string reAnswer = answer.Replace(" ", "");

                    obj_EtriOpenApi = new EtriOpenApi();
                    obj_EtriOpenApi.setAnswer(reAnswer);
                    obj_EtriOpenApi.useApi2();

                    ArrayList arrtmp1, arrtmp2;

                    arrtmp1 = new ArrayList();
                    arrtmp1 = obj_EtriOpenApi.getTextList();

                    arrtmp2 = new ArrayList();
                    arrtmp2 = obj_EtriOpenApi.getTypeList();

                    for (int i = 0; i < arrtmp1.Count; i++)
                    {
                        selectedAnswer_textBox.AppendText(arrtmp1[i].ToString() + "---");
                        selectedAnswer_textBox.AppendText(arrtmp2[i].ToString() + "\r\n");

                    }

                    selectedAnswer_textBox.AppendText("~~~~~~~~~~~~~~~~~~~~~~~" + "\r\n");

                }
                else//모두 인식
                {
                    string reAnswer = answer.Replace(" ", "");

                    obj_EtriOpenApi = new EtriOpenApi();
                    obj_EtriOpenApi.setAnswer(reAnswer);
                    obj_EtriOpenApi.useApi2();

                    ArrayList arrtmp1, arrtmp2;

                    arrtmp1 = new ArrayList();
                    arrtmp1 = obj_EtriOpenApi.getTextList();

                    arrtmp2 = new ArrayList();
                    arrtmp2 = obj_EtriOpenApi.getTypeList();

                    if (arrtmp1.Count == 1 && checkError(arrtmp1, answer) == false)//개체는 1개이며 모두 인식하면
                    {
                      
                        if (arrtmp2[0].ToString() != arr2[0].ToString() || arrtmp2.Count != arr2.Count)
                        {
                            blueAnswer = true;

                            for (int i = 0; i < arrtmp1.Count; i++)
                            {
                                selectedAnswer_textBox.AppendText(arrtmp1[i].ToString() + "---");
                                selectedAnswer_textBox.AppendText(arrtmp2[i].ToString() + "\r\n");
                                selectedAnswer_textBox.AppendText("~~~~~~~~~~~~~~~~~~~~~~~" + "\r\n");
                            }
                        }

                    }
                }

                for (int i = 0; i < arr.Count; i++)
                {
                    selectedAnswer_textBox.AppendText(arr[i].ToString() + "---");
                    selectedAnswer_textBox.AppendText(arr2[i].ToString() + "\r\n");
                }


                if (blueAnswer == true)//일부만 인식할 경우 빨강색 표시
                {
                    selectedAnswer_textBox.ForeColor = Color.Blue;
                    CheckIndividual_List[allData_ListBox.SelectedIndex] = true;
                }
                else if (redAnswer == true)//일부만 인식할 경우 빨강색 표시
                {
                    selectedAnswer_textBox.ForeColor = Color.Red;
                }


            }
            catch { }

            //이동시 questionType, sat수동 체크 reset
            questionType_listBox.SelectedIndex = -1;
            satMainCategory_listBox.SelectedIndex = -1;

            //현재 질문 번호 셋
            label11.Text = (allData_ListBox.SelectedIndex + 1).ToString();

            #endregion
        }

        private bool checkError(ArrayList arr, string ans)
        {
            #region Error 체크
            bool check = false;
            int arrLength = 0;

            for (int i = 0; i < arr.Count; i++)
            {
                string tmp = arr[i].ToString().Replace(" ", "");
                arrLength = arrLength + tmp.Length;
            }

            ans = ans.Replace(" ", "");

            if (arrLength != ans.Length)
                check = true;

            return check;
            #endregion 
        }

        
        private void questionType_listBox_SelectedChanged(object sender, EventArgs e)
        {
            #region 질문유형 리스트 박스
            if (questionType_listBox.SelectedIndex != 2)
            {
                ourSubmitQt_textBox.Text = questionType_listBox.Text;
                descriptive_listBox.Visible = false;
            }
            else//서술형
            {
                descriptive_listBox.Visible = true;
            }
            #endregion
        }


        private void satMainCategory_listBox_SelectedChanged(object sender, EventArgs e)
        {
            #region SAT대분류 listBoxChanged Event

            label2.Text = "SAT세분류 도움말";

            satSubCategory_listBox.Items.Clear();

            for (int i = 0; i < satMainCategory_listBox.SelectedItems.Count; i++)
            {
                switch (satMainCategory_listBox.SelectedIndices[i])
                {
                    case 0:
                        foreach (KeyValuePair<string, string> de in Dic_Person)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                    case 1:
                        foreach (KeyValuePair<string, string> de in Dic_Location)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                    case 2:
                        foreach (KeyValuePair<string, string> de in Dic_Organization)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                    case 3:
                        foreach (KeyValuePair<string, string> de in Dic_Artifacts)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                    case 4:
                        foreach (KeyValuePair<string, string> de in Dic_Date)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                    case 5:
                        foreach (KeyValuePair<string, string> de in Dic_Time)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                    case 6:
                        foreach (KeyValuePair<string, string> de in Dic_Civilization)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                    case 7:
                        foreach (KeyValuePair<string, string> de in Dic_Animal)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                    case 8:
                        foreach (KeyValuePair<string, string> de in Dic_Plant)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                    case 9:
                        foreach (KeyValuePair<string, string> de in Dic_Quantity)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                    case 10:
                        foreach (KeyValuePair<string, string> de in Dic_StudyField)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                    case 11:
                        foreach (KeyValuePair<string, string> de in Dic_Theory)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                    case 12:
                        foreach (KeyValuePair<string, string> de in Dic_Event)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                    case 13:
                        foreach (KeyValuePair<string, string> de in Dic_Material)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                    case 14:
                        foreach (KeyValuePair<string, string> de in Dic_Term)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                    case 15:
                        foreach (KeyValuePair<string, string> de in Dic_Etc)
                            satSubCategory_listBox.Items.Add(de.Key);
                        break;
                }
            }

            #endregion
        }

        private void descriptive_listBox_SelectedChanged(object sender, EventArgs e)
        {
            #region 서술형 세분류 리스트 박스
            ourSubmitQt_textBox.Text = "서술형-" + descriptive_listBox.Text;
            ourSubmitSat_textBox.Text = "ETC";
            #endregion
        }


        private void saveNext_Btn_Click(object sender, EventArgs e)
        {
            #region 적용 후 다음 버튼

            if (ourSubmitQt_textBox.Text.Length <= 0 || ourSubmitSat_textBox.Text.Length <= 0)
            {
                MessageBox.Show("정답을 입력해주세요");
                return;
            }

            if (ourSubmitQf_textBox.Text.Length > 0 && !selectedQuestion_TextBox.Text.Contains("{"))
            {
                MessageBox.Show("질문 초점 적용 버튼을 눌러주세요");
                return;
            }

            if (ourSubmitLat_textBox.Text.Length > 0 && !selectedQuestion_TextBox.Text.Contains("["))
            {
                MessageBox.Show("LAT 적용 버튼을 눌러주세요");
                return;
            }

            QuestionType_List[allData_ListBox.SelectedIndex] = ourSubmitQt_textBox.Text;
            QuestionFocus_List[allData_ListBox.SelectedIndex] = ourSubmitQf_textBox.Text;
            QuestionLat_List[allData_ListBox.SelectedIndex] = ourSubmitLat_textBox.Text;
            QuestionSat_List[allData_ListBox.SelectedIndex] = ourSubmitSat_textBox.Text;


            if (selectedQuestion_TextBox.Text.Contains("{") || selectedQuestion_TextBox.Text.Contains("["))
                QuestionTagged_List[allData_ListBox.SelectedIndex] = selectedQuestion_TextBox.Text;
            else
                QuestionTagged_List[allData_ListBox.SelectedIndex] = "";


            if (questionTypeCheckBox.Checked == true)
                ConfuseQt_List[allData_ListBox.SelectedIndex] = true;
            else
                ConfuseQt_List[allData_ListBox.SelectedIndex] = false;


            if (questionFocusCheckBox.Checked == true)
                ConfuseQf_List[allData_ListBox.SelectedIndex] = true;
            else
                ConfuseQf_List[allData_ListBox.SelectedIndex] = false;


            if (questionLatCheckBox.Checked == true)
                ConfuseLat_List[allData_ListBox.SelectedIndex] = true;
            else
                ConfuseLat_List[allData_ListBox.SelectedIndex] = false;


            if (questionSatCheckBox.Checked == true)
                ConfuseSat_List[allData_ListBox.SelectedIndex] = true;
            else
                ConfuseSat_List[allData_ListBox.SelectedIndex] = false;

            if (etriQt_textBox.Text != ourSubmitQt_textBox.Text)
                EtriQtCheck_List[allData_ListBox.SelectedIndex] = false;

            if (etriQf_textBox.Text != ourSubmitQf_textBox.Text)
                EtriQfCheck_List[allData_ListBox.SelectedIndex] = false;

            if (etriLat_textBox.Text != ourSubmitLat_textBox.Text)
                EtriLatCheck_List[allData_ListBox.SelectedIndex] = false;

            if (etriSat_textBox.Text != ourSubmitSat_textBox.Text)
                EtriSatCheck_List[allData_ListBox.SelectedIndex] = false;

            //각 타입 헷갈리는 체크 해제
            questionTypeCheckBox.Checked = false;
            questionFocusCheckBox.Checked = false;
            questionLatCheckBox.Checked = false;
            questionSatCheckBox.Checked = false;

            //다음 질문으로 넘어갈시 리셋
            questionType_listBox.SelectedIndex = -1;
            satMainCategory_listBox.SelectedIndex = -1;


            if (questionCount == 0) return;
            //allData_ListBox 다음 줄로 이동
            if (allData_ListBox.SelectedIndex < questionCount && allData_ListBox.SelectedIndex + 1 != questionCount)
            {
                allData_ListBox.SelectedIndex++;
                allData_ListBox.Select();
            }
            else
            {
                MessageBox.Show("마지막 질문 입니다.");

                string questionType = QuestionType_List[allData_ListBox.SelectedIndex].ToString();
                string questionFocus = QuestionFocus_List[allData_ListBox.SelectedIndex].ToString();
                string questionLat = QuestionLat_List[allData_ListBox.SelectedIndex].ToString();
                string questionSat = QuestionSat_List[allData_ListBox.SelectedIndex].ToString();

                ourSubmitQt_textBox.Text = questionType;
                ourSubmitQf_textBox.Text = questionFocus;
                ourSubmitLat_textBox.Text = questionLat;
                ourSubmitSat_textBox.Text = questionSat;

                return;
            }

            #endregion
        }

     
        private void clear_Btn_Click(object sender, EventArgs e)
        {
            #region clear 버튼
            selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Replace("{{", "");
            selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Replace("}}", "");
            selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Replace("[[", "");
            selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Replace("]]", "");

            ourSubmitQt_textBox.Text = "";
            ourSubmitQf_textBox.Text = "";
            ourSubmitLat_textBox.Text = "";
            ourSubmitSat_textBox.Text = "";
            #endregion
        }

        
        private void moveNext_Btn_Click(object sender, EventArgs e)
        {
            #region 다음으로 이동
            
           if (questionCount == 0) return;
           if (allData_ListBox.SelectedIndex < questionCount && allData_ListBox.SelectedIndex + 1 != questionCount)
            {
                allData_ListBox.SelectedIndex++;
                allData_ListBox.Select();
            }
            else return;
            #endregion
        }

        
        private void moveBefore_Btn_Click(object sender, EventArgs e)
        {
            #region 이전으로 이동
           
            if (questionCount == 0) return;
            if (allData_ListBox.SelectedIndex < questionCount && allData_ListBox.SelectedIndex - 1 != -1)
            {
                allData_ListBox.SelectedIndex--;
                allData_ListBox.Select();
            }
            else return;
            #endregion
        }

        
        private void moveTop_Btn_Click(object sender, EventArgs e)
        {
            #region 최상위로 이동
            allData_ListBox.SelectedIndex = 0;
            #endregion
        }

       
        private void moveEnd_Btn_Click(object sender, EventArgs e)
        {
            #region 최하위로 이동
            allData_ListBox.SelectedIndex = questionCount - 1;
            #endregion
        }


        
        public void Write_File()
        {
            #region 불러온 json 파일에 쓰기
            string write = workWriteParser(workText);
            StreamWriter stream_write = new StreamWriter(path, false, System.Text.Encoding.UTF8);
            stream_write.Write(write);
            stream_write.Close();
            #endregion
        }

        
        private void ourSubmitQt_textBox_MDClick(object sender, MouseEventArgs e)
        {
            #region Etri Open Api 질문유형 복사
            EtriQtCheck_List[allData_ListBox.SelectedIndex] = true;

            ourSubmitQt_textBox.Text = etriQt_textBox.Text;
            #endregion
        }

        
        private void ourSubmitQf_textBox_MDClick(object sender, MouseEventArgs e)
        {
            #region Etri Open Api 질문초점 복사
            EtriQfCheck_List[allData_ListBox.SelectedIndex] = true;

            if (ourSubmitQf_textBox.Text.Length <= 0)
                ourSubmitQf_textBox.Text = etriQf_textBox.Text;
            else//이미 있을 때
            {
                if (ourSubmitQf_textBox.Text.Contains(etriQf_textBox.Text))
                {
                    return;
                }
                else
                    ourSubmitQf_textBox.AppendText(":" + etriQf_textBox.Text);
            }
            #endregion
        }

        
        private void ourSubmitLat_textBox_MDClick(object sender, MouseEventArgs e)
        {
            #region Etri Open Api LAT 복사
            EtriLatCheck_List[allData_ListBox.SelectedIndex] = true;

            if (ourSubmitLat_textBox.Text.Length <= 0)
                ourSubmitLat_textBox.Text = etriLat_textBox.Text;
            else //이미 있을 때
                if (ourSubmitLat_textBox.Text.Contains(etriLat_textBox.Text))
                {
                    return;
                }
                else
                    ourSubmitLat_textBox.AppendText(":" + etriLat_textBox.Text);
            #endregion

        }

        
        private void ourSubmitSat_textBox_MDClick(object sender, MouseEventArgs e)
        {
            #region Etri Api SAT 복사
            EtriSatCheck_List[allData_ListBox.SelectedIndex] = true;

            ourSubmitSat_textBox.Text = etriSat_textBox.Text;
            #endregion
        }


        private void setQf_Btn_Click(object sender, EventArgs e)
        {
            #region 질문초점 적용 버튼 클릭
            ////////////////////////////////////////////////////////////하이라이트 된 부분 있을 때
            if (selectedQuestion_TextBox.SelectedText.Length > 0)
            {
                if (ourSubmitQf_textBox.Text == "")//textbox가 비었을 때
                {
                    //하이라이트 된 부분 셋
                    ourSubmitQf_textBox.Text = selectedQuestion_TextBox.SelectedText;

                    int b = selectedQuestion_TextBox.SelectionStart;
                    int a = selectedQuestion_TextBox.SelectionLength;
                    selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b, "{{");


                    selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b + a + 2, "}}");
                }
                else//무언가가 있을 때
                {
                    if (ourSubmitQf_textBox.Text.Contains(selectedQuestion_TextBox.SelectedText))//추가 하려는 단어가 이미 있으면
                    {
                        int b = selectedQuestion_TextBox.SelectionStart;
                        int a = selectedQuestion_TextBox.SelectionLength;
                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b, "{{");


                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b + a + 2, "}}");
                    }
                    else
                    {
                        ourSubmitQf_textBox.AppendText(":" + selectedQuestion_TextBox.SelectedText);

                        int b = selectedQuestion_TextBox.SelectionStart;
                        int a = selectedQuestion_TextBox.SelectionLength;
                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b, "{{");
                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b + a + 2, "}}");
                    }

                }
         
            }
            else//////////////////////////////////////////////////////하이라이트 된 부분 없을 때
            {
                System.Text.RegularExpressions.Regex cntStr = new System.Text.RegularExpressions.Regex(ourSubmitQf_textBox.Text);
                int count = int.Parse(cntStr.Matches(selectedQuestion_TextBox.Text, 0).Count.ToString());

                if (count == 1)//highlight 안 시키고 그냥 추가
                {
                    int b = selectedQuestion_TextBox.Text.IndexOf(ourSubmitQf_textBox.Text);
                    int a = ourSubmitQf_textBox.Text.Length;

                    if (selectedQuestion_TextBox.Text[b - 1] == '{')
                        MessageBox.Show("이미 추가되어있습니다");
                    else
                    {
                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b, "{{");
                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b + a + 2, "}}");
                    }

                }
                else//여러개 인 경우
                {
                    int b = ourSubmitQf_textBox.Text.LastIndexOf(":");
                    int a = ourSubmitQf_textBox.Text.Length;

                    string s = ourSubmitQf_textBox.Text.Substring(b + 1, a - b - 1);
                    int bb = selectedQuestion_TextBox.Text.IndexOf(s);

                    if (selectedQuestion_TextBox.Text[bb - 1] == '{')
                        MessageBox.Show("이미 추가되어있습니다");
                    else
                    {
                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(bb, "{{");
                        bb = selectedQuestion_TextBox.Text.IndexOf(s);
                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(bb + s.Length, "}}");
                    }

                }

            }
            #endregion
        }

        private void setLat_Btn_Click(object sender, EventArgs e)
        {
            #region LAT 적용 버튼 클릭
            //하이라이트 된 부분 있을 때
            if (selectedQuestion_TextBox.SelectedText.Length > 0)
            {
                if (ourSubmitLat_textBox.Text == "")//textbox가 비었을 때
                {
                    //하이라이트 된 부분 셋
                    ourSubmitLat_textBox.Text = selectedQuestion_TextBox.SelectedText;

                    //[[,]] set
                    int b = selectedQuestion_TextBox.SelectionStart;
                    //int b = selectedQuestion_TextBox.Text.IndexOf(selectedQuestion_TextBox.SelectedText);
                    int a = selectedQuestion_TextBox.SelectionLength;
                    selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b, "[[");


                    selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b + a + 2, "]]");
                }
                else//무언가가 있을 때
                {
                    if (ourSubmitLat_textBox.Text.Contains(selectedQuestion_TextBox.SelectedText))//추가 하려는 단어가 이미 있으면
                    {
                        int b = selectedQuestion_TextBox.SelectionStart;
                        //int b = selectedQuestion_TextBox.Text.IndexOf(selectedQuestion_TextBox.SelectedText);
                        int a = selectedQuestion_TextBox.SelectionLength;
                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b, "[[");


                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b + a + 2, "]]");
                    }
                    else//추가하려는 단어가 없으면
                    {
                        ourSubmitLat_textBox.AppendText(":" + selectedQuestion_TextBox.SelectedText);

                        int b = selectedQuestion_TextBox.SelectionStart;
                        //int b = selectedQuestion_TextBox.Text.IndexOf(selectedQuestion_TextBox.SelectedText);
                        int a = selectedQuestion_TextBox.SelectionLength;
                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b, "[[");


                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b + a + 2, "]]");
                    }
                }
            }
            else//하이라이트 된 부분 없을 때
            {
                System.Text.RegularExpressions.Regex cntStr = new System.Text.RegularExpressions.Regex(ourSubmitLat_textBox.Text);
                int count = int.Parse(cntStr.Matches(selectedQuestion_TextBox.Text, 0).Count.ToString());

                if (count == 1)//highlight 안 시키고 그냥 추가
                {
                    int b = selectedQuestion_TextBox.Text.IndexOf(ourSubmitLat_textBox.Text);
                    int a = ourSubmitLat_textBox.Text.Length;

                    if (selectedQuestion_TextBox.Text[b - 1] == '[')
                        MessageBox.Show("이미 추가되어있습니다");
                    else
                    {
                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b, "[[");
                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(b + a + 2, "]]");
                    }

                }
                else//여러개 인 경우
                {
                    int b = ourSubmitLat_textBox.Text.LastIndexOf(":");
                    int a = ourSubmitLat_textBox.Text.Length;

                    string s = ourSubmitLat_textBox.Text.Substring(b + 1, a - b - 1);

                    int bb = selectedQuestion_TextBox.Text.IndexOf(s);


                    if (selectedQuestion_TextBox.Text[bb - 1] == '[')
                        MessageBox.Show("이미 추가되어있습니다");
                    else
                    {
                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(bb, "[[");
                        bb = selectedQuestion_TextBox.Text.IndexOf(s);
                        selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Insert(bb + s.Length, "]]");
                    }

                }
            }
            #endregion
        }



        private void helpCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            #region 도움말
            if (helpCheckBox.Checked == true)
            {
                help_textBox.Visible = false;
            }
            else
            {
                help_textBox.Visible = true;
       
            }
            #endregion
        }


        private void ourSubmitQfClear_Btn_Click(object sender, EventArgs e)
        {
            #region Qf Clear
            selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Replace("{{", "");
            selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Replace("}}", "");

            ourSubmitQf_textBox.Text = "";
            #endregion
        }

        private void ourSubmitLatClear_Btn_Click(object sender, EventArgs e)
        {
            #region LAT Clear
            selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Replace("[[", "");
            selectedQuestion_TextBox.Text = selectedQuestion_TextBox.Text.Replace("]]", "");


            ourSubmitLat_textBox.Text = "";
            #endregion
        }

        private void 작업모드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region 작업모드 클릭
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt"; ;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                work_Panel.Enabled = true;
                작업모드ToolStripMenuItem.Checked = true;
                크로스체크모드ToolStripMenuItem.Checked = false;
                work_Panel.Visible = true;
                cross_Panel.Visible = false;

                allData_ListBox.Items.Clear();
                path = openFileDialog.FileName;
                FileStream fs_read = new FileStream(path, FileMode.Open, FileAccess.Read);
                StreamReader stream_read = new StreamReader(fs_read, System.Text.Encoding.UTF8);


                String mod = stream_read.ReadToEnd();
                workText = mod;

                stream_read.Close();
                fs_read.Close();

                try
                {
                    workReadParser(mod);
                    label12.Text = questionCount.ToString();
                  
                    //현재 체크
                    allData_ListBox.SelectedIndex = currentReadQuestion;
                    allData_ListBox.Select();
                }
                catch
                {

                }

            }
            #endregion
        }

        private void questionType_Label_Click(object sender, EventArgs e)
        {
            #region QT 라벨 클릭
            label2.Text = "questionType 도움말";
            string help = "정의 : 질문의 분류유형을 기재 \r\n \r\n기본적인 분류 유형은 단답형, 서술형, 나열형\r\n \r\n질문만 보고 판단하지 않고 '정답'을 기준으로 판단한다.\r\n \r\n1.단답형 : 단답형식의 명사(구)<술어가 포함되어 있지 않는 명사구>, 개체명 또는 어휘로 정답을 제시해야 하는 경우\r\n \r\n2. 나열형 : 정답이 1개 이상인 형태의 단답형 질문\r\n \r\n3.서술형 : 주관식 문장이나 개조식으로 정답을 제시하는 경우\r\n\r\n주의) 서술형의 경우 정의, 이유, 방법, 목적, 조건, 기타로 세부분류하여 기입.";
            help_textBox.Text = help;
            #endregion
        }

        private void questionFocus_Label_Click(object sender, EventArgs e)
        {
            #region QF 라벨 클릭
            label2.Text = "질문 초점 도움말";
            string help = "정의 : 질문에서 정답후보가 대치될 수 있는 위치 \r\n \r\n'지시대명사', '지시대명사 + (복합)명사', '의문사'로 구성\r\n \r\n 1. 정답을 지칭하는 지시대명사와 함께 쓰인 (복합)명사 -> 지시대명사+(복합)명사 가 질문초점임\r\n \r\n 2. 정답을 지칭하는 지시대명사 ‘이것’\r\n \r\n 3. 질문 내에 포함되어 있는 의문사\r\n \r\n ex)누구, 어떤, 무엇, 몇, 무슨, 어느, 어디, 언제, 얼마나, 얼마\r\n \r\n ‘몇’, ‘어떤’, ‘무슨’, ‘어느’는 뒤의 명사와 함께 질문 초점이 됨.";
            help_textBox.Text = help;
            #endregion
        }

        private void questionLat_Label_Click(object sender, EventArgs e)
        {
            #region LAT 라벨 클릭
            label2.Text = "LAT도움말";
            string help = "정의 : 질문 내에서 정답의 유형을 제약하는 단어절의 (복합)명사 \r\n \r\n복수개일 수 있다.\r\n \r\nex) 질문: 종교 전쟁을 끝낸 협정은?   답: 낭트칙령\r\n => LAT: 협정 <=";
            help_textBox.Text = help;
            #endregion
        }

        private void questionSat_Label_Click(object sender, EventArgs e)
        {
            #region SAT 라벨 클릭
            label2.Text = "SAT도움말";
            string help = "정의 : 질문에서 요구하는 정답의 의미적 유형 \r\n \r\n의미정답유형은 질문만 보고 판단하지 않고, 정답의 형태를 기준으로 판단한다\r\n \r\nEtri 개체명 가이드라인 참고\r\n \r\nex) 질문: 종교 전쟁을 끝낸 협정은?   답: 낭트칙령\r\n => SAT: EV_ACTIVITY <=\r\n \r\n";
            help_textBox.Text = help;
            #endregion
        }

        
        private void OfnextHighlight_Btn(object sender, EventArgs e)
        {
            #region Qt 이동버튼
            QfNextHighlight();
            #endregion
        }

        private void QfNextHighlight()
        {
            #region Qt 이동버튼 함수
            try
            {
                string word = ourSubmitQf_textBox.Text;

                int startIndex = selectedQuestion_TextBox.Text.IndexOf(word, qtStartIndex);
                int length = word.Length;

                selectedQuestion_TextBox.SelectionStart = startIndex;
                selectedQuestion_TextBox.SelectionLength = length;

                qtStartIndex = startIndex + length;
            }
            catch
            {
                string word = ourSubmitQf_textBox.Text;

                qtStartIndex = 0;

                int startIndex = selectedQuestion_TextBox.Text.IndexOf(word, qtStartIndex);
                int length = word.Length;

                selectedQuestion_TextBox.SelectionStart = startIndex;
                selectedQuestion_TextBox.SelectionLength = length;

                qtStartIndex = startIndex + length;
            }
            #endregion

        }

        private void LatnextHighlight_Btn(object sender, EventArgs e)
        {
            #region LAT 이동버튼
            LatNextHighlight();
            #endregion
        }

        private void LatNextHighlight()
        {
            #region LAT 이동버튼 함수
            try
            {
                string word = ourSubmitLat_textBox.Text;

                int startIndex = selectedQuestion_TextBox.Text.IndexOf(word, latStartIndex);
                int length = word.Length;

                selectedQuestion_TextBox.SelectionStart = startIndex;
                selectedQuestion_TextBox.SelectionLength = length;

                latStartIndex = startIndex + length;
            }
            catch
            {
                string word = ourSubmitLat_textBox.Text;

                latStartIndex = 0;

                int startIndex = selectedQuestion_TextBox.Text.IndexOf(word, latStartIndex);
                int length = word.Length;

                selectedQuestion_TextBox.SelectionStart = startIndex;
                selectedQuestion_TextBox.SelectionLength = length;

                latStartIndex = startIndex + length;
            }
            #endregion
        }

        private void questionMove_Btn_Click(object sender, EventArgs e)
        {
            #region 질문 이동 버튼
            try
            {
                int temp = Convert.ToInt32(questionMove_TextBox.Text);
                allData_ListBox.SelectedIndex = temp - 1;
                allData_ListBox.Select();

                questionMove_TextBox.Text = "";
            }
            catch
            {
                MessageBox.Show("유효한 질문 번호가 아닙니다.");
                questionMove_TextBox.Text = "";
            }
            #endregion
        }


        private void EtriApi_Btn_Click(object sender, EventArgs e)
        {
            #region EtriOpenApi 확인 버튼
            string question = null;
            string answer = null;

            if (ourSubmitQt_textBox.Text.Length > 0 && ourSubmitSat_textBox.Text.Length > 0)
            {
                question = Question_LIst[allData_ListBox.SelectedIndex].ToString();
                answer = Answer_List[allData_ListBox.SelectedIndex].ToString();
            }
            else
            {
                MessageBox.Show("정답을 먼저 입력하세요");
                return;
            }
            try
            {
                //질문초점,LAT
                obj_EtriOpenApi = new EtriOpenApi();
                obj_EtriOpenApi.setQuestion(question);
                obj_EtriOpenApi.useApi();

                etriQf_textBox.Text = obj_EtriOpenApi.getQuestionFocus();
                etriLat_textBox.Text = obj_EtriOpenApi.getLat();

            }
            catch
            {
                MessageBox.Show("다시 시도해 주세요");
            }
            #endregion
        }


        private void satSubCategory_listBox_MouseClick(object sender, MouseEventArgs e)
        {
            #region SAT 이벤트
            if (ourSubmitQt_textBox.Text.Contains("서술형"))
            {
                MessageBox.Show("서술형일 경우 ETC 입니다.");
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    ourSubmitSat_textBox.Text = " ";
                    ourSubmitSat_textBox.Text = Dic_All[satSubCategory_listBox.Text].ToString();

                    help_textBox.Text = " ";
                    help_textBox.Text = Dic_Help[satSubCategory_listBox.Text].ToString();
                }
                catch { }
            }
            #endregion

        }

        private void satSubCategory_listBox_MouseUp(object sender, MouseEventArgs e)
        {
            #region SAT 이벤트
            if (e.Button == MouseButtons.Right)
            {
                if (ourSubmitSat_textBox.Text == "")
                {
                    MessageBox.Show("다시 선택해주세요");
                    return;
                }
                try
                {
                    int index = this.satSubCategory_listBox.IndexFromPoint(e.Location);
                    string text = satSubCategory_listBox.Items[index].ToString();
                    satSubCategory_listBox.SelectedIndex = index;
                    satSubCategory_listBox.Select();

                    ourSubmitSat_textBox.AppendText(":" + Dic_All[text].ToString());
                }
                catch
                { }
            }
            #endregion
        }





        /*-----------------------------------------cross_Panel------------------------------------------*/


        private void 크로스체크ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region 크로스체크 클릭
            //저장할 파일
            OpenFileDialog openFileDialog_final = new OpenFileDialog();
            openFileDialog_final.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
            openFileDialog_final.RestoreDirectory = true;
            openFileDialog_final.Title = "CrossJson 파일 선택";

            cross_allData_ListBox.Items.Clear();

            //저장할 파일 OK
            if (openFileDialog_final.ShowDialog() == DialogResult.OK)
            {
                crossFinal_path = openFileDialog_final.FileName;
                FileStream fs_read_final = new FileStream(crossFinal_path, FileMode.Open, FileAccess.Read);
                StreamReader stream_read_final = new StreamReader(crossFinal_path, System.Text.Encoding.UTF8);

                String mod = stream_read_final.ReadToEnd();
                fs_read_final.Close();
                stream_read_final.Close();


                if (!crossCheckParser(mod))//check없으면 새로 써야함
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();

                    openFileDialog.Multiselect = true;
                    openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.Title = "WorkJson파일 2개 선택";

                    //두 파일 선택 후 OK
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {

                        if (openFileDialog.FileNames.Count() != 2)
                        {
                            MessageBox.Show("2개 선택해 주세요");
                            return;
                        }
                        else
                        {
                            cross1_path = openFileDialog.FileNames[0];
                            firstFileName = openFileDialog.SafeFileNames[0].Substring(0, openFileDialog.SafeFileNames[0].Length - 5);
                            FileStream fs_read1 = new FileStream(cross1_path, FileMode.Open, FileAccess.Read);
                            StreamReader stream_read1 = new StreamReader(fs_read1, System.Text.Encoding.UTF8);

                            String mod1 = stream_read1.ReadToEnd();
                            fs_read1.Close();
                            stream_read1.Close();

                            resetArrayList();
                            workReadParser(mod1, Check1_Question_Answer, Check1_Question_Tagged, Check1_QuestionType, Check1_QuestionFocus, Check1_QuestionLat, Check1_QuestionSat, Check1_ConfuseQt, Check1_ConfuseQf, Check1_ConfuseLat, Check1_ConfuseSat);


                            cross2_path = openFileDialog.FileNames[1];
                            secondFileName = openFileDialog.SafeFileNames[1].Substring(0, openFileDialog.SafeFileNames[1].Length - 5);
                            FileStream fs_read2 = new FileStream(cross2_path, FileMode.Open, FileAccess.Read);
                            StreamReader stream_read2 = new StreamReader(fs_read2, System.Text.Encoding.UTF8);

                            mod1 = stream_read2.ReadToEnd();
                            fs_read2.Close();
                            stream_read2.Close();

                            workReadParser(mod1, Check2_Question_Answer, Check2_Question_Tagged, Check2_QuestionType, Check2_QuestionFocus, Check2_QuestionLat, Check2_QuestionSat, Check2_ConfuseQt, Check2_ConfuseQf, Check2_ConfuseLat, Check2_ConfuseSat);

                            putSameData();
                            //앞의 저장할 파일에 새로 쓰기 1 2 저장 3은 나중에 쓸거
                            string writeText = crossWriteParser(mod1);
                            //crossText = mod1;

                            StreamWriter stream_write2 = new StreamWriter(crossFinal_path, false, System.Text.Encoding.UTF8);//true:이어쓰기 false:덮어쓰기
                            stream_write2.Write(writeText);
                            stream_write2.Close();

                        }
                        if (openFileDialog.FileNames.Count() == 2)
                        {
                            if (Check1_Question_Answer.Count != Check2_Question_Answer.Count)//두 파일을 읽었는데 질문수가 다르면
                            {
                                MessageBox.Show("파일의 질문수가 다릅니다");
                                resetArrayList();
                                return;
                            }

                        }

                    }//두 파일 선택후 ok

                    FileStream fs_read_final2 = new FileStream(crossFinal_path, FileMode.Open, FileAccess.Read);
                    StreamReader stream_read_final2 = new StreamReader(crossFinal_path, System.Text.Encoding.UTF8);

                    mod = stream_read_final2.ReadToEnd();
                    fs_read_final2.Close();
                    stream_read_final2.Close();
                }

                work_Panel.Visible = false;
                cross_Panel.Visible = true;
                listBox2.Visible = false;

                작업모드ToolStripMenuItem.Checked = false;
                크로스체크모드ToolStripMenuItem.Checked = true;

                crossText = mod;
                crossReadParser(mod);

                int a = openFileDialog_final.FileName.LastIndexOf("\\");
                int b = openFileDialog_final.FileName.LastIndexOf(".");

                cross_firstFileName_TextBox.Text = firstFileName;
                cross_secondFileName_TextBox.Text = secondFileName;
                cross_saveFileName_TextBox.Text = openFileDialog_final.FileName.Substring(a + 1, b - a - 1);

                cross_index = 0;
                selected_list_cross();
                Cross_Label_현재.Text = cross_index + 1 + "";
                Cross_Label_합계.Text = cross_allData_ListBox.Items.Count + "";

            }//저장할 파일 ok
            else
            {
                MessageBox.Show("저장할 텍스트를 선택하세요");
                return;
            }

            #endregion

        }

        private void crossReadParser(string text)
        {
            #region 크로스체크 읽기
            resetArrayList();

            JObject obj = JObject.Parse(text);
            JArray array = JArray.Parse(obj["data"].ToString());

            firstFileName = obj["firstfile"].ToString();
            secondFileName = obj["secondfile"].ToString();

            string qa, aw;
            string a1, a2, a3, a4, a5;
            string b1, b2, b3, b4, b5;
            string c1, c2, c3, c4, c5;

            bool aa1, aa2, aa3, aa4;
            bool bb1, bb2, bb3, bb4;
            bool cc1, cc2, cc3, cc4;


            foreach (JObject itemObj in array)
            {

                JArray ooo = JArray.Parse(itemObj["paragraphs"].ToString());


                foreach (JObject itemObj2 in ooo)
                {

                    JArray iii = JArray.Parse(itemObj2["qas"].ToString());

                    foreach (JObject itemObj3 in iii)
                    {

                        qa = itemObj3["question"].ToString();

                        a1 = itemObj3["question_tagged1"].ToString();
                        a2 = itemObj3["questionType1"].ToString();
                        a3 = itemObj3["questionFocus1"].ToString();
                        a4 = itemObj3["questionLAT1"].ToString();
                        a5 = itemObj3["questionSAT1"].ToString();

                        b1 = itemObj3["question_tagged2"].ToString();
                        b2 = itemObj3["questionType2"].ToString();
                        b3 = itemObj3["questionFocus2"].ToString();
                        b4 = itemObj3["questionLAT2"].ToString();
                        b5 = itemObj3["questionSAT2"].ToString();

                        c1 = itemObj3["question_tagged3"].ToString();
                        c2 = itemObj3["questionType3"].ToString();
                        c3 = itemObj3["questionFocus3"].ToString();
                        c4 = itemObj3["questionLAT3"].ToString();
                        c5 = itemObj3["questionSAT3"].ToString();

                        aa1 = Convert.ToBoolean(itemObj3["confuseQt1"]);
                        aa2 = Convert.ToBoolean(itemObj3["confuseQf1"]);
                        aa3 = Convert.ToBoolean(itemObj3["confuseLat1"]);
                        aa4 = Convert.ToBoolean(itemObj3["confuseSat1"]);

                        bb1 = Convert.ToBoolean(itemObj3["confuseQt2"]);
                        bb2 = Convert.ToBoolean(itemObj3["confuseQf2"]);
                        bb3 = Convert.ToBoolean(itemObj3["confuseLat2"]);
                        bb4 = Convert.ToBoolean(itemObj3["confuseSat2"]);

                        cc1 = Convert.ToBoolean(itemObj3["confuseQt3"]);
                        cc2 = Convert.ToBoolean(itemObj3["confuseQf3"]);
                        cc3 = Convert.ToBoolean(itemObj3["confuseLat3"]);
                        cc4 = Convert.ToBoolean(itemObj3["confuseSat3"]);

                        Check1_Question.Add(qa);

                        Check1_Question_Tagged.Add(a1);
                        Check1_QuestionType.Add(a2);
                        Check1_QuestionFocus.Add(a3);
                        Check1_QuestionLat.Add(a4);
                        Check1_QuestionSat.Add(a5);

                        Check2_Question_Tagged.Add(b1);
                        Check2_QuestionType.Add(b2);
                        Check2_QuestionFocus.Add(b3);
                        Check2_QuestionLat.Add(b4);
                        Check2_QuestionSat.Add(b5);

                        CheckEnd_Question_Tagged.Add(c1);
                        CheckEnd_QuestionType.Add(c2);
                        CheckEnd_QuestionFocus.Add(c3);
                        CheckEnd_QuestionLat.Add(c4);
                        CheckEnd_QuestionSat.Add(c5);

                        Check1_ConfuseQt.Add(aa1);
                        Check1_ConfuseQf.Add(aa2);
                        Check1_ConfuseLat.Add(aa3);
                        Check1_ConfuseSat.Add(aa4);

                        Check2_ConfuseQt.Add(bb1);
                        Check2_ConfuseQf.Add(bb2);
                        Check2_ConfuseLat.Add(bb3);
                        Check2_ConfuseSat.Add(bb4);

                        CheckEnd_ConfuseQt.Add(cc1);
                        CheckEnd_ConfuseQf.Add(cc2);
                        CheckEnd_ConfuseLat.Add(cc3);
                        CheckEnd_ConfuseSat.Add(cc4);


                        JArray yyy = JArray.Parse(itemObj3["answers"].ToString());

                        foreach (JObject itemObj4 in yyy)
                        {
                            aw = itemObj4["text"].ToString();

                            Check1_Question_Answer.Add(qa + " --- " + aw);

                            cross_allData_ListBox.Items.Add(qa + " --- " + aw);

                        }
                    }
                }
            }
            #endregion
        }

        private string crossWriteParser(string text)
        {   
            #region 크로스체크 쓰기
            JObject obj = JObject.Parse(text);
            JArray array = JArray.Parse(obj["data"].ToString());

            CrossFormatt.RootObject r = new CrossFormatt.RootObject();
            CrossFormatt.Datum d;
            r.data = new List<CrossFormatt.Datum>();

            CrossFormatt.Paragraph p;
            CrossFormatt.Qa q;
            CrossFormatt.Answer j;

            r.version = obj["version"].ToString();
            r.creator = obj["creator"].ToString();
            r.formatt = "Cross";
            r.check = true;
            r.firstfile = firstFileName;
            r.secondfile = secondFileName;

            int i = 0;
            
            foreach (JObject itemObj in array)
            {
                d = new CrossFormatt.Datum();
                d.paragraphs = new List<EtriWork.CrossFormatt.Paragraph>();

                d.title = itemObj["title"].ToString();

                r.data.Add(d);

                JArray ooo = JArray.Parse(itemObj["paragraphs"].ToString());


                foreach (JObject itemObj2 in ooo)
                {
                    p = new CrossFormatt.Paragraph();
                    p.qas = new List<EtriWork.CrossFormatt.Qa>();

                    p.context = itemObj2["context"].ToString();
                    p.context_en = itemObj2["context_en"].ToString();
                    p.context_tagged = itemObj2["context_tagged"].ToString();

                    d.paragraphs.Add(p);

                    JArray iii = JArray.Parse(itemObj2["qas"].ToString());

                    foreach (JObject itemObj3 in iii)
                    {

                        q = new CrossFormatt.Qa();
                        q.answers = new List<EtriWork.CrossFormatt.Answer>();

                        q.questionType1 = Check1_QuestionType[i].ToString();
                        q.questionFocus1 = Check1_QuestionFocus[i].ToString();
                        q.questionSAT1 = Check1_QuestionSat[i].ToString();
                        q.questionLAT1 = Check1_QuestionLat[i].ToString();
                        q.question_tagged1 = Check1_Question_Tagged[i].ToString();
                        q.confuseQt1 = Convert.ToBoolean(Check1_ConfuseQt[i]);
                        q.confuseQf1 = Convert.ToBoolean(Check1_ConfuseQf[i]);
                        q.confuseLat1 = Convert.ToBoolean(Check1_ConfuseLat[i]);
                        q.confuseSat1 = Convert.ToBoolean(Check1_ConfuseSat[i]);

                        q.questionType2 = Check2_QuestionType[i].ToString();
                        q.questionFocus2 = Check2_QuestionFocus[i].ToString();
                        q.questionSAT2 = Check2_QuestionSat[i].ToString();
                        q.questionLAT2 = Check2_QuestionLat[i].ToString();
                        q.question_tagged2 = Check2_Question_Tagged[i].ToString();
                        q.confuseQt2 = Convert.ToBoolean(Check2_ConfuseQt[i]);
                        q.confuseQf2 = Convert.ToBoolean(Check2_ConfuseQf[i]);
                        q.confuseLat2 = Convert.ToBoolean(Check2_ConfuseLat[i]);
                        q.confuseSat2 = Convert.ToBoolean(Check2_ConfuseSat[i]);

                        q.questionType3 = CheckEnd_QuestionType[i].ToString();
                        q.questionFocus3 = CheckEnd_QuestionFocus[i].ToString();
                        q.questionSAT3 = CheckEnd_QuestionSat[i].ToString();
                        q.questionLAT3 = CheckEnd_QuestionLat[i].ToString();
                        q.question_tagged3 = CheckEnd_Question_Tagged[i].ToString();

                        q.id = itemObj3["id"].ToString();
                        q.question = itemObj3["question"].ToString();
                        q.question_en = itemObj3["question_en"].ToString();

                        p.qas.Add(q);

                        JArray yyy = JArray.Parse(itemObj3["answers"].ToString());

                        foreach (JObject itemObj4 in yyy)
                        {
                            j = new CrossFormatt.Answer();
                            j.text = itemObj4["text"].ToString();
                            j.text_en = itemObj4["text_en"].ToString();
                            j.text_tagged = itemObj4["text_tagged"].ToString();
                            j.text_syn = itemObj4["text_syn"].ToString();
                            j.answer_start = Convert.ToInt32(itemObj4["answer_start"]);
                            j.answer_end = Convert.ToInt32(itemObj4["answer_end"]);

                            q.answers.Add(j);

                        }

                        i++;
                    }
                }
            }

            string crossjson = JsonConvert.SerializeObject(r, Formatting.Indented);
            return crossjson;
            #endregion
        }

        private bool crossCheckParser(string text)
        {
            #region 크로스파일 여부 체크
            try
            {
                JObject obj = JObject.Parse(text);
                JArray array = JArray.Parse(obj["data"].ToString());
                string c;
                c = obj["check"].ToString();
                return true;
            }
            catch
            {
                return false;
            }
            #endregion
        }


        private void workReadParser(string text, ArrayList qa, ArrayList tag, ArrayList qt, ArrayList qf, ArrayList lat, ArrayList sat, List<bool> checkQt, List<bool> checkQf, List<bool> checkLat, List<bool> checkSat)
        {
            #region 크로스 체크 모드에서 Work파일 읽기
            JObject obj = JObject.Parse(text);
            JArray array = JArray.Parse(obj["data"].ToString());

            string c;
            try
            {
                c = obj["formatt"].ToString();
            }
            catch
            {
                MessageBox.Show("Json파일 형식이 맞지 않습니다.");
                return;
            }

            string b;
            string a;

            string d, e, f, g, h;
            bool aa, bb, cc, dd;

            foreach (JObject itemObj in array)
            {
                JArray ooo = JArray.Parse(itemObj["paragraphs"].ToString());

                foreach (JObject itemObj2 in ooo)
                {
                    JArray iii = JArray.Parse(itemObj2["qas"].ToString());

                    foreach (JObject itemObj3 in iii)
                    {
                        b = itemObj3["question"].ToString();
                        d = itemObj3["questionType"].ToString();
                        e = itemObj3["questionFocus"].ToString();
                        f = itemObj3["questionSAT"].ToString();
                        g = itemObj3["questionLAT"].ToString();
                        h = itemObj3["question_tagged"].ToString();

                        aa = Convert.ToBoolean(itemObj3["confuseQt"]);
                        bb = Convert.ToBoolean(itemObj3["confuseQf"]);
                        cc = Convert.ToBoolean(itemObj3["confuseLat"]);
                        dd = Convert.ToBoolean(itemObj3["confuseSat"]);

                        qt.Add(d);
                        qf.Add(e);
                        lat.Add(g);
                        sat.Add(f);
                        tag.Add(h);

                        checkQt.Add(aa);
                        checkQf.Add(bb);
                        checkLat.Add(cc);
                        checkSat.Add(dd);

                        JArray yyy = JArray.Parse(itemObj3["answers"].ToString());

                        foreach (JObject itemObj4 in yyy)
                        {
                            a = itemObj4["text"].ToString();

                            qa.Add(b + " --- " + a);

                        }
                    }
                }
            }
            #endregion
        }


        private void resetArrayList()
        {
            #region Cross_Panel ArrayList Reset

            Check1_Question = new ArrayList();
            Check1_Question.Clear();
            Check2_Question = new ArrayList();
            Check2_Question.Clear();

            Check1_Question_Answer = new ArrayList();
            Check2_Question_Answer = new ArrayList();
            Check1_Question_Answer.Clear();
            Check2_Question_Answer.Clear();

            Check1_Question_Tagged = new ArrayList();
            Check2_Question_Tagged = new ArrayList();
            Check1_Question_Tagged.Clear();
            Check2_Question_Tagged.Clear();
            CheckEnd_Question_Tagged = new ArrayList();
            CheckEnd_Question_Tagged.Clear();

            Check1_QuestionType = new ArrayList();
            Check2_QuestionType = new ArrayList();
            Check1_QuestionType.Clear();
            Check2_QuestionType.Clear();
            CheckEnd_QuestionType = new ArrayList();
            CheckEnd_QuestionType.Clear();

            Check1_ConfuseQt = new List<bool>();
            Check2_ConfuseQt = new List<bool>();
            CheckEnd_ConfuseQt = new List<bool>();
            Check1_ConfuseQt.Clear();
            Check2_ConfuseQt.Clear();
            CheckEnd_ConfuseQt.Clear();

            Check1_QuestionFocus = new ArrayList();
            Check2_QuestionFocus = new ArrayList();
            Check1_QuestionFocus.Clear();
            Check2_QuestionFocus.Clear();
            CheckEnd_QuestionFocus = new ArrayList();
            CheckEnd_QuestionFocus.Clear();

            Check1_ConfuseQf = new List<bool>();
            Check2_ConfuseQf = new List<bool>();
            CheckEnd_ConfuseQf = new List<bool>();
            Check1_ConfuseQf.Clear();
            Check2_ConfuseQf.Clear();
            CheckEnd_ConfuseQf.Clear();


            Check1_QuestionLat = new ArrayList();
            Check2_QuestionLat = new ArrayList();
            Check1_QuestionLat.Clear();
            Check2_QuestionLat.Clear();
            CheckEnd_QuestionLat = new ArrayList();
            CheckEnd_QuestionLat.Clear();

            Check1_ConfuseLat = new List<bool>();
            Check2_ConfuseLat = new List<bool>();
            CheckEnd_ConfuseLat = new List<bool>();
            Check1_ConfuseLat.Clear();
            Check2_ConfuseLat.Clear();
            CheckEnd_ConfuseLat.Clear();


            Check1_QuestionSat = new ArrayList();
            Check2_QuestionSat = new ArrayList();
            Check1_QuestionSat.Clear();
            Check2_QuestionSat.Clear();
            CheckEnd_QuestionSat = new ArrayList();
            CheckEnd_QuestionSat.Clear();

            Check1_ConfuseSat = new List<bool>();
            Check2_ConfuseSat = new List<bool>();
            CheckEnd_ConfuseSat = new List<bool>();
            Check1_ConfuseSat.Clear();
            Check2_ConfuseSat.Clear();
            CheckEnd_ConfuseSat.Clear();

            #endregion
        }

        private void putSameData()
        {
            #region 두 파일중 같은데이터를 checkEnd에 집어넣음

            for (int i = 0; i < Check1_Question_Answer.Count; i++)
            {
                CheckEnd_Question_Tagged.Add("");
                CheckEnd_QuestionType.Add("");
                CheckEnd_QuestionFocus.Add("");
                CheckEnd_QuestionLat.Add("");
                CheckEnd_QuestionSat.Add("");

                //질문 초점과 LAT의 경우 위치를 따져야 한다.
                if (Check1_Question_Tagged[i].ToString() == Check2_Question_Tagged[i].ToString())
                {
                    CheckEnd_Question_Tagged[i] = Check1_Question_Tagged[i].ToString();
                }


                if (Check1_QuestionType[i].ToString() == Check2_QuestionType[i].ToString())
                    CheckEnd_QuestionType[i] = Check1_QuestionType[i].ToString();

                //둘중 하나만 같을 때 태깅 처리
                if (Check1_QuestionFocus[i].ToString() == Check2_QuestionFocus[i].ToString())
                {
                    CheckEnd_QuestionFocus[i] = Check1_QuestionFocus[i].ToString();

                    if (Check1_Question_Tagged[i].ToString() != Check2_Question_Tagged[i].ToString())
                    {
                        //함수
                        CheckEnd_Question_Tagged[i] = setQustionFocusTag(Check1_Question_Tagged[i].ToString(), Check2_Question_Tagged[i].ToString(), CheckEnd_QuestionFocus[i].ToString());

                    }
                }

                //둘중 하나만 같을 때 태깅 처리
                if (Check1_QuestionLat[i].ToString() == Check2_QuestionLat[i].ToString())
                {
                    CheckEnd_QuestionLat[i] = Check1_QuestionLat[i].ToString();

                    if (Check1_Question_Tagged[i].ToString() != Check2_Question_Tagged[i].ToString())
                    {
                        //함수
                        CheckEnd_Question_Tagged[i] = setQustionLatTag(Check1_Question_Tagged[i].ToString(), Check2_Question_Tagged[i].ToString(), CheckEnd_QuestionLat[i].ToString());

                    }
                }

                if (Check1_QuestionSat[i].ToString() == Check2_QuestionSat[i].ToString())
                    CheckEnd_QuestionSat[i] = Check1_QuestionSat[i].ToString();
            }

            #endregion
        }

        private string setQustionFocusTag(string arr1, string arr2, string text)
        {
            #region 질문초점 태그 넣기
            string checkEnd_TagText = "";
            int arr1_Start = 0;
            int arr2_Start = 0;
            try
            {
                //있으면 추가
                arr1 = arr1.Replace("[", "");
                arr1 = arr1.Replace("]", "");
                arr2 = arr2.Replace("[", "");
                arr2 = arr2.Replace("]", "");

                //둘다 {를 포함하고 있지 않다면 즉, 같은데 공백으로 같은 경우
                if (!arr1.Contains("{") || !arr2.Contains("{"))
                {
                    return checkEnd_TagText;//""값 반환
                }

                arr1_Start = arr1.IndexOf('{');
                arr2_Start = arr2.IndexOf('{');

                //각 text의 위치 비교 후 위치까지 맞으면
                if (arr1_Start == arr2_Start)
                {
                    arr1 = arr1.Replace("{", "");
                    arr1 = arr1.Replace("}", "");

                    checkEnd_TagText = arr1;


                    int a = checkEnd_TagText.IndexOf(text);
                    int b = text.Length;

                    checkEnd_TagText = checkEnd_TagText.Insert(a, "{{");
                    checkEnd_TagText = checkEnd_TagText.Insert(a + b + 2, "}}");

                }
            }
            catch { checkEnd_TagText = ""; }


            return checkEnd_TagText;
            #endregion
        }



        private string setQustionLatTag(string arr1, string arr2, string text)
        {
            #region LAT태그 넣기
            string checkEnd_TagText = "";
            int arr1_Start = 0;
            int arr2_Start = 0;

            arr1 = arr1.Replace("{", "");
            arr1 = arr1.Replace("}", "");
            arr2 = arr2.Replace("{", "");
            arr2 = arr2.Replace("}", "");

            //둘다 {를 포함하고 있지 않다면 즉, 같은데 공백으로 같은 경우
            if (!arr1.Contains("[") || !arr2.Contains("["))
            {
                return checkEnd_TagText;//""값 반환
            }

            arr1_Start = arr1.IndexOf('[');
            arr2_Start = arr2.IndexOf('[');

            try
            {
                //각 text의 위치 비교 후 위치까지 맞으면
                if (arr1_Start == arr2_Start)
                {
                    arr1 = arr1.Replace("[", "");
                    arr1 = arr1.Replace("]", "");

                    checkEnd_TagText = arr1;

                    int a = checkEnd_TagText.IndexOf(text);
                    int b = text.Length;

                    checkEnd_TagText = checkEnd_TagText.Insert(a, "[[");
                    checkEnd_TagText = checkEnd_TagText.Insert(a + b + 2, "]]");
                }
            }

            catch { }


            return checkEnd_TagText;
            #endregion
        }


        private void selected_list_cross()
        {
            #region selected_list_cross

            //cross_curWork_TextBox.Text = Check1_Question_Answer[cross_index].ToString();
            cross_allData_ListBox.SelectedIndex = cross_index;
            cross_allData_ListBox.Select();

            cross_firstQuestionTagged_TextBox.Text = Check1_Question_Tagged[cross_index].ToString();
            cross_firstQt_TextBox.Text = Check1_QuestionType[cross_index].ToString();
            cross_firstQf_TextBox.Text = Check1_QuestionFocus[cross_index].ToString();
            cross_firstLat_TextBox.Text = Check1_QuestionLat[cross_index].ToString();
            cross_firstSat_TextBox.Text = Check1_QuestionSat[cross_index].ToString();

            cross_secondQuestionTagged_TextBox.Text = Check2_Question_Tagged[cross_index].ToString();
            cross_secondQt_TextBox.Text = Check2_QuestionType[cross_index].ToString();
            cross_secondQf_TextBox.Text = Check2_QuestionFocus[cross_index].ToString();
            cross_secondLat_TextBox.Text = Check2_QuestionLat[cross_index].ToString();
            cross_secondSat_TextBox.Text = Check2_QuestionSat[cross_index].ToString();

            //EndTagged의 경우
            if (CheckEnd_Question_Tagged[cross_index].ToString() == "")//비었을 경우 질문 set
                cross_saveQuestionTagged_TextBox.Text = Check1_Question[cross_index].ToString();
            else
                cross_saveQuestionTagged_TextBox.Text = CheckEnd_Question_Tagged[cross_index].ToString();

            cross_saveQt_TextBox.Text = CheckEnd_QuestionType[cross_index].ToString();
            cross_saveQf_TextBox.Text = CheckEnd_QuestionFocus[cross_index].ToString();
            cross_saveLat_TextBox.Text = CheckEnd_QuestionLat[cross_index].ToString();
            cross_saveSat_TextBox.Text = CheckEnd_QuestionSat[cross_index].ToString();

            cross_firstQuestionTagged_TextBox.ForeColor = Color.Black;
            cross_secondQuestionTagged_TextBox.ForeColor = Color.Black;
            cross_firstQt_TextBox.ForeColor = Color.Black;
            cross_secondQt_TextBox.ForeColor = Color.Black;
            cross_firstQf_TextBox.ForeColor = Color.Black;
            cross_secondQf_TextBox.ForeColor = Color.Black;
            cross_firstLat_TextBox.ForeColor = Color.Black;
            cross_secondLat_TextBox.ForeColor = Color.Black;
            cross_firstSat_TextBox.ForeColor = Color.Black;
            cross_secondSat_TextBox.ForeColor = Color.Black;

            if (!cross_firstQuestionTagged_TextBox.Text.Equals(cross_secondQuestionTagged_TextBox.Text))
            {
                cross_firstQuestionTagged_TextBox.ForeColor = Color.Red;
                cross_secondQuestionTagged_TextBox.ForeColor = Color.Red;
            }

            if (!cross_firstQt_TextBox.Text.Equals(cross_secondQt_TextBox.Text))
            {
                cross_firstQt_TextBox.ForeColor = Color.Red;
                cross_secondQt_TextBox.ForeColor = Color.Red;
            }

            if (!cross_firstQf_TextBox.Text.Equals(cross_secondQf_TextBox.Text))
            {
                cross_firstQf_TextBox.ForeColor = Color.Red;
                cross_secondQf_TextBox.ForeColor = Color.Red;
            }

            if (!cross_firstLat_TextBox.Text.Equals(cross_secondLat_TextBox.Text))
            {
                cross_firstLat_TextBox.ForeColor = Color.Red;
                cross_secondLat_TextBox.ForeColor = Color.Red;
            }

            if (!cross_firstSat_TextBox.Text.Equals(cross_secondSat_TextBox.Text))
            {
                cross_firstSat_TextBox.ForeColor = Color.Red;
                cross_secondSat_TextBox.ForeColor = Color.Red;
            }


            if (Convert.ToBoolean(Check1_ConfuseQt[cross_index]))
                checkBox5.Checked = true;
            else
                checkBox5.Checked = false;

            if (Convert.ToBoolean(Check1_ConfuseQf[cross_index]))
                checkBox6.Checked = true;
            else
                checkBox6.Checked = false;

            if (Convert.ToBoolean(Check1_ConfuseLat[cross_index]))
                checkBox12.Checked = true;
            else
                checkBox12.Checked = false;

            if (Convert.ToBoolean(Check1_ConfuseSat[cross_index]))
                checkBox11.Checked = true;
            else
                checkBox11.Checked = false;


            if (Convert.ToBoolean(Check2_ConfuseQt[cross_index]))
                checkBox10.Checked = true;
            else
                checkBox10.Checked = false;

            if (Convert.ToBoolean(Check2_ConfuseQf[cross_index]))
                checkBox9.Checked = true;
            else
                checkBox9.Checked = false;

            if (Convert.ToBoolean(Check2_ConfuseLat[cross_index]))
                checkBox8.Checked = true;
            else
                checkBox8.Checked = false;

            if (Convert.ToBoolean(Check2_ConfuseSat[cross_index]))
                checkBox7.Checked = true;
            else
                checkBox7.Checked = false;


            if (Convert.ToBoolean(CheckEnd_ConfuseQt[cross_index]))
                checkBox1.Checked = true;
            else
                checkBox1.Checked = false;

            if (Convert.ToBoolean(CheckEnd_ConfuseQf[cross_index]))
                checkBox2.Checked = true;
            else
                checkBox2.Checked = false;

            if (Convert.ToBoolean(CheckEnd_ConfuseLat[cross_index]))
                checkBox3.Checked = true;
            else
                checkBox3.Checked = false;

            if (Convert.ToBoolean(CheckEnd_ConfuseSat[cross_index]))
                checkBox4.Checked = true;
            else
                checkBox4.Checked = false;

            #endregion
        }


        private void cross_Reset_Btn_Click(object sender, EventArgs e)
        {
            #region Reset버튼 클릭
            cross_saveQuestionTagged_TextBox.Text = "";
            cross_saveQt_TextBox.Text = "";
            cross_saveQf_TextBox.Text = "";
            cross_saveLat_TextBox.Text = "";
            cross_saveSat_TextBox.Text = "";
            #endregion
        }

        private void cross_Move_Btn_Click(object sender, EventArgs e)
        {
            #region 이동버튼
            try
            {
                cross_index = Convert.ToInt32(cross_Move_TextBox.Text) - 1;
                selected_list_cross();
                Cross_Label_현재.Text = cross_index + 1 + "";
                cross_Move_TextBox.Text = "";

                listBox1.SelectedIndex = -1;
                listBox3.SelectedIndex = -1;
                listBox4.SelectedIndex = -1;

            }
            catch
            {
                MessageBox.Show("유효한 질문 번호가 아닙니다.");
                cross_Move_TextBox.Text = "";
            }
            #endregion
        }

        private void cross_MoveTop_Btn_Click(object sender, EventArgs e)
        {
            #region 최상위 이동
            cross_index = 0;
            selected_list_cross();
            Cross_Label_현재.Text = cross_index + 1 + "";

            listBox1.SelectedIndex = -1;
            listBox3.SelectedIndex = -1;
            listBox4.SelectedIndex = -1;
            #endregion
        }

        private void cross_MoveBottom_Btn_Click(object sender, EventArgs e)
        {
            #region 최하위 이동
            cross_index = cross_allData_ListBox.Items.Count - 1;
            selected_list_cross();
            Cross_Label_현재.Text = cross_index + 1 + "";

            listBox1.SelectedIndex = -1;
            listBox3.SelectedIndex = -1;
            listBox4.SelectedIndex = -1;
            #endregion
        }

        private void listBox1_SelectedChanged(object sender, EventArgs e)
        {
            #region 질문유형 분류
            if (listBox1.SelectedIndex != 2)
            {
                cross_saveQt_TextBox.Text = listBox1.Text;
                //listBox2.Visible = false;
            }
            else//서술형
            {
                listBox2.Visible = true;
            }
            #endregion
        }

        private void listBox2_SelectedChanged(object sender, EventArgs e)
        {
            #region 서술형 세분류 변경시
            cross_saveQt_TextBox.Text = "서술형-" + listBox2.Text;
            #endregion
        }

        private void listBox3_SelectedChanged(object sender, EventArgs e)
        {
            #region SAT 변경시
            listBox4.Items.Clear();

            for (int i = 0; i < listBox3.SelectedItems.Count; i++)
            {
                switch (listBox3.SelectedIndices[i])
                {
                    case 0:
                        foreach (KeyValuePair<string, string> de in Dic_Person)
                            listBox4.Items.Add(de.Key);
                        break;
                    case 1:
                        foreach (KeyValuePair<string, string> de in Dic_Location)
                            listBox4.Items.Add(de.Key);
                        break;
                    case 2:
                        foreach (KeyValuePair<string, string> de in Dic_Organization)
                            listBox4.Items.Add(de.Key);
                        break;
                    case 3:
                        foreach (KeyValuePair<string, string> de in Dic_Artifacts)
                            listBox4.Items.Add(de.Key);
                        break;
                    case 4:
                        foreach (KeyValuePair<string, string> de in Dic_Date)
                            listBox4.Items.Add(de.Key);
                        break;
                    case 5:
                        foreach (KeyValuePair<string, string> de in Dic_Time)
                            listBox4.Items.Add(de.Key);
                        break;
                    case 6:
                        foreach (KeyValuePair<string, string> de in Dic_Civilization)
                            listBox4.Items.Add(de.Key);
                        break;
                    case 7:
                        foreach (KeyValuePair<string, string> de in Dic_Animal)
                            listBox4.Items.Add(de.Key);
                        break;
                    case 8:
                        foreach (KeyValuePair<string, string> de in Dic_Plant)
                            listBox4.Items.Add(de.Key);
                        break;
                    case 9:
                        foreach (KeyValuePair<string, string> de in Dic_Quantity)
                            listBox4.Items.Add(de.Key);
                        break;
                    case 10:
                        foreach (KeyValuePair<string, string> de in Dic_StudyField)
                            listBox4.Items.Add(de.Key);
                        break;
                    case 11:
                        foreach (KeyValuePair<string, string> de in Dic_Theory)
                            listBox4.Items.Add(de.Key);
                        break;
                    case 12:
                        foreach (KeyValuePair<string, string> de in Dic_Event)
                            listBox4.Items.Add(de.Key);
                        break;
                    case 13:
                        foreach (KeyValuePair<string, string> de in Dic_Material)
                            listBox4.Items.Add(de.Key);
                        break;
                    case 14:
                        foreach (KeyValuePair<string, string> de in Dic_Term)
                            listBox4.Items.Add(de.Key);
                        break;
                    case 15:
                        foreach (KeyValuePair<string, string> de in Dic_Etc)
                            listBox4.Items.Add(de.Key);
                        break;
                }
            }
            #endregion
        }


        private void cross_firstQuestionTagged_TextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cross_saveQuestionTagged_TextBox.Text = cross_firstQuestionTagged_TextBox.Text;
        }

        private void cross_firstQt_TextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cross_saveQt_TextBox.Text = cross_firstQt_TextBox.Text;
        }

        private void cross_firstQf_TextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cross_saveQf_TextBox.Text = cross_firstQf_TextBox.Text;
            taggingQf();

        }

        private void cross_firstLat_TextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cross_saveLat_TextBox.Text = cross_firstLat_TextBox.Text;
            taggingLat();
        }

        private void cross_firstSat_TextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cross_saveSat_TextBox.Text = cross_firstSat_TextBox.Text;
        }

        private void cross_secondQuestionTagged_TextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cross_saveQuestionTagged_TextBox.Text = cross_secondQuestionTagged_TextBox.Text;
        }

        private void cross_secondQt_TextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cross_saveQt_TextBox.Text = cross_secondQt_TextBox.Text;
        }

        private void cross_secondQf_TextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cross_saveQf_TextBox.Text = cross_secondQf_TextBox.Text;
            taggingQf();
        }

        private void cross_secondLat_TextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cross_saveLat_TextBox.Text = cross_secondLat_TextBox.Text;
            taggingLat();
        }

        private void cross_secondSat_TextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cross_saveSat_TextBox.Text = cross_secondSat_TextBox.Text;
        }

        private void cross_saveNext_Btn_Click(object sender, EventArgs e)//실제 저장 안한다
        {
            #region 적용 후 다음
            if (cross_saveQf_TextBox.Text.Length > 0 && !cross_saveQuestionTagged_TextBox.Text.Contains("{"))
            {
                MessageBox.Show("질문 초점 태깅을 확인해주세요");
                return;
            }

            if (cross_saveQf_TextBox.Text == "" && cross_saveQuestionTagged_TextBox.Text.Contains("{"))
            {
                MessageBox.Show("질문 초점 태깅을 확인해주세요");
                return;
            }

            if (cross_saveLat_TextBox.Text.Length > 0 && !cross_saveQuestionTagged_TextBox.Text.Contains("["))
            {
                MessageBox.Show("LAT 태깅을 확인해주세요");
                return;
            }

            if (cross_saveLat_TextBox.Text == "" && cross_saveQuestionTagged_TextBox.Text.Contains("["))
            {
                MessageBox.Show("LAT 태깅을 확인해주세요");
                return;
            }

            
            if (cross_saveQt_TextBox.Text.Length < 1 || cross_saveSat_TextBox.Text.Length < 1)
            {
                MessageBox.Show("빈 칸을 채워주세요");
                return;
            }

            //적용
            CheckEnd_QuestionType[cross_index] = cross_saveQt_TextBox.Text;
            CheckEnd_QuestionFocus[cross_index] = cross_saveQf_TextBox.Text;
            CheckEnd_QuestionLat[cross_index] = cross_saveLat_TextBox.Text;
            CheckEnd_QuestionSat[cross_index] = cross_saveSat_TextBox.Text;

            if (cross_saveQuestionTagged_TextBox.Text.Contains("{{") || cross_saveQuestionTagged_TextBox.Text.Contains("[["))//둘중 하나 포함되어있으면 set
                CheckEnd_Question_Tagged[cross_index] = cross_saveQuestionTagged_TextBox.Text;
            else
                CheckEnd_Question_Tagged[cross_index] = "";

            if (checkBox1.Checked)
                CheckEnd_ConfuseQt[cross_index] = true;
            else
                CheckEnd_ConfuseQt[cross_index] = false;

            if (checkBox2.Checked)
                CheckEnd_ConfuseQf[cross_index] = true;
            else
                CheckEnd_ConfuseQf[cross_index] = false;

            if (checkBox3.Checked)
                CheckEnd_ConfuseLat[cross_index] = true;
            else
                CheckEnd_ConfuseLat[cross_index] = false;

            if (checkBox4.Checked)
                CheckEnd_ConfuseSat[cross_index] = true;
            else
                CheckEnd_ConfuseSat[cross_index] = false;

            //ListBox 초기화
            listBox1.SelectedIndex = -1;
            listBox3.SelectedIndex = -1;
            listBox4.SelectedIndex = -1;

            //저장 메소드


            if (!Cross_CheckBox_다른것만보기.Checked)
            {
                if (cross_index < cross_allData_ListBox.Items.Count - 1)
                    cross_index++;
                selected_list_cross();
                Cross_Label_현재.Text = cross_index + 1 + "";
            }
            else
            {
                while (cross_index < cross_allData_ListBox.Items.Count - 1)
                {
                    cross_index++;
                    selected_list_cross();
                    Cross_Label_현재.Text = cross_index + 1 + "";

                    if (!(cross_firstQuestionTagged_TextBox.Text.Equals(cross_secondQuestionTagged_TextBox.Text) && cross_firstQt_TextBox.Text.Equals(cross_secondQt_TextBox.Text) && cross_firstLat_TextBox.Text.Equals(cross_secondLat_TextBox.Text) && cross_firstSat_TextBox.Text.Equals(cross_secondSat_TextBox.Text) && cross_firstQf_TextBox.Text.Equals(cross_secondQf_TextBox.Text)))
                        break;
                }
            }
            #endregion
        }

        private void taggingQf()
        {
            #region 질문초점 태깅
            try
            {

                if (cross_saveQf_TextBox.Text.Contains(":"))//구분자 기준으로 자르기
                {
                    string[] result = cross_saveQf_TextBox.Text.Split(new char[] { ':' });

                    for (int i = 0; i < result.Length; i++)  // 배열은 0 부터 저장되며, 배열의 길이만큼 순환
                    {

                        int b = cross_saveQuestionTagged_TextBox.Text.IndexOf(result[i]);//시작
                        int a = result[i].Length;//길이

                        if (cross_saveQuestionTagged_TextBox.Text[b - 1] == '{')
                            continue;
                        // MessageBox.Show("이미 추가되어있습니다");
                        else
                        {
                            cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(b, "{{");
                            cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(b + a + 2, "}}");
                        }

                    }
                }

                else//한개 일때
                {

                    cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Replace("{{", "");
                    cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Replace("}}", "");

                    int b = cross_saveQuestionTagged_TextBox.Text.IndexOf(cross_saveQf_TextBox.Text);//시작

                    int a = cross_saveQf_TextBox.Text.Length;//길이

                    cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(b, "{{");
                    cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(b + a + 2, "}}");
                    
                }

            }
            catch { }
            #endregion
        }

        private void taggingLat()
        {
            #region LAT 태깅
            try
            {

                if (cross_saveLat_TextBox.Text.Contains(":"))//구분자 기준으로 자르기
                {
                    string[] result = cross_saveLat_TextBox.Text.Split(new char[] { ':' });

                    for (int i = 0; i < result.Length; i++)  // 배열은 0 부터 저장되며, 배열의 길이만큼 순환
                    {

                        int b = cross_saveQuestionTagged_TextBox.Text.IndexOf(result[i]);//시작
                        int a = result[i].Length;//길이

                        if (cross_saveQuestionTagged_TextBox.Text[b - 1] == '[')
                            continue;
                        // MessageBox.Show("이미 추가되어있습니다");
                        else
                        {
                            cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(b, "[[");
                            cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(b + a + 2, "]]");
                        }

                    }
                }
                else//한개 일때
                {

                    cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Replace("[[", "");
                    cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Replace("]]", "");

                    int b = cross_saveQuestionTagged_TextBox.Text.IndexOf(cross_saveLat_TextBox.Text);//시작

                    int a = cross_saveLat_TextBox.Text.Length;//길이

                    cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(b, "[[");
                    cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(b + a + 2, "]]");
                }

            }
            catch { }
            #endregion
        }


        private void cross_MoveNext_Btn_Click(object sender, EventArgs e)
        {
            #region 다음질문 이동
            listBox1.SelectedIndex = -1;
            listBox3.SelectedIndex = -1;
            listBox4.SelectedIndex = -1;

            if (!Cross_CheckBox_다른것만보기.Checked)
            {
                if (cross_index < cross_allData_ListBox.Items.Count - 1)
                    cross_index++;
                selected_list_cross();
                Cross_Label_현재.Text = cross_index + 1 + "";
            }
            else
            {
                while (cross_index < cross_allData_ListBox.Items.Count - 1)
                {
                    cross_index++;
                    selected_list_cross();
                    Cross_Label_현재.Text = cross_index + 1 + "";

                    if (!(cross_firstQuestionTagged_TextBox.Text.Equals(cross_secondQuestionTagged_TextBox.Text) && cross_firstQt_TextBox.Text.Equals(cross_secondQt_TextBox.Text) && cross_firstLat_TextBox.Text.Equals(cross_secondLat_TextBox.Text) && cross_firstSat_TextBox.Text.Equals(cross_secondSat_TextBox.Text) && cross_firstQf_TextBox.Text.Equals(cross_secondQf_TextBox.Text)))
                        break;
                }
            }
            #endregion
        }

        private void cross_MoveBefore_Btn_Click(object sender, EventArgs e)
        {
            #region 이전질문 이동
            listBox1.SelectedIndex = -1;
            listBox3.SelectedIndex = -1;
            listBox4.SelectedIndex = -1;

            if (!Cross_CheckBox_다른것만보기.Checked)
            {
                if (cross_index >= 1)
                    cross_index--;
                selected_list_cross();
                Cross_Label_현재.Text = cross_index + 1 + "";
            }
            else
            {
                while (cross_index >= 1)
                {
                    cross_index--;
                    selected_list_cross();
                    Cross_Label_현재.Text = cross_index + 1 + "";

                    if (!(cross_firstQuestionTagged_TextBox.Text.Equals(cross_secondQuestionTagged_TextBox.Text) && cross_firstQt_TextBox.Text.Equals(cross_secondQt_TextBox.Text) && cross_firstLat_TextBox.Text.Equals(cross_secondLat_TextBox.Text) && cross_firstSat_TextBox.Text.Equals(cross_secondSat_TextBox.Text) && cross_firstQf_TextBox.Text.Equals(cross_secondQf_TextBox.Text)))
                        break;
                }
            }
            #endregion
        }

        private void cross_saveManual_Btn_Click(object sender, EventArgs e)
        {
            #region 파일 수동저장
            try
            {
                string saveText = crossWriteParser(crossText);
                StreamWriter stream_write3 = new StreamWriter(crossFinal_path, false, System.Text.Encoding.UTF8);//true:이어쓰기 false:덮어쓰기
                stream_write3.Write(saveText);
                stream_write3.Close();

                MessageBox.Show("파일이 수동 저장 되었습니다");
            }
            catch
            {
                MessageBox.Show("파일저장에 실패하였습니다");
            }
            #endregion
        }

        private void cross_Qf_Btn_Click(object sender, EventArgs e)
        {
            #region 질문초점 적용
            if (cross_saveQf_TextBox.Text == "")
            {
                cross_saveQf_TextBox.Text = cross_saveQuestionTagged_TextBox.SelectedText;

                int start = cross_saveQuestionTagged_TextBox.SelectionStart;
                int length = cross_saveQuestionTagged_TextBox.SelectionLength;

                cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(start, "{{");
                cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(start + 2 + length, "}}");

            }
            else //무언가 있을 때
            {
                cross_saveQf_TextBox.AppendText(":" + cross_saveQuestionTagged_TextBox.SelectedText);

                int start = cross_saveQuestionTagged_TextBox.SelectionStart;
                int length = cross_saveQuestionTagged_TextBox.SelectionLength;

                cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(start, "{{");
                cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(start + 2 + length, "}}");

            }
            #endregion
        }

        private void cross_Lat_Btn_Click(object sender, EventArgs e)
        {
            #region LAT 적용
            if (cross_saveLat_TextBox.Text == "")
            {
                cross_saveLat_TextBox.Text = cross_saveQuestionTagged_TextBox.SelectedText;

                cross_saveLat_TextBox.Text = cross_saveQuestionTagged_TextBox.SelectedText;

                int start = cross_saveQuestionTagged_TextBox.SelectionStart;
                int length = cross_saveQuestionTagged_TextBox.SelectionLength;

                cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(start, "[[");
                cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(start + 2 + length, "]]");
            }
            else //무언가 있을 때
            {

                cross_saveLat_TextBox.AppendText(":" + cross_saveQuestionTagged_TextBox.SelectedText);

                int start = cross_saveQuestionTagged_TextBox.SelectionStart;
                int length = cross_saveQuestionTagged_TextBox.SelectionLength;

                cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(start, "[[");
                cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Insert(start + 2 + length, "]]");

            }
            #endregion
        }

        private void cross_QfCLR_Btn_Click(object sender, EventArgs e)
        {
            #region 질문초점 Clear
            cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Replace("{{", "");
            cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Replace("}}", "");

            cross_saveQf_TextBox.Text = "";
            #endregion
        }

        private void cross_LatCLR_Btn_Click(object sender, EventArgs e)
        {
            #region LAT Clear
            cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Replace("[[", "");
            cross_saveQuestionTagged_TextBox.Text = cross_saveQuestionTagged_TextBox.Text.Replace("]]", "");

            cross_saveLat_TextBox.Text = "";
            #endregion
        }

        private void cross_allData_ListBox_Changed(object sender, EventArgs e)
        {
            #region Cross_전체 데이터 리스트박스 변할경우
            cross_index = cross_allData_ListBox.SelectedIndex;

            selected_list_cross();

            Cross_Label_현재.Text = cross_index + 1 + "";
            #endregion
        }

   

        private void listBox4_MouseUp(object sender, MouseEventArgs e)
        {
            #region SAT 복수개 세팅
            if (e.Button == MouseButtons.Right)
            {
                if (cross_saveSat_TextBox.Text == "")
                {
                    MessageBox.Show("다시 선택해주세요");
                    return;
                }

                try
                {
                    int index = this.listBox4.IndexFromPoint(e.Location);
                    string text = listBox4.Items[index].ToString();
                    listBox4.SelectedIndex = index;
                    listBox4.Select();

                    cross_saveSat_TextBox.AppendText(":" + Dic_All[text].ToString());
                }
                catch
                { }
            }
            #endregion
        }

        private void listBox4_MouseClick(object sender, MouseEventArgs e)
        {
            #region SAT 세팅
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    cross_saveSat_TextBox.Text = " ";
                    cross_saveSat_TextBox.Text = Dic_All[listBox4.Text].ToString();
                }
                catch { }
            }
            #endregion
        }




        /*-----------------------------------------메뉴---------------------------------------------------*/

        private void 종료시저장(object sender, FormClosingEventArgs e)
        {
            #region 종료시 저장여부
            DialogResult result = MessageBox.Show("저장 후 종료하시겠습니까?", "저장 후 종료", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                try
                {   //저장
                    if (작업모드ToolStripMenuItem.Checked == true)
                    {
                        Write_File();
                        Dispose(true);
                    }
                    else//크로스 체크 모드 체크
                    {
                        try
                        {
                            string saveText = crossWriteParser(crossText);
                            StreamWriter stream_write3 = new StreamWriter(crossFinal_path, false, System.Text.Encoding.UTF8);//true:이어쓰기 false:덮어쓰기
                            stream_write3.Write(saveText);
                            stream_write3.Close();
                            Dispose(true);
                        }
                        catch
                        {
                            MessageBox.Show("파일저장에 실패하였습니다");
                        }

                    }
                }
                catch
                {
                    MessageBox.Show("저장할 파일이 없습니다");
                }
            }
            else if (result == DialogResult.No)
            {
                Dispose(true);
            }
            else
            {
                e.Cancel = true;
                return;
            }
            #endregion
        }

        private void 저장하기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region 저장하기
            try
            {
                Write_File();
                MessageBox.Show("저장에 성공하였습니다");
            }
            catch
            {
                MessageBox.Show("저장에 실패하였습니다.");
            }
            #endregion
        }


        private void workExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Work Excel 양방향 변환기 불러오기
            //Form_main jsonToExcel = new Form_main();
            //jsonToExcel.ShowDialog();
            #endregion
        }

        private void eSquadWorkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region SquadJson->CosmosJson 변환버튼 클릭

            string conversionPath = null;
            string conversionText = null;//변환되어 우리json에 저장될 변수
            string mod2 = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt"; ;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                conversionPath = openFileDialog.FileName;
                FileStream fs_read = new FileStream(conversionPath, FileMode.Open, FileAccess.Read);
                mod2 = System.IO.File.ReadAllText(conversionPath);

                fs_read.Close();

                //try
                //{
                conversionText = conversionEWriteParser(mod2);

                //쓰기는 새 파일에
                SaveFileDialog openFileDialog_final = new SaveFileDialog();
                openFileDialog_final.Title = "새로 저장";
                openFileDialog_final.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt"; ;
                openFileDialog_final.RestoreDirectory = true;
                openFileDialog_final.InitialDirectory = @"C:\";

                if (openFileDialog_final.ShowDialog() == DialogResult.OK)
                {
                    FileStream filestream = new FileStream(openFileDialog_final.FileName, FileMode.Create, FileAccess.Write);
                    StreamWriter stream_write = new StreamWriter(filestream, Encoding.UTF8);//true:이어쓰기 false:덮어쓰기
                    stream_write.Write(conversionText);
                    stream_write.Close();
                }
                MessageBox.Show("변환 후 저장 성공");
            }

            #endregion
        }

        private string conversionEWriteParser(string text)
        {
            #region E_SquadJson->WorkJson 변환

            JObject obj = JObject.Parse(text);
            JArray array = JArray.Parse(obj["data"].ToString());

            WorkFormatt.RootObject r = new WorkFormatt.RootObject();
            WorkFormatt.Datum d;
            r.data = new List<WorkFormatt.Datum>();

            WorkFormatt.Paragraph p;

            WorkFormatt.Qa q;

            WorkFormatt.Answer j;
           
            r.version = obj["version"].ToString();
            r.creator = obj["creator"].ToString();
            r.formatt = "Work";

            foreach (JObject itemObj in array)
            {
                d = new WorkFormatt.Datum();
                d.paragraphs = new List<EtriWork.WorkFormatt.Paragraph>();

                d.title = itemObj["title"].ToString();

                r.data.Add(d);

                JArray ooo = JArray.Parse(itemObj["paragraphs"].ToString());


                foreach (JObject itemObj2 in ooo)
                {
                    p = new WorkFormatt.Paragraph();
                    p.qas = new List<EtriWork.WorkFormatt.Qa>();

                    p.context = itemObj2["context_original"].ToString();
                   
                    d.paragraphs.Add(p);

                    JArray iii = JArray.Parse(itemObj2["qas"].ToString());

                    foreach (JObject itemObj3 in iii)
                    {
                        q = new WorkFormatt.Qa();
                        q.answers = new List<EtriWork.WorkFormatt.Answer>();

                        q.id = itemObj3["id"].ToString();
                        q.question = itemObj3["question_original"].ToString();
                      
                        p.qas.Add(q);



                        JArray yyy = JArray.Parse(itemObj3["answers"].ToString());

                        foreach (JObject itemObj4 in yyy)
                        {
                            j = new WorkFormatt.Answer();
                            j.text = itemObj4["text_origin"].ToString();
                            j.answer_end = Convert.ToInt32(itemObj4["answer_end"]);
                            j.answer_start = Convert.ToInt32(itemObj4["answer_start"]);


                            q.answers.Add(j);


                        }
                    }
                }
            }

            string json = JsonConvert.SerializeObject(r, Formatting.Indented);

            return json;

            #endregion
        }

    
        private void checkContainsQusetionCount()
        {
            #region ETRI JSON 중복문제 제거
            int count = 0; int overlapCount = 0;
            List<string> AllCheck = new List<string>();//근거단락+질문+정답
            List<string> tmp = new List<string>();

            tmp2 = new List<int>();//중복문제 번호

            MessageBox.Show("질문 개수는" + questionCount);

            for (int i = 0; i < questionCount; i++)
            {
                AllCheck.Add(Context_List[i].ToString() + "+" + Question_LIst[i].ToString() + "+" + Answer_List[i].ToString());
            }

            for (int i = 0; i < questionCount; i++)
            {
                if (tmp.Contains(AllCheck[i]))//포함되어 있으면
                {
                    tmp2.Add(i);
                    overlapCount++;
                    continue;
                }
                else
                {
                    count++;
                    tmp.Add(AllCheck[i]);
                }
            }

            MessageBox.Show("중복 제외한 개수는" + count.ToString());
            MessageBox.Show("중복 개수는" + overlapCount.ToString());

            for (int i = 0; i < tmp2.Count; i++)
            {
                MessageBox.Show("중복 문제는" + tmp2[i].ToString() + "  " + Question_LIst[tmp2[i]].ToString());
            }
            #endregion
        }


        private string conversionRemoveOverlabWriteParser(string text, List<int> tmp2)
        {
            #region EtriFormatt 중복제거
            Question_LIst.Clear();
            Answer_List.Clear();

            Context_List.Clear();

            QuestionType_List.Clear();
            QuestionFocus_List.Clear();
            QuestionLat_List.Clear();
            QuestionSat_List.Clear();
            QuestionTagged_List.Clear();

            ConfuseQt_List.Clear();
            ConfuseQf_List.Clear();
            ConfuseLat_List.Clear();
            ConfuseSat_List.Clear();
            CheckIndividual_List.Clear();

            EtriQtCheck_List.Clear();
            EtriQfCheck_List.Clear();
            EtriLatCheck_List.Clear();
            EtriSatCheck_List.Clear();

            Time_List.Clear();

            JObject obj = JObject.Parse(text);
            JArray array = JArray.Parse(obj["data"].ToString());

            EtriFormatt.RootObject r = new EtriFormatt.RootObject();
            EtriFormatt.Datum d;
            r.data = new List<EtriFormatt.Datum>();
            EtriFormatt.Paragraph p;
            EtriFormatt.Qa q;
            EtriFormatt.Answer j;
           
            r.version = obj["version"].ToString();
            r.creator = obj["creator"].ToString();

            int i = 0;
            int tmp = 0;

            foreach (JObject itemObj in array)
            {
                //if (checkOverlap_List[i] == false && checkContextOverlap_List[i] == false)//둘다 같으면 패스
                if (i == tmp2[tmp])//둘다 같으면 패스
                {
                    i++;
                    if (tmp + 1 < tmp2.Count)
                    {
                        tmp++;//indexOut조심
                    }
                    continue;
                }
                else//정답문장과 질문 한개라도 다르면
                {
                    overlabCount++;
                    i++;
                }

                d = new EtriFormatt.Datum();
                d.paragraphs = new List<EtriWork.EtriFormatt.Paragraph>();

                d.title = itemObj["title"].ToString();

                r.data.Add(d);

                JArray ooo = JArray.Parse(itemObj["paragraphs"].ToString());


                foreach (JObject itemObj2 in ooo)
                {
                    p = new EtriFormatt.Paragraph();
                    p.qas = new List<EtriWork.EtriFormatt.Qa>();

                    p.context = itemObj2["context"].ToString();
                    p.context_en = itemObj2["context_en"].ToString();
                    p.context_tagged = itemObj2["context_tagged"].ToString();

                    d.paragraphs.Add(p);

                    JArray iii = JArray.Parse(itemObj2["qas"].ToString());

                    foreach (JObject itemObj3 in iii)
                    {
                        q = new EtriFormatt.Qa();
                        q.answers = new List<EtriWork.EtriFormatt.Answer>();
                        q.id = itemObj3["id"].ToString();
                        q.question = itemObj3["question"].ToString();
                        q.question_en = itemObj3["question_en"].ToString();
                        q.question_tagged = itemObj3["question_tagged"].ToString();
                        q.questionType = itemObj3["questionType"].ToString();
                        q.questionFocus = itemObj3["questionFocus"].ToString();
                        q.questionSAT = itemObj3["questionSAT"].ToString();
                        q.questionLAT = itemObj3["questionLAT"].ToString();
                        p.qas.Add(q);

                        JArray yyy = JArray.Parse(itemObj3["answers"].ToString());

                        foreach (JObject itemObj4 in yyy)
                        {
                            j = new EtriFormatt.Answer();
                            j.text = itemObj4["text"].ToString();
                            j.text_en = itemObj4["text_en"].ToString();
                            j.text_tagged = itemObj4["text_tagged"].ToString();
                            j.text_syn = itemObj4["text_syn"].ToString();
                            j.answer_end = Convert.ToInt32(itemObj4["answer_end"]);
                            j.answer_start = Convert.ToInt32(itemObj4["answer_start"]);

                            q.answers.Add(j);


                        }
                    }
                }
            }

            MessageBox.Show("총 개수는" + i.ToString() + "중복 제거 후 개수는" + overlabCount.ToString());

            string json = JsonConvert.SerializeObject(r, Formatting.Indented);

            return json;

            #endregion
        }

        private void 관리자모드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region 관리자 메뉴 클릭
            StatusForm sForm = new StatusForm();
            sForm.ShowDialog();
            #endregion
        }

        private void 중복문제제거ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region 중복제거 메뉴 클릭
            string conversionPath = null;
            string conversionText = null;//변환되어 우리json에 저장될 변수
            string mod2 = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt"; ;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                conversionPath = openFileDialog.FileName;
                FileStream fs_read = new FileStream(conversionPath, FileMode.Open, FileAccess.Read);
                mod2 = System.IO.File.ReadAllText(conversionPath);

                fs_read.Close();

                try
                {
                    workReadParser(mod2);
                    checkContainsQusetionCount();
                    conversionText = conversionRemoveOverlabWriteParser(mod2, tmp2);

                    //쓰기는 새 파일에
                    SaveFileDialog openFileDialog_final = new SaveFileDialog();
                    openFileDialog_final.Title = "새로 저장";
                    openFileDialog_final.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt"; ;
                    openFileDialog_final.RestoreDirectory = true;
                    openFileDialog_final.InitialDirectory = @"C:\";

                    if (openFileDialog_final.ShowDialog() == DialogResult.OK)
                    {

                        FileStream filestream = new FileStream(openFileDialog_final.FileName, FileMode.Create, FileAccess.Write);
                        StreamWriter stream_write = new StreamWriter(filestream, Encoding.UTF8);
                        stream_write.Write(conversionText);
                        stream_write.Close();
                    }
                    MessageBox.Show("변환 후 저장 성공");
                }
                catch
                {
                    MessageBox.Show("변환 후 저장 실패");
                }

            }
            #endregion
        }

        private void jSONEXCEL간변환ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region JSON과 EXCEL간 변환
            JSON_ExcelDirectionalConverter.Form1 form1 = new JSON_ExcelDirectionalConverter.Form1();
            form1.ShowDialog();
            #endregion
        }



    }
}
