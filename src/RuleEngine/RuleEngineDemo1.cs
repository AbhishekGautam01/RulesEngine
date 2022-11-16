using RulesEngine.Extensions;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace RuleEngine
{
    public class RuleEngineDemo1
    {
        public static void BasicRuleChecking()
        {
            List<Rule> rules = new List<Rule>();

            Rule rule = new Rule();
            rule.RuleName = "Test Rule";
            rule.SuccessEvent = "Count is within the tolerance limit";
            rule.ErrorMessage = "Over execceded.";
            rule.Expression = "count < 3";
            rule.RuleExpressionType = RuleExpressionType.LambdaExpression; 
            rules.Add(rule);

            var workflows = new List<Workflow>();   
            Workflow exampleWorkflow = new Workflow();

            exampleWorkflow.Rules = rules;  
            exampleWorkflow.WorkflowName = "Example Workflow"; 

            workflows.Add(exampleWorkflow);

            var bre = new RulesEngine.RulesEngine(workflows.ToArray());

            dynamic datas = new ExpandoObject();
            datas.count = 1;

            var inputs = new dynamic[]
            {
                datas
            };

            List<RuleResultTree> results = bre.ExecuteAllRulesAsync("Example Workflow", inputs).Result;

            bool outcome = false;

            // Different ways to show test results;
            outcome = results.TrueForAll( r => r.IsSuccess );

            results.OnSuccess((eventName) =>
            {
                Console.WriteLine($"Result '{eventName}' is as expected.");
                outcome = true;
            });

            results.OnFail(() => {
                outcome = false;
            });

            Console.WriteLine($"Test outcome: {outcome}.");
        }
    }
}
