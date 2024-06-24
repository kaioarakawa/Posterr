# Poster project

This project was made with the aim of demonstrating the user's technical capacity, it was made for the company Stider. This project was developed based on the description given in the [link](https://onstrider.notion.site/Strider-Full-stack-Assessment-2-0-fae4b4caac4b4052b2576ab036fe35db#0af5214b3ddf4d6ca42590cb1d43722d)

## Getting Started

The following prerequisites are required to build and run the solution (IF YOU WILL RUN LOCALLY):

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (latest version)
- [Node.js](https://nodejs.org/) (latest LTS)
- Change inside the ./Posterr-Backend/API/appsettings.json the value of DefaultConnection to use yor local database
```
"DefaultConnection": "Server=(localdb)\\Local;Database=PosterrDB;Trusted_Connection=True;MultipleActiveResultSets=true"
```
And set inside the ./Posterr-Frontend/.env to your port
```
REACT_APP_API_URL=http://localhost:5000/api
```

After that, we recommend downloading the Visual Studio IDE to run the project more quickly and efficiently.
and to run the frontend just
```bash
npm install
npm start
```

The following prerequisites are required to build and run the solution (if you will with Docker):
- [Docker](https://www.docker.com/) (latest LTS / if you needed)
- Change inside the ./Posterr-Backend/API/appsettings.json the value of DefaultConnection
```
"DefaultConnection": "Server=db;Database=PosterrDB;User=sa;Password=YourStrongPassword123;MultipleActiveResultSets=true;Encrypt=False;TrustServerCertificate=True"
```
And set inside the ./Posterr-Frontend/.env
```
REACT_APP_API_URL=http://localhost:5000/api
```

After that the easiest way to get started is run (in root path, where docker-compose is located):
```
docker-compose up
```

## Swagger

In the backend it will be possible to find the [Swagger](https://swagger.io/) as its documentation, it is available on the route /swagger/index.html
Example: [https://localhost:44306/swagger/index.html](https://localhost:44306/swagger/index.html)

## Storybook

This project is available for use in StoryBook, and you can run it using the command:
```
npm run storybook
```

## Tests

Tests were developed for the project, on the frontend and backend. For the frontend you can run it using the command:
```
npm run test
```

For the backend, we recommend using the Visual Studio IDE to run the tests so the user has a better visual experience.

## Database

The project is configured to use SQL Server by default.

When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.

Running database migrations is easy. Ensure you add the following flags to your command (values assume you are executing from repository root)

* `--project src/Infrastructure` (optional if in this folder)
* `--startup-project src/Web`
* `--output-dir Data/Migrations`

For example, to add a new migration from the root folder:

 `dotnet ef migrations add "SampleMigration" --project src\Infrastructure`

 The database was developed with the idea of ​​being simple and scalable, it contains two tables, Users and Posts.

 ### User
The `User` class represents application users with the following properties:

- **Id** (`Guid`): Primary key, auto-generated.
  - Annotation: `[Key]`
- **Name** (`string`): User's name.
- **Username** (`string`): Required, max length 50.
  - Annotations: `[Required]`, `[MaxLength(50)]`
- **CreatedAt** (`DateTime`): Default to current date.
  - Annotation: `[DefaultValue("getdate()")]`
- **UpdatedAt** (`DateTime`): Last update date.
- **Posts** (`ICollection<Post>`): User's posts.
  - Relation: `virtual`

### Post
The `Post` class represents user posts with the following properties:

- **Content** (`string?`): Optional, max length 777.
  - Annotation: `[MaxLength(777)]`
- **UserId** (`Guid`): Foreign key referencing `User`.
  - Annotation: `[ForeignKey("UserId")]`
- **User** (`User`): Navigation property.
  - Relation: `virtual`
- **OriginalPostId** (`int?`): Optional foreign key referencing original post.
  - Annotation: `[ForeignKey("OriginalPostId")]`
- **OriginalPost** (`Post?`): Navigation property for original post.
  - Relation: `virtual`

## Deploy

This project is prepared to be deployed on any cloud, before that you need to make some configurations in the docker files and in the bank connection, but as an improvement it is something that will easily be done

## Technologies

* [Docker](https://www.docker.com/)
* [ASP.NET Core 8](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
* [Entity Framework Core 8](https://docs.microsoft.com/en-us/ef/core/)
* [React 18](https://react.dev/)
* [Tailwindcss](https://tailwindcss.com/)
* [SweetAlert2](https://sweetalert2.github.io/)
* [StoryBook](https://storybook.js.org/)
* [NUnit](https://nunit.org/), [Moq](https://github.com/moq)


## Critique

In this project it was something that I saw a great possibility of scalability, because of this a lot of things can be improved in it so that when it reaches high usage the system does not become overloaded and remains stable.

### Structure

Both the front and back are made with the best possible structure for the moment, however I see that for the backend it is necessary to add new divisions for the project to have the identity of a complete clean architecture. For the front I see the division into more components for better reuse.

### Querys e Cache

A good idea is to improve the queries made in the system, an example would be to get the user's posts, currently a calculation is made every time requested, for new ideas perhaps the implementation of a cache (Redis) for this data or the creation of a field that saves the same within user.
Optimize database indexes to speed up query performance.

### Validations

Firstly, I see that better validation for requests is necessary, so I see FluentValidation as an accessible tool to create all the necessary rules.
One better error handling and logging to return more informative error message and validate more possible errors.

### New Features

For new features I see that developing followers is important, as is the possibility of adding photos to the post. For these needs, the backend is already fully prepared.

### Design System

As the idea is something scalable for the front, a good idea is to create an entire design system of its own with original and unique palettes to leave the system with a Posterr look and also for any extension to follow the same basis. One idea would be to implement StoryBook completely and effectively.

### Requests

For the frontend, I would change some requests and mounts that I'm doing so that I don't always need to request data that is immutable most of the time. Greater use of useContext or the use of global states like Redux.

### Image upload

For image uploading, I see it as a necessary feature and as a next step, depending on the cloud used, it will be used to store these images. In my opinion, Azure BLOB Storage, S3 Bucket and MinIO are good ideas to implement. Along with them you need a molded CDN and I also recommend the use of a tool to make better use of these images, as they can be in large quantities and have high resolution, my idea as an intermediary would be ImageKit.

### UX & UI 

Improve the user interface and user experience to get a better usability and accessibility, create responsive design for mobile and another screen sizes


