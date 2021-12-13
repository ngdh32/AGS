using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AGSDocumentCore.Models.Enums;

namespace AGSDocumentCore.Models.Entities
{
    public class AGSFolder : AGSEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public List<AGSPermission> Permissions { get; private set; }
        private List<AGSFile> _files { get; set; }
        public ReadOnlyCollection<AGSFile> Files => _files.AsReadOnly();
        private List<AGSFolder> _childrenFolders { get; set; }
        public ReadOnlyCollection<AGSFolder> ChildrenFolders => _childrenFolders.AsReadOnly();


        public void AddNewFolder()
        {

        }

        public void DeleteFolder(string folderId)
        {
            var toBeRemoved = _childrenFolders.FirstOrDefault(x => x.Id == folderId);
            if (toBeRemoved != null)
                _childrenFolders.Remove(toBeRemoved);
        }

        public void UpdateFolder(string folderId)
        {

        }

        public void AddNewFile()
        {

        }

        public void DeleteFile(string fileId)
        {

        }

        public void AddPermission(string departmentId, AGSPermissionType permissionType)
        {

        }

        public void DeletePermission()
        {

        }

        public AGSFolder()
        {
        }
    }
}
