using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace sohoUniLib
{
    public class uniXml
    {


        public XmlElement subNode(XmlDocument xmldoc, string _subNode, string _nodeName, string _value)
        {
            XmlElement score = xmldoc.CreateElement(_nodeName);
            score.InnerXml = _value;
            return score;
        }
        public void delNode(int index, string _filePath, string _nodeNm)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(_filePath);
            XmlNodeList bookNodes = XmlDoc.DocumentElement.SelectNodes(_nodeNm);
            bookNodes[index].ParentNode.RemoveChild(bookNodes[index]);
            XmlDoc.Save(_filePath);
            XmlDoc = null;

        }
        public void delNodeAll(string _filePath, string _nodeNm)
        {
            uniUtil util = new uniUtil();
            XmlDocument XmlDoc = new XmlDocument();
           
            try
            {
                XmlDoc.Load(_filePath);
                XmlNodeList bookNodes = XmlDoc.DocumentElement.SelectNodes(_nodeNm);
                for (int i = 0; i < bookNodes.Count; i++)
                {
                    bookNodes[i].ParentNode.RemoveChild(bookNodes[i]);
                }
                XmlDoc.Save(_filePath);
            }
            catch(IOException e1)
            {
                util.setLogFile("delNodeAll error:" + e1.ToString());
                string xmlText = "<?xml version=\"1.0\" standalone=\"yes\"?>\r\n"
                                    + "<dataset>\r\n"
                                    + "</dataset>\r\n";
                //파일 생성
                util.fileCreate(_filePath, _filePath.Substring(_filePath.LastIndexOf("\\") + 1), xmlText);
            }
            catch(Exception e)
            {
                util.setLogFile("delNodeAll error:" + e.ToString());
              
            }
            
            XmlDoc = null;
            util = null;

        }
        public string[] xmlInfoLst(string _fullFileNm, string _nodePath)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(_fullFileNm);
            int i = 0;
            string[] filelist = new string[XmlDoc.SelectNodes(@"" + _nodePath).Count];
            foreach(XmlNode node in XmlDoc.SelectNodes("//fileList"))
            {
                filelist[i++] = node["file_name"].InnerText;
            }
            return filelist;
        }
        protected XmlNode CreateNode(XmlDocument doc, string _name, string innerxml)
        {
            XmlNode node = doc.CreateElement(string.Empty, _name, string.Empty);
            node.InnerXml = innerxml;
            return node;
        }
        //노드값 변경 
        public bool xmlNodeChange(string _filePath, string _mp3Nm, string _result)
        {
            uniUtil util = new uniUtil();
            bool return_check = false;
            try
            {
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.Load(_filePath);

                XmlNode findNode = XmlDoc.SelectSingleNode(@"/dataset/fileList/file_name[text() = '" + _mp3Nm + "']");
                findNode.Attributes["download_result"].Value = _result;
                if (_result.Equals("Y"))
                {
                    findNode.Attributes["download_date"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }                
                XmlDoc.Save(_filePath);
                XmlDoc = null;
            }
            catch (Exception e)
            {
                return_check = false;
                util.setLogFile("xmlNodeUpdate error:" + e.ToString());
            }
            util = null;
            return return_check;
        }
        public bool xmlNodeChangeUni(string _filePath, string _singleNode, string _findNode, string _findeNm, string _changeNodeNM)
        {
            uniUtil util = new uniUtil();
            bool return_check = false;
            try
            {
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.Load(_filePath);

                //Convert.ToString(Convert.ToInt32(nodeCnt) + 1)

                XmlNode findNode = XmlDoc.SelectSingleNode(@"/dataset/" + _singleNode + "/" + _findNode + "[text() = '" + _findeNm + "']");
                findNode.Attributes["playCnt"].Value = Convert.ToString(Convert.ToInt32(findNode.Attributes["playCnt"].Value) + 1);
                XmlDoc.Save(_filePath);
                XmlDoc = null;
            }
            catch (Exception e)
            {
                return_check = false;
                util.setLogFile("xmlNodeUpdate error:" + e.ToString());
            }
            util = null;
            return return_check;
        }
        public int xmlNodeCount(string _filePath, string _nodePath)
        {
            uniUtil util = new uniUtil();
            try
            {                
                XmlDocument readDoc = new XmlDocument();
                readDoc.Load(_filePath);                
                return readDoc.SelectNodes(@""+_nodePath).Count;
            }
            catch(Exception e)
            {
                util.setLogFile("xmlNodeCount error:" + e.ToString());
                return 0;
            }
            
            
        }
        public bool xmlAgentInfoCreateNode(string _filePath, string _brodCode, string _centerStartTime, string _centerEndTime, string _streFileNm, string _centerId)
        {
            XmlDocument XmlDoc = new XmlDocument();
            uniUtil util = new uniUtil();
            bool return_check = false;
            try
            {
                XmlDoc.Load(_filePath);
                XmlNode newNode;
                newNode = XmlDoc.SelectSingleNode("dataset");

                XmlElement subNode = XmlDoc.CreateElement("brodBasicInfo");
                XmlElement score = XmlDoc.CreateElement("BROD_CODE");
                score.InnerXml = _brodCode;
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("CENTER_STARTTIME");
                score.InnerXml = _centerStartTime;
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("CENTER_ENDTIME");
                score.InnerXml = _centerEndTime;
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("STRE_FILE_NM");
                score.InnerXml = _streFileNm;
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("CENTER_ID");
                score.InnerXml = _centerId;
                subNode.AppendChild(score);
                newNode.AppendChild(subNode);
                XmlDoc.Save(_filePath);
                return_check = true;
            }
            catch (Exception e)
            {
                return_check = false;
                util.setLogFile("xmlAgentInfoCreateNode error:" + e.ToString());
            }
            util = null;
            XmlDoc = null;
            return return_check;
        }
        public bool xmlOrderCreateNode(string _filePath, string _msgSeq, string _xmlProcessNm, string _ProcessCk)
        {
            XmlDocument XmlDoc = new XmlDocument();
            uniUtil util = new uniUtil();
            bool return_check = false;
            try
            {
                XmlDoc.Load(_filePath);
                XmlNode newNode;
                newNode = XmlDoc.SelectSingleNode("dataset");

                XmlElement subNode = XmlDoc.CreateElement("orderList");
                XmlElement score = XmlDoc.CreateElement("MSG_SEQ");
                score.InnerXml = _msgSeq;
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("XML_PROCESS_NAME");
                score.InnerXml = _xmlProcessNm;
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("PROCESSCHECK");
                score.InnerXml = _ProcessCk;
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("SENDDATE");
                score.InnerXml = "";
                subNode.AppendChild(score);
                newNode.AppendChild(subNode);
                XmlDoc.Save(_filePath);
                return_check = true;
            }
            catch (Exception e)
            {
                return_check = false;
                util.setLogFile("xmlNodeUpdate error:" + e.ToString());
            }
            util = null;
            XmlDoc = null;
            return return_check;
        }
        public string basicPlayList(string _filePath, string _playOder)
        {
            XDocument doc = XDocument.Load(_filePath);
            uniUtil util = new uniUtil();
            var brodFiles = (from brodFile in doc.Descendants("fileList")
                             where brodFile.Element("groupTimeGubun").Value == "TIME_INPUT_2" &&
                                     Convert.ToInt32(brodFile.Element("strartTime").Value) <= Convert.ToInt32(DateTime.Now.ToString("HHmm"))
                                     && Convert.ToInt32(brodFile.Element("endTime").Value) >=
                                     Convert.ToInt32(DateTime.Now.ToString("HHmm"))
                             orderby Convert.ToInt32(brodFile.Element("playOrder").Value) ascending
                             select new
                             {
                                 FileNm = brodFile.Element("file_name").Value,
                                 Order = brodFile.Element("playOrder").Value
                             }).ToList();// .ToList();
           
            if (brodFiles.Count < 1)
            {

               
                brodFiles = (from brodFile in doc.Descendants("fileList")
                             where brodFile.Element("groupTimeGubun").Value == "TIME_INPUT_1"
                             orderby Convert.ToInt32(brodFile.Element("playOrder").Value) ascending
                             select new
                             {
                                 FileNm = brodFile.Element("file_name").Value,
                                 Order = brodFile.Element("playOrder").Value
                             }).ToList();
            }
            string fileNm = string.Empty;
            foreach (var brodFile in brodFiles)
            {

              
                if (Convert.ToInt32(brodFile.Order) > Convert.ToInt32(_playOder))
                {
                    fileNm = brodFile.FileNm + "|" + brodFile.Order;
                    break;
                }
                else
                {
                    fileNm = brodFiles[0].FileNm + "|" + brodFiles[0].Order;
                }

            }
            return fileNm;
        }
        //플레이 카운터 설정
        public void BasicPlayInfo(string _filePath, string _playFile, string _xmlTxt)
        {
            string fileInfo = _filePath + DateTime.Now.ToString("yyyyMMdd") + ".xml";
            //플레이 정보 기록 하기 
            uniUtil util = new uniUtil();
            util.setLogFile("playFile:" + _playFile);
            if (util.fileCreate(_filePath, DateTime.Now.ToString("yyyyMMdd") + ".xml", _xmlTxt) == true)
            {
                string nodeCnt = xmlNodeRead(fileInfo, "filePlayList", "playFile", _playFile);
                //Console.Write("nodeCnt:" + nodeCnt);
                if (nodeCnt == "")
                {
                    xmlCreateNodeAttr("filePlayList", fileInfo, "playFile", _playFile, "1");
                }
                else
                {
                    //노드 삭제 후 재 입력
                    xmlNodeChangeUni(fileInfo, "filePlayList", "playFile", _playFile, "playCnt");
                }

            }
            util = null;
        }
        //xml to json 변환
        public string xmlToJson(string _filePath)
        {
            string xmlString = System.IO.File.ReadAllText(_filePath);
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(xmlString);
            return JsonConvert.SerializeXmlNode(XmlDoc);
        }
        public string xmlToJson(string _filePath, string _subNodeNm)
        {
            string jsonResult = string.Empty;
            XmlDocument XmlDoc = new XmlDocument();
            uniUtil util = new uniUtil();
            try
            {
                XmlDoc.Load(_filePath);
                //XmlNode newNode;
                //newNode = XmlDoc.SelectSingleNode(_rootNm);
                
                //노드 리스트 나오게 하기?
                if (_subNodeNm.Length > 0)
                {
                    jsonResult = "";
                    XmlNodeList nodeList = XmlDoc.GetElementsByTagName(_subNodeNm);
                    
                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        jsonResult += "{\"" + _subNodeNm.ToString() + "\":\"" + nodeList[i].InnerText + "\",\"playCnt\":\"" + nodeList[i].Attributes["playCnt"].Value + "\"}";
                        if (i < (nodeList.Count -1))
                            jsonResult += ",";
                    }
                }

            }
            catch (Exception e)
            {
                util.setLogFile("xmlToJson error:" + e.ToString());
            }
            util = null;
            return jsonResult;
        }
        public string xmlToJson(string _filePath , string _rootNm, List<string> _subNodeNm)
        {
            string jsonResult = string.Empty;
            XmlDocument XmlDoc = new XmlDocument();
            uniUtil util = new uniUtil();
            try
            {
                XmlDoc.Load(_filePath);
                XmlNode newNode;
                newNode = XmlDoc.SelectSingleNode(_rootNm);

                //노드 리스트 나오게 하기?


            }
            catch(Exception e)
            {   
                util.setLogFile("xmlToJson error:" + e.ToString());
            }
            util = null;
            return jsonResult;
        }
        public bool xmlReportCreateNode(string _filePath, string _brodTime, string _fileNm, string _bordSeq)
        {
            XmlDocument XmlDoc = new XmlDocument();
            uniUtil util = new uniUtil();
            bool return_check = false;
            try
            {
                XmlDoc.Load(_filePath);
                XmlNode newNode;
                newNode = XmlDoc.SelectSingleNode("dataset");

                XmlElement subNode = XmlDoc.CreateElement("brodList");
                XmlElement score = XmlDoc.CreateElement("BROD_TIME");
                score.InnerXml = _brodTime;
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("STRE_FILE_NM");
                score.InnerXml = _fileNm;
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("BROD_SEQ");
                score.InnerXml = _bordSeq;
                subNode.AppendChild(score);
                newNode.AppendChild(subNode);
                XmlDoc.Save(_filePath);
                return_check = true;
            }
            catch (Exception e)
            {
                return_check = false;
                util.setLogFile("xmlNodeUpdate error:" + e.ToString());
            }
            util = null;
            XmlDoc = null;
            return return_check;
        }
        public bool xmlCreateNode(string _filePath, string _fileUrl, string _fileNm, string _sort, string _timeInterval)
        {
            XmlDocument XmlDoc = new XmlDocument();
            uniUtil util = new uniUtil();
            bool return_check = false;
            try
            {
                XmlDoc.Load(_filePath);
                XmlNode newNode;
                newNode = XmlDoc.SelectSingleNode("dataset");

                XmlElement subNode = XmlDoc.CreateElement("fileList");
                XmlElement score = XmlDoc.CreateElement("file_path");
                score.InnerXml = _fileUrl;
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("file_name");
                score.InnerXml = _fileNm;
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("file_sort");
                score.InnerXml = _sort;
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("time_intervl");
                score.InnerXml = _timeInterval;
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("download_result");
                score.InnerXml = "N";
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("download_date");
                score.InnerXml = "";
                subNode.AppendChild(score);
                newNode.AppendChild(subNode);
                XmlDoc.Save(_filePath);
                return_check = true;
            }
            catch(Exception e)
            {
                return_check = false;
                util.setLogFile("xmlNodeUpdate error:" + e.ToString());
            }
            util = null;
            XmlDoc = null;
            return return_check;
        }
        public bool xmlCreateNodeBasicMusic(string _filePath, string _fileNm, string _strartTime, string _endTime, string _groupTimeGubun, string _order, string _basicCode)
        {
            XmlDocument XmlDoc = new XmlDocument();
            uniUtil util = new uniUtil();
            bool return_check = false;
            try
            {
                XmlDoc.Load(_filePath);
                XmlNode newNode;
                newNode = XmlDoc.SelectSingleNode("dataset");

                XmlElement subNode = XmlDoc.CreateElement("fileList");
                XmlElement score = XmlDoc.CreateElement("file_name");
                score.InnerXml = _fileNm;
                subNode.AppendChild(score);

                score = XmlDoc.CreateElement("strartTime");
                score.InnerXml = _strartTime;
                subNode.AppendChild(score);

                score = XmlDoc.CreateElement("endTime");
                score.InnerXml = _endTime;
                subNode.AppendChild(score);

                score = XmlDoc.CreateElement("groupTimeGubun");
                score.InnerXml = _groupTimeGubun;
                subNode.AppendChild(score);

                score = XmlDoc.CreateElement("playOrder");
                score.InnerXml = _order;
                subNode.AppendChild(score);

                score = XmlDoc.CreateElement("basicCode");
                score.InnerXml = _basicCode;
                subNode.AppendChild(score);

                score = XmlDoc.CreateElement("playCnt");
                score.InnerXml = "0";
                subNode.AppendChild(score);
                
                newNode.AppendChild(subNode);
                XmlDoc.Save(_filePath);
                return_check = true;
            }
            catch (Exception e)
            {
                return_check = false;
                util.setLogFile("xmlNodeUpdate error:" + e.ToString());
            }
            util = null;
            XmlDoc = null;
            return return_check;
        }
        public const string xmlText = "<?xml version=\"1.0\" standalone=\"yes\"?>\r\n"
                                    + "<dataset>\r\n"
                                    + "</dataset>\r\n";
        public bool xmlCreateNodeMusic(string _filePath, string _fileUrl, string _fileNm)
        {
            XmlDocument XmlDoc = new XmlDocument();
            uniUtil util = new uniUtil();
            bool return_check = false;
            try
            {
                //파일 사이즈 확인 후 파일 사이즈가 0이면 생성 

                //여기 부분 확인


                

                if (!File.Exists(_filePath))
                {
                    File.WriteAllText(_filePath, xmlText);
                }
                long length = new System.IO.FileInfo(_filePath).Length;
                if (length < 1)
                {
                    //xml document 생성
                    File.WriteAllText(_filePath, xmlText);
                }
                
                XmlDoc.Load(_filePath);
                XmlNode newNode;
                newNode = XmlDoc.SelectSingleNode("dataset");

                XmlElement subNode = XmlDoc.CreateElement("fileList");
                XmlElement score = XmlDoc.CreateElement("file_path");
                score.InnerXml = _fileUrl;
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("file_name");
                score.InnerXml = _fileNm;
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("download_result");
                score.InnerXml = "N";
                subNode.AppendChild(score);
                score = XmlDoc.CreateElement("download_date");
                score.InnerXml = "";
                subNode.AppendChild(score);
                newNode.AppendChild(subNode);
                XmlDoc.Save(_filePath);
                return_check = true;
            }
            catch (Exception e)
            {
                return_check = false;
                util.setLogFile("xmlNodeUpdate error:" + e.ToString());
            }
            util = null;
            XmlDoc = null;
            return return_check;
        }        
        public bool xmlCreateNode(string _singleNode, string _filePath, string _nodeNm, string _nodeValue)
        {
            XmlDocument XmlDoc = new XmlDocument();
            uniUtil util = new uniUtil();
            bool return_check = false;
            try
            {
                XmlDoc.Load(_filePath);
                XmlNode newNode;

                newNode = XmlDoc.SelectSingleNode(@"/dataset/" + _singleNode);

                XmlElement score = XmlDoc.CreateElement(_nodeNm);
                score.InnerXml = _nodeValue;
                newNode.AppendChild(score);
                XmlDoc.Save(_filePath);
                return_check = true;
            }
            catch (Exception e)
            {
                return_check = false;
                util.setLogFile("xmlNodeUpdate error:" + e.ToString());
            }
            util = null;
            XmlDoc = null;
            return return_check;
        }
        public bool xmlCreateNodeAttr(string _singleNode, string _filePath, string _nodeNm, string _nodeValue, string _cnt)
        {
            XmlDocument XmlDoc = new XmlDocument();
            uniUtil util = new uniUtil();
            bool return_check = false;
            try
            {
                XmlDoc.Load(_filePath);
                XmlNode newNode;

                newNode = XmlDoc.SelectSingleNode(@"/dataset/" + _singleNode);

                XmlElement score = XmlDoc.CreateElement(_nodeNm);
                score.InnerXml = _nodeValue;
                score.SetAttribute("playCnt", _cnt);
                newNode.AppendChild(score);
                XmlDoc.Save(_filePath);
                

                return_check = true;
            }
            catch (Exception e)
            {
                return_check = false;
                util.setLogFile("xmlNodeUpdate error:" + e.ToString());
            }
            util = null;
            XmlDoc = null;
            return return_check;
        }
        //싱글 노드 찾기 
        public string  xmlNodeRead(string _filePath, string _basicInfo)
        {
            uniUtil util = new uniUtil();
            string resultTxt = string.Empty;
            try
            {

                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.Load(_filePath);
                XmlNode findNode = XmlDoc.SelectSingleNode(@""+ _basicInfo + "");                
                resultTxt = findNode.InnerText;                
                XmlDoc = null;                
            }
            catch (Exception e)
            {
                resultTxt = "";
                util.setLogFile("xmlNodeRead error:" + e.ToString());
            }
            util = null;
            return resultTxt;
        }
        public string xmlNodeRead(string _filePath, string _singlePath, string _findeNm, string _value)
        {
            uniUtil util = new uniUtil();
            string resultTxt = string.Empty;
            try
            {

                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.Load(_filePath);

                XmlNode findNode = XmlDoc.SelectSingleNode(@"/dataset/" + _singlePath + "/" + _findeNm + "[text()='" + _value + "']");
                resultTxt = findNode.InnerText;
                XmlDoc = null;
            }
            catch (Exception e)
            {

                util.setLogFile("xmlNodeRead error:" + e.ToString());
            }
            util = null;
            return resultTxt;
        }

        public bool xmlNodeUpdate(string _filePath,  string _fileNm, string _result)
        {
            uniUtil util = new uniUtil();
            bool return_check = false;
            try
            {
                
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.Load(_filePath);
                string resultTime = string.Empty;

                resultTime = _result.Equals("Y") ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : "";
                
                XmlNode findNode = XmlDoc.SelectSingleNode(@"/dataset/fileList/file_name[text()='" + _fileNm + "']");
                findNode.ParentNode.RemoveChild(findNode.ParentNode.SelectSingleNode("download_result"));
                findNode.ParentNode.RemoveChild(findNode.ParentNode.SelectSingleNode("download_date"));                                
                findNode.ParentNode.AppendChild(CreateNode(XmlDoc, "download_result", _result));                
                findNode.ParentNode.AppendChild(CreateNode(XmlDoc, "download_date", resultTime));
                XmlDoc.Save(_filePath);
                XmlDoc = null;
                return_check = true;
            }
            catch(Exception e)
            {
                return_check = false;
                util.setLogFile("xmlNodeUpdate error:" + e.ToString());
            }
            util = null;
            return return_check;
        }

    }
}
