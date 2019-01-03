using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;

namespace EtriWork
{
    public partial class SATStatsForm : Form
    {
        int m_PERSON;
        int m_ETC;
        int[] m_LOCATION;
        int[] m_ORGANIZATION;
        int[] m_ARTIFACTS;
        int[] m_DATE;
        int[] m_TIME;
        int[] m_MATERIAL;
        int[] m_CIVILIZATION;
        int[] m_EVENT;
        int[] m_PLANT;
        int[] m_QUANTITY;
        int[] m_THEORY;
        int[] m_STUDY_FIELD;
        int[] m_TERM;
        int[] m_ANIMAL;

        public static StatusSATDto satDto = new StatusSATDto();

        

        ArrayList Sat_List;

        public SATStatsForm()
        {
            InitializeComponent();
            init();
            Set_Sat();
            Result_Static();
            Getstatus();
        }
        public StatusSATDto Getstatus()
        {
            satDto.PS_NAME = PS_NAME.Text;
            satDto.ETC = ETC.Text;
            satDto.LOCATION = LOCATION.Text;
            satDto.ORGANIZATION = ORGANIZATION.Text;
            satDto.ARTIFACTS = ARTIFACTS.Text;
            satDto.DATE = DATE.Text;
            satDto.TIME = TIME.Text;
            satDto.CIVILIZATION = CIVILIZATION.Text;
            satDto.EVENT = EVENT.Text;
            satDto.PLANT = PLANT.Text;
            satDto.QUANTITY = QUANTITY.Text;
            satDto.THEORY = THEORY.Text;
            satDto.STUDY_FIELD = STUDY_FIELD.Text;
            satDto.TERM = TERM.Text;
            satDto.ANIMAL = ANIMAL.Text;
            return satDto;
        }
        public void Set_Sat()
        {
            int start = StatusForm.startNum;
            int end = StatusForm.endNum;
        
            Sat_List = StatusForm.Check1_QuestionSat;

            //Count
            for (int i = start - 1; i < end; i++)
            {
                if(Sat_List[i].ToString().Contains(":"))//복수개
                {
                    string[] result = Sat_List[i].ToString().Split(new char[] { ':' });

                    for(int j=0; j<result.Length; j++)
                    {
                        //MessageBox.Show(result[j]);
                        Add_Count(result[j]);
                    }
                }
                else
                {
                    Add_Count(Sat_List[i].ToString());
                }
            }
        }

