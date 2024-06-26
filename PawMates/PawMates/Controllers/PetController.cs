﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PawMates.Core.Contracts.PetInterface;
using PawMates.Core.Models.PetViewModels;
using PawMates.Extensions;
using System.Globalization;
using static PawMates.Infrastructure.Data.DataConstants.DataConstants;

namespace PawMates.Controllers
{
    [Authorize]
    public class PetController : Controller
    {
        private readonly IPetService petService;

        public PetController(IPetService _petService)
        {
			petService = _petService;
        }


		[HttpGet]
		public async Task<IActionResult> Add()
		{
			var model = new PetFormViewModel();

			model.PetTypes = await petService.GetPetTypes();

			return View(model);

		}

		[HttpPost]
		public async Task<IActionResult> Add(PetFormViewModel model)
        {
            var userId = User.Id();

			var result = await petService.CreatePetAsync(model, userId);

			if (result == -1)
			{
                ModelState.AddModelError(nameof(model.DateOfBirth), $"Invalid Date! Format must be: {DateOfBirthFormat}");
            }
            else if(result == 0)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
			{
				model.PetTypes = await petService.GetPetTypes();

                return View(model);
			}

			return RedirectToAction(nameof(All));
		}
       
        [HttpGet]
        public async Task<IActionResult> All([FromQuery] AllPetsQueryModel model)
        {
            var pets = await petService.AllAsync(
                model.PetType,
                model.CurrentPage,
                model.PetsPerPage);

            model.TotalPetsCount = pets.TotalPetsCount;
            model.Pets = pets.Pets;
            model.PetTypes = await petService.AllPetTypeNamesAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyPets()
        {
            var model = await petService.GetMyPetsAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)     
        {
            if (await petService.ExistsAsync(id) == false)
            {
                return NotFound();
            }
            var model = await petService.GetPetDetailsAsync(id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if ((await petService.ExistsAsync(id) == false))
            {
                return NotFound();
            }

            if (await petService.SameOwnerAsync(id, User.Id()) == false)
            {
                return Forbid();
            };

            var petToDelete = await petService.PetByIdAsync(id);

            var model = new PetDeleteViewModel()
            {
                Id = petToDelete.Id,
                Name = petToDelete.Name,
                DateOfBirth = petToDelete.DateOfBirth.ToString(DateOfBirthFormat, CultureInfo.InvariantCulture),
            };

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(PetDeleteViewModel model)
        {
            if ((await petService.ExistsAsync(model.Id) == false))
            {
                return NotFound();
            }

            if (await petService.SameOwnerAsync(model.Id, User.Id()) == false)
            {
                return Forbid();
            };

            if (model == null)
            {
                return NotFound();
            }

            await petService.DeleteAsync(model.Id);

            return RedirectToAction(nameof(MyPets));
        }

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
            if ((await petService.ExistsAsync(id)) == false)
            {
                return NotFound();
            }

            var userId = User.Id();
            if (await petService.SameOwnerAsync(id, userId) == false)
            {
                return Forbid();
            };

            var pet = await petService.PetByIdAsync(id);

            if (pet == null)
            {
                return NotFound();
            }

            var model = new PetFormViewModel()
			{
				Name = pet.Name,
				Breed = pet.Breed,
				DateOfBirth=pet.DateOfBirth.ToString(DateOfBirthFormat),
				MainColor = pet.MainColor,
				SecondaryColor = pet.SecondaryColor,
				Gender = pet.Gender,
				PetTypeId = pet.PetTypeId,
				ImageUrl = pet.ImageUrl,
				Weight = pet.Weight,
			};

			model.PetTypes =await petService.GetPetTypes();

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(PetFormViewModel model, int id)
		{
            if (id != model.Id)
            {
                return NotFound();
            }

            if (await petService.ExistsAsync(model.Id) == false)
            {
                return NotFound();
            }

            if (await petService.SameOwnerAsync(model.Id, User.Id()) == false)
            {
                return Forbid();
            };

            if (await petService.EditPetAsync(model.Id, model) == -1)
            {
                ModelState.AddModelError(nameof(model.DateOfBirth), $"Invalid Date! Format must be: {DateOfBirthFormat}");
            }

            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            return RedirectToAction(nameof(All));
        }
    }
}
