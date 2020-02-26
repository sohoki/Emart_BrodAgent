using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;


namespace sohoUniLib
{
    public class ServerComm_json
    {
        public static string sp_JsonString(didInfo did_info, string _messageGubun)
        {

            string _returnMessage = string.Empty;
            try
            {

                switch (_messageGubun)
                {
                    case "SP_BRODSTATE_NEW":
                        _returnMessage = "{\"command_type\":\"SP_BRODSTATE_NEW\",\"command_data\":[{\"DID_ID\":\"" + did_info.didId + "\"," 
                                       +  "\"DID_MAC\":\"" + did_info.didMac + "\"}]}";
                        break;
                    case "SP_BRODSTATE":
                        _returnMessage = "{\"command_type\":\"SP_BRODSTATE\",\"command_data\":[{\"DID_ID\":\"" + did_info.didId + "\","
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\"}]}";
                        break;
                    case "SP_DIDAUTH":
                        _returnMessage = "{\"command_type\":\"SP_DIDAUTH\",\"command_data\":[{\"DID_ID\":\"" + did_info.didId + "\"," 
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\"," 
                                       + "\"DID_IP\":\"" + did_info.didIpAddress + "\"," 
                                       + "\"DID_INTERVAL\":\"" + did_info.didInterval + "\"," 
                                       + "\"DID_TYPE\":\"" + did_info.didType + "\"}]}";
                        break;
                    case "SP_BRODSCH":
                        _returnMessage = "{\"command_type\":\"SP_BRODSCH\",\"command_data\":[{\"DID_ID\":\"" + did_info.didId + "\","
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\"}]}";
                        break;
                    case "SP_BRODSCHNEW":
                        _returnMessage = "{\"command_type\":\"SP_BRODSCHNEW\",\"command_data\":[{\"DID_ID\":\"" + did_info.didId + "\","
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\"}]}";
                        break;
                    //DID 단말기
                    case "SP_DIDSTATE":
                        _returnMessage = "{\"command_type\":\"SP_DIDSTATE\",\"command_data\":[{\"DID_ID\":\"" + did_info.didId + "\"," 
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\"}]}";
                        break;
                    //콘텐츠 리스트 받기
                    case "SP_DIDCONTENTLST":
                        _returnMessage = "{\"command_type\":\"SP_DIDCONTENTLST\",\"command_data\":[{\"DID_ID\":\"" + did_info.didId + "\","
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\"}]}";
                        break;
                    case "SP_DIDCONTENTFILELST": //파일 다운로드
                        _returnMessage = "{\"command_type\":\"SP_DIDCONTENTFILELST\",\"command_data\":[{\"DID_ID\":\"" + did_info.didId + "\","
                                       + "\"SCH_CODE\":\"" + did_info.schCode + "\"}]}";
                        break;
                    case "SP_REBRODSCH":
                        _returnMessage = "{\"command_type\":\"SP_REBRODSCH\",\"command_data\":[{\"DID_ID\":\"" + did_info.didId + "\","
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\"}]}";
                        break;
                    case "SP_REBRODSCHNEW":
                        _returnMessage = "{\"command_type\":\"SP_REBRODSCHNEW\",\"command_data\":[{\"DID_ID\":\"" + did_info.didId + "\","
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\"}]}";
                        break;
                    case "SP_BRODSCHFILELST":
                        _returnMessage = "{\"command_type\":\"SP_BRODSCHFILELST\",\"command_data\":[{\"BROD_CODE\":\"" + did_info.brodCode + "\","
                                       + "\"DID_ID\":\"" + did_info.didId + "\","
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\"}]}";
                        break;
                    case "SP_BRODCONTENTLSTUPDATECHECK":
                        _returnMessage = "[{\"DID_ID\":\"" + did_info.didId + "\"," 
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\"," 
                                       + "\"HIS_SEQ\":\"" + did_info.etcMessage + "\"}]";
                        break;
                    case "SP_BRODSCHLST":
                        _returnMessage = "{\"command_type\":\"SP_BRODSCHLST\",\"command_data\":[{\"DID_ID\":\"" + did_info.didId + "\","
                                       + " \"DID_MAC\":\"" + did_info.didMac + "\","
                                       + "\"BROD_CODE\":\"" + did_info.brodCode + "\"}]}";
                        break;
                    case "SP_BRODDOWNCHECK":
                        _returnMessage = "{\"command_type\":\"SP_BRODDOWNCHECK\",\"command_data\":[{\"CENTER_ID\":\"" + did_info.centerId + "\","
                                       + " \"BROD_CODE\":\"" + did_info.brodCode + "\","
                                       + "\"DID_DOWNCHECK\":\"" + did_info.didDownCheck + "\"}]}";
                        break;
                    case "SP_ORDERLST":
                        _returnMessage = "{\"command_type\":\"SP_ORDERLST\",\"command_data\":[{\"DID_ID\":\"" + did_info.didId + "\","
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\"}]}";
                        break;
                        
                    case "SP_DIDREBOOT":   //재부팅 
                        _returnMessage = "{\"command_type\":\"SP_DIDREBOOT\",\"command_data\": [{\"DID_ID\":\"" + did_info.didId + "\","
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\"}]}";
                        break;
                    case "SP_DIDENDTIME": //종료 시간 설정 
                        _returnMessage = "{\"command_type\":\"SP_DIDENDTIME\",\"command_data\": [{\"DID_ID\":\"" + did_info.didId + "\"," 
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\"," 
                                       + "\"DID_ENDTIME\":\"" + did_info.didEndTime + "\"}]}";
                        break;
                    case "SP_DIDMONITER":  //파일 업로드 이기 때문에 다른 방법으로 다시 하기 
                        _returnMessage = "[{\"DID_NM\":\"" + did_info.didId + "\"," 
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\"," 
                                       + "\"MSG_SEQ\":\"" + did_info.msgSeq + "\"," 
                                       + "\"DID_PICTURE\":\"" + did_info.etcFileNm + "\"}]";
                        break;
                    case "SP_DIDREBOOTRESULT":  //재시작 후 전송 
                        _returnMessage = "{\"command_type\":\"SP_DIDREBOOTRESULT\",\"command_data\": [{\"DID_ID\":\"" + did_info.didId + "\"," 
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\"," 
                                       + "\"ERROR_MESSAGE\":\"" + did_info.errorMessage + "\"," 
                                       + "\"MSG_SEQ\":\"" + did_info.msgSeq + "\"}]}";
                        break;
                    case "SP_DIDENDTIMERESULT":  // 종료시간 변경 후 서버 전송
                        _returnMessage = "{\"command_type\":\"SP_DIDENDTIMERESULT\",\"command_data\":[{\"DID_ID\":\"" + did_info.didId + "\","
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\","
                                       + "\"ERROR_MESSAGE\":\"" + did_info.errorMessage + "\","
                                       + "\"MSG_SEQ\":\"" + did_info.msgSeq + "\"}]}";
                        break;
                    case "SP_DIDCONTENTFILELSTUPDATECHECK":  // 종료시간 변경 후 서버 전송
                        _returnMessage = "{\"command_type\":\"SP_DIDCONTENTFILELSTUPDATECHECK\",\"command_data\": [{\"DID_ID\":\"" + did_info.didId + "\","
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\","
                                       + "\"HIS_SEQ\":\"" + did_info.hisSeq + "\"}]}";
                        break;
                    case "SP_DIDCONTENTLSTUPDATECHECK":  // 종료시간 변경 후 서버 전송
                        _returnMessage = "{\"command_type\":\"SP_DIDCONTENTLSTUPDATECHECK\",\"command_data\": [{\"DID_ID\":\"" + did_info.didId + "\","
                                       + "\"DID_MAC\":\"" + did_info.didMac + "\","
                                       + "\"HIS_SEQ\":\"" + did_info.hisSeq + "\"}]}";
                        break;
                    case "SP_NEXTCONTENTINFO":  // 종료시간 변경 후 서버 전송
                        _returnMessage = "{\"command_type\":\"SP_NEXTCONTENTINFO\",\"command_data\": [{\"CON_SEQ\":\"" + did_info.conNextSeq + "\"}]}";
                        break;
                    case "SP_NEXTCONTENTFILELST":  // 종료시간 변경 후 서버 전송
                        _returnMessage = "{\"command_type\":\"SP_NEXTCONTENTFILELST\",\"command_data\": [{\"CON_SEQ\":\"" + did_info.conNextSeq + "\"}]}";
                        break;
                    case "SP_BASICSCHFILELST":  //기초 음원 파일 
                        _returnMessage = "{\"command_type\":\"SP_BASICSCHFILELST\",\"command_data\": [{\"BASIC_CODE\":\"" + did_info.basicCode + "\"}]}";
                        break;
                    case "SP_BASICDOWNCHECK":
                        _returnMessage = "{\"command_type\":\"SP_BASICDOWNCHECK\",\"command_data\": [{\"CENTER_ID\":\"" + did_info.centerId + "\","
                                       + "\"BASIC_CODE\":\"" + did_info.basicCode + "\","
                                       + "\"DID_DOWNCHECK\":\"Y\"}]}";
                        break;
                    case "SP_BASICSCHLST_NEW":
                        _returnMessage = "{\"command_type\":\"SP_BASICSCHLST_NEW\",\"command_data\": [{\"CENTER_ID\":\"" + did_info.centerId + "\","
                                       + "\"BASIC_CODE\":\"" + did_info.basicCode + "\","
                                       + "\"DID_DOWNCHECK\":\"Y\"}]}";
                        break;
                    case "SP_BASICSCHLST_NEWFILEINFO":
                        _returnMessage = "{\"command_type\":\"SP_BASICSCHLST_NEWFILEINFO\",\"command_data\": [{\"BASIC_CODE\":\"" + did_info.basicCode + "\"}]}";
                        break;
                    case "SP_BASICSCHFILELST_INFO":
                        _returnMessage = "{\"command_type\":\"SP_BASICSCHFILELST_INFO\",\"command_data\": [{\"BASIC_CODE\":\"" + did_info.basicCode + "\"}]}";
                        break;


                }


            }
            catch (Exception ex)
            {
                uniUtil.setLogFileS("sp_JsonString error:" + ex.ToString());
            }
            return _returnMessage;
        }
        public static string jsonResult(string _jsonTxt)
        {
            

            string _returlMessage = string.Empty;

            try
            {
                JObject json = JObject.Parse(_jsonTxt);

                switch (json["command_type"].ToString())
                {
                    case "SP_DIDAUTH":
                        _returlMessage = json["result"].ToString();
                        break;
                    case "SP_BRODSTATE_NEW":
                        _returlMessage = json["result"]["BROD_CNT"].ToString() + "/" + json["result"]["ORD_CNT"].ToString() + "/" + json["result"]["MSG_CNT"].ToString() + "/" + json["result"]["BASIC_CNT"].ToString();
                        break;
                    case "SP_BRODSTATE_NEW_VERSION":
                        _returlMessage = json["result"]["BROD_CNT"].ToString() + "/" + json["result"]["ORD_CNT"].ToString() + "/" + json["result"]["MSG_CNT"].ToString() + "/" + json["result"]["BASIC_CNT"].ToString() + "/" + json["result"]["VERSION_INFO"].ToString();
                        break;
                    case "SP_BRODSTATE":
                        _returlMessage = json["result"]["BROD_CNT"].ToString() + "/" + json["result"]["ORD_CNT"].ToString() + "/" + json["result"]["MSG_CNT"].ToString();
                        break;
                    case "SP_DIDSTATE":
                        _returlMessage = json["result"]["SCH_CNT"].ToString() + "|" + json["result"]["ORD_CNT"].ToString() + "|" + json["result"]["MSG_CNT"].ToString();
                        break;                        
                    case "SP_BRODSCH":
                        _returlMessage = json["BRODINFO"][0]["BROD_CODE"].ToString() + "|" + json["BRODINFO"][0]["CENTER_STARTTIME"].ToString() + "|" + json["BRODINFO"][0]["CENTER_ENDTIME"].ToString() + "|" + json["BRODINFO"][0]["CENTER_ID"].ToString();
                        break;
                    case "SP_BRODSCHNEW":
                        _returlMessage = json["BRODINFO"][0]["BROD_CODE"].ToString() + "|" + json["BRODINFO"][0]["CENTER_STARTTIME"].ToString() + "|" + json["BRODINFO"][0]["CENTER_ENDTIME"].ToString() + "|" + json["BRODINFO"][0]["CENTER_ID"].ToString();
                        break;
                    case "SP_REBRODSCH":
                        _returlMessage = json["BRODINFO"][0]["BROD_CODE"].ToString() + "|" + json["BRODINFO"][0]["CENTER_STARTTIME"].ToString() + "|" + json["BRODINFO"][0]["CENTER_ENDTIME"].ToString() + "|" + json["BRODINFO"][0]["CENTER_ID"].ToString();
                        break;
                    case "SP_REBRODSCHNEW":
                        _returlMessage = json["BRODINFO"][0]["BROD_CODE"].ToString() + "|" + json["BRODINFO"][0]["CENTER_STARTTIME"].ToString() + "|" + json["BRODINFO"][0]["CENTER_ENDTIME"].ToString() + "|" + json["BRODINFO"][0]["CENTER_ID"].ToString();
                        break;
                    case "SP_BRODDOWNCHECK":
                        _returlMessage = json["result"].ToString();
                        break;
                    case "SP_BASICDOWNCHECK":
                        _returlMessage = json["result"].ToString();
                        break;                        
                    case "SP_DIDREBOOTRESULT":
                        _returlMessage = json["result"].ToString();
                        break;
                    case "SP_BASICPLAYUPDATE":
                        _returlMessage = json["result"].ToString();
                        break;
                    //콘텐츠 리스트 받아오기 
                    case "SP_DIDCONTENTLST":
                        uniUtil.setLogFileS("CONINFO:" +json["CONINFO"].ToString() + ":" + json["CONINFO"].ToString().Length);
                        if (json["CONINFO"].ToString().Length > 3)
                        {
                            _returlMessage = json["CONINFO"][0]["CON_USEYN"].ToString() + "|" + json["CONINFO"][0]["CON_LOCALFILE"].ToString() + "|" + json["CONINFO"][0]["CON_NEXTSEQ"].ToString() + "|" + json["CONINFO"][0]["SCH_EMERGUBUN"].ToString() + "|" + json["CONINFO"][0]["HIS_SEQ"].ToString() + "|" + json["CONINFO"][0]["SCH_CODE"].ToString();
                        }
                        else
                        {
                            _returlMessage = "ERROR";
                        }
                        break;
                    case "SP_DIDCONTENTFILELST":
                        _returlMessage = json["CONINFO"].ToString() ;
                        break;
                    case "SP_DIDCONTENTLSTUPDATECHECK": 
                        _returlMessage = json["result"].ToString();
                        break;
                    case "SP_DIDCONTENTFILELSTUPDATECHECK":
                        _returlMessage = json["result"].ToString();
                        break;
                    case "SP_NEXTCONTENTINFO":
                        _returlMessage = json["CONINFO"][0]["CON_USEYN"].ToString() + "|" + json["CONINFO"][0]["CON_FILE"].ToString() + "|" + json["CONINFO"][0]["CON_NEXTSEQ"].ToString() + "|" + json["CONINFO"][0]["CON_SEQ"].ToString();
                        break;
                    case "SP_NEXTCONTENTFILELST":
                        _returlMessage = json["CONINFO"].ToString();
                        break;
                    case "SP_BASICSCHFILELST":
                        _returlMessage = json["FILEINFO"].ToString();
                        break;

                }
            }
            catch(Exception e)
            {
                uniUtil.setLogFileS("jsonResult error:" + e.ToString());
            }


            return _returlMessage;
        }
        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }
        public static DataTable jsonArrayList(string _jsonTxt, string _arrayPivot)
        {
            JArray filearray = new JArray();

            DataTable dt = null;

            try
            {
                var resultObjects = AllChildren(JObject.Parse(_jsonTxt))
                            .First(c => c.Type == JTokenType.Array && c.Path.Contains(_arrayPivot))
                            .Children<JObject>();
                foreach (JObject result in resultObjects)
                {
                    JObject row = new JObject();
                    foreach (JProperty property in result.Properties())
                    {
                        row.Add(property.Name.ToString(), property.Value.ToString());                        
                    }
                    filearray.Add(row);
                    row = null;
                }
                dt = JsonConvert.DeserializeObject<DataTable>(filearray.ToString());
            }
            catch(Exception e)
            {
                uniUtil.setLogFileS("jsonArrayList error:" + e.ToString());                
            }
            filearray = null;
            return dt;
        }
    }

    public class didInfo
    {
        public string didId { get; set; }
        public string didIp { get; set; }
        public string didInterval { get; set; }
        public string didMac { get; set; }
        public string didType { get; set; }
        public string didIpAddress { get; set; }
        public string didEndTime { get; set; }
        public string etcMessage { get; set; }
        public string etcFileNm { get; set; }
        public string msgSeq { get; set; }
        public string errorMessage { get; set; }
        public string brodCode { get; set; }
        public string centerId { get; set; }
        public string didDownCheck { get; set; }
        public string basicMp3 { get; set; }
        public string schCode {get; set; }
        public string localFile { get; set; }
        public string hisSeq { get; set; }
        public string conNextSeq { get; set; }
        public string basicCode { get; set; }
        public string conScreen { get; set; }
        public string didVersion { get; set; }
            

    }
    public class fileDownInfo
    {
        public string streFileNm { get; set; }
        public string fileStreCours { get; set; }
        public string atchFileId { get; set; }   
        
    }
    public class brodReport
    {
        public string brodSeq { get; set; }
        public string streFileNm { get; set; }
        public string brodTime { get; set; }
    }
}