        public void Add_Count(String s)
        {
            switch (s)
            {
                case "PS_NAME":
                    m_PERSON++;
                    break;
                case "ETC":
                    m_ETC++;
                    break;
                case "LC_OTHERS":
                    m_LOCATION[0]++;
                    break;
                case "LCP_COUNTRY":
                    m_LOCATION[1]++;
                    break;
                case "LCP_PROVINCE":
                    m_LOCATION[2]++;
                    break;
                case "LCP_COUNTY":
                    m_LOCATION[3]++;
                    break;
                case "LCP_CITY":
                    m_LOCATION[4]++;
                    break;
                case "LCP_CAPITALCITY":
                    m_LOCATION[5]++;
                    break;
                case "LCG_RIVER":
                    m_LOCATION[6]++;
                    break;
                case "LCG_OCEAN":
                    m_LOCATION[7]++;
                    break;
                case "LCG_BAY":
                    m_LOCATION[8]++;
                    break;
                case "LCG_MOUNTAIN":
                    m_LOCATION[9]++;
                    break;
                case "LCG_ISLAND":
                    m_LOCATION[10]++;
                    break;
                case "LCG_CONTINENT":
                    m_LOCATION[11]++;
                    break;
                case "LC_TOUR":
                    m_LOCATION[12]++;
                    break;
                case "LC_SPACE":
                    m_LOCATION[13]++;
                    break;

                case "OG_OTHERS":
                    m_ORGANIZATION[0]++;
                    break;
                case "OGG_ECONOMY":
                    m_ORGANIZATION[1]++;
                    break;
                case "OGG_EDUCATION":
                    m_ORGANIZATION[2]++;
                    break;
                case "OGG_MILITARY":
                    m_ORGANIZATION[3]++;
                    break;
                case "OGG_MEDIA":
                    m_ORGANIZATION[4]++;
                    break;
                case "OGG_SPORTS":
                    m_ORGANIZATION[5]++;
                    break;
                case "OGG_ART":
                    m_ORGANIZATION[6]++;
                    break;
                case "OGG_MEDICINE":
                    m_ORGANIZATION[7]++;
                    break;
                case "OGG_RELIGION":
                    m_ORGANIZATION[8]++;
                    break;
                case "OGG_SCIENCE":
                    m_ORGANIZATION[9]++;
                    break;
                case "OGG_LIBRARY":
                    m_ORGANIZATION[10]++;
                    break;
                case "OGG_LAW":
                    m_ORGANIZATION[11]++;
                    break;
                case "OGG_POLITICS":
                    m_ORGANIZATION[12]++;
                    break;
                case "OGG_FOOD":
                    m_ORGANIZATION[13]++;
                    break;
                case "OGG_HOTEL":
                    m_ORGANIZATION[14]++;
                    break;

                case "AF_CULTURAL_ASSET":
                    m_ARTIFACTS[0]++;
                    break;
                case "AF_BUILDING":
                    m_ARTIFACTS[1]++;
                    break;
                case "AF_MUSICAL_INSTRUMENT":
                    m_ARTIFACTS[2]++;
                    break;
                case "AF_ROAD":
                    m_ARTIFACTS[3]++;
                    break;
                case "AF_WEAPON":
                    m_ARTIFACTS[4]++;
                    break;
                case "AF_TRANSPORT":
                    m_ARTIFACTS[5]++;
                    break;
                case "AF_WORKS":
                    m_ARTIFACTS[6]++;
                    break;
                case "AFW_DOCUMENT":
                    m_ARTIFACTS[7]++;
                    break;
                case "AFW_PERFORMANCE":
                    m_ARTIFACTS[8]++;
                    break;
                case "AFW_VIDEO":
                    m_ARTIFACTS[9]++;
                    break;
                case "AFW_ART_CRAFT":
                    m_ARTIFACTS[10]++;
                    break;
                case "AFW_MUSIC":
                    m_ARTIFACTS[11]++;
                    break;
                case "AF_WARES":
                    m_ARTIFACTS[12]++;
                    break;

                case "DT_OTHERS":
                    m_DATE[0]++;
                    break;
                case "DT_DURATION":
                    m_DATE[1]++;
                    break;
                case "DT_DAY":
                    m_DATE[2]++;
                    break;
                case "DT_MONTH":
                    m_DATE[3]++;
                    break;
                case "DT_YEAR":
                    m_DATE[4]++;
                    break;
                case "DT_SEASON":
                    m_DATE[5]++;
                    break;
                case "DT_GEOAGE":
                    m_DATE[6]++;
                    break;
                case "DT_DYNASTY":
                    m_DATE[7]++;
                    break;

                case "TI_OTHERS":
                    m_TIME[0]++;
                    break;
                case "TI_DURATION":
                    m_TIME[1]++;
                    break;
                case "TI_HOUR":
                    m_TIME[2]++;
                    break;
                case "TI_MINUTE":
                    m_TIME[3]++;
                    break;
                case "TI_SECOND":
                    m_TIME[4]++;
                    break;

                case "CV_NAME":
                    m_CIVILIZATION[0]++;
                    break;
                case "CV_TRIBE":
                    m_CIVILIZATION[1]++;
                    break;
                case "CV_SPORTS":
                    m_CIVILIZATION[2]++;
                    break;
                case "CV_SPORTS_INST":
                    m_CIVILIZATION[3]++;
                    break;
                case "CV_POLICY":
                    m_CIVILIZATION[4]++;
                    break;
                case "CV_TAX":
                    m_CIVILIZATION[5]++;
                    break;
                case "CV_FUNDS":
                    m_CIVILIZATION[6]++;
                    break;
                case "CV_LANGUAGE":
                    m_CIVILIZATION[7]++;
                    break;
                case "CV_BUILDING_TYPE":
                    m_CIVILIZATION[8]++;
                    break;
                case "CV_FOOD":
                    m_CIVILIZATION[9]++;
                    break;
                case "CV_DRINK":
                    m_CIVILIZATION[10]++;
                    break;
                case "CV_CLOTHING":
                    m_CIVILIZATION[11]++;
                    break;
                case "CV_POSITION":
                    m_CIVILIZATION[12]++;
                    break;
                case "CV_RELATION":
                    m_CIVILIZATION[13]++;
                    break;
                case "CV_OCCUPATION":
                    m_CIVILIZATION[14]++;
                    break;
                case "CV_CURRENCY":
                    m_CIVILIZATION[15]++;
                    break;
                case "CV_PRIZE":
                    m_CIVILIZATION[16]++;
                    break;
                case "CV_LAW":
                    m_CIVILIZATION[17]++;
                    break;
                case "CV_FOOD_STYLE":
                    m_CIVILIZATION[18]++;
                    break;

                case "AM_OTHERS":
                    m_ANIMAL[0]++;
                    break;
                case "AM_INSECT":
                    m_ANIMAL[1]++;
                    break;
                case "AM_BIRD":
                    m_ANIMAL[2]++;
                    break;
                case "AM_FISH":
                    m_ANIMAL[3]++;
                    break;
                case "AM_MAMMALIA":
                    m_ANIMAL[4]++;
                    break;
                case "AM_AMPHIBIA":
                    m_ANIMAL[5]++;
                    break;
                case "AM_REPTILIA":
                    m_ANIMAL[6]++;
                    break;
                case "AM_TYPE":
                    m_ANIMAL[7]++;
                    break;
                case "AM_PART":
                    m_ANIMAL[8]++;
                    break;

                case "PT_OTHERS":
                    m_PLANT[0]++;
                    break;
                case "PT_FRUIT":
                    m_PLANT[1]++;
                    break;
                case "PT_FLOWER":
                    m_PLANT[2]++;
                    break;
                case "PT_TREE":
                    m_PLANT[3]++;
                    break;
                case "PT_GRASS":
                    m_PLANT[4]++;
                    break;
                case "PT_TYPE":
                    m_PLANT[5]++;
                    break;
                case "PT_PART":
                    m_PLANT[6]++;
                    break;

                case "QT_OTHERS":
                    m_QUANTITY[0]++;
                    break;
                case "QT_AGE":
                    m_QUANTITY[1]++;
                    break;
                case "QT_SIZE":
                    m_QUANTITY[2]++;
                    break;
                case "QT_LENGTH":
                    m_QUANTITY[3]++;
                    break;
                case "QT_COUNT":
                    m_QUANTITY[4]++;
                    break;
                case "QT_MAN_COUNT":
                    m_QUANTITY[5]++;
                    break;
                case "QT_WEIGHT":
                    m_QUANTITY[6]++;
                    break;
                case "QT_PERCENTAGE":
                    m_QUANTITY[7]++;
                    break;
                case "QT_SPEED":
                    m_QUANTITY[8]++;
                    break;
                case "QT_TEMPERATURE":
                    m_QUANTITY[9]++;
                    break;
                case "QT_VOLUME":
                    m_QUANTITY[10]++;
                    break;
                case "QT_ORDER":
                    m_QUANTITY[11]++;
                    break;
                case "QT_PRICE":
                    m_QUANTITY[12]++;
                    break;
                case "QT_PHONE":
                    m_QUANTITY[13]++;
                    break;
                case "QT_SPORTS":
                    m_QUANTITY[14]++;
                    break;
                case "QT_CHANNEL":
                    m_QUANTITY[15]++;
                    break;
                case "QT_ALBUM":
                    m_QUANTITY[16]++;
                    break;
                case "QT_ZIPCODE":
                    m_QUANTITY[17]++;
                    break;

                case "FD_OTHERS":
                    m_STUDY_FIELD[0]++;
                    break;
                case "FD_SCIENCE":
                    m_STUDY_FIELD[1]++;
                    break;
                case "FD_SOCIAL_SCIENCE":
                    m_STUDY_FIELD[2]++;
                    break;
                case "FD_MEDICINE":
                    m_STUDY_FIELD[3]++;
                    break;
                case "FD_ART":
                    m_STUDY_FIELD[4]++;
                    break;
                case "FD_PHILOSOPHY":
                    m_STUDY_FIELD[5]++;
                    break;

                case "TR_OTHERS":
                    m_THEORY[0]++;
                    break;
                case "TR_SCIENCE":
                    m_THEORY[1]++;
                    break;
                case "TR_SOCIAL_SCIENCE":
                    m_THEORY[2]++;
                    break;
                case "TR_ART":
                    m_THEORY[3]++;
                    break;
                case "TR_PHILOSOPHY":
                    m_THEORY[4]++;
                    break;
                case "TR_MEDICINE":
                    m_THEORY[5]++;
                    break;

                case "EV_OTHERS":
                    m_EVENT[0]++;
                    break;
                case "EV_ACTIVITY":
                    m_EVENT[1]++;
                    break;
                case "EV_WAR_REVOLUTION":
                    m_EVENT[2]++;
                    break;
                case "EV_SPORTS":
                    m_EVENT[3]++;
                    break;
                case "EV_FESTIVAL":
                    m_EVENT[4]++;
                    break;

                case "MT_ELEMENT":
                    m_MATERIAL[0]++;
                    break;
                case "MT_METAL":
                    m_MATERIAL[1]++;
                    break;
                case "MT_ROCK":
                    m_MATERIAL[2]++;
                    break;
                case "MT_CHEMICAL":
                    m_MATERIAL[3]++;
                    break;

                case "TM_COLOR":
                    m_TERM[0]++;
                    break;
                case "TM_DIRECTION":
                    m_TERM[1]++;
                    break;
                case "TM_CLIMATE":
                    m_TERM[2]++;
                    break;
                case "TM_SHAPE":
                    m_TERM[3]++;
                    break;
                case "TM_CELL_TISSUE":
                    m_TERM[4]++;
                    break;
                case "TMM_DISEASE":
                    m_TERM[5]++;
                    break;
                case "TMM_DRUG":
                    m_TERM[6]++;
                    break;
                case "TMI_HW":
                    m_TERM[7]++;
                    break;
                case "TMI_SW":
                    m_TERM[8]++;
                    break;
                case "TMI_SITE":
                    m_TERM[9]++;
                    break;
                case "TMI_EMAIL":
                    m_TERM[10]++;
                    break;
                case "TMI_MODEL":
                    m_TERM[11]++;
                    break;
                case "TMI_SERVICE":
                    m_TERM[12]++;
                    break;
                case "TMI_PROJECT":
                    m_TERM[13]++;
                    break;
                case "TMIG_GENRE":
                    m_TERM[14]++;
                    break;
                case "TM_SPORTS":
                    m_TERM[15]++;
                    break;
                //default: MessageBox.Show(s);
                //    break;
            }
        }

