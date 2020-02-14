using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emart_BrodAgent
{
    class agentConstInfo
    {
        public const string auth_resultOK = "인증에 성공 하셨습니다.";
        public const string auth_resultFasleF = "지문 인증에 실패 하셨습니다. ";
        public const string auth_resultFasleC = "인증에 실패 하였습니다. 아이디 또는 카드가 일치 하지 않습니다. ";


        public const string device_error = "단말기 연결시 문제가 발생 하였습니다.";
        public const string uni_error = "서비스 장애 입니다. 관리자에게 문의 하세요.";


        public const string alert_Message1 = "인증확인";
        public const string alert_Message2 = "인증실패";
        public const string alert_Message3 = "단말기오류";
        //xml 전문 상수값
        public const string xml_login_AuthP = "Card";
        public const string xml_login_AuthF_N = "FingerN";
        public const string xml_login_AuthF_U = "FingerC";
        public const string xml_login = "Login";


        public const string auth_ErrorMessage1 = "서버 IP를 입력해 주세요.";
        public const string auth_ErrorMessage2 = "인증 디바이스를 입력해 주세요.";        
        public const string auth_ErrorMessage4 = "서버 통신 주기를 입력해 주세요(분간격).";
        public const string auth_ErrorMessage5 = "맥 주소를 선택해 주세요.";
        public const string auth_SaveInfo = "설정이 저장하시겠습니까?.";
        public const string auth_SaveInfoRefuse = "초기 설정을 다시 해 주세요.";
        

        public const string reg_path = "SOFTWARE\\atensys_info";
        public const string save_info = "설정값 저장";


        public const string license_errTxt1 = "정상적인 시리얼이 아닙니다.";
        public const string license_errTxt2 = "라이센스 수량 초과 입니다.";
        public const string license_errTxt3 = "라이센스 사용 기간 초과 입니다.";

        public const string contentType_01 = "application/x-www-form-urlencoded";
        public const string contentType_02 = "application/json";

        public const string tel_Const01 = "DIDTYPE01";

        public const string xmlMessageTyepe01 = "SP_BRODSTATE_NEW";
        //public const string xmlMessageTyepe01 = "SP_BRODSTATE";
        public const string xmlMessageTyepe02 = "SP_DIDAUTH";
        //public const string xmlMessageTyepe03 = "SP_BRODSCH";
        public const string xmlMessageTyepe03 = "SP_BRODSCHNEW";
        public const string xmlMessageTyepe04 = "SP_BRODSCHFILELST";
        public const string xmlMessageTyepe05 = "SP_BRODSCHLST";        
        public const string xmlMessageTyepe06 = "SP_BRODDOWNCHECK";
        public const string xmlMessageTyepe07 = "SP_ORDERLST";
        public const string xmlMessageTyepe08 = "SP_DIDREBOOT";

        public const string xmlMessageTyepe09 = "SP_DIDENDTIME";
        public const string xmlMessageTyepe10 = "SP_DIDMONITER";
        public const string xmlMessageTyepe11 = "SP_DIDREBOOTRESULT";
        public const string xmlMessageTyepe12 = "SP_DIDENDTIMERESULT";

        public const string xmlMessageTyepe13 = "SP_DIDRESTART";
        public const string xmlMessageTyepe14 = "SP_DIDSHUTDOWN";
        //public const string xmlMessageTyepe15 = "SP_REBRODSCH";
        public const string xmlMessageTyepe15 = "SP_REBRODSCHNEW";
        public const string xmlMessageTyepe16 = "SP_BASICSCHFILELST";
        public const string xmlMessageTyepe17 = "SP_BASICDOWNCHECK";
        public const string xmlMessageTyepe18 = "SP_BASICSCHLST_NEW";
        public const string xmlMessageTyepe19 = "SP_BASICSCHLST_NEWFILEINFO";
        public const string xmlMessageTyepe20 = "SP_BASICPLAYUPDATE";


        
        public const string xmlPlayXmlInfo = "playFile";


        //인증
        public const string serverJsonUtrl01 = "/backoffice/sub/operManage/jsonAuth.do";
        //서버 시간 가지고 오기 
        public const string serverJsonUtrl02 = "/backoffice/sub/operManage/serverDate.do";
        //xml 파일
        public const string xmlFileName = "mp3fileList.xml"; //다운로드 목록
        public const string xmlBrodFileName = "brodInfo.xml"; //재생 목록 
        public const string xmlBrodBasicName = "brodBasicInfo.xml"; //음원 기본 정보
        public const string xmlBrodOrderList = "brodOrderList.xml"; //명령 정보
        public const string xmlBasicFileList = "basicFileList.xml"; //기초음원 정보
        public const string xmlBasicFileList_New = "basicFileListNew.xml"; //기초음원 정보
        public const string xmlBasicBrodList_New = "basicBrodListNew.xml"; //기초음원 정보

        public const string xmlText = "<?xml version=\"1.0\" standalone=\"yes\"?>\r\n"
                                    + "<dataset>\r\n"
                                    + "</dataset>\r\n";

        public const string xmlPlayText = "<?xml version=\"1.0\" standalone=\"yes\"?>\r\n"
                                    + "<dataset>\r\n"
                                    + "   <filePlayList>\r\n"
                                    + "   </filePlayList>\r\n"
                                    + "</dataset>\r\n";

    }
}
