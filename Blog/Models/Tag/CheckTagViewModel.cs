﻿using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class CheckTagViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]

        public string? Title { get; set; }
        
        public bool Checked { get; set; }

    }
}
