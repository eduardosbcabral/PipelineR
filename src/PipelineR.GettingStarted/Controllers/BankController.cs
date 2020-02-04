﻿using AspNetScaffolding.Controllers;
using Microsoft.AspNetCore.Mvc;
using PipelineR.GettingStarted.Models;
using PipelineR.GettingStarted.Workflows.Bank;

namespace PipelineR.GettingStarted.Controllers
{
    [Route("bank")]
    public class BankController : BaseController
    {
        private readonly IBankPipelineBuilder _bankPipelineBuilder;
        
        public BankController(IBankPipelineBuilder bankPipelineBuilder)
        {
            _bankPipelineBuilder = bankPipelineBuilder;
        }

        [HttpGet("{accountKey}")]
        public IActionResult Get([FromRoute] int accountKey)
        {
            var model = new DepositModel(10, accountKey);
            var response = _bankPipelineBuilder.Deposit(model);
            return new ObjectResult(response.Errors ?? response.Result()) { StatusCode = response.StatusCode };
        }
    }
}