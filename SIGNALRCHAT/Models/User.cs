﻿namespace SIGNALRCHAT.Models
{
    public sealed class User
    {
        public User()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public int FileId { get; set; }
        public File File { get; set; }
    }
}
