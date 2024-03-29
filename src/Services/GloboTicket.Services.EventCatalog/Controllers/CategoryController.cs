﻿using AutoMapper;
using GloboTicket.Services.EventCatalog.Models;
using GloboTicket.Services.EventCatalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloboTicket.Services.EventCatalog.Controllers
{
    [Route("api/categories")]
    public class CategoryController: ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> Get()
        {
            var result = await _categoryRepository.GetAllCategories();
            return Ok(_mapper.Map<List<CategoryDto>>(result));
        }
    }
}
