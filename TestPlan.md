# GetEmployed.cs - Test Plan

### Change Log

| Version | Change Date | By            | Description                                                                    |
| ------- | ----------- | ------------- | ------------------------------------------------------------------------------ |
| v1      | 2026-02-24  | Marco Gerra   | Creating Page                                                                  |
| v1.1    | 2026-03-03  | Marco Gerra   | Added Tests for GameService                                                    |
| v1.2    | 2026-03-04  | Marco Gerra   | Added Job and User validation                                                  |
| v1.3    | 2026-03-05  | Marco Gerra   | Added some more Job Test to pass sprint evaluation                             |
| v1.4    | 2026-03-06  | Luke Steski   | Slight adjustment to test history, spelling corrections                        |
| v1.5    | 2026-03-08  | Marco Gerra   | Added More descriptive test names                                              |
| v2.0    | 2026-03-21  | Marco Gerra   | Added Tests for Quiz Game                                                      |
| v2.1    | 2026-03-26  | Karin Plavin  | Updated layout, added testing levels table, added section for mutation testing |
| v2.2    | 2026-03-26  | Daniel Gorban | Added generic word unit tests and user game unit tests                         |
| v2.3    | 2026-03-27  | Marco Gerra   | Added Acceptence Tests                                                         |

### Roles and Responsibilities

| Name             | NetID    | GitHub username | Role                               |
| ---------------- | -------- | --------------- | ---------------------------------- |
| Marco Gerra      | gerram   | ilikeRocks0     | Front-end developer, Meeting Taker |
| Christian Tadena | tadenac  | chtadena        | Front-end developer                |
| Luke Steski      | steskil  | lukesteski      | Back-end developer, Report writer  |
| Karin Plavin     | plavink  | KarinP03        | Back-end developer                 |
| Ty Paragas       | paragast | typrgs          | Database Manager, Report writer    |
| Daniel Gorban    | gorband  | DanielGorban    | Back-end developer                 |

## Scope

Features that will be tested, as of the end of sprint 3:

**Application management**: Allows users to save posts, mark which ones they applied for, add notes, and sort them using their own personal folder structure.
Scope tested:

**Resume and Cover Letter Improvement**: Lets users improve their writing skills by showing them how to structure sentences, suggest different words, or showing which words to avoid. This is done through three areas: a generic word detector, ana ction sentence designer, and a quiz game to teach them how to identify well-written resume statements.
Scope tested:

**Search jobs**: A more traditional job board that is tailored to technology positions. Filters allow users to narrow down available jobs to their desired needs and skills. Users can also save jobs from this page.
Scope tested:

**Dating job game**: A Tinder inspired matching system where a jub seeking user is given a job and swipes right to save it or left to skip it. Employer users can also participate in the game, but it shows them candidates on the platform instead. In both cases, the number of jobs saved and skipped in the session is displayed.
Scope tested:

**Add Job**: Users and companies can both add job postings to the website, as well as delete and update them later as needed.
Scope tested:

**Job Search Community**: Users and employers both have their own public facing profiles to share who they are and what they're looking for with the community. We also allow users to comment on job postings, and view each other's profiles and company profiles to learn more about them.
Scope tested:

- Accessing Jobs from the database
  - Experiences must have a non-empty string for company name, position title, job description
  - Jobs must have a non-empty string for Job Title, Application Link, Position Type, Employement Type, Job description
  - Jobs must have a valid Application Link that follows this format (\*.\*) or (\*.\*/\*) and does not contain any invalid characters for urls
- Handling state logic of the dating job game (none of the same jobs appearing twice in the game)
  - Cannot Reject or Accept a job before initialization
  - Test Regular Accepting or Rejecting jobs
  - Spamming Accept or Reject only changes game stats for valid interaction
  - Cannot Call GameStats for uninitialized games
- Testing Users from the database
  - User ID must be greater than 0
  - Email, Username, Password, About must all be non empty strings
  - Email must follow the format (\*@\*.\*)

- Testing Quiz Game Validation
  - Make sure weak sentence is not empty
  - Make sure strong sentence is not empty

- Testing Quiz Game Logic
  - Cannot call Answer before we call GetNextItem
  - Stats should be empty at the start of the game
  - When you run out of Quiz Items the Quiz Game returns null
  - When calling GetNextItem before answering it is registered as a skip
  - Answering with the strong string results in a increase in correct
  - Answering with the weak string results in a increase in incorrect
  - Answering with any other string results in a InvalidException
  - Answering after strong string twice/weak string twice results in an InvalidException

