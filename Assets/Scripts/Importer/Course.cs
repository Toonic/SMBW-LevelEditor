
using Fushigi.util;
using Fushigi.Byml;
using Fushigi.Byml.Writer;
using Fushigi.Byml.Writer.Primitives;
using Fushigi;
using Fushigi.course;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace Fushigi.course
{
    public class Course
    {

        public Byml.Byml courseInfo;

        public Course(string courseName)
        {
            mCourseName = courseName;
            mAreas = new List<CourseArea>();
            LoadFromRomFS();
        }

        public string GetName()
        {
            return mCourseName;
        }

        public void LoadFromRomFS()
        {
            byte[] courseBytes = RomFS.GetFileBytes($"BancMapUnit/{mCourseName}.bcett.byml.zs");
            /* grab our course information file */
            courseInfo = new Byml.Byml(new MemoryStream(FileUtil.DecompressData(courseBytes)));

            var root = (BymlHashTable)courseInfo.Root;
            var stageList = (BymlArrayNode)root["RefStages"];

            for (int i = 0; i < stageList.Length; i++)
            {
                string stageParamPath = ((BymlNode<string>)stageList[i]).Data.Replace("Work/", "").Replace(".gyml", ".bgyml");
                string stageName = Path.GetFileName(stageParamPath).Split(".game")[0];
                mAreas.Add(new CourseArea(stageName));
            }
        }

        public void SaveToRomFS(BymlHashTable root, string exportPath, string currentArea)
        {
            var mem = new MemoryStream();
            new Byml.Byml(root).Save(mem);
            var compRessedData = FileUtil.CompressData(mem.ToArray());
            File.WriteAllBytes(exportPath + $"/BancMapUnit/{currentArea}.bcett.byml.zs", compRessedData);
        }

        private byte[] ConvertToByteArray(BymlHashTable root)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, root);
                return memoryStream.ToArray();
            }
        }

        public CourseArea GetArea(int idx)
        {
            return mAreas.ElementAt(idx);
        }

        public CourseArea GetArea(string name)
        {
            foreach (CourseArea area in mAreas)
            {
                if (area.GetName() == name)
                {
                    return area;
                }
            }

            return null;
        }

        public int GetAreaCount()
        {
            return mAreas.Count;
        }

        string mCourseName;
        List<CourseArea> mAreas;
    }
}