        public void init()
        {
            Sat_List = new ArrayList();
            m_PERSON = 0;
            m_ETC = 0;
            m_LOCATION = new int[14] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; ;
            m_ORGANIZATION = new int[15] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            m_ARTIFACTS = new int[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; ;
            m_DATE = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            m_TIME = new int[5] { 0, 0, 0, 0, 0 };
            m_MATERIAL = new int[4] { 0, 0, 0, 0 };
            m_CIVILIZATION = new int[19] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            m_EVENT = new int[5] { 0, 0, 0, 0, 0 };
            m_PLANT = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            m_QUANTITY = new int[18] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            m_THEORY = new int[6] { 0, 0, 0, 0, 0, 0 };
            m_STUDY_FIELD = new int[6] { 0, 0, 0, 0, 0, 0 };
            m_TERM = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            m_ANIMAL = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };


            LOCATION.Text = "";
            ORGANIZATION.Text = "";
            ARTIFACTS.Text = "";
            DATE.Text = "";
            TIME.Text = "";
            MATERIAL.Text = "";
            CIVILIZATION.Text = "";
            EVENT.Text = "";
            PLANT.Text = "";
            QUANTITY.Text = "";
            THEORY.Text = "";
            STUDY_FIELD.Text = "";
            TERM.Text = "";
            ANIMAL.Text = "";

            PS_NAME.Text = "";
            LC_OTHERS.Text = "";
            LCP_COUNTRY.Text = "";
            LCP_PROVINCE.Text = "";
            LCP_COUNTY.Text = "";
            LCP_CITY.Text = "";
            LCP_CAPITALCITY.Text = "";
            LCG_RIVER.Text = "";
            LCG_OCEAN.Text = "";
            LCG_BAY.Text = "";
            LCG_MOUNTAIN.Text = "";
            LCG_ISLAND.Text = "";
            LCG_CONTINENT.Text = "";
            LC_TOUR.Text = "";
            LC_SPACE.Text = "";
            OG_OTHERS.Text = "";
            OGG_ECONOMY.Text = "";
            OGG_EDUCATION.Text = "";
            OGG_MILITARY.Text = "";
            OGG_MEDIA.Text = "";
            OGG_SPORTS.Text = "";
            OGG_ART.Text = "";
            OGG_MEDICINE.Text = "";
            OGG_RELIGION.Text = "";
            OGG_SCIENCE.Text = "";
            OGG_LIBRARY.Text = "";
            OGG_LAW.Text = "";
            OGG_POLITICS.Text = "";
            OGG_FOOD.Text = "";
            OGG_HOTEL.Text = "";
            AF_CULTURAL_ASSET.Text = "";
            AF_BUILDING.Text = "";
            AF_MUSICAL_INSTRUMENT.Text = "";
            AF_ROAD.Text = "";
            AF_WEAPON.Text = "";
            AF_TRANSPORT.Text = "";
            AF_WORKS.Text = "";
            AFW_DOCUMENT.Text = "";
            AFW_PERFORMANCE.Text = "";
            AFW_VIDEO.Text = "";
            AFW_ART_CRAFT.Text = "";
            AFW_MUSIC.Text = "";
            AF_WARES.Text = "";
            DT_OTHERS.Text = "";
            DT_DURATION.Text = "";
            DT_DAY.Text = "";
            DT_MONTH.Text = "";
            DT_YEAR.Text = "";
            DT_SEASON.Text = "";
            DT_GEOAGE.Text = "";
            DT_DYNASTY.Text = "";
            TI_OTHERS.Text = "";
            TI_DURATION.Text = "";
            TI_HOUR.Text = "";
            TI_MINUTE.Text = "";
            TI_SECOND.Text = "";
            CV_NAME.Text = "";
            CV_TRIBE.Text = "";
            CV_SPORTS.Text = "";
            CV_SPORTS_INST.Text = "";
            CV_POLICY.Text = "";
            CV_TAX.Text = "";
            CV_FUNDS.Text = "";
            CV_LANGUAGE.Text = "";
            CV_BUILDING_TYPE.Text = "";
            CV_FOOD.Text = "";
            CV_DRINK.Text = "";
            CV_CLOTHING.Text = "";
            CV_POSITION.Text = "";
            CV_RELATION.Text = "";
            CV_OCCUPATION.Text = "";
            CV_CURRENCY.Text = "";
            CV_PRIZE.Text = "";
            CV_LAW.Text = "";
            CV_FOOD_STYLE.Text = "";
            AM_OTHERS.Text = "";
            AM_INSECT.Text = "";
            AM_BIRD.Text = "";
            AM_FISH.Text = "";
            AM_MAMMALIA.Text = "";
            AM_AMPHIBIA.Text = "";
            AM_REPTILIA.Text = "";
            AM_TYPE.Text = "";
            AM_PART.Text = "";
            PT_OTHERS.Text = "";
            PT_FRUIT.Text = "";
            PT_FLOWER.Text = "";
            PT_TREE.Text = "";
            PT_GRASS.Text = "";
            PT_TYPE.Text = "";
            PT_PART.Text = "";
            QT_OTHERS.Text = "";
            QT_AGE.Text = "";
            QT_SIZE.Text = "";
            QT_LENGTH.Text = "";
            QT_COUNT.Text = "";
            QT_MAN_COUNT.Text = "";
            QT_WEIGHT.Text = "";
            QT_PERCENTAGE.Text = "";
            QT_SPEED.Text = "";
            QT_TEMPERATURE.Text = "";
            QT_VOLUME.Text = "";
            QT_ORDER.Text = "";
            QT_PRICE.Text = "";
            QT_PHONE.Text = "";
            QT_SPORTS.Text = "";
            QT_CHANNEL.Text = "";
            QT_ALBUM.Text = "";
            QT_ZIPCODE.Text = "";
            FD_OTHERS.Text = "";
            FD_SCIENCE.Text = "";
            FD_SOCIAL_SCIENCE.Text = "";
            FD_MEDICINE.Text = "";
            FD_ART.Text = "";
            FD_PHILOSOPHY.Text = "";
            TR_OTHERS.Text = "";
            TR_SCIENCE.Text = "";
            TR_SOCIAL_SCIENCE.Text = "";
            TR_ART.Text = "";
            TR_PHILOSOPHY.Text = "";
            TR_MEDICINE.Text = "";
            EV_OTHERS.Text = "";
            EV_ACTIVITY.Text = "";
            EV_WAR_REVOLUTION.Text = "";
            EV_SPORTS.Text = "";
            EV_FESTIVAL.Text = "";
            MT_ELEMENT.Text = "";
            MT_METAL.Text = "";
            MT_ROCK.Text = "";
            MT_CHEMICAL.Text = "";
            TM_COLOR.Text = "";
            TM_DIRECTION.Text = "";
            TM_CLIMATE.Text = "";
            TM_SHAPE.Text = "";
            TM_CELL_TISSUE.Text = "";
            TMM_DISEASE.Text = "";
            TMM_DRUG.Text = "";
            TMI_HW.Text = "";
            TMI_SW.Text = "";
            TMI_SITE.Text = "";
            TMI_EMAIL.Text = "";
            TMI_MODEL.Text = "";
            TMI_SERVICE.Text = "";
            TMI_PROJECT.Text = "";
            TMIG_GENRE.Text = "";
            TM_SPORTS.Text = "";
        }

