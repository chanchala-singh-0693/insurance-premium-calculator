# insurance-premium-calculator
Create a new repository on GitHub

# Insurance Premium Calculator
Web app that calculates monthly insurance premium based on occupation and personal details.
**Backend:** .NET 8 Web API &nbsp;|&nbsp; **Frontend:** React JS

## What Does This App Do?
1. Fill in a simple form - name, age, date of birth, occupation, and death cover amount
2. When pick occupation, the monthly premium is calculated automatically
3. The result is shown on the same screen

## How Is the Premium Calculated?
```
Monthly Premium = (Death Cover Amount x Occupation Factor x Age) / 1000 / 12
```
**Example:** Doctor, Age 30, Death Cover $500,000
 (500,000 * 1.5 * 30) / 1000 / 12 = **1,875.00 / month**

### Occupation & Factor Table
| Occupation | Rating        | Factor |
|------------|---------------|--------|
| Doctor     | Professional  | 1.5    |
| Author     | White Collar  | 2.25   |
| Cleaner    | Light Manual  | 11.50  |
| Florist    | Light Manual  | 11.50  |
| Farmer     | Heavy Manual  | 31.75  |
| Mechanic   | Heavy Manual  | 31.75  |
| Other      | Heavy Manual  | 31.75  |


## Project Structure (Simple View)
```
insurance-premium/

 backend/                               .NET Web API
    InsurancePremium.Core/              Data models & interfaces
    InsurancePremium.Infrastructure/    Reads/writes data (repository)
    InsurancePremium.Services/          Premium calculation logic
    InsurancePremium.API/               API endpoints (controllers)
    InsurancePremium.Tests/             Unit tests

    frontend/                           React JS app
        src/
            components/                 Form + Result screen
            services/                   API Call
```
