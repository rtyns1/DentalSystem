*** 12th June 2026, Friday ***

- first, need to understand the project, its architecutre and some of the features we expect.
- I will be using a dummy DB, to avoid conflicting with the actual Database.
- So, i need to think of what structure:: N tier / API centric architecture --- separates the UI from the baceknd completely.
- This is more scalable, and allows for multiple clients --- desktop, mobile etc as long as they connect to the API.
- A distributed system with a clear separation btwn backend and the frontend. With a shared library.
- DentalSystem/
├── Dental.Shared/
│   └── Models/
│       ├── Patient.cs
│       ├── Visit.cs
│       ├── TreatmentPlan.cs
│       └── Payment.cs
├── Dental.API/
│   ├── Controllers/
│   │   └── PatientsController.cs
│   ├── Data/
│   │   └── AppDbContext.cs
│   ├── Migrations/
│   ├── appsettings.json
│   └── Program.cs
└── Dental.Desktop/
    ├── Pages/
    │   └── Patients.razor
    ├── Shared/
    │   └── NavMenu.razor
    ├── MauiProgram.cs
    └── MainPage.xaml

    ### 25th June 2026, Thursday

    -- avent done much programming in any way in the last 3 weeks, very inefficnent.
    So now i have to over compensate for all the days i missed out on.
    -- So for today i need to make great leaps-- i already know most of what i am going to do, the reason i am doing this is so that i can understand the code more deeply, and also be prepared for my meeting with Adam.
    Without a doubt i will need to answer desin and specific questions that will show that i really have been learning and that i have progressed alot from when he last interacted with me.

    - Write all the models.
    - Write all the controllers.
    - Set up the database and migrations.
    - Implement the API endpoints.
    - Test the API endpoints with swagger.
    - API documentation and tseting-- document each and evry apect of the API endpoints.
    - Write the UI pages for the desktop application.
    - Understand how Blazor hybrid maui works an ow to implement it in the project
    - Document heavily, and also, post the post.