- Testing QuizGameConnector
  - Make sure user has an active session
  - When reinitializing session it should reset stats

- Testing QuizGameService
  - Make sure current user is not negative
  - Make sure answer is not empty

## Test Methodology

### Testing Tools

- NUnit - Testing Framework
- Coverlet - Code Cover report
- Stryker.NET - Mutation Testing

### Test Levels

| Test Level          | Scope & Requirement                                                                                       | Methodology                                                                                                                    |
| ------------------- | --------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------ |
| Unit Testing        | At least 10 tests per core feature, for a total of at least 60 tests.                                     | NUnit used to mock the dependencies and create tests for every public function to verify logic works as possible in isolation. |
| Integration Testing | 10 tests total covering interactions between core features.                                               | NUnit used to test API flow for common calls as they flow through features.                                                    |
| Acceptance Testing  | End-user testing for every user story, for 10 tests in total.                                             | Team members will manually walk through the application to test each user story's criteria.                                    |
| Regression Testing  | Running unit and integration tests on every pull request and every push to the `dev` and `main` branches. | Using our GitHub Actions CI pipeline, all unit and integration tests are run automatically.                                    |

### CI/CD Workflow

On pull requests or merges the CI will restore dependencies, build both the backend and frontend and then run tests.
Then for CD, when the code is merged, it builds/tests for front and back end. Then it creates images of each and pushes them to Docker Hub.

### Mutation Testing

Stryker.NET was used to conduct mutation testing on our application. 71.32% Mutation Score For Business Logic. 77.38% Mutation Score For Back-end Queries.

### Acceptence Tests

#### Create Account

1. On the login page, click Register now!
2. Click Job Seeker.
3. Fill out Job Seeker information.
4. Observe, you should see a Registration Successful

#### Login

1. fill in the email slot with a valid email.
2. fill in the password with a valid password that is paired with the email.
3. Observe, you should be taken to the home page.

#### Log Out

1. on the home page, click the top right circle
2. a bar should've dropped down, click Logout
3. Observe, you should be taken to the login page

#### Add Job

1. on the home page, click the top right circle.
2. a bar should've dropped down, click Profile.
3. go to the title Active Postings, click + Post a New Job
4. Fill in valid data
5. Click Post Job
6. Observe, the new job added to the Active Posting List

#### Remove Job

1. on the home page, click the top right circle.
2. a bar should've dropped down, click Profile.
3. go to the title Active Postings, click the trash icon of the active postings
4. A pop up of Delete job posting should appear, click Yes
5. Observe, the job posting is gone

#### Quick Hire

1. on the home page, click the top right circle.
2. a bar should've dropped down, My Applications.
3. Observe, no jobs should be in that list.
4. Click Quick Hire, you should see a job pop up.
5. Swipe Right on the job.
6. Go back to My Applications.
7. Observe, The new Job is saved.
8. Go back Quick Hire, you should see a new job pop up.
9. Swipe Left on the job
10. Go back to My Applications.
11. Observe, No new Job is saved.

#### Search Job

1. On the home page, click Find Jobs
2. Observe, there is a list of Jobs.
3. type "Software" in the search bar.
4. Observe, there only exists Jobs with "Software" in the name.
5. Empty the search bar.
6. In Job Type Select Co-op.
7. Observe, there only exists Jobs with Co-op term.
8. Clear Job Type.
9. In Language/Tool Click C#.
10. Observe, there only exists Jobs with C# term.
11. Clear Language/Tool.
12. Scroll down and click page 2.
13. Observe the list has changed.

#### Quiz Game

1. On Home Page, click the Resume Helper Tools.
2. Find Quiz Game section, Click Check it out!
3. Observe, all the stats values are 0.
4. Play the game, try to get a correct and an incorrect.
5. Keep hitting Skip.
6. Observe, Eventually you should get "no more!" text

#### Generic Word Game

1. On Home Page, click the Resume Helper Tools
2. Find Generic Word Detection, Click Check it out!
3. Type something in the box that contains hardworking and detail-oriented
4. Observe, hardworking and detail-oriented is highlighted

#### Add Comment

1. On Home Page, Click the Find Jobs.
2. Click any job.
3. Click view comments.
4. Click into Add a comment bar
5. Type something.
6. click the send button.
7. Observe, how the comment gets added.
