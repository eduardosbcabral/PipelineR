﻿using PipelineR.GettingStarted.Models;
using PipelineR.GettingStarted.Workflows.Bank.Steps;
using System;

namespace PipelineR.GettingStarted.Workflows.Bank
{
    public class BankPipelineBuilder : IBankPipelineBuilder
    {
        private readonly IServiceProvider ServiceProvider;

        public BankPipelineBuilder(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public StepHandlerResult CreateAccount(CreateAccountModel model)
        {
            return Pipeline<BankContext>
                        .Configure(ServiceProvider)
                        .AddStep<ISearchAccountStep>()
                        .AddStep<ICreateAccountStep>()
                            .When(ctx => ctx.Account != null)
                        .Execute(model);
        }

        public StepHandlerResult Deposit(DepositModel model)
        {
            return Pipeline<BankContext>
                        .Configure(ServiceProvider)
                        .AddStep<ISearchAccountStep>()
                        .AddStep<IDepositAccountStep>()
                            .When<IDepositAccountCondition>()
                        .Execute(model);
        }
    }
    
    public interface IBankPipelineBuilder
    {
        StepHandlerResult CreateAccount(CreateAccountModel model);
        StepHandlerResult Deposit(DepositModel model);
    }
}