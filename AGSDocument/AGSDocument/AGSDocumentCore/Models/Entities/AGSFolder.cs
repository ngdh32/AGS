using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AGSDocumentCore.Models.DTOs.Commands;
using AGSDocumentCore.Models.Enums;

namespace AGSDocumentCore.Models.Entities
{
    public class AGSFolder : AGSEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime CreatedDate { get; private set; }
        private List<AGSFile> _files { get; set; } = new();
        private List<AGSFolder> _childrenFolders { get; set; } = new();
        private List<AGSPermission> _permissions { get; set; } = new();
        public ReadOnlyCollection<AGSFile> Files => _files.AsReadOnly();
        public ReadOnlyCollection<AGSFolder> ChildrenFolders => _childrenFolders.AsReadOnly();
        public ReadOnlyCollection<AGSPermission> Permissions => _permissions.AsReadOnly();


        public void AddNewFolder(string name, string description, string createdBy, List<AGSPermission> permissions)
        {
            _childrenFolders.Add(new AGSFolder()
            {
                Name = name,
                Description = description,
                CreatedBy = createdBy,
                CreatedDate = DateTime.UtcNow,
                _files = new(),
                _childrenFolders = new(),
                _permissions = permissions
            });
        }

        public void DeleteFolder(string folderId)
        {
            var toBeRemoved = _childrenFolders.FirstOrDefault(x => x.Id == folderId);
            if (toBeRemoved != null)
                _childrenFolders.Remove(toBeRemoved);
        }

        public void UpdateFolder(string name, string description, List<AGSPermission> permissions)
        {
            this.Name = name;
            this.Description = description;
            this._permissions = permissions;
        }

        public void AddNewFile(AGSFile file)
        {
            _files.Add(file);
        }

        public void DeleteFile(string fileId)
        {
            var toBeRemoved = _files.FirstOrDefault(x => x.Id == fileId);
            if (toBeRemoved != null)
                _files.Remove(toBeRemoved);
        }

        public void SetPermissions(List<AGSPermission> permissions)
        {
            _permissions = permissions;
        }
        

        public AGSFolder()
        {
        }
    }
}
