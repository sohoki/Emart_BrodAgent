using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sohoUniLib
{
    public class UtilConstant
    {
        

        public const string EncryptPassString = "sohoki_agent";

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
        public const string auth_ErrorMessage2 = "인증 디바이스를 선택해 주세요.";
        public const string auth_ErrorMessage3 = "인증 디바이스를 선택해 주세요.";
        public const string auth_ErrorMessage4 = "서버 통신 주기를 입력해 주세요(분간격).";
        public const string auth_ErrorMessage5 = "맥 주소를 선택해 주세요.";
        public const string auth_SaveInfo = "설정이 저장하시겠습니까?.";
        public const string auth_SaveInfoRefuse = "초기 설정을 다시 해 주세요.";
        public const string auth_UserIdMessage = "사번을 입력해 주세요.";
        public const string auth_ResultRefusedMessage = "잘못된 사번 이거나 이미 예약 중인 PC입니다.";

        public const string reg_path = "SOFTWARE\\atensys_info";
        public const string save_info = "설정값 저장";


        public const string license_errTxt1 = "정상적인 시리얼이 아닙니다.";
        public const string license_errTxt2 = "라이센스 수량 초과 입니다.";
        public const string license_errTxt3 = "라이센스 사용 기간 초과 입니다.";

        public const string cmdType01 = "SP_DIDSTATE";   //상태값
        public const string cmdType02 = "SP_DIDAUTH";    //단말기 인증
        public const string cmdType03 = "SP_DIDCONTENTLST";   //콘텐츠 리스트
        public const string cmdType04 = "SP_DIDCONTENTLSTUPDATECHECK"; //콘텐츠 리스트 업데이트 확인        
        public const string cmdType05 = "SP_DIDCONTENTFILELST";            //콘텐츠 파일 리스트
        public const string cmdType06 = "SP_DIDCONTENTFILELSTUPDATECHECK"; //콘텐츠 파일 리스트 다운로드 확인
        public const string cmdType07 = "SP_DIDREBOOT";                    //서버 요청사항 - 재부팅
        public const string cmdType8 = "SP_DIDENDTIME";                   //서버 요청사항 - 종료시간 설정
        public const string cmdType09 = "SP_DIDMONITER";                   //서버 요청사항 - 화면캡처
        public const string cmdType10 = "SP_DIDREBOOTRESULT";              //서버 요청사항 - 재부팅 완료
        public const string cmdType11 = "SP_DIDENDTIMERESULT";             //서버 요청사항 - 종료시간 설정 완료
        




    }
}
