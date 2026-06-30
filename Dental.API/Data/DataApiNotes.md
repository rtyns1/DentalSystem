** ##APPDBCONTEXT **
- AppDbContext is the bridge btwn the C# models and the actual SQL server database tables. It is the core of Entity Framework Core.
- It inherits from the DbContext class and provides methods for querying and saving data to the database.
- It contains DbSet<T> properties for each model class, which represent the tables in the database.
- It has an OnModelCreating method that allows you to configure the relationshis, indexes, and cascade delete rules using the Fluent API.
- It is injected into the controllers via dependency injection, allowing you to access the database context in your API endpoints.

** What is DbSet<T>? **
- Its a class that represents a database table for a specific entity type T. It provides methods for querying and manipulating the data in that table.
- For the Patient Model class, DbSet<Patient> Patients{get; set;} represents the Patients table in the database.
- An analogy could be -- Consider the database as a filing cabinet, and each drawer is a table. 
- The DbSet<T> is like the handle that lets you oen that specific drawer , look inside , add new files, remove files or search for a specific file.
- It has methods like Add, Remove , Find and ToList that allow you to perform CRUD operations on the data in that table.
- CRUD means create, update, read and delete operations on the data in the database.
- DbSet<T> is a generic class meaning it:
-- can work with any type of entity class, not just a specific one.
-- can be used to represent any table in the database, as long as the entity class is defined and mapped to that table.
-- can be used to perform operations on the data in that table, regardless of the specific type of entity.
-- can be used to define relationships between tables , such as one to many, many to  one etce.
- It is an EF Core class---> it is needed bcause EF core needs to know:::
- - What table exists in the database -- From the DbSet<T> properties in the AppDbContext.
	- What the strucutre of the table is -- From the entity class definition and its properties.
	- How to query a table -- _db.Patients gives an IQueryable<Patient> that EF core can translate to SQL.
	- How to add a new record to a table -- _db.Patients.Add(newPatient) tells EF Core to track a new entity for nisertion.
	- How to delete a record from a table -- _db.Patients.Remove(existingPatient) tells EF Core to mark it for deletion.
	- How to save changes -- _dbSaveChangeAsync() goes through all tracked entities and generates the correct SQL( INSERT, UPDATE, DELETE)
- Without DbSet<T>, EF core would not know how to interact with the database tables,and then i would need to write raw SQL queries for every operation.

** So, NOW WHAT DOES DBSET<T> ACTUALLY DO?? **
