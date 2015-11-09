using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace FolderTreeReadWrite
{
    class Program
    {
        static void Main(string[] args)
        {
            //根目录
            string rootPath = @"E:\";
            var dir = new DirectoryInfo(rootPath);
            //递归获得目录树
            var doc = new XDocument(GetDirectoryXml(dir));
            //将目录树存入文件haha.txt
            System.IO.File.WriteAllText("haha.txt", doc.ToString());

            //读入目录树文件
            XDocument xdoc = XDocument.Load("haha.txt");
            //获得文件中第一级的所有XElements,也就是我们的根目录E
            List<XElement> els = xdoc.Elements().ToList<XElement>();
            //获得文件中第一级第一个Element的子Elements(也就是第二级)
            List<XElement> es = els[0].Elements().ToList<XElement>();
            for (int i = 0; i < es.Count; i++)
            {//遍历第二级
                XElement   currElement = es[i];
                //按照文件结构的设计,每一个element都只有一个attribute
                XAttribute currElementsAttribute = currElement.FirstAttribute;
                //name也就是当前element的名字,或者是dir,或者是file,如果是dir那就可以继续展开这个element,得到他的子elements
                string name = currElement.Name.ToString();
                //当前element的attribute的value存了文件名或文件夹名
                string path = currElementsAttribute.Value.ToString();
                Console.WriteLine(name + ":" + path);
            }
            Console.ReadLine();
        }

        public static XElement GetDirectoryXml(DirectoryInfo dir)
        {
            //为当前文件夹新建一个element,这个element的name是dir,为这个element new 一个attribute,存文件夹名
            var info = new XElement("dir",
                           new XAttribute("name", dir.Name));
            
            //将为当前文件夹中所有的文件新建element,这个element的name是file,为这个element new一个attribute,存文件名
            foreach (var file in dir.GetFiles())
                info.Add(new XElement("file",
                             new XAttribute("name", file.Name)));

            //如果当前文件夹中有子文件夹,递归调用本函数
            foreach (var subDir in dir.GetDirectories())
                info.Add(GetDirectoryXml(subDir));

            return info;
        }
    }
}
