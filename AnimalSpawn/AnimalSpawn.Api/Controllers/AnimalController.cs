using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalSpawn.Api.Responses;
using AnimalSpawn.Domain.DTOs;
using AnimalSpawn.Domain.Entities;
using AnimalSpawn.Domain.Interfaces;
using AnimalSpawn.Infraestructure.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace AnimalSpawn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalService _service;
        private readonly IMapper _mapper;
        public AnimalController(IAnimalService service, IMapper mapper)
        {
            _service = service;
            this._mapper = mapper;
        }
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAnimal(id);
            var response = new ApiResponse<bool>(true);
            return Ok(response);
        }
        [HttpPut("{id:int}")]
        public IActionResult Put(int id, AnimalRequestDto animalDto)
        {
            var animal = _mapper.Map<Animal>(animalDto);
            animal.Id = id;
            animal.UpdateAt = DateTime.Now;
            animal.UpdatedBy = 2;
            _service.UpdateAnimal(animal);
            var response = new ApiResponse<bool>(true);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var animals = _service.GetAnimals();
            var animalsDto = _mapper.Map<IEnumerable<Animal>,
            IEnumerable<AnimalResponseDto>>(animals);
            var response = new ApiResponse<IEnumerable<AnimalResponseDto>>(animalsDto);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Post(AnimalRequestDto animalDto)
        {
            var animal = _mapper.Map<AnimalRequestDto, Animal>(animalDto);
            await _service.AddAnimal(animal);
            var animalresponseDto = _mapper.Map<Animal, AnimalResponseDto>(animal);
            var response = new ApiResponse<AnimalResponseDto>(animalresponseDto);
            return Ok(response);
        }
       
    }

}
