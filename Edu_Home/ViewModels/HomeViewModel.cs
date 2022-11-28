﻿using Edu_Home.DAL.Entities;

namespace Edu_Home.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; } = new List<Slider>();
        public List<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}
