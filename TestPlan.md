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
| v2.1    | 2026-03-24  | Ty Paragas    | Added tests for object/entity adapters, CommentsService, JobGameService        |
| v2.2    | 2026-03-26  | Karin Plavin  | Updated layout, added testing levels table, added section for mutation testing |
| v2.3    | 2026-03-26  | Daniel Gorban | added generic word unit tests and user game unit tests                         |
| v2.4    | 2026-03-27  | Marco Gerra   | Added Acceptence Tests                                                         |
| v2.5    | 2026-03-27  | Ty Paragas    | Added integration tests                                                        |
| v2.6    | 2026-03-27  | Luke Steski   | Added unit tests for user services                                             |
| v2.7    | 2026-03-27  | Karin Plavin  | Updated layout of scope section, added add job testing information             |
| v3.0    | 2026-04-02  | Karin Plavin  | Added initial info for load testing                                            |
| v3.1    | 2026-04-03  | Karin Plavin  | Updated load testing information                                               |
| v3.2    | 2026-04-05  | Karin Plavin  | Updated load testing information                                               |
| v3.3    | 2026-04-05  | Karin Plavin  | Updated load testing information                                               |

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

**Application management**: Allows users to save and unsave jobs from various areas of the application and view their list of saved jobs.
Scope tested:

- A list of jobs that have been saved should be able to be retrieved from the database separately from the list of all jobs, and subsets of the saved jobs can be retrieved when certain filters are applied (e.g. just jobs that list Java as a language used in the position).
- Saving jobs via the search jobs page and the job seeker's game view, and unsaving jobs from the search jobs page and the list of the user's saved jobs.
- Retrieved saved jobs are complete; they contain all necesssary job information (employer, title, locations, languages, job description, etc.).
- Filters and specific search terms can be applied to get a subset of jobs.
- Ensuring saved jobs cannot be saved again and can only change to unsaved, and that unsaved jobs can only change saved.

**Resume and Cover Letter Improvement**: Lets users improve their writing skills by showing them how to structure sentences, suggest different words, or showing which words to avoid. This is done through a generic word detector and a quiz game to teach them how to identify well-written resume statements.
Scope tested:

- Recognition of when the word detector has no provided text yet, and a lack of detection behaviour when there's nothing to detect.
- Recognition of when the word detector has been provided with text containing repeated words and punctuation around or in words, and appropriate detection based on the content.
- Ensuring the word detector works as expected with varying capitalizations and spacing of text provided to it.
- Access to the sentence strength game and its stats is only provided when the user is logged in and the game has been properly initialized.
- The sentence strength quiz game assesses whether answers are correct or incorrect accurately and doesn't skip to the next pair of sentences prematurely.
- Sentence strength quiz game should update the amount answered correctly and incorrectly accurately (stats).
- Ensuring sentences are retrieved properly from the database and are displayed properly.
- Ensuring spamming or rapid user behaviour is addressed properly with caught exceptions.

**Search jobs**: A more traditional job board that is tailored to technology positions. Filters allow users to narrow down available jobs to their desired needs and skills. Users can also save jobs from this page.
Scope tested:

- A list of jobs that have been uploaded and exist in the application should be able to be retrieved from the database separately from the list of all jobs, and subsets of the jobs can be retrieved when certain filters are applied (e.g. just jobs that list Java as a language used in the position).
- Filters and specific search terms can be applied to get a subset of jobs.
- Retrieved saved jobs are complete; they contain all necesssary job information (employer, title, locations, languages, job description, etc.).

**Dating job game**: A Tinder inspired matching system where a jub seeking user is given a job and swipes right to save it or left to skip it. Employer users can also participate in the game, but it shows them candidates on the platform instead. In both cases, the number of jobs saved and skipped in the session is displayed.
Scope tested:

- Access to the game and its stats is only provided when the user is logged in and the game has been properly initialized.
- The game assesses whether a job (job seeker view) or user (employer view) has been saved or passed on, and doesn't skip to presenting the next job prematurely.
- The game should update the amount saved and passed on accurately (stats).
- Ensuring jobs and users are retrieved properly from the database and are displayed properly.
- Ensuring spamming or rapid user behaviour is addressed properly with caught exceptions.
- Ensuring jobs and users do not appear multiple times in the same game instance.

**Add Job**: Users and companies can both add job postings to the website, as well as delete and update them later as needed.
Scope tested:

