﻿using System.ComponentModel.DataAnnotations.Schema;

namespace WalksAPI.Models.Domain {
    public class Image {
        public Guid Id { get; set; }
        // will not be in database, as we dont store images directly in database but in file system
        [NotMapped] 
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }
        public string FilePath { get; set; }
    }
}
