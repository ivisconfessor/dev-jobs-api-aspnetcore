using DevJobs.API.Entities;
using DevJobs.API.Models;
using DevJobs.API.Persistence;
using DevJobs.API.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevJobs.API.Controllers
{
    [Route("api/v1/job-vacancies")]
    [ApiController]
    public class JobVacanciesController : ControllerBase
    {
        private readonly IJobVacancyRepository _repository;
        public JobVacanciesController(IJobVacancyRepository repository)
        {
            _repository = repository;
        }

        // GET api/v1/job-vacancies
        [HttpGet]
        public IActionResult Get()
        {
            var jobVacancies = _repository.GetAll();

            return Ok(jobVacancies);
        }

        // GET api/v1/job-vacancies/7
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var jobVacancy = _repository.GetById(id);

            if (jobVacancy == null)
                return NotFound();

            return Ok(jobVacancy);
        }

        // POST api/v1/job-vacancies
        /// <summary>
        /// Cadastrar uma vaga de emprego.
        /// </summary>
        /// <remarks>
        /// {
        ///   "title": "Dev .NET Jr",
        ///   "description": "Vaga para sustentação e elaboração de aplicações .NET Core.",
        ///   "company": "Artigo Tech",
        ///   "isRemote": true,
        ///   "salaryRange": "3000 - 5000"
        /// }
        /// </remarks>
        /// <param name="model">Dados da vaga.</param>
        /// <returns>Objeto recém-criado.</returns>
        /// <response code="201">Sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPost]
        public IActionResult Post(AddJobVacancyInputModel model)
        {
            var jobVacancy = new JobVacancy(
                model.Title,
                model.Description,
                model.Company,
                model.IsRemote,
                model.SalaryRange
            );

            if (jobVacancy.Title.Length > 30)
                return BadRequest("Título precisa ter menos de 30 caracteres.");

            _repository.Add(jobVacancy);

            return CreatedAtAction(
                "GetById", 
                new { id = jobVacancy.Id },
                jobVacancy);
        }

        // PUT api/v1/job-vacancies/7
        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateJobVacancyInputModel model)
        {
            var jobVacancy = _repository.GetById(id);

            if (jobVacancy == null)
                return NotFound();

            jobVacancy.Update(model.Title, model.Description);
            
            _repository.Update(jobVacancy);
            
            return NoContent();
        } 
    }
}