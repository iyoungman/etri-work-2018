using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;

namespace EtriWork
{
    class StatusSaveToExcel
    {
        private string FILE_PATH = "C:\\Users\\chunso\\Desktop\\Temp\\통계파일";
        private Excel.Application objApp;
        private Excel.Workbooks objWorkbooks;
        private Excel.Workbook objWorkbook;
        private Excel.Sheets objWorksheets;
        private Excel.Worksheet objWorksheet;
        private Excel.Range range;

        private StatusSeosulDto seosulDto;
        private StatusETRUDto etriDto;
        private StatusQTDto qtDto;
        private StatusSATDto satDto;



        public StatusSaveToExcel(StatusETRUDto statusETRUDto, StatusQTDto statusQTDto, StatusSATDto statusSATDto, StatusSeosulDto statusSeosulDto)
        {
            this.seosulDto = statusSeosulDto;
            this.etriDto = statusETRUDto;
            this.qtDto = statusQTDto;
            this.satDto = statusSATDto;
        }

        public string saveFile()
        {
            bool excelOpen = false; var missing = Type.Missing;
            int rowCnt = 20, columnCnt = 10;
            object[,] sheetValue = new object[rowCnt, columnCnt];

            #region 데이터 입력
            sheetValue[0, 0] = "파일 통계";
            
            sheetValue[1, 0] = "총개수";
            sheetValue[1, 1] = etriDto.totalCnt;
            
            sheetValue[2, 2] = "전체평균";
            sheetValue[2, 3] = "표준편차";
            sheetValue[2, 5] = "개수";
            sheetValue[3, 0] = "시간";
            sheetValue[3, 2] = etriDto.timeAve;
            sheetValue[3, 3] = etriDto.timeDev;
            sheetValue[3, 5] = etriDto.timeCnt;
            sheetValue[4, 0] = "Etri 질문유형";
            sheetValue[4, 2] = etriDto.etriQTAve;
            sheetValue[4, 3] = etriDto.etriQTDev;
            sheetValue[4, 5] = etriDto.etriQTCnt;
            sheetValue[5, 0] = "Etri 질문초점";
            sheetValue[5, 2] = etriDto.etriQFAve;
            sheetValue[5, 3] = etriDto.etriQFDev;
            sheetValue[5, 5] = etriDto.etriQFCnt;
            sheetValue[6, 0] = "Etri LAT";
            sheetValue[6, 2] = etriDto.etriLATAve;
            sheetValue[6, 3] = etriDto.etriLATDev;
            sheetValue[6, 5] = etriDto.etriLATCnt;
            sheetValue[7, 0] = "Etri SAT";
            sheetValue[7, 2] = etriDto.etriSATAve;
            sheetValue[7, 3] = etriDto.etriSATDev;
            sheetValue[7, 5] = etriDto.etriSATCnt;

            sheetValue[8, 2] = "비율";
            sheetValue[8, 3] = "개수";
            sheetValue[9, 0] = "단답형";
            sheetValue[9, 2] = qtDto.dandabRto;
            sheetValue[9, 3] = qtDto.dandabCnt;
            sheetValue[10, 0] = "나열형";
            sheetValue[10, 2] = qtDto.nayulRto;
            sheetValue[10, 3] = qtDto.nayulCnt;
            sheetValue[11, 0] = "서술형";
            sheetValue[11, 2] = qtDto.seosulRto;
            sheetValue[11, 3] = qtDto.seosulCnt;

            sheetValue[13, 0] = "서술형-정의";
            sheetValue[13, 2] = seosulDto.defineRto;
            sheetValue[13, 3] = seosulDto.defineCnt;
            sheetValue[14, 0] = "서술형-이유";
            sheetValue[14, 2] = seosulDto.reasonRto;
            sheetValue[14, 3] = seosulDto.reasonCnt;
            sheetValue[15, 0] = "서술형-방법";
            sheetValue[15, 2] = seosulDto.wayRto;
            sheetValue[15, 3] = seosulDto.wayCnt;
            sheetValue[16, 0] = "서술형-목적";
            sheetValue[16, 2] = seosulDto.purposeRto;
            sheetValue[16, 3] = seosulDto.purposeCnt;
            sheetValue[17, 0] = "서술형-조건";
            sheetValue[17, 2] = seosulDto.conditionRto;
            sheetValue[17, 3] = seosulDto.conditionCnt;
            sheetValue[18, 0] = "서술형-기타";
            sheetValue[18, 2] = seosulDto.etcRto;
            sheetValue[18, 3] = seosulDto.etcCnt;
            sheetValue[19, 0] = "서술형-의미";
            sheetValue[19, 2] = seosulDto.meanRto;
            sheetValue[19, 3] = seosulDto.meanCnt;

            sheetValue[0, 8] = "SAT";
            sheetValue[1, 9] = "개수";
            sheetValue[2, 8] = "PS_NAME";
            sheetValue[2, 9] = satDto.PS_NAME;
            sheetValue[3, 8] = "ETC";
            sheetValue[3, 9] = satDto.ETC;
            sheetValue[4, 8] = "LOCATION";
            sheetValue[4, 9] = satDto.LOCATION;
            sheetValue[5, 8] = "ORGANIZATION";
            sheetValue[5, 9] = satDto.ORGANIZATION;
            sheetValue[6, 8] = "ARTIFACTS";
            sheetValue[6, 9] = satDto.ARTIFACTS;
            sheetValue[7, 8] = "DATE";
            sheetValue[7, 9] = satDto.DATE;
            sheetValue[8, 8] = "TIME";
            sheetValue[8, 9] = satDto.TIME;
            sheetValue[9, 8] = "CIVILIZATION";
            sheetValue[9, 9] = satDto.CIVILIZATION;
            sheetValue[10, 8] = "EVENT";
            sheetValue[10, 9] = satDto.EVENT;
            sheetValue[11, 8] = "PLANT";
            sheetValue[11, 9] = satDto.PLANT;
            sheetValue[12, 8] = "QUANTITY";
            sheetValue[12, 9] = satDto.QUANTITY;
            sheetValue[13, 8] = "THEORY";
            sheetValue[13, 9] = satDto.THEORY;
            sheetValue[14, 8] = "STUDY_FIELD";
            sheetValue[14, 9] = satDto.STUDY_FIELD;
            sheetValue[15, 8] = "TERM";
            sheetValue[15, 9] = satDto.TERM;
            sheetValue[16, 8] = "ANIMAL";
            sheetValue[16, 9] = satDto.ANIMAL;
            #endregion

            try
            {
                excelOpen = true;
                objApp = new Excel.Application();
                objWorkbooks = objApp.Workbooks;

                objWorkbook = objWorkbooks.Add(missing);
                objWorksheets = objWorkbook.Worksheets;

                objWorksheet = (Excel.Worksheet)objWorksheets.get_Item(1);
                objWorksheet.Name = "통계";

                Excel.Range c1 = objWorksheet.Cells[2, 1];
                Excel.Range c2 = objWorksheet.Cells[rowCnt + 1, columnCnt];
                range = objWorksheet.get_Range(c1, c2);
                range.Value = sheetValue;
                Marshal.FinalReleaseComObject(c1);
                Marshal.FinalReleaseComObject(c2);
                Marshal.FinalReleaseComObject(range);

                Marshal.ReleaseComObject(objWorksheet);

                string savePath = Path.ChangeExtension(FILE_PATH, "xlsx");
                FileInfo fi = new FileInfo(savePath);
                if (fi.Exists)
                {
                    fi.Delete();
                }

                objWorkbook.SaveAs(savePath, Excel.XlFileFormat.xlOpenXMLWorkbook,
                missing, missing, false, false, Excel.XlSaveAsAccessMode.xlNoChange,
                Excel.XlSaveConflictResolution.xlUserResolution, true, missing, missing, missing);

                objWorkbook.Close(false, missing, missing);
                objWorkbooks.Close();
                objApp.Quit();

                Marshal.FinalReleaseComObject(objWorkbook);
                Marshal.FinalReleaseComObject(objWorkbooks);
                Marshal.FinalReleaseComObject(objApp);

                objApp = null;
                excelOpen = false;
            }
            catch (Exception e)
            {
                if (excelOpen)
                {
                    Marshal.FinalReleaseComObject(range);
                    Marshal.FinalReleaseComObject(objWorksheet);

                    Marshal.FinalReleaseComObject(objWorksheets);

                    objWorkbook.Close(false, missing, missing);
                    objWorkbooks.Close();
                    objApp.Quit();

                    Marshal.FinalReleaseComObject(objWorkbook);
                    Marshal.FinalReleaseComObject(objWorkbooks);
                    Marshal.FinalReleaseComObject(objApp);

                    objApp = null;
                }
                return e.ToString();
            }
            return "Excel파일로 출력하였습니다.";
        }

    }
}
