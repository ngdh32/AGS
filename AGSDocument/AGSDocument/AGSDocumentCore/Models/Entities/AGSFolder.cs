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
        public DateTime CreatedDate { get; private set; } = DateTime.Now;
        private List<AGSFile> _files { get; set; } = new();
        private List<AGSFolder> _childrenFolders { get; set; } = new();
        private List<AGSPermission> _permissions { get; set; } = new();
        public ReadOnlyCollection<AGSFile> Files => _files.AsReadOnly();
        public ReadOnlyCollection<AGSFolder> ChildrenFolders => _childrenFolders.AsReadOnly();
        public ReadOnlyCollection<AGSPermission> Permissions => _permissions.AsReadOnly();


        public void AddNewFolder(string name, string description, string createdBy, List<AGSPermission> permissions)
        {
            // add validation here...
            _childrenFolders.Add(new AGSFolder(name, description, createdBy, permissions));
        }

        public void UpdateFolder(string name, string description)
        {
            // add validation here...
            this.Name = name;
            this.Description = description;
        }

        public void AddNewFile(AGSFile file)
        {
            // add validation here...
            _files.Add(file);
        }

        public void DeleteFile(string fileId)
        {
            // add validation here...
            var toBeRemoved = _files.FirstOrDefault(x => x.Id == fileId);
            if (toBeRemoved != null)
                _files.Remove(toBeRemoved);
        }

        public void SetPermissions(List<AGSPermission> permissions)
        {
            // add validation here...
            _permissions = permissions;
        }
        public AGSFolder(string id, string name, string createdBy, DateTime createdDate)
        {
            // add validation here...
            this.Id = id;
            this.Name = name;
            this.CreatedBy = createdBy;
            this.CreatedDate = createdDate;
        }

        public AGSFolder(string id, string name, string createdBy, DateTime createdDate, List<AGSFile> files, List<AGSFolder> childrenFolders, List<AGSPermission> permissions)
        {
            // add validation here...
            this.Id = id;
            this.Name = name;
            this.CreatedBy = createdBy;
            this.CreatedDate = createdDate;
            this._files = files;
            this._childrenFolders = childrenFolders;
            this._permissions = permissions;
        }

        public AGSFolder(string name, string description, string createdBy, List<AGSPermission> permissions)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(nameof(name));

            if (string.IsNullOrEmpty(createdBy))
                throw new ArgumentException(nameof(createdBy));

            this.Name = name;
            this.Description = description;
            this.CreatedBy = createdBy;
            this._permissions = permissions ?? new();
            this._childrenFolders = new ();
            this._files = new ();
            
        }
    }
}
