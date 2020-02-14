using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;


namespace sohoUniLib
{
    public class xmlUtil
    {

        public bool xmlCreateFile (string _path, string _xmlFileNm)
        {
            try
            {
                string mp3FilePath = System.Environment.CurrentDirectory + "\\fileList\\";
                string strDir = Path.GetDirectoryName(mp3FilePath);
                DirectoryInfo diDir = new DirectoryInfo(strDir);
                if (!diDir.Exists)
                {
                    diDir.Create();
                    diDir = new DirectoryInfo(strDir);
                }
                if (!File.Exists(_path + "\\" + _xmlFileNm))
                {
                    string xmlText = "<?xml version='1.0' encoding='UTF-8' ?>\r\n"
                                   + "<dataset>\r\n"
                                   + "</dataset>\r\n";
                    File.WriteAllText(_path, xmlText);

                }
                return true;
            }
            
            catch (Exception e)
            {
                uniUtil.setLogFileS(string.Format("xmlCreateFile error : {0} ", e.ToString()));                
                return false;
            }
        }
        protected XmlNode CreateNode(XmlDocument doc, string _name, string _innerXml)
        {
            XmlNode node = doc.CreateElement(string.Empty, _name, string.Empty);
            node.InnerXml = _innerXml;
            return node;
        }

    }
}