- Only allowing an existing user to add a job to the application.
- Recognizing when a user who is adding a job is an employer or a job seeker, and updating the job's poster (uploader) name accordingly.
- Ensuring only a logged in user can add jobs, and only the creator of a job can delete or edit it.
- Ensuring the format of the job information is valid (e.g. email is formatted correctly), or preventing actions from being completed on the job accordingly (e.g. thrown and caught exception).

**Job Search Community**: Users and employers both have their own public facing profiles to share who they are and what they're looking for with the community. We also allow users to comment on job postings, and view each other's profiles and company profiles to learn more about them.
Scope tested:

- User accounts can be created, either as a job seeker or employer, only with correct credentials.
- A user account that exists can be logged into when providing the correct credentials for it.
- Incorrect credentials like empty email or password upon user creation or log in attempts trigger the appropriate response (e.g. caught exception).
- Credentials for user emails match specific formatting when creating an account or logging in.
- Profiles can only be viewed if they contain well-formatted credentials (e.g. an email address in a valid format).
- Comments can be added to and be retrieved for a job if the job is valid.
- Comments can only be added or exist if the user account for them is valid and the comment contains real content (e.g. not empty).

**Additional, general areas that have been tested**: these span across multiple features.

- Accessing Jobs from the database
  - Experiences must have a non-empty string for company name, position title, job description
  - Jobs must have a non-empty string for Job Title, Application Link, Position Type, Employement Type, Job description
  - Jobs must have a valid Application Link that follows this format (\*.\*) or (\*.\*/\*) and does not contain any invalid characters for urls

- Accessing Users from the database
  - User ID must be greater than 0
  - Email, Username, Password, About must all be non empty strings
  - Email must follow the format (\*@\*.\*)

* Using Adapters
  - Ensure all adapters for objects/entities correctly validate object/entity attributes

## Test Methodology

### Testing Tools

- NUnit - Testing Framework
- Coverlet - Code Cover report
- Stryker.NET - Mutation Testing

### Test Levels

| Test Level          | Scope & Requirement                                                                                                                                                                                                                                                  | Methodology                                                                                                                    |
| ------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------ |
| Unit Testing        | At least 10 tests per core feature, for a total of at least 60 tests.                                                                                                                                                                                                | NUnit used to mock the dependencies and create tests for every public function to verify logic works as possible in isolation. |
| Integration Testing | 10 tests total covering interactions between core features. Testing includes creation, conversion and retrieval of business objects using a connection to a test database, as well as verifying common interactions between the service layer and persistence layer. | NUnit used to test API flow for common calls as they flow through features.                                                    |
| Acceptance Testing  | End-user testing for every user story (13 stories)                                                                                                                                                                                                                   | Team members will manually walk through the application to test each user story's criteria.                                    |
| Regression Testing  | Running unit and integration tests on every pull request and every push to the `dev` and `main` branches.                                                                                                                                                            | Using our GitHub Actions CI pipeline, all unit and integration tests are run automatically.                                    |

### CI/CD Workflow

On pull requests or merges the CI will restore dependencies, build both the backend and frontend and then run tests.
Then for CD, when the code is merged, it builds/tests for front and back end. Then it creates images of each and pushes them to Docker Hub.

### Mutation Testing

Stryker.NET was used to conduct mutation testing on our application. 71.32% Mutation Score For Business Logic. 77.38% Mutation Score For Back-end Queries.

### Load Testing

We used NBomber to create and run load testing on our application.

Some of our features only utilize one kind of request (POST or GET) as that is all they require to fulfill the necessary functionality. In order to ensure sufficient coverage of the code base, some features run multiple of one kind or request or multiple request types.

| Feature                             | GET Request                           | POST Request                                                                              | DELETE Request         | PUT Request      |
| ----------------------------------- | ------------------------------------- | ----------------------------------------------------------------------------------------- | ---------------------- | ---------------- |
| Application Management              | Viewing all saved jobs                | Saving a job, unsaving a job                                                              |                        |                  |
| Search Jobs                         | Viewing all jobs                      |                                                                                           |                        |                  |
| Add Jobs                            |                                       | Adding a new job                                                                          | Deleting a job         |                  |
| Resume and Cover Letter Improvement |                                       | Identifying generic words in a sentence, initializing and playing the sentence power quiz |                        |                  |
| Dating Job Game                     |                                       | Initializing the job and user games, accepting and rejecting jobs or users                |                        |                  |
| Job Search Community                | Viewing a job's comments              | Logging in a user, logging out a user, adding new experience                              | Removing an experience | Updating profile |
| Not feature specific                | Accessing the application's main page |                                                                                           |                        |                  |

### Acceptance Tests

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
