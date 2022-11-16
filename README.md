# RulesEngine
* WIKI PAGE : https://github.com/microsoft/RulesEngine/wiki
* Demo App: https://github.com/microsoft/RulesEngine/tree/main/demo 
## Overview 
* Rule engine is Nuget package for abstracting business login/rules/policies out of a system.
* It allows to store rules outside the core logic of system, thus ensuring that any change in rules don't affect the core system.
* **Installation** :  [Nuget URL](https://www.nuget.org/packages/RulesEngine/) 
* `Command`: NuGet\Install-Package RulesEngine -Version 4.0.0

## Usage
* The rules needs to be stored based on the [Schema Definition](https://github.com/microsoft/RulesEngine/blob/main/schema/workflow-schema.json) which can be stored in: 
    * Azure Blob Storage, 
    * Cosmos DB, 
    * Azure App Configuration, 
    * Entity Framework, 
    * SQL Server
    * File Systems
* For Rule Type expression, the rule is written as lambda expression. 
* **Example**

```json
[
  {
    "WorkflowName": "Discount",
    "Rules": [
      {
        "RuleName": "GiveDiscount10",
        "SuccessEvent": "10",
        "ErrorMessage": "One or more adjust rules failed.",
        "ErrorType": "Error",
        "RuleExpressionType": "LambdaExpression",
        "Expression": "input1.country == \"india\" AND input1.loyaltyFactor <= 2 AND input1.totalPurchasesToDate >= 5000"
      },
      {
        "RuleName": "GiveDiscount20",
        "SuccessEvent": "20",
        "ErrorMessage": "One or more adjust rules failed.",
        "ErrorType": "Error",
        "RuleExpressionType": "LambdaExpression",
        "Expression": "input1.country == \"india\" AND input1.loyaltyFactor >= 3 AND input1.totalPurchasesToDate >= 10000"
      }
    ]
  }
]
```

* Rules can be injected using 
```csharp 
var rulesEngine = new RulesEngine(workflow);
```

* workflow is deserialized objects based on the schema. Once the rule engine is initialized the rules can be executed by calling `ExecuteAllRulesAsync`: 

```csharp
List<RuleResultTree> response = await rulesEngine.ExecuteAllRulesAsync(workflowName, input);
```
* Here the workflow name is the name of the workflow which is `discount` as per above example, input is object which needs to checked. 
* The response will contain a list of [RulesResultTree](https://github.com/microsoft/RulesEngine/wiki/Getting-Started#ruleresulttree) which gives information if a particular rule passed or failed. 
