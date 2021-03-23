using AppDatabase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServer7.Api.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogger<CompaniesController> _logger;

       
      
        public CompaniesController(ICompanyRepository companyRepository, ILogger<CompaniesController> logger)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _logger = logger;
        }
        [HttpGet]
        //[HttpHead] //添加对 Http Head 请求的支持，Http Head 请求只获取 Header 信息，没有 Body（视频P16）
        public async Task<IActionResult> GetCompanies()
        {
            var companyId = new Guid("bbdee09c-089b-4d30-bece-44df5923716c");
            var companies = await _companyRepository.GetCompaniesAsync();
            return  Ok(companies);
        }
        [HttpGet(template: "{companyId}")]
        public async Task<IActionResult> GetCompany(Guid companyId)
        {
            var company = await _companyRepository.GetCompanyAsync(companyId);
            if(company==null)
            {
                return NotFound();
            }
            return  Ok(company);
        }
    }
}
