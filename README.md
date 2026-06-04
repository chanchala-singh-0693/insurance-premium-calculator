# insurance-premium-calculator
Create a new repository on GitHub

# Insurance Premium Calculator
Web app that calculates monthly insurance premium based on occupation and personal details.
**Backend:** .NET 8 Web API &nbsp;|&nbsp; **Frontend:** React JS

## What Does This App Do?
1. Fill in a simple form - name, age, date of birth, occupation, and death cover amount
2. When pick occupation, the monthly premium is calculated automatically
3. The result is shown on the same screen
4. Switch to the **History** tab to see all past calculations

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
            components/                 Form + Result screen + History
            services/                   API Call
```
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
|


> **Architecture used:** Layered Architecture + Repository Pattern


## Database Design

Three tables are needed to store the data:

```
Occupation               OccupationRating

Id        (PK)           Id          (PK)
Name                     RatingName
RatingId  (FK)           Factor

PremiumCalculation

Id               (PK)
MemberName
AgeNextBirthday
DateOfBirth
OccupationId     (FK)
DeathSumInsured
MonthlyPremium
CalculatedAt
```

> Note: The app uses in-memory data (no real database needed to run it).


## How to Run It

### What You Need First

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [Git](https://git-scm.com/)


### Step 1 - Start Backend (API)

```bash
cd backend/InsurancePremium.API
dotnet restore
dotnet run
```

 API is now running at `https://localhost:55454`
 View the API docs at `http://localhost:5174/swagger`


### Step 2 - Run the Tests

```bash
cd backend/InsurancePremium.Tests
dotnet test
```

You should see all tests pass


### Step 3 - Start the Frontend (React)

```bash
cd frontend
npm install
npm run dev
```

Open browser at `http://localhost:5173`


## API Endpoints

| Method | URL | What It Does |
|--------|-----|-------------|
| GET | `/api/occupation` | Returns the list of occupations |
| GET | `/api/occupation/ratings` | Returns all rating factors |
| POST | `/api/premiumcalculation/calculate` | Calculates the monthly premium |
| GET | `/api/premiumcalculation/history` | Return history data |