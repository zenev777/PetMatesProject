﻿using Microsoft.EntityFrameworkCore;
using PawMates.Infrastructure.Data.Enums;
using PawMates.Infrastructure.Data.IdentityModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PawMates.Infrastructure.Data.DataConstants.DataConstants;

namespace PawMates.Infrastructure.Data.Models
{
    public class Pet
    {
        [Key]
        [Comment("Pet identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(PetNameMaxLenght)]
        [Comment("Pet's name")]
        public string Name { get; set; } = string.Empty;

        [Comment("Pet's image")]
        public string? ImageUrl { get; set; }

        [Required]
        [Comment("Pet's birthday")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(PetBreedMaxLenght)]
        [Comment("Pet's breed")]
        public string Breed { get; set; } = string.Empty;

        [Required]
        [MaxLength(PetColorMaxLenght)]
        [Comment("Pet's main color")]
        public string MainColor { get; set; } = string.Empty;

        [MaxLength(PetColorMaxLenght)]
        [Comment("Pet's second color")]
        public string? SecondaryColor { get; set; }

        [Required]
        [Comment("Pet's gender")]
        public Gender Gender { get; set; }

        [Required]
        [MaxLength(PetWeightMaxLenght)]
        [Comment("Pet's weight")]
        public double Weight { get; set; }

        public string OwnerId { get; set; } = string.Empty;
        [ForeignKey(nameof(OwnerId))]
        public ApplicationUser Owner { get; set; } = null!;

        [Required]
        public int PetTypeId { get; set; }
        [ForeignKey(nameof(PetTypeId))]
        public PetType PetType { get; set; } = null!;
    }
}
