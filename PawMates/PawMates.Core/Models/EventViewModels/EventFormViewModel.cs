﻿using System.ComponentModel.DataAnnotations;
using static PawMates.Infrastructure.Data.DataConstants.DataConstants;

namespace PawMates.Core.Models.EventViewModels
{
    public class EventFormViewModel
    {
        [Required(ErrorMessage = RequireErrorMessage)]
        [StringLength(EventNameMaxLenght, MinimumLength = EventNameMinLenght, ErrorMessage = StringLengthErrorMessage)]
        public string Name { get; set; } = string.Empty;

        [StringLength(EventDescriptionMaxLenght, MinimumLength = EventDescriptionMinLenght, ErrorMessage = StringLengthErrorMessage)]
        public string? Description { get; set; }

        [Required(ErrorMessage = RequireErrorMessage)]
        [StringLength(EventLocationMaxLenght, MinimumLength = EventLocationMinLenght, ErrorMessage = StringLengthErrorMessage)]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = RequireErrorMessage)]
        [Display(Name = "Starts On")]
        public string StartsOn { get; set; } = string.Empty;

        public int Id { get; set; }

    }
}