        public void Result_Static()
        {
            PS_NAME.Text = "" + m_PERSON;

            ETC.Text = "" + m_ETC;

            LC_OTHERS.Text = "" + m_LOCATION[0];
            LCP_COUNTRY.Text = "" + m_LOCATION[1];
            LCP_PROVINCE.Text = "" + m_LOCATION[2];
            LCP_COUNTY.Text = "" + m_LOCATION[3];
            LCP_CITY.Text = "" + m_LOCATION[4];
            LCP_CAPITALCITY.Text = "" + m_LOCATION[5];
            LCG_RIVER.Text = "" + m_LOCATION[6];
            LCG_OCEAN.Text = "" + m_LOCATION[7];
            LCG_BAY.Text = "" + m_LOCATION[8];
            LCG_MOUNTAIN.Text = "" + m_LOCATION[9];
            LCG_ISLAND.Text = "" + m_LOCATION[10];
            LCG_CONTINENT.Text = "" + m_LOCATION[11];
            LC_TOUR.Text = "" + m_LOCATION[12];
            LC_SPACE.Text = "" + m_LOCATION[13];

            OG_OTHERS.Text = "" + m_ORGANIZATION[0];
            OGG_ECONOMY.Text = "" + m_ORGANIZATION[1];
            OGG_EDUCATION.Text = "" + m_ORGANIZATION[2];
            OGG_MILITARY.Text = "" + m_ORGANIZATION[3];
            OGG_MEDIA.Text = "" + m_ORGANIZATION[4];
            OGG_SPORTS.Text = "" + m_ORGANIZATION[5];
            OGG_ART.Text = "" + m_ORGANIZATION[6];
            OGG_MEDICINE.Text = "" + m_ORGANIZATION[7];
            OGG_RELIGION.Text = "" + m_ORGANIZATION[8];
            OGG_SCIENCE.Text = "" + m_ORGANIZATION[9];
            OGG_LIBRARY.Text = "" + m_ORGANIZATION[10];
            OGG_LAW.Text = "" + m_ORGANIZATION[11];
            OGG_POLITICS.Text = "" + m_ORGANIZATION[12];
            OGG_FOOD.Text = "" + m_ORGANIZATION[13];
            OGG_HOTEL.Text = "" + m_ORGANIZATION[14];

            AF_CULTURAL_ASSET.Text = "" + m_ARTIFACTS[0];
            AF_BUILDING.Text = "" + m_ARTIFACTS[1];
            AF_MUSICAL_INSTRUMENT.Text = "" + m_ARTIFACTS[2];
            AF_ROAD.Text = "" + m_ARTIFACTS[3];
            AF_WEAPON.Text = "" + m_ARTIFACTS[4];
            AF_TRANSPORT.Text = "" + m_ARTIFACTS[5];
            AF_WORKS.Text = "" + m_ARTIFACTS[6];
            AFW_DOCUMENT.Text = "" + m_ARTIFACTS[7];
            AFW_PERFORMANCE.Text = "" + m_ARTIFACTS[8];
            AFW_VIDEO.Text = "" + m_ARTIFACTS[9];
            AFW_ART_CRAFT.Text = "" + m_ARTIFACTS[10];
            AFW_MUSIC.Text = "" + m_ARTIFACTS[11];
            AF_WARES.Text = "" + m_ARTIFACTS[12];

            DT_OTHERS.Text = "" + m_DATE[0];
            DT_DURATION.Text = "" + m_DATE[1];
            DT_DAY.Text = "" + m_DATE[2];
            DT_MONTH.Text = "" + m_DATE[3];
            DT_YEAR.Text = "" + m_DATE[4];
            DT_SEASON.Text = "" + m_DATE[5];
            DT_GEOAGE.Text = "" + m_DATE[6];
            DT_DYNASTY.Text = "" + m_DATE[7];

            TI_OTHERS.Text = "" + m_TIME[0];
            TI_DURATION.Text = "" + m_TIME[1];
            TI_HOUR.Text = "" + m_TIME[2];
            TI_MINUTE.Text = "" + m_TIME[3];
            TI_SECOND.Text = "" + m_TIME[4];

            CV_NAME.Text = "" + m_CIVILIZATION[0];
            CV_TRIBE.Text = "" + m_CIVILIZATION[1];
            CV_SPORTS.Text = "" + m_CIVILIZATION[2];
            CV_SPORTS_INST.Text = "" + m_CIVILIZATION[3];
            CV_POLICY.Text = "" + m_CIVILIZATION[4];
            CV_TAX.Text = "" + m_CIVILIZATION[5];
            CV_FUNDS.Text = "" + m_CIVILIZATION[6];
            CV_LANGUAGE.Text = "" + m_CIVILIZATION[7];
            CV_BUILDING_TYPE.Text = "" + m_CIVILIZATION[8];
            CV_FOOD.Text = "" + m_CIVILIZATION[9];
            CV_DRINK.Text = "" + m_CIVILIZATION[10];
            CV_CLOTHING.Text = "" + m_CIVILIZATION[11];
            CV_POSITION.Text = "" + m_CIVILIZATION[12];
            CV_RELATION.Text = "" + m_CIVILIZATION[13];
            CV_OCCUPATION.Text = "" + m_CIVILIZATION[14];
            CV_CURRENCY.Text = "" + m_CIVILIZATION[15];
            CV_PRIZE.Text = "" + m_CIVILIZATION[16];
            CV_LAW.Text = "" + m_CIVILIZATION[17];
            CV_FOOD_STYLE.Text = "" + m_CIVILIZATION[18];

            AM_OTHERS.Text = "" + m_ANIMAL[0];
            AM_INSECT.Text = "" + m_ANIMAL[1];
            AM_BIRD.Text = "" + m_ANIMAL[2];
            AM_FISH.Text = "" + m_ANIMAL[3];
            AM_MAMMALIA.Text = "" + m_ANIMAL[4];
            AM_AMPHIBIA.Text = "" + m_ANIMAL[5];
            AM_REPTILIA.Text = "" + m_ANIMAL[6];
            AM_TYPE.Text = "" + m_ANIMAL[7];
            AM_PART.Text = "" + m_ANIMAL[8];

            PT_OTHERS.Text = "" + m_PLANT[0];
            PT_FRUIT.Text = "" + m_PLANT[1];
            PT_FLOWER.Text = "" + m_PLANT[2];
            PT_TREE.Text = "" + m_PLANT[3];
            PT_GRASS.Text = "" + m_PLANT[4];
            PT_TYPE.Text = "" + m_PLANT[5];
            PT_PART.Text = "" + m_PLANT[6];

            QT_OTHERS.Text = "" + m_QUANTITY[0];
            QT_AGE.Text = "" + m_QUANTITY[1];
            QT_SIZE.Text = "" + m_QUANTITY[2];
            QT_LENGTH.Text = "" + m_QUANTITY[3];
            QT_COUNT.Text = "" + m_QUANTITY[4];
            QT_MAN_COUNT.Text = "" + m_QUANTITY[5];
            QT_WEIGHT.Text = "" + m_QUANTITY[6];
            QT_PERCENTAGE.Text = "" + m_QUANTITY[7];
            QT_SPEED.Text = "" + m_QUANTITY[8];
            QT_TEMPERATURE.Text = "" + m_QUANTITY[9];
            QT_VOLUME.Text = "" + m_QUANTITY[10];
            QT_ORDER.Text = "" + m_QUANTITY[11];
            QT_PRICE.Text = "" + m_QUANTITY[12];
            QT_PHONE.Text = "" + m_QUANTITY[13];
            QT_SPORTS.Text = "" + m_QUANTITY[14];
            QT_CHANNEL.Text = "" + m_QUANTITY[15];
            QT_ALBUM.Text = "" + m_QUANTITY[16];
            QT_ZIPCODE.Text = "" + m_QUANTITY[17];

            FD_OTHERS.Text = "" + m_STUDY_FIELD[0];
            FD_SCIENCE.Text = "" + m_STUDY_FIELD[1];
            FD_SOCIAL_SCIENCE.Text = "" + m_STUDY_FIELD[2];
            FD_MEDICINE.Text = "" + m_STUDY_FIELD[3];
            FD_ART.Text = "" + m_STUDY_FIELD[4];
            FD_PHILOSOPHY.Text = "" + m_STUDY_FIELD[5];

            TR_OTHERS.Text = "" + m_THEORY[0];
            TR_SCIENCE.Text = "" + m_THEORY[1];
            TR_SOCIAL_SCIENCE.Text = "" + m_THEORY[2];
            TR_ART.Text = "" + m_THEORY[3];
            TR_MEDICINE.Text = "" + m_THEORY[4];
            TR_PHILOSOPHY.Text = "" + m_THEORY[5];

            EV_OTHERS.Text = "" + m_EVENT[0];
            EV_ACTIVITY.Text = "" + m_EVENT[1];
            EV_WAR_REVOLUTION.Text = "" + m_EVENT[2];
            EV_SPORTS.Text = "" + m_EVENT[3];
            EV_FESTIVAL.Text = "" + m_EVENT[4];

            MT_ELEMENT.Text = "" + m_MATERIAL[0];
            MT_METAL.Text = "" + m_MATERIAL[1];
            MT_ROCK.Text = "" + m_MATERIAL[2];
            MT_CHEMICAL.Text = "" + m_MATERIAL[3];

            TM_COLOR.Text = "" + m_TERM[0];
            TM_DIRECTION.Text = "" + m_TERM[1];
            TM_CLIMATE.Text = "" + m_TERM[2];
            TM_SHAPE.Text = "" + m_TERM[3];
            TM_CELL_TISSUE.Text = "" + m_TERM[4];
            TMM_DISEASE.Text = "" + m_TERM[5];
            TMM_DRUG.Text = "" + m_TERM[6];
            TMI_HW.Text = "" + m_TERM[7];
            TMI_SW.Text = "" + m_TERM[8];
            TMI_SITE.Text = "" + m_TERM[9];
            TMI_EMAIL.Text = "" + m_TERM[10];
            TMI_MODEL.Text = "" + m_TERM[11];
            TMI_SERVICE.Text = "" + m_TERM[12];
            TMI_PROJECT.Text = "" + m_TERM[13];
            TMIG_GENRE.Text = "" + m_TERM[14];
            TM_SPORTS.Text = "" + m_TERM[15];
            int sum = 0;
            int total_sum = 0;
            for (int i = 0; i < m_LOCATION.Length; i++)
            {
                sum += m_LOCATION[i];
                total_sum += m_LOCATION[i];
            }
            LOCATION.Text = sum + "";
            sum = 0;

            for (int i = 0; i < m_ORGANIZATION.Length; i++)
            {
                sum += m_ORGANIZATION[i];
                total_sum += m_ORGANIZATION[i];
            }
            ORGANIZATION.Text = sum + "";
            sum = 0;

            for (int i = 0; i < m_ARTIFACTS.Length; i++)
            {
                sum += m_ARTIFACTS[i];
                total_sum += m_ARTIFACTS[i];
            }
            ARTIFACTS.Text = sum + "";
            sum = 0;

            for (int i = 0; i < m_DATE.Length; i++)
            {
                sum += m_DATE[i];
                total_sum += m_DATE[i];
            }
            DATE.Text = sum + "";
            sum = 0;

            for (int i = 0; i < m_TIME.Length; i++)
            {
                sum += m_TIME[i];
                total_sum += m_TIME[i];
            }
            TIME.Text = sum + "";
            sum = 0;

            for (int i = 0; i < m_MATERIAL.Length; i++)
            {
                sum += m_MATERIAL[i];
                total_sum += m_MATERIAL[i];
            }
            MATERIAL.Text = sum + "";
            sum = 0;

            for (int i = 0; i < m_CIVILIZATION.Length; i++)
            {
                sum += m_CIVILIZATION[i];
                total_sum += m_CIVILIZATION[i];
            }
            CIVILIZATION.Text = sum + "";
            sum = 0;

            for (int i = 0; i < m_EVENT.Length; i++)
            {
                sum += m_EVENT[i];
                total_sum += m_EVENT[i];
            }
            EVENT.Text = sum + "";
            sum = 0;

            for (int i = 0; i < m_PLANT.Length; i++)
            {
                sum += m_PLANT[i];
                total_sum += m_PLANT[i];
            }
            PLANT.Text = sum + "";
            sum = 0;

            for (int i = 0; i < m_QUANTITY.Length; i++)
            {
                sum += m_QUANTITY[i];
                total_sum += m_QUANTITY[i];
            }
            QUANTITY.Text = sum + "";
            sum = 0;

            for (int i = 0; i < m_THEORY.Length; i++)
            {
                sum += m_THEORY[i];
                total_sum += m_THEORY[i];
            }
            THEORY.Text = sum + "";
            sum = 0;

            for (int i = 0; i < m_STUDY_FIELD.Length; i++)
            {
                sum += m_STUDY_FIELD[i];
                total_sum += m_STUDY_FIELD[i];
            }
            STUDY_FIELD.Text = sum + "";
            sum = 0;

            for (int i = 0; i < m_TERM.Length; i++)
            {
                sum += m_TERM[i];
                total_sum += m_TERM[i];
            }
            TERM.Text = sum + "";
            sum = 0;

            for (int i = 0; i < m_ANIMAL.Length; i++)
            {
                sum += m_ANIMAL[i];
                total_sum += m_ANIMAL[i];
            }
            ANIMAL.Text = sum + "";
            sum = 0;
            TOTAL.Text = total_sum + m_ETC + m_PERSON + "";
        }




    }
}
