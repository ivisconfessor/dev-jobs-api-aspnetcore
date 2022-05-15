using DevJobs.API.Entities;
using DevJobs.API.Models;
using DevJobs.API.Persistence;
using DevJobs.API.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DevJobs.API.Controllers
{
    [Route("api/v1/job-vacancies/{id}/applications")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IJobVacancyRepository _repository;
        private readonly ISendGridClient _emailClient;

        public JobApplicationsController(IJobVacancyRepository repository, ISendGridClient emailClient)
        {
            _repository = repository;
            _emailClient = emailClient;
        }

        // POST api/v1/job-vacancies/7/applications
        [HttpPost]
        public async Task<IActionResult> Post(int id, AddJobApplicationInputModel model)
        {
            var jobVacancy = _repository.GetById(id);

            if (jobVacancy == null)
                return NotFound();

            var application = new JobApplication(
                model.ApplicantName,
                model.ApplicantEmail,
                id
            );

            _repository.AddApplication(application);

            var message = new SendGridMessage {
                From = new EmailAddress("yivato6289@dufeed.com", "Artigo Tech"),
                Subject = $"Dev Jobs - Vaga: {jobVacancy.Title}",
                PlainTextContent = $"Você acabou de se candidatar a vaga para {jobVacancy.Title} na empresa {jobVacancy.Company}. Fique de olho no seu e-mail para acompanhar as próximas etapas do processo seletivo."
            };

            message.AddTo(model.ApplicantEmail, model.ApplicantName);

            await _emailClient.SendEmailAsync(message);

            return NoContent();
        }
    }
}