﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PawMates.Core.Models.PetViewModels
{
    public class PetTypesViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

    }
}