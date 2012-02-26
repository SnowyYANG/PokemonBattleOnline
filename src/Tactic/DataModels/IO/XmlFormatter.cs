using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace LightStudio.Tactic.DataModels.IO
{
    internal static class XmlFormatter
    {
        public static XElement ToXml(this IFile file)
        {
            var element = new XElement(FileString);
            element.Add(
                new XAttribute(NameString, file.Name),
                new XAttribute(IdentifierString, file.Identifier));
            return element;
        }

        public static XElement ToXml(this IDirectory directory)
        {
            return directory.ToXml(false);
        }

        public static XElement ToXml(this RootDirectory rootDir)
        {
            return rootDir.ToXml(true);
        }

        private static XElement ToXml(this IDirectory directory, bool isRoot)
        {
            XElement element;
            if (isRoot)
            {
                element = new XElement(RootDirectoryString);
            }
            else
            {
                element = new XElement(DirectoryString);
                element.Add(new XAttribute(NameString, directory.Name));
            }
            element.Add(from dir in directory.Directories.Values select dir.ToXml());
            element.Add(from file in directory.Files.Values select file.ToXml());
            return element;
        }

        public static DataFile ToDataFile(this XElement fileXml, IDirectory parent)
        {
            var file = new DataFile(parent, fileXml.GetName(),fileXml.GetIdentifier());
            return file;
        }

        public static DataDirectory ToDataDirectory(this XElement dirXml, IDirectory parent)
        {
            var dir = new DataDirectory(parent, dirXml.GetName());
            dir.AppendChildren(dirXml);
            return dir;
        }

        public static RootDirectory ToRootDirectory(this XElement dirXml, string baseDir)
        {
            var dir = new RootDirectory(baseDir);
            dir.AppendChildren(dirXml);
            return dir;
        }

        private static void AppendChildren(this IDirectory dir, XElement xml)
        {
            foreach (var childFile in xml.GetChildrenFiles(dir))
            {
                dir.Files[childFile.Name]=childFile;
            }
            foreach (var childDir in xml.GetChildrenDirectories(dir))
            {
                dir.Directories[childDir.Name] = childDir;
            }
        }

        private static IEnumerable<DataDirectory> GetChildrenDirectories(this XElement dirXml, IDirectory parent)
        {
            return from el in dirXml.Elements(DirectoryString)
                   select el.ToDataDirectory(parent);
        }

        private static IEnumerable<DataFile> GetChildrenFiles(this XElement dirXml, IDirectory parent)
        {
            return from el in dirXml.Elements(FileString)
                   select el.ToDataFile(parent);
        }

        public static string GetIdentifier(this XElement xml)
        {
            return xml.GetAttributeValueOrEmpty(IdentifierString);
        }

        public static string GetName(this XElement xml)
        {
            return xml.GetAttributeValueOrEmpty(NameString);
        }

        private static string GetAttributeValueOrEmpty(this XElement xml, string attributeName)
        {
            var attribute = xml.Attribute(attributeName);
            return attribute != null ? attribute.Value : string.Empty;
        }

        public const string NameString = "Name";
        public const string FileString = "File";
        public const string RootDirectoryString = "RootDirectory";
        public const string DirectoryString = "Directory";
        public const string IdentifierString = "Identifier";
        public const string DataCollectionString = "DataCollection";
    }
}
